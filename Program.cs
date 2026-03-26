using Microsoft.Data.Sqlite;

Console.WriteLine("==============================================");
Console.WriteLine("   C# 14 + EF Core 10      ");
Console.WriteLine("==============================================");
Console.WriteLine();

// EF Core 10: keep connection open so the in-memory database persists for the app lifetime
await using var connection = new SqliteConnection("DataSource=:memory:");
await connection.OpenAsync();

await using var db = new AppDbContext(connection);
await db.Database.EnsureCreatedAsync(); // creates schema from model — no migrations needed

// Wire up services
var analysisService = new TextAnalysisService(); // wraps C# 14 extension members
var databaseService = new DatabaseService(db);   // wraps EF Core 10 operations

// Seed tags for demonstration
await databaseService.SeedTagsAsync();

// Step 1: collect input
Console.Write("Enter a sentence to analyze: ");
var rawInput = Console.ReadLine();

// C# 14: field keyword — Text setter validates and defaults empty input inside UserInput.cs
var entry = new UserInput { Text = rawInput, TagId = 1 };

// Step 2: analyse via TextAnalysisService
Console.WriteLine($"\n  Analysing: \"{entry.Text}\"");

if (analysisService.IsEmpty(entry.Text)) // C# 14: extension property via service
    Console.WriteLine("  (input was null/empty — default text applied)");

entry.Analysis = analysisService.Analyze(entry.Text); // populates the EF Core complex type

// EF Core 10: JSON column — populate metadata (stored as JSON in database)
entry.Metadata = new InputMetadata
{
    Source = "Console",
    Version = 1,
    Timestamp = DateTime.UtcNow,
};

// C# 14: ??= — assigns DateTime.UtcNow only when LastProcessedTime is null
entry.LastProcessedTime ??= DateTime.UtcNow;

Console.WriteLine($"  Word Count      : {entry.Analysis.WordCount}");
Console.WriteLine($"  Character Count : {entry.Analysis.CharacterCount}");
Console.WriteLine($"  Metadata Source : {entry.Metadata.Source}");

// Step 3: persist via DatabaseService
await databaseService.SaveAsync(entry);
Console.WriteLine("\n  Record saved.\n");

// Step 4: EF Core 10 — LeftJoin demonstration
Console.WriteLine(new string('-', 46));
Console.WriteLine("  EF Core 10: LeftJoin (all inputs + tags)");
Console.WriteLine(new string('-', 46));

var joined = await databaseService.GetAllWithLeftJoinAsync();
foreach (var (input, tag) in joined)
{
    Console.WriteLine($"  ID: {input.Id} | Text: {input.Text} | Tag: {tag?.Name ?? "(none)"}");
}
Console.WriteLine();

// Step 5: read back all records via DatabaseService
var records = await databaseService.GetAllAsync();

Console.WriteLine(new string('-', 46));
Console.WriteLine("  All Records in Database (Named Filter Active)");
Console.WriteLine(new string('-', 46));

foreach (var r in records)
{
    Console.WriteLine($"ID           : {r.Id}");
    Console.WriteLine($"Text         : {r.Text}");
    Console.WriteLine($"Words        : {r.Analysis.WordCount}");        // Complex Type column
    Console.WriteLine($"Characters   : {r.Analysis.CharacterCount}");   // Complex Type column
    Console.WriteLine($"Metadata     : {r.Metadata.Source} v{r.Metadata.Version}"); // JSON column
    Console.WriteLine($"Processed At : {r.LastProcessedTime:u}");
    Console.WriteLine();
}

Console.WriteLine();
