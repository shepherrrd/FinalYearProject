using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using FinalYearProject.Infrastructure.Data.Models;

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
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterApplication();
builder.Services.RegisterCors();
builder.Services.RegisterAuthorization();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();

        var result = new ValidationResultModel
        {
            Status = false,
            Message = "Some Errors were found ",
            Errors = errors
        };

        return new BadRequestObjectResult(result);
    };
}); ;
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
app.UseCors("MyCorsPolicy");
app.MapControllers();

app.Run();
