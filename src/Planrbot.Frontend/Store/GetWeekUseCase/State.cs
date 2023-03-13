using System.Collections.Immutable;

namespace Planrbot.Frontend.Store.TodoUseCase;

//[FeatureState]
//public class ToDoState
//{
//	public bool IsLoading { get; }
//	public IEnumerable<ToDoItem> ToDoItems { get; }
//	public DateTime Week { get; } 

//	private ToDoState() { }
//	public ToDoState(bool isLoading, IEnumerable<ToDoItem> todoLists, DateTime week)
//	{
//		IsLoading = isLoading;
//		ToDoItems = todoLists ?? Array.Empty<ToDoItem>();
//		Week = week;
//	}
//}

//public record ToDoState
//{
//	public bool IsLoading { get; init; }
//	public IEnumerable<ToDoItem> ToDoItems { get; init; }
//	public DateTime Week { get; init; }
//}

//[FeatureState]
public record GetWeekState(bool IsLoading, IDictionary<Guid, PlanrTaskViewModel> ToDoItems, DateTime Week);

public class PlanrTaskViewModel : PlanrTask
{ 

}