﻿using BinaryCook.API.Models.Common;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Data.Specifications;
using BinaryCook.Core.Domain.Model.Recipes;

namespace BinaryCook.API.Models.Recipes
{
	public class RecipeFilterModel : FilterModel<Recipe>
	{
		/// <summary>
		///     Category name
		/// </summary>
		public string Name { get; set; }

		// TODO:
		public override ISorter<Recipe> ToSorter() => null;

		// TODO:
		public override ISpecification<Recipe> ToSpecification() => null;
	}
}