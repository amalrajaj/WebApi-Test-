using DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<DemoDbContext>();

var valDbfromjson = builder.Configuration["ConnectionStrings:conString"];
builder.Services.AddDbContext<DemoDbContext>(options => {
    options.UseSqlServer(valDbfromjson);
});

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
