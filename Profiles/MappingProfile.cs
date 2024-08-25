using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Tiny.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 使用反射自动扫描并配置所有的映射
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            var maps = (from type in types
                        from instance in type.GetInterfaces()
                        where instance.IsGenericType && instance.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                        select new
                        {
                            Source = instance.GetGenericArguments()[0],
                            Destination = type
                        }).ToList();

            foreach (var map in maps)
            {
                CreateMap(map.Source, map.Destination);
                CreateMap(map.Destination, map.Source);
            }
        }
    }
}
