﻿using System.Linq;
using Autofac;
using BinaryCook.Core;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Services;
using BinaryCook.Infrastructure.IoC;
using Microsoft.AspNetCore.Http;
using ISession = BinaryCook.Core.ISession;

namespace BinaryCook.API.Infrastructure.IoC
{
	public class ApiModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			var assemblies = AssemblyFinder.FindAssembliesFromCurrentDomain(x => x.FullName.Contains("BinaryCook")).ToArray();

			builder.RegisterAssemblyTypes(assemblies)
				.AssignableTo<IServiceInitializer>()
				.As<IServiceInitializer>();

			builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>()
				.SingleInstance();
			builder.RegisterType<UserSession>().As<ISession>();
		}

		private class UserSession : ISession
		{
			private readonly IHttpContextAccessor _httpContextAccessor;

			public UserSession(IClock clock, IHttpContextAccessor httpContextAccessor)
			{
				_httpContextAccessor = httpContextAccessor;
				Clock = clock;
			}

			public ICorrelation Correlation => new Correlation(_httpContextAccessor.HttpContext.TraceIdentifier);

			// TODO:
			public IUser User
			{
				get
				{
					var user = _httpContextAccessor.HttpContext.User;
					return SystemUser.Unauthorized;
				}
			}

			public IClock Clock { get; }
		}
	}
}