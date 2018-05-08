﻿using System;
using BinaryCook.Core;
using BinaryCook.Core.Data;
 using BinaryCook.Core.Domain.Model.Recipes;
using BinaryCook.Core.Domain.Repositories;

namespace BinaryCook.Infrastructure.Core.Data.Repositories.Impl
{
	public class RecipeRepository : Repository<Recipe, Guid>, IRecipeRepository
	{
		public RecipeRepository(Context context, ISession session) : base(context, session)
		{
		}
	}
}