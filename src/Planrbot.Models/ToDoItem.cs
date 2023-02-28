namespace Planrbot.Models;
public class ToDoItem
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Description { get; set; }
	public bool IsComplete { get; set; }
}