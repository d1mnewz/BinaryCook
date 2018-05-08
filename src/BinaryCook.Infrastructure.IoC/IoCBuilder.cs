using Autofac;
using Autofac.Core;
using BinaryCook.Infrastructure.IoC.Modules;

namespace BinaryCook.Infrastructure.IoC
{
    public class IoCBuilder : ContainerBuilder
    {
        public IoCBuilder()
        {
            Add(new DefaultModule());
            Add(new RepositoryModule());
        }

        public IoCBuilder Add(IModule module)
        {
            this.RegisterModule(module);
            return this;
        }

        public IoCBuilder Add<T>() where T : IModule, new()
        {
            this.RegisterModule<T>();
            return this;
        }
    }
}