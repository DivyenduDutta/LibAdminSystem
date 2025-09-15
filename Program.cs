using LibAdminSystem.Context;
using LibAdminSystem.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<LibraryContext>(options =>
            options.UseMySql(
                context.Configuration.GetConnectionString("LibAdminSystem"),
                ServerVersion.AutoDetect(context.Configuration.GetConnectionString("LibAdminSystem"))
            ));
    })
    .Build();

using var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

SeedData.Initialize(context);


/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run(); */
