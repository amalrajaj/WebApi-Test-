
using WebApiProject.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterApplicationServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var app = builder.Build();
app.ConfigureMiddleware();
// Configure the HTTP request pipeline.
app.Run();

 