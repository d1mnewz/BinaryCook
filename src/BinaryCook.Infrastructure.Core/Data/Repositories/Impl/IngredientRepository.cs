﻿using System;
using BinaryCook.Core;
using BinaryCook.Core.Data;
 using BinaryCook.Core.Domain.Model.Ingredients;
using BinaryCook.Core.Domain.Repositories;

namespace BinaryCook.Infrastructure.Core.Data.Repositories.Impl
{
	public class IngredientRepository : Repository<Ingredient, Guid>, IIngredientRepository
	{
		public IngredientRepository(Context context, ISession session) : base(context, session)
		{
		}
	}
}