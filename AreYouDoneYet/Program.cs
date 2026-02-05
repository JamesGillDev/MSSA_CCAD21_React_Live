using AreYouDoneYetDataLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS configuration
var allowedCorsHost = builder.Configuration
    .GetSection("AllowedHostsCors")
    .GetValue<string>("ReactHostBase")
    ?? throw new InvalidOperationException("AllowedHostsCors:ReactHostBase configuration is required");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                new Uri(origin).Host.Equals(allowedCorsHost, StringComparison.OrdinalIgnoreCase))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

// Enable CORS - add this BEFORE UseAuthorization
app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();

app.Run();
