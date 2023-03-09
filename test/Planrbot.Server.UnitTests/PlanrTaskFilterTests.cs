namespace Planrbot.Server.UnitTests;

public class PlanrTaskFilterTests
{
	[Fact]
	public void PlanrTaskFilter_Case1()
	{
		var filter = new PlanrTaskRangeFilter()
		{
			FirstDayOfWeek = DayOfWeek.Thursday,
			WeekDate = DateOnly.FromDateTime(DateTime.Parse("2023-03-10"))
		};

		Assert.True(filter.StartOfWeekDate.Value == DateOnly.FromDateTime(DateTime.Parse("2023-03-09")));
	}

	[Fact]
	public void PlanrTaskFilter_Case2()
	{
		var filter = new PlanrTaskRangeFilter()
		{
			FirstDayOfWeek = DayOfWeek.Thursday,
			WeekDate = DateOnly.FromDateTime(DateTime.Parse("2023-03-06"))
		};

		Assert.True(filter.StartOfWeekDate.Value == DateOnly.FromDateTime(DateTime.Parse("2023-03-02")));
	}

	[Fact]
	public void PlanrTaskFilter_Validate_FromTo_1()
	{
		var filter = new PlanrTaskRangeFilter()
		{
			From = DateOnly.FromDateTime(DateTime.Parse("2023-03-06"))
		};

		var valCtx = new ValidationContext(filter);

		Assert.True(filter.Validate(valCtx).Count() == 1);
	}

	[Fact]
	public void PlanrTaskFilter_Validate_FromTo_2()
	{
		var filter = new PlanrTaskRangeFilter()
		{
			To = DateOnly.FromDateTime(DateTime.Parse("2023-03-06"))
		};

		var valCtx = new ValidationContext(filter);

		Assert.True(filter.Validate(valCtx).Count() == 1);
	}

	[Fact]
	public void PlanrTaskFilter_Validate_FromTo_3()
	{
		var filter = new PlanrTaskRangeFilter()
		{
			From = DateOnly.FromDateTime(DateTime.Parse("2023-03-02")),
			To = DateOnly.FromDateTime(DateTime.Parse("2023-03-06"))
		};

		var valCtx = new ValidationContext(filter);

		Assert.True(filter.Validate(valCtx).Count() == 0);
	}

	[Fact]
	public void PlanrTaskFilter_Validate_None()
	{
		var filter = new PlanrTaskRangeFilter();

		var valCtx = new ValidationContext(filter);

		Assert.True(filter.Validate(valCtx).Count() == 0);
	}
}