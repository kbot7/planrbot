using MudBlazor;

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
			var items = await http.GetFromJsonAsync<PlanrTaskViewModel[]>($"api/ToDo?weekDate={action.Week:yyyy-MM-dd}");
			dispatcher.Dispatch(new GetTasksSuccess(items ?? Array.Empty<PlanrTaskViewModel>(), action.Week));
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
			var item = await result.Content.ReadFromJsonAsync<PlanrTaskViewModel>();
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
	public async Task HandleUpdateTaskAsync(AddTask action, IDispatcher dispatcher)
	{
		var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
		try
		{
			var result = await http.PostAsJsonAsync<PlanrTask>($"api/ToDo", action.Item);
			var item = await result.Content.ReadFromJsonAsync<PlanrTaskViewModel>();
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
	public async Task HandleDeleteTaskAsync(DeleteTask action, IDispatcher dispatcher)
	{
		var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
		try
		{
			var result = await http.DeleteAsync($"api/ToDo/{action.Id}");
			var success = result.IsSuccessStatusCode;
			var errorMessage = success == false ? "HTTP Error" : null;
			var resultAction = new DeleteTaskResult(action.Id, success, errorMessage);
			dispatcher.Dispatch(resultAction);
		}
		//catch (AccessTokenNotAvailableException exception)
		//{
		//    exception.Redirect();
		//}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new DeleteTaskResult(action.Id, false, ex.Message));
			throw;
		}
	}

	[EffectMethod]
	public Task HandleDeleteTaskResultAsync(DeleteTaskResult action, IDispatcher dispatcher)
	{
		var message = action.Success ? "successful" : "unsuccessful";
		_snackbar.Add($"Delete task {message}", action.Success ? Severity.Normal : Severity.Error);
		return Task.CompletedTask;
	}

	[EffectMethod]
	public Task HandleUpdateTaskAsync(UpdateTaskResult action, IDispatcher dispatcher)
	{
		_snackbar.Add("Update task successful");
		return Task.CompletedTask;
	}
}