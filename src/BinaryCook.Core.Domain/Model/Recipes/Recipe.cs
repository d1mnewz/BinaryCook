using System;
using System.Collections.Generic;
using System.Linq;
using BinaryCook.Core.Code;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Extensions;
using BinaryCook.Core.Domain.Model.Common;
using BinaryCook.Core.Domain.Model.Ingredients;
using BinaryCook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Domain.Model.Recipes
{
	public class Recipe : AggregateRoot
	{
		private ICollection<RecipeStep> _steps { get; set; } = new HashSet<RecipeStep>();
		private ICollection<RecipeIngredient> _ingredients { get; set; } = new HashSet<RecipeIngredient>();

		private Recipe()
		{
		}

		public Recipe(string name, string author) : this()
		{
			Requires.NotNull(name, nameof(name));
			Requires.NotNull(author, nameof(author));

			Update(name: name, author: author);
			WithImage(Image.None);
		}

		public string Name { get; protected set; }
		public string Description { get; protected set; }
		public string Author { get; protected set; }
		public Image Image { get; protected set; }

		public IEnumerable<Guid> Ingredients => _ingredients.NotDeleted().Select(x => x.IngredientId).AsEnumerable();
		public IEnumerable<RecipeStep> Steps => _steps.NotDeleted().AsEnumerable();

		public void WithImage(Image image)
		{
			Requires.NotNull(image, nameof(image));

			Image = image;
		}

		public void Update(
			Maybe<string> name = null,
			Maybe<string> description = null,
			Maybe<string> author = null)
		{
			name.Required()
				.NotEmpty(nameof(name))
				.Do(x => Name = x);

			author.Required()
				.NotEmpty(nameof(author))
				.Do(x => Author = x);

			description.Required()
				.NotEmpty(nameof(description))
				.Do(x => Description = x);
		}

		public void AddIngredients(params Ingredient[] ingredients)
		{
			Requires.NotNull(ingredients, nameof(ingredients));

			foreach (var ingredient in ingredients)
			{
				if (_ingredients.Any(x => x.IngredientId == ingredient.Id)) continue;

				_ingredients.Add(new RecipeIngredient(ingredient, this));
				//TODO: add event
			}
		}

		public void RemoveIngredients(params Ingredient[] ingredients)
		{
			Requires.NotNull(ingredients, nameof(ingredients));

			foreach (var ingredient in ingredients)
			{
				if (_ingredients.All(x => x.IngredientId != ingredient.Id)) continue;

				var existing = _ingredients.FirstOrDefault(x => x.IngredientId == ingredient.Id);
				existing.Delete();
				//TODO: add event
			}
		}

		public void AddSteps(params RecipeStep[] steps)
		{
			Requires.NotNull(steps, nameof(steps));

			foreach (var step in steps)
			{
				if (_steps.Any(x => x.Id == step.Id)) continue;

				_steps.Add(step);
				//TODO: add event
			}
		}

		public void RemoveSteps(params RecipeStep[] steps)
		{
			Requires.NotNull(steps, nameof(steps));

			foreach (var step in steps)
			{
				if (_steps.All(x => x.Id != step.Id)) continue;

				var existing = _steps.FirstOrDefault(x => x.Id == step.Id);
				existing.Delete();
				//TODO: add event
			}
		}
	}

	public class RecipeEntityConfiguration : EntityConfiguration<Recipe>
	{
		protected override void Initialize(ModelBuilder builder, EntityTypeBuilder<Recipe> cfg)
		{
			cfg.Property(x => x.Name).NVarChar(512);
			cfg.Property(x => x.Description).NVarChar(2048);
			cfg.Property(x => x.Author);


			cfg.OwnsOne(x => x.Image, ImageConfiguration.Configure);


			cfg.Ignore(x => x.Ingredients);
			cfg.Ignore(x => x.Steps);

			cfg.HasMany(typeof(RecipeStep), "_steps")
				.WithOne()
				.HasForeignKey("RecipeId");

			base.Initialize(builder, cfg);
		}
	}
}