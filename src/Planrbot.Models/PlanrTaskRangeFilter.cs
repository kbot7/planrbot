using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public record PlanrTaskRangeFilter : IValidatableObject
{
	public DateOnly? Date { get; init; }
	public DateOnly? WeekDate { get; init; }
	public DateOnly? From { get; init; }
	public DateOnly? To { get; init; }
	public DayOfWeek? FirstDayOfWeek { get; init; } = DayOfWeek.Sunday;
	[NotMapped]
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