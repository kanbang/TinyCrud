using CRUD;
using CRUD.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tiny.Data;
using Scrutor;
using Tiny.Services;

var builder = WebApplication.CreateBuilder(args);

// 添加 DbContext 配置，使用 SQLite
builder.Services.AddDbContext<TinyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



// 添加 AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// builder.Services.AddScoped<IBaseRepository<Product>, BaseRepository<Product, TinyDbContext>>();

// 将 DbContext 映射到 TinyDbContext
builder.Services.AddScoped<DbContext, TinyDbContext>();  
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
// 添加泛型服务
builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));


// 使用 Scrutor 自动注入派生自 BaseService 的类
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ProductService>()
    .AddClasses(classes => classes.AssignableTo(typeof(BaseService<,>)))
    .AsSelfWithInterfaces()
    .WithScopedLifetime()
);

// 添加特定服务
// builder.Services.AddScoped<ProductService>();

// 添加控制器
builder.Services.AddControllers();


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