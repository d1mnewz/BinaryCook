using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BinaryCook.API.Infrastructure.Extensions
{
	public static class DocumentationExtensions
	{
		private class LowercaseDocumentFilter : IDocumentFilter
		{
			public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
			{
				var paths = swaggerDoc.Paths;

				//	generate the new keys
				var newPaths = new Dictionary<string, PathItem>();
				var removeKeys = new List<string>();
				foreach (var path in paths)
				{
					var newKey = path.Key.ToLower();
					if (newKey == path.Key) continue;

					removeKeys.Add(path.Key);
					newPaths.Add(newKey, path.Value);
				}

				//	add the new keys
				foreach (var path in newPaths)
					swaggerDoc.Paths.Add(path.Key, path.Value);

				//	remove the old keys
				foreach (var key in removeKeys)
					swaggerDoc.Paths.Remove(key);
			}
		}

		private static string Humanize(this string value) => value.Replace(".", " ");

		public static IServiceCollection AddDocumentation<T>(this IServiceCollection services, IHostingEnvironment env)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Title = env.ApplicationName.Humanize(),
					Version = "v1"
				});
				c.DocumentFilter<LowercaseDocumentFilter>();
				var xml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{env.ApplicationName}.xml");
				if (File.Exists(xml))
					c.IncludeXmlComments(xml);
				c.MapType<DateTime>(() =>
					new Schema
					{
						Type = "string",
						Format = "yyyy-MM-ddTHH:mm:ss",
						Example = DateTime.UtcNow
					}
				);
			});
			return services;
		}

		public static IApplicationBuilder AddDocumentation<T>(this IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseSwagger(c => { });
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName.Humanize());
				c.SupportedSubmitMethods();
			});
			return app;
		}
	}
}