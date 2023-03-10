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
		decimal half = (decimal)days / 2;
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
		var result = await _controller.Get(filter, It.IsAny<CancellationToken>());

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
		var result = await _controller.Get(filter, It.IsAny<CancellationToken>());

		// Assert
		Assert.True(result is OkObjectResult);
		var okResult = Assert.IsType<OkObjectResult>(result);
		var content = Assert.IsType<List<PlanrTask>>(okResult.Value);
		Assert.Equal(35, content.Count);
	}

	[Fact]
	public async Task Get_Id_Valid()
	{
		// Arrange
		await _context.Database.EnsureDeletedAsync();
		await _context.Database.EnsureCreatedAsync();
		var id = Guid.NewGuid();
		var planrTask = new PlanrTask() { Id = id, Date = DateOnly.FromDateTime(DateTime.Today), Description = "Test Task", IsComplete = false };
		_context.PlanrTasks.Add(planrTask);
		await _context.SaveChangesAsync();

		// Act
		var result = await _controller.Get(id, It.IsAny<CancellationToken>());

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var content = Assert.IsType<PlanrTask>(okResult.Value);
		Assert.NotNull(content);
		Assert.Equal(id, content.Id);
		Assert.Equal("Test Task", content.Description);
		Assert.False(content.IsComplete);
		Assert.Equal(DateOnly.FromDateTime(DateTime.Today), content.Date);
	}

	[Fact]
	public async Task Post_Valid()
	{
		// Arrange
		await _context.Database.EnsureDeletedAsync();
		await _context.Database.EnsureCreatedAsync();
		await _context.SaveChangesAsync();

		// Act
		var planrTask = new PlanrTask() { Date = DateOnly.FromDateTime(DateTime.Today), Description = "Test Task" };
		var result = await _controller.Post(planrTask, It.IsAny<CancellationToken>());

		// Assert
		Assert.True(result is OkObjectResult);
		var okResult = Assert.IsType<OkObjectResult>(result);
		var content = Assert.IsType<PlanrTask>(okResult.Value);
		Assert.NotNull(content);
		Assert.Equal("Test Task", content.Description);
		Assert.Equal(DateOnly.FromDateTime(DateTime.Today), content.Date);
	}

	[Fact]
	public async Task Put_Valid()
	{
		// Arrange
		await _context.Database.EnsureDeletedAsync();
		await _context.Database.EnsureCreatedAsync();
		var planrTask = new PlanrTask() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today), Description = "Test Task", IsComplete = false };
		_context.PlanrTasks.Add(planrTask);
		await _context.SaveChangesAsync();

		// Act
		planrTask.Description = "Modified";
		planrTask.IsComplete = true;
		var result = await _controller.Put(planrTask.Id, planrTask, It.IsAny<CancellationToken>());

		// Assert
		Assert.True(result is OkObjectResult);
		var okResult = Assert.IsType<OkObjectResult>(result);
		var content = Assert.IsType<PlanrTask>(okResult.Value);
		Assert.NotNull(content);
		Assert.Equal("Modified", content.Description);
		Assert.True(content.IsComplete);
		Assert.Equal(DateOnly.FromDateTime(DateTime.Today), content.Date);
	}

	[Fact]
	public async Task Delete_Valid()
	{
		// Arrange
		await _context.Database.EnsureDeletedAsync();
		await _context.Database.EnsureCreatedAsync();
		var planrTask = new PlanrTask() { Id = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Today), Description = "Test Task", IsComplete = false };
		_context.PlanrTasks.Add(planrTask);
		await _context.SaveChangesAsync();

		// Act
		planrTask.Description = "Modified";
		planrTask.IsComplete = true;
		var result = await _controller.Delete(planrTask.Id, It.IsAny<CancellationToken>());

		// Assert
		Assert.True(result is OkResult);
		Assert.False(_context.PlanrTasks.Any());
	}

}