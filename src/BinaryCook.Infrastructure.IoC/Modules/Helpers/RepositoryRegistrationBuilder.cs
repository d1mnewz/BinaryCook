using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using BinaryCook.Core.Data;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Infrastructure.Core.Data.Repositories;

namespace BinaryCook.Infrastructure.IoC.Modules.Helpers
{
    internal static class RepositoryRegistrationKeys
    {
        public const string CachedRead = "cached-by-id-read";
        public const string Save = "save-repository";
        public const string Delete = "delete-repository";
    }

    internal class RepositoryRegistrationBuilder<TRoot, TId> where TRoot : class, IAggregateRoot<TId>
    {
        private Type _contextType;
        private readonly ContainerBuilder _builder;
        private readonly Dictionary<string, Service> _implementations = new Dictionary<string, Service>();
        private Type _implementation;

        public RepositoryRegistrationBuilder(ContainerBuilder builder)
        {
            _implementation = typeof(Repository<TRoot, TId>);
            _builder = builder;
        }

        public RepositoryRegistrationBuilder<TRoot, TId> ImplementsRead()
        {
            _implementations["read"] = new TypedService(typeof(IRead<TRoot, TId>));
            return this;
        }

        public RepositoryRegistrationBuilder<TRoot, TId> ImplementsSave()
        {
            _implementations["save"] = new KeyedService(RepositoryRegistrationKeys.Save, typeof(ISave<TRoot, TId>));
            return this;
        }

        public RepositoryRegistrationBuilder<TRoot, TId> ImplementsDelete()
        {
            _implementations["delete"] = new KeyedService(RepositoryRegistrationKeys.Delete, typeof(IDelete<TRoot, TId>));
            return this;
        }

        public RepositoryRegistrationBuilder<TRoot, TId> WithContext<TContext>() where TContext : Context
        {
            _contextType = typeof(TContext);
            return this;
        }

        public RepositoryRegistrationBuilder<TRoot, TId> WithCachedRead(string key = RepositoryRegistrationKeys.CachedRead)
        {
            _implementations["read"] = new KeyedService(key, typeof(IRead<TRoot, TId>));
            return this;
        }

        public RepositoryRegistrationBuilder<TRoot, TId> WithImplementation<TImplementation>() where TImplementation : Repository<TRoot, TId>
        {
            _implementation = typeof(TImplementation);
            return this;
        }

        public void Register()
        {
            var result = _builder.RegisterType(_implementation)
                .As(_implementations.Values.ToArray());
            result.WithParameter((pi, ct) => pi.ParameterType == typeof(Context), (pi, ct) => ct.Resolve(_contextType));
            result.InstancePerDependency();
        }
    }
    
    internal static class RepositoryRegistrationBuilderExtensions
    {
        public static RepositoryRegistrationBuilder<TRoot, TId> ForRepository<TRoot, TId>(this ContainerBuilder builder)
            where TRoot : class, IAggregateRoot<TId> => new RepositoryRegistrationBuilder<TRoot, TId>(builder);
    }
}