using System.Text;
using API;
using API.Data;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddDbContext<DataContext>(opt =>
//             {
//                 opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
//             });

//  builder.Services.AddScoped<ITokenService, TokenServices>();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options => 
//  {
//     options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters 
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
//         ValidateIssuer = false,
//         ValidateAudience = false
//     };
//  });

// the above services- we have extended our own service application in extension folder

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
 


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
