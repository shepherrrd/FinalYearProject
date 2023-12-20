using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var env = builder.Environment;
var parentPath = Directory.GetParent(env.ContentRootPath)?.FullName ?? "";
builder.Configuration
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

builder.Configuration.AddEnvironmentVariables();

builder.Services.RegisterPersistence(builder.Configuration);
builder.Services.RegisterIdentity();
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
