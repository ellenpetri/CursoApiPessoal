using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Interface;
using ApiCatalogo.Logging;
using ApiCatalogo.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));
builder.Services.AddScoped<ApiLoggingFilter>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Adicionar provedor do log customizado
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration { LogLevel = LogLevel.Information }));

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

builder.Services.AddControllers(options => { options.Filters.Add(typeof(ApiExceptionFilter)); })
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();