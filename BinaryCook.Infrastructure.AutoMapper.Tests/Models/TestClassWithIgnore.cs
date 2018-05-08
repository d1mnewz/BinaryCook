using System;
using AutoMapper;
using BinaryCook.Infrastructure.AutoMapper.Mapping;

namespace BinaryCook.Infrastructure.AutoMapper.Tests.Models
{
	public class TestClassWithIgnore
	{
		private TestClassWithIgnore()
		{
			Id = Guid.NewGuid();
		}

		public TestClassWithIgnore(string name) : this()
		{
			Name = name;
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
	}

	public class TestClassWithIgnoreModel : BiMapping<TestClassWithIgnoreModel, TestClassWithIgnore>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public override IMappingExpression<TestClassWithIgnoreModel, TestClassWithIgnore> CreateForwardMap(IMapperConfigurationExpression cfg = null)
		{
			return base.CreateForwardMap(cfg).ForMember(x => x.Id, x => x.Ignore());
		}
	}

	public class TestClassWithIgnoreModelMapping : Mapping<TestClassWithIgnoreModel, TestClassWithIgnore>
	{
		public override IMappingExpression<TestClassWithIgnoreModel, TestClassWithIgnore> CreateMap(IMapperConfigurationExpression cfg)
		{
			return base.CreateMap(cfg).ForMember(x => x.Id, x => x.Ignore());
		}
	}
}