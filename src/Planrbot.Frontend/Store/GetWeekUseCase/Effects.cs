namespace Planrbot.Frontend.Store.TodoUseCase;

public class GetWeekHttpEffect : Effect<GetTasksByWeek>
{
	private readonly IHttpClientFactory _httpFactory;

	public GetWeekHttpEffect(IHttpClientFactory httpFactory)
	{
		_httpFactory = httpFactory;
	}

	public override async Task HandleAsync(GetTasksByWeek action, IDispatcher dispatcher)
	{
		try
		{
			var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
			var items = await http.GetFromJsonAsync<PlanrTask[]>($"api/ToDo?date={action.Week:yyyy-MM-dd}");
			dispatcher.Dispatch(new GetTasksSuccess(items ?? Array.Empty<PlanrTask>(), action.Week));
		}
		//catch (AccessTokenNotAvailableException exception)
		//{
		//    exception.Redirect();
		//}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new GetTasksError(ex.Message));
			throw;
		}
	}
}

//public class UpdateToDoListHttpEffect : Effect<UpdateToDoItemAction>
//{
//	private readonly IHttpClientFactory _httpFactory;

//	public UpdateToDoListHttpEffect(IHttpClientFactory httpFactory)
//	{
//		_httpFactory = httpFactory;
//	}

//	public override async Task HandleAsync(UpdateToDoItemAction action, IDispatcher dispatcher)
//	{
//		var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
//		try
//		{
//			var result = await http.PutAsJsonAsync<ToDoItem>($"api/ToDo/{action.Item.Id}", action.Item);
//			var resultAction = new UpdateToDoItemResultAction(result);


//			var items = await http.GetFromJsonAsync<ToDoItem[]>($"api/ToDo?date={action.Week:s}");
//			dispatcher.Dispatch(new GetWeekResultAction(items ?? Array.Empty<ToDoItem>(), action.Week));
//		}
//		//catch (AccessTokenNotAvailableException exception)
//		//{
//		//    exception.Redirect();
//		//}
//		catch (Exception ex)
//		{
//			dispatcher.Dispatch(new GetWeekErrorAction(ex.Message));
//			throw;
//		}
//	}
//}
