using Microsoft.EntityFrameworkCore;

// Service for database operations — wraps EF Core 10 save and query logic
public class DatabaseService(AppDbContext db)
{
    // Adds the entry to change tracking and persists it to the SQLite database
    public async Task SaveAsync(UserInput entry)
    {
        db.UserInputs.Add(entry);
        await db.SaveChangesAsync();
    }

    // Returns all stored UserInput records, including the flattened complex type columns
    public async Task<List<UserInput>> GetAllAsync() =>
        await db.UserInputs.ToListAsync();

    // EF Core 10: LeftJoin — returns all UserInputs with optional Tag (null if no tag)
    public async Task<List<(UserInput Input, Tag? Tag)>> GetAllWithLeftJoinAsync()
    {
        return await db.UserInputs
            .LeftJoin(db.Tags, u => u.TagId, t => t.Id, (u, t) => new { u, t })
            .Select(x => ValueTuple.Create(x.u, x.t))
            .ToListAsync();
    }
    // Adds seed tags to the database
    public async Task SeedTagsAsync()
    {
        if (!await db.Tags.AnyAsync())
        {
            db.Tags.AddRange(
                new Tag { Name = "Important" },
                new Tag { Name = "Archived" },
                new Tag { Name = "Draft" }
            );
            await db.SaveChangesAsync();
        }
    }
}
