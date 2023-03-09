namespace Planrbot.Frontend.Store.TodoUseCase;

public static class Reducers
{
	[ReducerMethod]
	public static GetWeekState ReduceGetWeekAction(GetWeekState state, GetWeekAction action) =>
		new(true, Array.Empty<ToDoItem>(), action.Week);

	[ReducerMethod]
	public static GetWeekState ReduceGetWeekResultAction(GetWeekState state, GetWeekResultAction action)
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

