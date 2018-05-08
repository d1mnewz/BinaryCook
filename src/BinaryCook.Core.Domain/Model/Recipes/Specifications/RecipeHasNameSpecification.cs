﻿using System;
using System.Linq.Expressions;
using BinaryCook.Core.Code;
using BinaryCook.Core.Data.Specifications;

namespace BinaryCook.Core.Domain.Model.Recipes.Specifications
{
	public class RecipeHasNameSpecification : Specification<Recipe>
	{
		private readonly string _name;

		public RecipeHasNameSpecification(string name)
		{
			Requires.NotNull(name, nameof(name));
			_name = name;
		}

		public override Expression<Func<Recipe, bool>> Expression => category => _name == category.Name;
	}
}