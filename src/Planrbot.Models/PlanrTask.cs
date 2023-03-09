namespace Planrbot.Models;
public record PlanrTask
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
	public string Description { get; set; } = string.Empty;
	public bool IsComplete { get; set; } = false;
}