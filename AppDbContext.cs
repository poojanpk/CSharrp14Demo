using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// EF Core 10: DbContext — primary constructor receives the open connection
public class AppDbContext(SqliteConnection connection) : DbContext
{
    // Exposes the UserInputs table; Set<T>() avoids nullable backing-field warnings
    public DbSet<UserInput> UserInputs => Set<UserInput>();
    public DbSet<Tag> Tags => Set<Tag>();

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

            // EF Core 10: JSON column — stores Metadata as JSON {"Source":"...","Version":1,...}
            entity.OwnsOne(u => u.Metadata, nav => nav.ToJson());

            // EF Core 10: query filter — filters out inactive records unless IgnoreQueryFilters() is called
            entity.HasQueryFilter(u => u.IsActive);

            // Many-to-one relationship with Tag
            entity.HasOne(u => u.Tag)
                  .WithMany(t => t.UserInputs)
                  .HasForeignKey(u => u.TagId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(t => t.Id);
        });
    }
}
