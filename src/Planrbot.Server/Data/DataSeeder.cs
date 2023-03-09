namespace Planrbot.Server.Data;

public class DataSeeder
{
	private readonly MainDbContext _db;
	public DataSeeder(MainDbContext db)
	{
		_db = db;

	}

	private static readonly string[] _exampleTasks = new string[]
	{
		"Do the dishes",
		"Wash laundry",
		"Vacuum bedroom",
		"Vacuum office",
		"Clean bathroom",
		"Takeout trash",
		"Sweep kitchen",
		"Scrub kitchen floor",
		"Scrub toilet",
		"Dusting",
		"Shovel snow",
		"Fold laundry",
		"Pickup office"
	};

	private List<PlanrTask> GenerateToDoItems()
	{
		var items = new List<PlanrTask>();
		for (int d = -365; d < 365; d++)
		{
			for (int i = 0; i < 4; i++)
			{
				items.Add(new PlanrTask() { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(d)), IsComplete = Random.Shared.Next(2) == 0, Description = _exampleTasks[Random.Shared.Next(_exampleTasks.Length)] });
			}
		}
		return items;
	}

	public async Task SeedAsync()
	{
		await _db.PlanrTasks.AddRangeAsync(GenerateToDoItems());
		await _db.SaveChangesAsync();
	}
}