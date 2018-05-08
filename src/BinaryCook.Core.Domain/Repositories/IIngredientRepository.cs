﻿using System;
using BinaryCook.Core.Data.Repositories;
 using BinaryCook.Core.Domain.Model.Ingredients;

namespace BinaryCook.Core.Domain.Repositories
{
	public interface IIngredientRepository :
		IRead<Ingredient, Guid>,
		ISave<Ingredient, Guid>
	{
	}
}