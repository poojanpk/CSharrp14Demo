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
}
