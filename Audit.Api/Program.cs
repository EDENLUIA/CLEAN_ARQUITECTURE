using Audit.Application;
using Audit.Core.Interfaces;
using Audit.Infrastructure;
using Audit.Infrastructure.Repositories;
using Audit.Infrastructure.ServiceExtension;
using Microsoft.EntityFrameworkCore;
using Nest;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
//var connectionString = builder.Configuration.GetConnectionString("MyAppDB");

//builder.Services.AddDbContext<DbContextClass>(options => options.UseSqlServer(connectionString));


var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


var configuration = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
      .Build();

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
        .BasicAuthentication("elastic", "fisuncp") 
        .DefaultIndex("permissions");
    return new ElasticClient(settings);
});

builder.Services.AddDIServices(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
