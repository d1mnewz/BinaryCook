using System;
using System.Collections.Generic;
using BinaryCook.Infrastructure.AutoMapper.Mapping;
using BinaryCook.Infrastructure.AutoMapper.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BinaryCook.Infrastructure.AutoMapper.Tests
{
    public class AutoMapperFixture : IDisposable
    {
        public readonly IServiceCollection Services;
        private readonly IServiceProvider _serviceProvider;
        public const string CollectionDefinition = "AutoMapper";

        private readonly IDictionary<Type, IMapping> _mappings = new Dictionary<Type, IMapping>
        {
            {typeof(TestClassBiMappedModel), new TestClassBiMappedModel()},
            {typeof(TestClassWithIgnoreModel), new TestClassWithIgnoreModel()},
        };

        public AutoMapperFixture()
        {
            Services = new ServiceCollection();

            foreach (var mapping in _mappings)
                Services.AddSingleton(mapping.Key, mapping.Value);

            Services.AddAutoMapper();
            Services.AddSingleton(typeof(IMapper<,>), typeof(Mapper<,>));

            _serviceProvider = Services.BuildServiceProvider();
        }

        public IMapper<TFrom, TTo> Mapper<TFrom, TTo>() => _serviceProvider.GetService<IMapper<TFrom, TTo>>();

        public void Dispose()
        {
        }
    }

    [CollectionDefinition(AutoMapperFixture.CollectionDefinition)]
    public class AutoMapperCollectionFixture : ICollectionFixture<AutoMapperFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}