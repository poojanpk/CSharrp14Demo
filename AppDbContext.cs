using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// EF Core 10: DbContext — primary constructor receives the open connection
public class AppDbContext(SqliteConnection connection) : DbContext
{
    // Exposes the UserInputs table; Set<T>() avoids nullable backing-field warnings
    public DbSet<UserInput> UserInputs => Set<UserInput>();

    // Uses the injected connection so all calls share the same in-memory database
    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(connection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInput>(entity =>
        {
            entity.HasKey(u => u.Id);

            // EF Core 10: maps TextAnalysis as a complex type —
            // its properties are flattened into UserInputs, no separate table
            entity.ComplexProperty(u => u.Analysis);
        });
    }
}
