using System;
using System.Threading.Tasks;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Testing.Core;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BinaryCook.Infrastructure.Caching.Tests
{
    public class CachedReadRepositoryShould : TestBase
    {
        private readonly IRead<TestAggregateRoot, Guid> _readRepo;
        private readonly IRead<TestAggregateRoot, Guid> _cachedRepo;
        private readonly ICacheManager _cacheManager;

        public CachedReadRepositoryShould()
        {
            _readRepo = Substitute.For<IRead<TestAggregateRoot, Guid>>();
            _cacheManager = new MemoryCacheManager(TimeSpan.FromMinutes(1));
            _cachedRepo = new CachedReadRepository<TestAggregateRoot, Guid>(_readRepo, _cacheManager);
        }

        [Fact]
        public async Task GetById()
        {
            var stub = new TestAggregateRoot("test");
            _readRepo.GetAsync(stub.Id).Returns(stub);

            var result = await _cachedRepo.GetAsync(stub.Id);
            
            result.Should().Be(stub);
            await _readRepo.Received(1).GetAsync(stub.Id);

            result = await _cachedRepo.GetAsync(stub.Id);
            result.Should().Be(stub);

            await _readRepo.Received(1).GetAsync(stub.Id);
        }
    }
}