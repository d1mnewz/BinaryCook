﻿using System;
using BinaryCook.Core.Data.Repositories;
 using BinaryCook.Core.Domain.Model.Recipes;

namespace BinaryCook.Core.Domain.Repositories
{
	public interface IRecipeRepository :
		IRead<Recipe, Guid>,
		ISave<Recipe, Guid>
	{
	}
}