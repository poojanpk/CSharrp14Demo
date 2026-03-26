// EF Core 10 entity — holds one user sentence and its computed analysis
public class UserInput
{
    public int Id { get; set; }

    // C# 14: 'field' accesses the compiler-generated backing field directly —
    // no separate private variable needed; setter defaults null/empty input
    public string Text
    {
        get => field;
        set => field = string.IsNullOrEmpty(value) ? "No input provided" : value;
    } = string.Empty;

    // EF Core 10: complex type — columns flattened into this table (no JOIN)
    public TextAnalysis Analysis { get; set; } = new();

    // EF Core 10: JSON column — stored as single JSON blob in the database
    public InputMetadata Metadata { get; set; } = new();

    // C# 14: ??= is used against this in Program.cs (assign only when null)
    public DateTime? LastProcessedTime { get; set; }

    // EF Core 10: for named query filter demonstration
    public bool IsActive { get; set; } = true;

    // EF Core 10: for LeftJoin/RightJoin — many-to-one relationship
    public int? TagId { get; set; }
    public Tag? Tag { get; set; }
}
