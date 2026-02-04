using AreYouDoneYetDataLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("AreYouDoneYetDbConnection");
    builder.Services.AddDbContext<AreYouDoneYetDbContext>(options =>
        options.UseSqlServer(connectionString));

var app = builder.Build();

// Apply pending migrations on startup (guarded)
if (!string.IsNullOrWhiteSpace(connectionString))
{
    try
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AreYouDoneYetDbContext>();

        // CanConnect may throw if the server isn't reachable, so keep it in the try.
        if (dbContext.Database.CanConnect())
        {
            dbContext.Database.Migrate();
        }
        else
        {
            app.Logger.LogWarning("Database not reachable; skipping migrations.");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Database migration failed; app will continue without applying migrations.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
