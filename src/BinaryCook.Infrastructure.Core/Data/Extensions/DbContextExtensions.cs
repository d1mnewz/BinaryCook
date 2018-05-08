using System;
using System.Collections.Generic;
using BinaryCook.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryCook.Infrastructure.Core.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddSqlServerDbContext<T>(this IServiceCollection services, string connectionString = null) where T : DbContext
        {
            services.AddBinaryCookDbContext<T>((options, sp, cs) =>
            {
                options.UseSqlServer(cs,
                    x => x.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: new List<int>())
                );
                options.EnableSensitiveDataLogging(true);
            }, connectionString);
            return services;
        }
    }
}