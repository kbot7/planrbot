namespace Planrbot.Frontend.Store;

public record GetTasksByWeek(DateTime Week);
public record GetTasksSuccess(PlanrTask[] Items, DateTime Week);
public record GetTasksError(string Error);
public record AddTask(PlanrTask Item);
public record AddTaskResult(PlanrTask Item);
public record AddTaskError(string Error);
public record UpdateTask(PlanrTask Item);
public record UpdateTaskResult(PlanrTask Item);
public record UpdateTaskError(string Error);
public record GetTask(Guid Id);
public record GetTaskResult(PlanrTask Item);
public record GetTaskError(string Error);
public record DeleteTask(Guid Id);
public record DeleteTaskResult(bool Success);
public record DeleteTaskError(string Error);