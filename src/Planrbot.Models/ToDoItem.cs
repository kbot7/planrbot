namespace Planrbot.Models;
public record ToDoItem
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime Date { get; set; } = DateTime.Today;
	public string Description { get; set; } = string.Empty;
	public bool IsComplete { get; set; } = false;
}