using MudBlazor;

using static MudBlazor.CategoryTypes;

namespace Planrbot.Frontend.Store.TodoUseCase;

public class GetWeekHttpEffect // : Effect<GetTasksByWeek>
{
	private readonly IHttpClientFactory _httpFactory;
	private readonly ISnackbar _snackbar;

	public GetWeekHttpEffect(IHttpClientFactory httpFactory, ISnackbar snackbar)
	{
		_httpFactory = httpFactory;
		_snackbar = snackbar;
	}

	[EffectMethod]
	public async Task HandleGetTasksByWeekAsync(GetTasksByWeek action, IDispatcher dispatcher)
	//public override async Task HandleAsync(GetTasksByWeek action, IDispatcher dispatcher)
	{
		try
		{
			var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
			var items = await http.GetFromJsonAsync<PlanrTask[]>($"api/ToDo?weekDate={action.Week:yyyy-MM-dd}");
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

	[EffectMethod]
	public async Task HandleUpdateTaskAsync(UpdateTask action, IDispatcher dispatcher)
	{
		var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
		try
		{
			var result = await http.PutAsJsonAsync<PlanrTask>($"api/ToDo/{action.Item.Id}", action.Item);
			var item = await result.Content.ReadFromJsonAsync<PlanrTask>();
			var resultAction = new UpdateTaskResult(item);
			dispatcher.Dispatch(resultAction);
		}
		//catch (AccessTokenNotAvailableException exception)
		//{
		//    exception.Redirect();
		//}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new UpdateTaskError(ex.Message));
			throw;
		}
	}

	[EffectMethod]
	public async Task HandleUpdateTaskAsync(UpdateTaskResult action, IDispatcher dispatcher)
	{
		_snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
		_snackbar.Add("Update task successful");
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