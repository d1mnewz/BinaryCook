using AutoMapper;
using BinaryCook.Infrastructure.AutoMapper.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryCook.Infrastructure.AutoMapper
{
    public static class AutoMapperInitializer
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    foreach (var mapping in sp.GetServices<IMapping>()){
                        mapping.Create(cfg);
                    }
                });
                return config.CreateMapper(sp.GetService);
            });
        }
    }
}