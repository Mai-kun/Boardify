namespace BoardifyApp.Models;

public class Column
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public List<TaskCard> Tasks { get; set; } = [];
}