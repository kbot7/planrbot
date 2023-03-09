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
	public async Task<IEnumerable<ToDoItem>> GetByWeek(CancellationToken ct, DateTime date = default)
	{
		if (date == default)
		{
			date = DateTime.Today;
		}

		var daysAfterSunday = (int)date.DayOfWeek * -1;

		var firstSunday = date.AddDays(daysAfterSunday);

		var items = await _db.ToDoItems.Where(t => t.Date >= firstSunday && t.Date <= firstSunday.AddDays(6)).OrderBy(t => t.Date).ToListAsync(ct);

		return items;
	}

	[HttpGet("{id}")]
	public Task<ToDoItem> GetById(Guid id, CancellationToken ct)
	{
		return _db.ToDoItems.SingleAsync(t => t.Id == id, ct);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> SaveById([FromRoute]Guid id, [FromBody]ToDoItem item, CancellationToken ct)
	{
		var exists = await _db.ToDoItems.AnyAsync(t => t.Id == id, ct);
		if (!exists) { return NotFound(); }
		if (item.Id != id) { return BadRequest(); }

		_db.Update(item);
		await _db.SaveChangesAsync(ct);
		return new OkObjectResult(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] ToDoItem item, CancellationToken ct)
	{
		item.Id = Guid.NewGuid();
		_db.ToDoItems.Add(item);
		await _db.SaveChangesAsync(ct);
		return new OkObjectResult(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteById(Guid id, CancellationToken ct)
	{
		var item = await _db.ToDoItems.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (item != null)
		{
			_db.ToDoItems.Remove(item);
			await _db.SaveChangesAsync(ct);
			return Ok();
		}
		else
		{
			return NotFound();
		}
	}
}