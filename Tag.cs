// EF Core 10: entity for demonstrating LeftJoin/RightJoin queries
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Navigation: one tag can be linked to many inputs
    public List<UserInput> UserInputs { get; set; } = new();
}
