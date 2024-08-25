using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using CRUD.Interfaces;

namespace CRUD
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            // Register all derived classes of BaseService
            services.Scan(scan => scan
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.AssignableTo(typeof(BaseService<,,,,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
            );

            // Register all derived classes of BaseRepository
            services.Scan(scan => scan
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.AssignableTo(typeof(BaseRepository<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
            );

            // 添加泛型服务
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<,,,,>), typeof(BaseService<,,,,>));

            return services;
        }
    }
}
