﻿using System;
using BinaryCook.API.Models.Common;
 using BinaryCook.Core.Domain.Model.Recipes;
using BinaryCook.Infrastructure.AutoMapper.Mapping;

namespace BinaryCook.API.Models.Recipes
{
	/// <summary>
	/// View model for <seealso cref="Recipe"/>
	/// </summary>
	public class RecipeViewModel : ViewModel<Guid>
	{
		/// <summary>
		/// Book Category name
		/// </summary>
		public string Name { get; set; }
	}

	/// <summary>
	/// Mapping configuration for <see cref="RecipeViewModel"/>>
	/// </summary>
	public class RecipeMapping : Mapping<Recipe, RecipeViewModel>
	{
	}
}