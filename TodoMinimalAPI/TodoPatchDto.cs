public class TodoPatchDto
{
    // datatype followed by ? is used to specify nullable properties
    public string? Name { get; set; }
    public bool? IsComplete { get; set; }
}