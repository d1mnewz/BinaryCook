﻿using System;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Recipes;

namespace BinaryCook.Application.Services.Recipes.Handlers
{
	public class RecipeQueryHandler : GenericQueryHandler<Recipe, Guid>
	{
		public RecipeQueryHandler(IRead<Recipe, Guid> repo) : base(repo)
		{
		}
	}
}