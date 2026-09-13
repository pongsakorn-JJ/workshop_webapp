namespace TodoApi.Models;

public class Todoitem
{
    public int id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool MyProperty { get; set; }
    public DateTime CreatedAt { get; set; }
}
