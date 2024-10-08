using CRUD;
using CRUD.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tiny.Data;
using Scrutor;
using Tiny.Services;
using Tiny.Repositories;
using Tiny.DTOs;


var builder = WebApplication.CreateBuilder(args);

// 添加 DbContext 配置，使用 SQLite
builder.Services.AddDbContext<TinyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 添加 AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 将 DbContext 映射到 TinyDbContext
builder.Services.AddScoped<DbContext, TinyDbContext>();

// Add CRUD services
builder.Services.AddCrudServices();

// 注册 JsonFileService 服务


// builder.Services.AddSingleton<IJsonFileService>(provider =>
// {
//     // 初始化服务并传入JSON文件路径
//     return new JsonFileService<JsonClass>("settings.json");

// });

builder.Services.AddSingleton<IJsonFileService<IotConfig>>(sp =>
      new JsonFileService<IotConfig>("iot_config.json"));


// 添加控制器
builder.Services.AddControllers().AddNewtonsoftJson();

        

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();