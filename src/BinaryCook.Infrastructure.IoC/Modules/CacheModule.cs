using System.Collections.Generic;
using System.Linq;
using Autofac;
using BinaryCook.Core.Code;
using BinaryCook.Core.Extensions;
using BinaryCook.Infrastructure.Caching;
using BinaryCook.Infrastructure.Caching.Configurations;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace BinaryCook.Infrastructure.IoC.Modules
{
    public class CacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AssemblyFinder.FindAssembliesFromCurrentDomain(x => x.FullName.Contains("BinaryCook")).ToArray();

            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<ICacheConfiguration>()
                .As<ICacheConfiguration>();

            builder.Register(CreateSerializer)
                .As<ISerializer>()
                .SingleInstance();

            builder.Register(CreateCacheManager)
                .Keyed<ICacheManager>("default-cache")
                .SingleInstance();

            builder.RegisterDecorator<ICacheManager>((ctx, inner) => new RetryAwareCacheManager(inner, 2, 3), "default-cache");
        }

        private static ISerializer CreateSerializer(IComponentContext ctx)
        {
            ProtobufSerializer.Initialize(ctx.Resolve<IEnumerable<ICacheConfiguration>>().ToList());
            return new ProtobufSerializer();
        }

        private static ICacheManager CreateCacheManager(IComponentContext ctx)
        {
            var connectionString = ctx.Resolve<IConfiguration>().GetValue<string>("redis");
            Ensure.That(!connectionString.IsEmptyString(), $"Redis connection string is empty");
            var cacheManager = new RedisCacheManager(ConnectionMultiplexer.Connect(connectionString), ctx.Resolve<ISerializer>());
            return cacheManager;
        }
    }
}