using FinalYearProject.Infrastructure.Infrastructure.Auth;
using FinalYearProject.Infrastructure.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using FinalYearProject.Infrastructure.Data.Models;
using FinalYearProject.Infrastructure.Infrastructure.Swagger;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var env = builder.Environment;
var parentPath = Directory.GetParent(env.ContentRootPath)?.FullName ?? "";
builder.Configuration
    .AddJsonFile(Path.Combine(parentPath, "sharedsettings.json"), true)
    .AddJsonFile("sharedsettings.json", true)
    .AddJsonFile("appsettings.json", true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

builder.Configuration.AddEnvironmentVariables();


builder.Services.RegisterApplication();
builder.Services.RegisterPersistence(builder.Configuration);
builder.Services.RegisterIdentity();
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterAuthorization();
builder.Services.RegisterJwt(builder.Configuration);
builder.Services.RegisterSwagger();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddAuthorization();
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
    {
        app.UseSwaggerService();
    });
}
app.UseHttpsRedirection();
app.UseCors("MyCorsPolicy");
app.UseStaticFiles();
app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
