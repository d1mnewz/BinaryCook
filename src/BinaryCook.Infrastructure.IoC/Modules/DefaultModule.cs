using Autofac;
using BinaryCook.Core;

namespace BinaryCook.Infrastructure.IoC.Modules
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Clock>().As<IClock>();
        }
    }
}