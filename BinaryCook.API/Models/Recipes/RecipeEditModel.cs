﻿using BinaryCook.API.Models.Common;
 using BinaryCook.Core.Domain.Model.Recipes;

namespace BinaryCook.API.Models.Recipes
{
	/// <summary>
	/// Model to create/edit <see cref="Recipe"/>
	/// </summary>
	public class RecipeEditModel : EditModel
	{
		/// <summary>
		///     Recipe name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Recipe Description
		/// </summary>
		public string Description { get; set; }
	}
}