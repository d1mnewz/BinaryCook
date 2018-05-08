﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinaryCook.Core.Commands;
using BinaryCook.Infrastructure.AutoMapper;

namespace BinaryCook.API.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static List<TTo> MapWith<TFrom, TTo>(this IQueryResult<TFrom> items, IMapper<TFrom, TTo> mapper) => items.Select(mapper.Map).ToList();

        public static async Task<List<TTo>> MapWith<TFrom, TTo>(this Task<IQueryResult<TFrom>> task, IMapper<TFrom, TTo> mapper)
        {
            var items = await task;
            return items.MapWith(mapper);
        }
    }
}