using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace INSY_01.Blazor.Data;

/// <summary>
/// Connection to the local Docker MySQL
/// (container <c>mysql-new</c> on 127.0.0.1:3306, database <c>transactions</c>).
/// </summary>
public static class  Db
{
    public const string ConnectionString =
        "Server=127.0.0.1;Port=3306;User=root;Password=insy;Database=transactions;";
}

/// <summary>
/// One row of <c>tickets_cc</c>: how many tickets are still available
/// for one event, plus the concurrency token.
/// </summary>
public class Ticket
{
    public int Id { get; set; }
    public int Available { get; set; }

    /// <summary>
    /// Optimistic concurrency token. Because of
    /// <see cref="ConcurrencyCheckAttribute" /> EF Core appends
    /// <c>AND row_version = &lt;the value this context read&gt;</c> to every
    /// UPDATE it generates: if another session committed in the meantime the
    /// UPDATE matches 0 rows and <c>SaveChanges()</c> throws
    /// <c>DbUpdateConcurrencyException</c> instead of silently overwriting it.
    /// </summary>
    [ConcurrencyCheck]
    public int RowVersion { get; set; }
}

/// <summary>
/// One <see cref="AppDbContext" /> instance = one "user"
/// (its own connection / its own MySQL session).
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Creates a context that reports every SQL statement it executes,
    /// prefixed with <paramref name="label" />, through
    /// <paramref name="onSql" />.
    /// </summary>
    public static AppDbContext Create(string label, Action<string> onSql)
        => new(new DbContextOptionsBuilder<AppDbContext>()
            .UseMySQL(Db.ConnectionString)
            .LogTo(msg => onSql($"[{label}] {msg}"), LogLevel.Information)
            .Options);

    /// <summary>Context without logging (used once at startup).</summary>
    public static AppDbContext Create() => Create("db", _ => { });

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Ticket>(e =>
        {
            e.ToTable("tickets_cc");
            e.HasKey(t => t.Id);
            e.Property(t => t.Id).ValueGeneratedNever();
            e.Property(t => t.Available).HasColumnName("available");
            e.Property(t => t.RowVersion).HasColumnName("row_version");
        });
    }

    /// <summary>
    /// Creates <c>tickets_cc</c> when it is missing and seeds the demo row
    /// (Id=1, available=100, row_version=0) when the table is empty.
    /// </summary>
    public async Task EnsureReadyAsync()
    {
        await Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS tickets_cc (
                Id          INT NOT NULL PRIMARY KEY,
                available   INT NOT NULL,
                row_version INT NOT NULL DEFAULT 0
            )
            """);

        if (!await Tickets.AnyAsync())
        {
            Tickets.Add(new Ticket { Id = 1, Available = 100, RowVersion = 0 });
            await SaveChangesAsync();
        }
    }
}
