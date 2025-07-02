var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddCarter();

//register mediatr services and register services from all assemblies and where to find the command and qeuery handler classes
//since mediastr is located in building blocks this now knows that it needs to regsiter and serach in this project
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});
builder.Services.AddMarten(options =>
{
    //connection string to the database
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//configure the hhtp request pipeline
app.MapCarter();

app.Run();
