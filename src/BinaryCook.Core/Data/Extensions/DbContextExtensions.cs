using System;
using BinaryCook.Core.Code;
using BinaryCook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryCook.Core.Data.Extensions
{
    public static class DbContextExtensions
    {
        private static string GetConnectionString<T>(IConfiguration configuration, string connectionStringName = null)
            where T : DbContext
        {
            var connstr = connectionStringName.IsEmptyString()
                ? configuration[typeof(T).Name]
                : configuration[connectionStringName];

            Ensure.That(!string.IsNullOrWhiteSpace(connstr), $"Could not find a connection string for {typeof(T).Name}.");
            return connstr;
        }

        public static IServiceCollection AddBinaryCookDbContext<T>(this IServiceCollection services,
            Action<DbContextOptionsBuilder<T>, IServiceProvider, string> optionsAction,
            string connectionString = null) where T : DbContext
        {
            services.AddDbContext<T>((sp, options) =>
            {
                connectionString = connectionString ?? GetConnectionString<T>(sp.GetService<IConfiguration>());
                optionsAction((DbContextOptionsBuilder<T>) options, sp, connectionString);
            });
            return services;
        }
    }
}