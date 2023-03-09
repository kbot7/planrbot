namespace Planrbot.Frontend.Store.TodoUseCase;

public class Feature : Feature<GetWeekState>
{
	public override string GetName() => "GetWeek";
	protected override GetWeekState GetInitialState() => new(true, Array.Empty<ToDoItem>(), DateTime.Today);
}
