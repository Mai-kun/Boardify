namespace BoardifyApp.Models;

public class Board
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<Column> Columns { get; set; } = [];
}