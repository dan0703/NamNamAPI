using NamNamAPI.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using NamNamAPI.Controllers;
using NamNamAPI.Business;
using NamNamAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MySqlConfiguration
builder.Services.AddDbContext<NamnamContext>(options =>
                options.UseMySql(
                    "server=nam-nam-bd.mysql.database.azure.com;database=namnam;uid=NamNamAdminBD;pwd=Azure2023", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"
                )));

//add controllers
//builder.Services.AddScoped<LoginProvider>();
builder.Services.AddScoped<TestProvider>();
builder.Services.AddScoped<CategoryProvider>();
builder.Services.AddScoped<IngredientProvider>();
builder.Services.AddScoped<RecipeProvider>();

builder.Services.AddScoped<InstructionProvider>();


builder.Services.AddScoped<ImageProvider>();

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
