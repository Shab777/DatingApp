using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// To avoid getting nasty data on ang. client site. we need to add cors in services.

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowCredentials()
           .AllowAnyHeader()
           .WithOrigins("https://localhost:4200");
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://locoalhost:4200"));

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
