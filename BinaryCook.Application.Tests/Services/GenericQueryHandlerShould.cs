using System.Collections.Generic;
using System.Threading.Tasks;
using BinaryCook.Application.Core.Quering;
using BinaryCook.Application.Services;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Testing.Core;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BinaryCook.Application.Tests.Services
{
    public abstract class GenericQueryHandlerShould<TRoot, TId> : TestBase where TRoot : class, IAggregateRoot<TId>
    {
        protected readonly IRead<TRoot, TId> Read;
        protected abstract GenericQueryHandler<TRoot, TId> QueryHandler { get; }
        protected virtual IQueryHandler<TRoot, FindByIdQuery<TId>> FindByIdQueryHandler => QueryHandler;
        protected virtual IQueryHandler<TRoot, FindByIdsQuery<TId>> FindByIdsQueryHandler => QueryHandler;
        protected virtual IQueryHandler<TRoot, FindByFilterQuery<TRoot>> FindByFilterQueryHandler => QueryHandler;

        protected abstract IList<TRoot> Stubs { get; }
        //protected abstract IFilter<TRoot> Filter { get; }

        protected GenericQueryHandlerShould()
        {
            Read = Substitute.For<IRead<TRoot, TId>>();
        }

        [Fact]
        public async Task FindById()
        {
            var stub = Stubs[0];
            Read.GetAsync(stub.Id).Returns(stub);

            var query = new FindByIdQuery<TId>(stub.Id);

            var result = await FindByIdQueryHandler.FetchAsync(query);

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().Contain(stub);

            await Read.Received().GetAsync(Arg.Is(stub.Id));
        }

//        [Fact]
//        public async Task FindByIds()
//        {
//            var stubs = Stubs;
//
//            var specification = new FindByIdsSpecification<TRoot, TId>(stubs.Select(x => x.Id).ToList());
//            Read.GetAsync(specification).Returns(x => new PagedList<TRoot>(stubs, 1, Stubs.Count));
//
//            var query = new FindByIdsQuery<TId>(stubs.Select(x => x.Id).ToList());
//
//            var result = await FindByIdsQueryHandler.FetchAsync(query);
//
//            result.Should().NotBeNullOrEmpty();
//            result.Should().HaveCount(stubs.Count);
//            result.Should().Contain(stubs);
//
//            await Read.Received().GetAsync(Arg.Is(specification));
//        }
    }
}