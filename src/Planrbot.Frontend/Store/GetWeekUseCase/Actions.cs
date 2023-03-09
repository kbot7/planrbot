namespace Planrbot.Frontend.Store;

public record GetWeekAction(DateTime Week);
public record GetWeekResultAction(ToDoItem[] Items, DateTime Week);
public record GetWeekErrorAction(string Error);