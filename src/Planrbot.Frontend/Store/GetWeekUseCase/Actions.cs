namespace Planrbot.Frontend.Store;

public record GetTasksByWeek(DateTime Week);
public record GetTasksSuccess(PlanrTaskViewModel[] Items, DateTime Week);
public record GetTasksError(string Error);
public record AddTask(PlanrTaskViewModel Item);
public record UpdateTask(PlanrTaskViewModel Item);
public record UpdateTaskResult(PlanrTaskViewModel Item);
public record UpdateTaskError(string Error);
public record GetTask(Guid Id);
public record GetTaskResult(PlanrTaskViewModel Item);
public record GetTaskError(string Error);
public record DeleteTask(Guid Id);
public record DeleteTaskResult(bool Success);
public record DeleteTaskError(string Error);