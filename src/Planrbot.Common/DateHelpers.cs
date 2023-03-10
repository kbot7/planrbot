namespace Planrbot.Common;
public static class DateHelpers
{
	public static DateTime GetFirstDayOfWeek(DateTime input, DayOfWeek weekStartDay)
	{
		var daysSinceStartOfWeek = (int)input.DayOfWeek - (int)weekStartDay;
		if (daysSinceStartOfWeek < 0)
		{
			daysSinceStartOfWeek += 7;
		}
		return input.AddDays(daysSinceStartOfWeek * -1);
	}

	public static DateOnly GetFirstDayOfWeek(DateOnly input, DayOfWeek weekStartDay)
	{
		var daysSinceStartOfWeek = (int)input.DayOfWeek - (int)weekStartDay;
		if (daysSinceStartOfWeek < 0)
		{
			daysSinceStartOfWeek += 7;
		}
		return input.AddDays(daysSinceStartOfWeek * -1);
	}
}