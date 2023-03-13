using System.Collections.Immutable;

namespace Planrbot.Frontend.Store.TodoUseCase;

public class Feature : Feature<GetWeekState>
{
	public override string GetName() => "GetWeek";
	protected override GetWeekState GetInitialState() => new(true, ImmutableDictionary<Guid, PlanrTaskViewModel>.Empty, DateTime.Today);
}