using System.Linq;
using Autofac;
using Autofac.Core;
using BinaryCook.Application.Core.Aspects.Logging;
using BinaryCook.Core.Commands;
using BinaryCook.Infrastructure.IoC;

namespace BinaryCook.Application.DI.Modules
{
    public class QueryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AssemblyFinder.FindAssembliesFromCurrentDomain(x => x.FullName.Contains("BinaryCook.Application")).ToArray();

            builder.RegisterGenericDecorator(typeof(LoggingAwareQueryHandler<,>), typeof(IQueryHandler<,>), "query-handler");

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<,>)) && !t.IsGenericType)
                .As(t => t.GetInterfaces()
                    .Where(x => x.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                    .Select(x => new KeyedService("query-handler", x))
                );
        }
    }
}