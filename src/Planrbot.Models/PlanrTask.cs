namespace Planrbot.Models;
public class PlanrTask
{
	public Guid Id { get; set; }
	public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
	public string Description { get; set; } = string.Empty;
	public bool IsComplete { get; set; } = false;
}