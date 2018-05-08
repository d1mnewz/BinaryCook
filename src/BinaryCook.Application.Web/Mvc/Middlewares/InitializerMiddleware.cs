using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BinaryCook.Core;
using BinaryCook.Core.Code;
using BinaryCook.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BinaryCook.Application.Web.Mvc.Middlewares
{
    public class InitializerMiddleware
    {
        private readonly RequestDelegate _next;
        private static bool _initialized;
        private static readonly SemaphoreSlim Lock = new SemaphoreSlim(1);

        public InitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IClock clock, IList<IServiceInitializer> initializers, ILogger<InitializerMiddleware> logger)
        {
            if (!_initialized)
            {
                try
                {
                    await Lock.WaitAsync(TimeSpan.FromMinutes(1));
                    var globalSw = Stopwatch.StartNew();
                    Ensure.That(!_initialized, "Initializers already executed");
                    logger.LogTrace($"{GetType().Name} started at [{clock.UtcNow}] in {globalSw.ElapsedMilliseconds}ms");

                    foreach (var initializer in initializers)
                    {
                        logger.LogTrace($"{initializer.GetType().Name} started at [{clock.UtcNow}]");
                        var sw = Stopwatch.StartNew();
                        try
                        {
                            await initializer.InitializeAsync();
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, $"{initializer.GetType().Name} failed at [{clock.UtcNow}]");
                        }
                        sw.Stop();
                        logger.LogTrace($"{initializer.GetType().Name} initialized at [{clock.UtcNow}] in {sw.ElapsedMilliseconds}ms");
                    }

                    globalSw.Stop();
                    logger.LogTrace($"{GetType().Name} finished at [{clock.UtcNow}] in {globalSw.ElapsedMilliseconds}ms");
                }
                catch (Exception e)
                {
                    logger.LogCritical(e, $"{GetType().Name} failed at [{clock.UtcNow}]");
                }
                finally
                {
                    _initialized = true;
                    Lock.Release();
                }
            }

            await _next(context);
        }
    }
}