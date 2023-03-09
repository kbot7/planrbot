namespace Planrbot.Frontend.Store.TodoUseCase;

public static class Reducers
{
	[ReducerMethod]
	public static GetWeekState ReduceGetWeekAction(GetWeekState state, GetTasksByWeek action) =>
		new(true, Array.Empty<PlanrTask>(), action.Week);

	[ReducerMethod]
	public static GetWeekState ReduceGetWeekResultAction(GetWeekState state, GetTasksSuccess action)
	{
		return state with
		{
			ToDoItems = action.Items,
			IsLoading = false,
			Week = action.Week
		};
	}
}

		//new(false, action.Items, action.Week);

