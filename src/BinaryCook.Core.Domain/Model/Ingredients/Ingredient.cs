using System;
using System.Collections.Generic;
using System.Linq;
using BinaryCook.Core.Code;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Domain.Model.Recipes;
using BinaryCook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Domain.Model.Ingredients
{
	[Serializable]
	public class Ingredient : AggregateRoot
	{
		private ICollection<RecipeIngredient> _recipes { get; set; } = new HashSet<RecipeIngredient>();

		private Ingredient()
		{
		}

		public Ingredient(string name) : this()
		{
			Requires.NotNull(name, nameof(name));
			Update(name);
		}

		public string Name { get; private set; }

		public void Update(Maybe<string> name = null)
		{
			name.Required()
				.NotEmpty(nameof(name))
				.Do(x => Name = x);
		}

		public IEnumerable<Guid> Recipes => _recipes.Select(x => x.RecipeId).AsEnumerable();
	}


	public class IngredientEntityConfiguration : EntityConfiguration<Ingredient>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<Ingredient> cfg)
		{
			cfg.Property(x => x.Name);

			cfg.Ignore(x => x.Recipes);
//			cfg.HasMany(typeof(RecipeIngredient), "_recipes")
//				.WithOne()
//				.HasForeignKey(nameof(RecipeIngredient.IngredientId));

			base.Initialize(builder, cfg);
		}
	}
}