namespace Planrbot.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public partial class ToDoController : ControllerBase
{
	private readonly ILogger<ToDoController> _logger;
	private readonly MainDbContext _db;

	public ToDoController(ILogger<ToDoController> logger, MainDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	[HttpGet]
	public async Task<IActionResult> Get(
		[FromQuery] PlanrTaskRangeFilter criteria,
		CancellationToken ct)
	{
		(DateOnly start, DateOnly end) = criteria.ToDateTuple();

		var items = await _db.PlanrTasks.Where(t => t.Date >= start && t.Date <= end).OrderBy(t => t.Date).ToListAsync(ct);

		return new OkObjectResult(items);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(Guid id, CancellationToken ct)
	{
		return new OkObjectResult(await _db.PlanrTasks.SingleAsync(t => t.Id == id, ct));
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] PlanrTask item, CancellationToken ct)
	{
		var exists = await _db.PlanrTasks.AnyAsync(t => t.Id == id, ct);
		if (!exists) { return NotFound(); }
		if (item.Id != id) { return BadRequest(); }

		_db.Update(item);
		await _db.SaveChangesAsync(ct);
		return new OkObjectResult(item);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] PlanrTask item, CancellationToken ct)
	{
		item.Id = Guid.NewGuid();
		_db.PlanrTasks.Add(item);
		await _db.SaveChangesAsync(ct);
		return new OkObjectResult(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
	{
		var item = await _db.PlanrTasks.FirstOrDefaultAsync(t => t.Id == id, ct);
		if (item != null)
		{
			_db.PlanrTasks.Remove(item);
			await _db.SaveChangesAsync(ct);
			return Ok();
		}
		else
		{
			return NotFound();
		}
	}
}