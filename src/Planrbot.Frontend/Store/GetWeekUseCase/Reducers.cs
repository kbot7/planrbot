using System.Collections.Immutable;
using System.Linq;

using static MudBlazor.CategoryTypes;

namespace Planrbot.Frontend.Store.TodoUseCase;

public static class Reducers
{
	[ReducerMethod]
	public static GetWeekState ReduceGetWeekAction(GetWeekState state, GetTasksByWeek action) =>
		new(true, ImmutableDictionary<Guid, PlanrTask>.Empty, action.Week);

	[ReducerMethod]
	public static GetWeekState ReduceGetTasksSuccessAction(GetWeekState state, GetTasksSuccess action)
	{
		action.Deconstruct(out PlanrTask[] items, out DateTime week);
		var dict = items.ToDictionary(t => t.Id, t => t);
		return state with
		{
			ToDoItems = dict,
			IsLoading = false,
			Week = action.Week
		};
	}

	[ReducerMethod]
	public static GetWeekState ReduceUpdateTaskResultAction(GetWeekState state, UpdateTaskResult action)
	{
		action.Deconstruct(out PlanrTask item);
		state.Deconstruct(out bool loading, out IDictionary<Guid, PlanrTask> items, out DateTime week);

		items[item.Id] = item;
		return state with
		{
			ToDoItems = items,
			IsLoading = loading,
			Week = week
		};
	}
}
