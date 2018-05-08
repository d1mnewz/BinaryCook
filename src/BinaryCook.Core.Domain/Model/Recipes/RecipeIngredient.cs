using System;
using System.Collections.Generic;
using BinaryCook.Core.Code;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Extensions;
using BinaryCook.Core.Domain.Model.Ingredients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Domain.Model.Recipes
{
	public class RecipeIngredient : IUnremovable
	{
		public RecipeIngredient(Ingredient ingredient, Recipe recipe)
		{
			Requires.NotNull(ingredient, nameof(ingredient));
			Requires.NotNull(recipe, nameof(recipe));

			IngredientId = ingredient.Id;
			RecipeId = recipe.Id;
		}

		private RecipeIngredient()
		{
		}

		public Guid IngredientId { get; private set; }
		public Guid RecipeId { get; private set; }

		public bool IsDeleted { get; private set; }

		[System.ComponentModel.DataAnnotations.Schema.NotMapped]
		public IEnumerable<string> Types => null;

		public void Delete()
		{
			IsDeleted = true;
		}
	}

	public class RecipeIngredientEntityConfiguration : EntityConfiguration<RecipeIngredient>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<RecipeIngredient> cfg)
		{
			cfg.HasKey(x => new { x.IngredientId, x.RecipeId });

			cfg.HasOne(typeof(Recipe))
				.WithMany("_ingredients")
				.HasForeignKey(nameof(RecipeIngredient.RecipeId));

			cfg.HasOne(typeof(Ingredient))
				.WithMany("_recipes")
				.HasForeignKey(nameof(RecipeIngredient.IngredientId));

			cfg.WithDefaultUnremovableFilter();

			base.Initialize(builder, cfg);
		}
	}
}