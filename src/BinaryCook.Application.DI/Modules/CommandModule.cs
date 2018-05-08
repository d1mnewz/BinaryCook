using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using BinaryCook.Application.Core.Aspects.Logging;
using BinaryCook.Application.Core.Aspects.Validations;
using BinaryCook.Core.Commands;
using BinaryCook.Infrastructure.IoC;

namespace BinaryCook.Application.DI.Modules
{
	public class CommandModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var assemblies = AssemblyFinder.FindAssembliesFromCurrentDomain(x => x.FullName.Contains("BinaryCook.Application")).ToArray();

			//For(typeof(IHandleAsync<>)).DecorateAllWith(typeof(TransactionAwareHandler<>));          
			builder.RegisterGenericDecorator(typeof(LoggingAwareCommandHandler<>), typeof(ICommandHandler<>), "command-handler",
				"logging-command-handler-decorator");
			builder.RegisterGenericDecorator(typeof(ValidationAwareCommandHandler<>), typeof(ICommandHandler<>), "logging-command-handler-decorator");

			builder.RegisterAssemblyTypes(assemblies)
				.Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)) && !t.IsGenericType)
				.As(t => new KeyedService("command-handler", GetIHandleAsyncType(t)));
		}

		private static Type GetIHandleAsyncType(Type type) => type
			.GetInterfaces()
			.Single(i => i.IsClosedTypeOf(typeof(ICommandHandler<>)));
	}
}