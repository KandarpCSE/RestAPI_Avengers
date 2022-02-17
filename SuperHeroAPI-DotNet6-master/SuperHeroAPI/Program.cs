global using SuperHeroAPI.Data;
global using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Middleware;
using SuperHeroAPI.Repositories;
using SuperHeroAPI.optionClass;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new GroupingByNamespaceConvention());
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    var titleBase = "Avengers API";
    var description = "This is a Web API for Avengers Hero";
    var License = new OpenApiLicense()
    {
        Name = "Kandarp"
    };
    var Contact = new OpenApiContact()
    {
        Name = "Kandarp Patel",
        Email = "kandarp.patel@marutitech.com",
    };

    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = titleBase + " v1",
        Description = description,
        License = License,
        Contact = Contact
    });

    config.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = titleBase + " v2",
        Description = description,
        License = License,
        Contact = Contact
    });
});

builder.Services.AddControllers().AddNewtonsoftJson();
// saving key value of third party API

IConfigurationSection sec = builder.Configuration.GetSection("MySettings");
builder.Services.Configure<SuperHeroOption>(sec);

builder.Services.AddScoped<ISuperHeroRepo, SuperHeroRepo>();

//Serilog Configuration



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "AvengersAPI v1");
        config.SwaggerEndpoint("/swagger/v2/swagger.json", "AvengersAPI v2");
    });
    
}
app.UseRequestResponseLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
