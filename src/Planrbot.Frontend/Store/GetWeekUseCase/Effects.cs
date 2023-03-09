namespace Planrbot.Frontend.Store.TodoUseCase;

public class Effects : Effect<GetWeekAction>
{
	private readonly IHttpClientFactory _httpFactory;

	public Effects(IHttpClientFactory httpFactory)
	{
		_httpFactory = httpFactory;
	}

	public override async Task HandleAsync(GetWeekAction action, IDispatcher dispatcher)
	{
		try
		{
			var http = _httpFactory.CreateClient("Planrbot.Web.ServerAPI");
			var items = await http.GetFromJsonAsync<ToDoItem[]>($"api/ToDo?date={action.Week:s}");
			dispatcher.Dispatch(new GetWeekResultAction(items ?? Array.Empty<ToDoItem>(), action.Week));
		}
		//catch (AccessTokenNotAvailableException exception)
		//{
		//    exception.Redirect();
		//}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new GetWeekErrorAction(ex.Message));
			throw;
		}
	}
}
