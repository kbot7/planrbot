using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using static System.Runtime.InteropServices.JavaScript.JSType;

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
	public async Task<IActionResult> GetByWeek(
		[FromQuery] PlanrTaskRangeFilter criteria,
		CancellationToken ct)
	{
		(DateOnly start, DateOnly end) = criteria.ToDateTuple();

		var items = await _db.PlanrTasks.Where(t => t.Date >= start && t.Date <= end).OrderBy(t => t.Date).ToListAsync(ct);

		return new OkObjectResult(items);
	}

	[HttpGet("{id}")]
	public Task<PlanrTask> GetById(Guid id, CancellationToken ct)
	{
		return _db.PlanrTasks.SingleAsync(t => t.Id == id, ct);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> SaveById([FromRoute]Guid id, [FromBody]PlanrTask item, CancellationToken ct)
	{
		var exists = await _db.PlanrTasks.AnyAsync(t => t.Id == id, ct);
		if (!exists) { return NotFound(); }
		if (item.Id != id) { return BadRequest(); }

		_db.Update(item);
		await _db.SaveChangesAsync(ct);
		return new OkObjectResult(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] PlanrTask item, CancellationToken ct)
	{
		item.Id = Guid.NewGuid();
		_db.PlanrTasks.Add(item);
		await _db.SaveChangesAsync(ct);
		return new OkObjectResult(item);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteById(Guid id, CancellationToken ct)
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
public class PlanrTaskRangeFilter : IValidatableObject
{
	[FromQuery(Name = "Date")]
	public DateOnly? WeekDate { get; init; }
	[FromQuery(Name = "From")]
	public DateOnly? From { get; init; }
	[FromQuery(Name = "To")]
	public DateOnly? To { get; init; }
	[FromQuery(Name = "FirstDayOfWeek")]
	public DayOfWeek? FirstDayOfWeek { get; init; } = DayOfWeek.Sunday;
	[BindNever]
	public DateOnly? StartOfWeekDate
	{
		get
		{
			if (WeekDate == null || FirstDayOfWeek == null) return null;
			var daysSinceStartOfWeek = (int)WeekDate.Value.DayOfWeek - (int)FirstDayOfWeek.Value;
			if (daysSinceStartOfWeek < 0)
			{
				daysSinceStartOfWeek += 7;
			}
			return WeekDate.Value.AddDays(daysSinceStartOfWeek * -1);
		}
	}

	public (DateOnly From, DateOnly To) ToDateTuple()
	{
		DateOnly start;
		DateOnly end;
		if (From.HasValue && To.HasValue)
		{
			start = From.Value;
			end = To.Value;
		}
		else if (StartOfWeekDate.HasValue)
		{
			start = StartOfWeekDate.Value;
			end = StartOfWeekDate.Value.AddDays(6);
		}
		else
		{
			var startWeek = new PlanrTaskRangeFilter
			{
				WeekDate = DateOnly.FromDateTime(DateTime.Today)
			}.StartOfWeekDate;
			start = startWeek.Value;
			end = startWeek.Value.AddDays(6);
		}
		return (start, end);
	}

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (From.HasValue && !To.HasValue)
		{
			yield return new ValidationResult(
				"'To' parameter is required when 'From' is used",
				new[] { nameof(To) });
		}
		if (To.HasValue && !From.HasValue)
		{
			yield return new ValidationResult(
				"'From' parameter is required when 'To' is used",
				new[] { nameof(From) });
		}
	}
}