namespace Planrbot.Web.Server.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ToDoController : ControllerBase
{

	private readonly ILogger<ToDoController> _logger;
	private readonly MainDbContext _db;

	public ToDoController(ILogger<ToDoController> logger, MainDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	[HttpGet]
	public async Task<IEnumerable<ToDoItem>> GetByWeek(DateTime date = default)
	{
		if (date == default)
		{
			date = DateTime.Today;
		}

		var daysAfterSunday = (int)date.DayOfWeek * -1;

		var firstSunday = date.AddDays(daysAfterSunday);

		var items = await _db.ToDoItems.Where(t => t.Date >= firstSunday && t.Date <= firstSunday.AddDays(6)).OrderBy(t => t.Date).ToListAsync();

		return items;
	}

	[HttpGet("{id}")]
	public Task<ToDoItem> GetById(Guid id)
	{
		return _db.ToDoItems.SingleAsync(t => t.Id == id);
	}

	[HttpPost("{id}")]
	public async Task SaveById([FromRoute]Guid id, [FromBody]ToDoItem item)
	{
		_db.Update(item);
		await _db.SaveChangesAsync();
	}
}