﻿using System;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Ingredients;

namespace BinaryCook.Application.Services.Ingredients.Handlers
{
	public class IngredientQueryHandler : GenericQueryHandler<Ingredient, Guid>
	{
		public IngredientQueryHandler(IRead<Ingredient, Guid> repo) : base(repo)
		{
		}
	}
}