public class TodoItemDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    // constructor
    public TodoItemDto() { }
    // constructor that uses fat arrow to define expression in a single line
    // also uses tuple destructuring to unpack the items from passes in Todo
    // into the data transfer object (DTO) TodoItemDto
    public TodoItemDto(Todo todoItem) =>
    (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
}