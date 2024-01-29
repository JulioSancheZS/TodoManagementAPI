using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoManagementAPI.Models;
using TodoManagementAPI.Repository.Contrato;
using TodoManagementAPI.Repository.Implementacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//Conexion Sql server
builder.Services.AddDbContext<TodoManagementApiContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("SqlConexion"));
});

//Repositorios
builder.Services.AddScoped<IAuthenticationRepository, AuthenticacionRepository>();
builder.Services.AddScoped<IEstadoTareaRepository, EstadoTareaRepository>();
builder.Services.AddScoped<ITareaRepositoty, TareaRepository>();

//Automapper
builder.Services.AddAutoMapper(typeof(TodoManagementAPI.Utilidades.AutoMapper));

//JWT
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.UseCors();
app.Run();
