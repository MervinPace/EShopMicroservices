using System.Net.Mime;
using BuildingBlocks.ValidationBehaviours;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container

var assembly = typeof(Program).Assembly;
builder.Services.AddValidatorsFromAssembly(assembly);

//register mediatr services and register services from all assemblies and where to find the command and qeuery handler classes
//since mediastr is located in building blocks this now knows that it needs to regsiter and serach in this project
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>)); //register validator with generic type (,) for Mediatr Pipeline Pre and Post Processor
}); 

//this is used if we are to inject the IValidator in the handler class constructor and calling 
// var result = await validator.ValidateAsync(command, cancellationToken);
//we will use validation with Mediatr pipeline behaviour using Pre-Processer 
// builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    //connection string to the database
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//configure the hhtp request pipeline
app.MapCarter();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = MediaTypeNames.Text.Plain;

        await context.Response.WriteAsync("An exception was thrown.");

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
        }

        if (exceptionHandlerPathFeature?.Path == "/")
        {
            await context.Response.WriteAsync(" Page: Home.");
        }
    });
});

app.Run();