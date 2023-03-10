using System.ComponentModel.DataAnnotations;
using Planrbot.Common;

public record PlanrTaskRangeFilter : IValidatableObject
{
	public DateOnly? Date { get; init; }
	public DateOnly? WeekDate { get; init; }
	public DateOnly? From { get; init; }
	public DateOnly? To { get; init; }
	public DayOfWeek? FirstDayOfWeek { get; init; } = DayOfWeek.Sunday;
	public (DateOnly From, DateOnly To) ToDateTuple()
	{
		DateOnly start;
		DateOnly end;
		if (Date.HasValue) 
		{
			start = Date.Value;
			end = Date.Value;
		} 
		else if (From.HasValue && To.HasValue)
		{
			start = From.Value;
			end = To.Value;
		}
		else if (WeekDate.HasValue && FirstDayOfWeek.HasValue)
		{
			start = DateHelpers.GetFirstDayOfWeek(WeekDate.Value, FirstDayOfWeek.Value);
			end = start.AddDays(6);
		}
		else
		{
			start = DateHelpers.GetFirstDayOfWeek(DateOnly.FromDateTime(DateTime.Today), FirstDayOfWeek.Value);
			end = start.AddDays(6);
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