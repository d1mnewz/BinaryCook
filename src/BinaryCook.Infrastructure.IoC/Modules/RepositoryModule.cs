using Autofac;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Infrastructure.Caching;
using BinaryCook.Infrastructure.Core.Data.Repositories;
using BinaryCook.Infrastructure.IoC.Modules.Helpers;

namespace BinaryCook.Infrastructure.IoC.Modules
{
	public class RepositoryModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// TODO: register repositories
 
			builder.RegisterGenericDecorator(typeof(CachedReadRepository<,>), typeof(IRead<,>), RepositoryRegistrationKeys.CachedRead);
			builder.RegisterGenericDecorator(typeof(SaveEventPublisher<,>), typeof(ISave<,>), RepositoryRegistrationKeys.Save);
			builder.RegisterGenericDecorator(typeof(DeleteEventPublisher<,>), typeof(IDelete<,>), RepositoryRegistrationKeys.Delete);
		}
	}
}