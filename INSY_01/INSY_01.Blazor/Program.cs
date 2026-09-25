using INSY_01.Blazor;
using INSY_01.Blazor.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5199");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Make sure the demo table exists and is seeded before the first visitor.
try
{
    await using var db = AppDbContext.Create();
    await db.EnsureReadyAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"[INSY_01] Cannot prepare the database:\n{ex}");
    Console.Error.WriteLine("[INSY_01] Is the 'mysql-new' Docker container running?  docker start mysql-new");
    return 1;
}

// Serve the framework's static assets (e.g. _framework/blazor.web.js).
app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
return 0;
