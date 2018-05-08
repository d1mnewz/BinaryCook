﻿using System.Linq;
using Autofac;
using BinaryCook.Application.Core.Validations;
using BinaryCook.Core.Messaging;
using BinaryCook.Infrastructure.AutoMapper;
using BinaryCook.Infrastructure.AutoMapper.Mapping;
using BinaryCook.Infrastructure.IoC;

namespace BinaryCook.Application.DI.Modules
{
	public class CommonModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Bus>().As<IBus>();
			builder.RegisterGeneric(typeof(Mapper<,>)).As(typeof(IMapper<,>))
				.SingleInstance();

			var assemblies = AssemblyFinder.FindAssembliesFromCurrentDomain(x => x.FullName.Contains("BinaryCook")).ToArray();

			builder.RegisterAssemblyTypes(assemblies)
				.AsClosedTypesOf(typeof(IValidator<>))
				.AsImplementedInterfaces();

			builder.RegisterAssemblyTypes(assemblies)
				.AssignableTo<IMapping>()
				.As<IMapping>();
		}
	}
}