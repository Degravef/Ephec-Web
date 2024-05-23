using Bll.Config;
using Bll.Interfaces;
using Dal;
using Dal.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddBllServices();
builder.Services.AddDalServices();
var jwtConfig = new JwtConfig();
builder.Configuration.Bind("JwtConfig", jwtConfig);
builder.Services.AddSingleton<IJwtConfig>(jwtConfig);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
