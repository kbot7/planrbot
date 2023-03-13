using System.Collections.Immutable;
using System.Linq;

using static MudBlazor.CategoryTypes;

namespace Planrbot.Frontend.Store.TodoUseCase;

public static class Reducers
{
	[ReducerMethod]
	public static GetWeekState ReduceGetWeekAction(GetWeekState state, GetTasksByWeek action) =>
		new(true, ImmutableDictionary<Guid, PlanrTaskViewModel>.Empty, action.Week);

	[ReducerMethod]
	public static GetWeekState ReduceGetTasksSuccessAction(GetWeekState state, GetTasksSuccess action)
	{
		action.Deconstruct(out PlanrTaskViewModel[] items, out DateTime week);
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
		action.Deconstruct(out PlanrTaskViewModel item);
		state.Deconstruct(out bool loading, out IDictionary<Guid, PlanrTaskViewModel> items, out DateTime week);

		items[item.Id] = item;
		return state with
		{
			ToDoItems = items,
			IsLoading = loading,
			Week = week
		};
	}

	[ReducerMethod]
	public static GetWeekState ReduceDeleteTaskAction(GetWeekState state, DeleteTaskResult action)
	{
		state.Deconstruct(out bool loading, out IDictionary<Guid, PlanrTaskViewModel> items, out DateTime week);

		items.Remove(action.Id);
		return state with
		{
			ToDoItems = items,
			IsLoading = loading,
			Week = week
		};
	}
}