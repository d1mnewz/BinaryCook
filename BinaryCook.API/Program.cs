using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using BinaryCook.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BinaryCook.API
{

	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.UseShutdownTimeout(TimeSpan.FromSeconds(15))
				.ConfigureAppConfiguration(Configure)
				.ConfigureLogging(Configure)
				.Build();

		//This method creates configuration
		private static void Configure(WebHostBuilderContext ctx, IConfigurationBuilder builder)
		{
			var env = ctx.HostingEnvironment;
			builder.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
			builder.AddEnvironmentVariables();

			var conf = builder.Build();
			if (conf.GetValue("KeyVault:IsEnabled", false))
				builder.AddAzureKeyVault(
					$"https://{conf["KeyVault:Vault"]}.vault.azure.net/",
					conf["KeyVault:AppId"],
					new CertificateHelper(StoreName.My, StoreLocation.CurrentUser).GetByType(X509FindType.FindByThumbprint,
						conf["KeyVault:Certificate:ThumbPrint"])
				);
		}

		private static void Configure(WebHostBuilderContext ctx, ILoggingBuilder builder)
		{
			builder.ClearProviders();
			var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(ctx.Configuration);
			builder.AddSerilog(logger.CreateLogger());
		}
	}
}