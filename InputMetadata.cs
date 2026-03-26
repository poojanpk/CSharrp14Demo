// EF Core 10: JSON column — stored as single JSON blob, not individual columns
public class InputMetadata
{
    public string Source { get; set; } = "Console";
    public int Version { get; set; } = 1;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
