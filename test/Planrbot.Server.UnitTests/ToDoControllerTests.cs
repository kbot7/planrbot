namespace Planrbot.Server.UnitTests;
public class ToDoControllerTests : IDisposable
{
	private readonly MainDbContext _context;
	private readonly ToDoController _controller;
	private readonly AutoMocker _mocker;

	public ToDoControllerTests()
	{
		_mocker = new AutoMocker();

		var options = new DbContextOptionsBuilder<MainDbContext>()
			.UseInMemoryDatabase(databaseName: "PlanrbotUnitTests")
			.Options;

		_context = new MainDbContext(options);
		_mocker.Use<MainDbContext>(_context);
		_controller = _mocker.CreateInstance<ToDoController>();
	}

	public void Dispose() => _context.Dispose();

	private static List<PlanrTask> GetSeedData(int days, int tasksPerDay = 5)
	{
		decimal half = (decimal)days/2;
		int startDay = 0 - (int)Math.Floor(half);
		int endDay = 0 + (int)Math.Ceiling(half);
		endDay = endDay == 0 ? 1 : endDay;

		List<PlanrTask> tasks = new();
		for (int i = startDay; i < endDay; i++)
		{
			for (int j = 0; j < tasksPerDay; j++)
			{
				tasks.Add(new PlanrTask { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)), Description = $"Task {j}", IsComplete = j % 2 == 0 });
			}
		}
		return tasks;
	}

	[Fact]
	public async Task Get_NoParams()
	{
		// Arrange
		await _context.Database.EnsureDeletedAsync();
		await _context.Database.EnsureCreatedAsync();
		_context.PlanrTasks.AddRange(GetSeedData(1));
		await _context.SaveChangesAsync();

		// Act
		var filter = new PlanrTaskRangeFilter();
		var result = await _controller.GetByWeek(filter, It.IsAny<CancellationToken>());

		// Assert
		Assert.True(result is OkObjectResult);
		var okResult = Assert.IsType<OkObjectResult>(result);
		var content = Assert.IsType<List<PlanrTask>>(okResult.Value);
		Assert.Equal(5, content.Count);
	}

	[Fact]
	public async Task Get_WeekDate()
	{
		// Arrange
		await _context.Database.EnsureDeletedAsync();
		await _context.Database.EnsureCreatedAsync();
		_context.PlanrTasks.AddRange(GetSeedData(20, 5));
		await _context.SaveChangesAsync();

		// Act
		var filter = new PlanrTaskRangeFilter()
		{
			WeekDate = DateOnly.FromDateTime(DateTime.Today)
		};
		var result = await _controller.GetByWeek(filter, It.IsAny<CancellationToken>());

		// Assert
		Assert.True(result is OkObjectResult);
		var okResult = Assert.IsType<OkObjectResult>(result);
		var content = Assert.IsType<List<PlanrTask>>(okResult.Value);
		Assert.Equal(35, content.Count);
	}

}
