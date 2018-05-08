using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BinaryCook.Application.DI.Modules;
using BinaryCook.Application.Web.Mvc.Middlewares;
using BinaryCook.API.Infrastructure.Extensions;
using BinaryCook.API.Infrastructure.IoC;
using BinaryCook.Infrastructure.AutoMapper;
using BinaryCook.Infrastructure.Core.Data;
using BinaryCook.Infrastructure.Core.Data.Extensions;
using BinaryCook.Infrastructure.IoC;
using BinaryCook.Infrastructure.IoC.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryCook.API
{
public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = CreateBuilder();

            services.AddMvc(opts =>
                {
                    //var policy = new AuthorizationPolicyBuilder()
                    //    .RequireAuthenticatedUser()
                    //    .Build();
                    //opts.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddControllersAsServices()
                .AddTagHelpersAsServices()
                .AddViewComponentsAsServices();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddDocumentation<Startup>(Environment);

            services.AddSqlServerDbContext<BinaryCookDbContext>();

            //services.AddBinaryCookIdentity();
            
            services.AddAutoMapper();

            builder.Populate(services);

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            if (Environment.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                RequestCultureProviders = new IRequestCultureProvider[]
                {
                    new AcceptLanguageHeaderRequestCultureProvider(),
                    new QueryStringRequestCultureProvider()
                }
            });
            app.AddDocumentation<Startup>(Environment);
            app.UseMiddleware<InitializerMiddleware>();
            app.UseMvcWithDefaultRoute();

            appLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }

        protected virtual IoCBuilder CreateBuilder() => new IoCBuilder()
            .Add(new CacheModule())
            .Add(new CommonModule())
            .Add(new CommandModule())
            .Add(new QueryModule())
            .Add(new ApiModule());

       
    }
}