using System.Collections.Generic;
using BinaryCook.Core.Data;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Domain.Model.Ingredients;
using BinaryCook.Core.Domain.Model.Recipes;
using Microsoft.EntityFrameworkCore;

namespace BinaryCook.Infrastructure.Core.Data
{
	public class BinaryCookDbContext : Context
	{
		protected override string Schema => "main";

		protected override List<IEntityConfiguration> EntityConfigurations => new List<IEntityConfiguration>
		{
			#region Recipe

			new RecipeEntityConfiguration(),
			new RecipeIngredientEntityConfiguration(),
			new RecipeStepEntityConfiguration(),

			#endregion

			#region Ingredient

			new IngredientEntityConfiguration()

			#endregion
		};

		public BinaryCookDbContext(DbContextOptions<BinaryCookDbContext> options) : base(options)
		{
		}
	}
}