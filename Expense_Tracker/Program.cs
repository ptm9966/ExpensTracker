using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(GetDevConnectionString(builder.Configuration)));

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2VVhhQlFac1pJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkNjWn9edHNRRmZYWEM=");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

// Add health check endpoints
app.MapGet("/healthz", () => Results.Ok("Healthy"));
// Refine /ready endpoint to ensure proper database connection check
app.MapGet("/ready", async (IServiceProvider services) =>
{
    try
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var canConnect = await dbContext.Database.CanConnectAsync();
        if (!canConnect)
        {
            return Results.StatusCode(503); // Service Unavailable
        }
        return Results.Ok("Ready");
    }
    catch (Exception ex)
    {
        // Log the exception if needed
        return Results.StatusCode(503); // Service Unavailable
    }
});
app.MapMetrics("/metrics");

app.Run();

static string GetDevConnectionString(IConfiguration configuration)
{
    var directConnectionString = configuration.GetConnectionString("DevConnection");
    if (!string.IsNullOrWhiteSpace(directConnectionString))
    {
        return directConnectionString;
    }

    var host = configuration["SqlServer:Host"];
    var database = configuration["SqlServer:Database"];
    var user = configuration["SqlServer:User"];
    var password = configuration["SqlServer:Password"];

    if (string.IsNullOrWhiteSpace(host) ||
        string.IsNullOrWhiteSpace(database) ||
        string.IsNullOrWhiteSpace(user) ||
        string.IsNullOrWhiteSpace(password))
    {
        throw new InvalidOperationException(
            "Database configuration is missing. Set ConnectionStrings__DevConnection or SqlServer__Host, SqlServer__Database, SqlServer__User, and SqlServer__Password.");
    }

    var port = configuration["SqlServer:Port"];
    var encrypt = bool.TryParse(configuration["SqlServer:Encrypt"], out var encryptValue) && encryptValue;
    var trustServerCertificate = !bool.TryParse(configuration["SqlServer:TrustServerCertificate"], out var trustValue) || trustValue;
    var multipleActiveResultSets = bool.TryParse(configuration["SqlServer:MultipleActiveResultSets"], out var marsValue) && marsValue;
    var timeoutSeconds = int.TryParse(configuration["SqlServer:ConnectionTimeout"], out var timeoutValue) ? timeoutValue : 30;

    var builder = new SqlConnectionStringBuilder
    {
        DataSource = string.IsNullOrWhiteSpace(port) ? host : $"{host},{port}",
        InitialCatalog = database,
        UserID = user,
        Password = password,
        Encrypt = encrypt,
        TrustServerCertificate = trustServerCertificate,
        MultipleActiveResultSets = multipleActiveResultSets,
        ConnectTimeout = timeoutSeconds
    };

    return builder.ConnectionString;
}
