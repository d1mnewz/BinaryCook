using System;
using System.Linq;
using BinaryCook.Core.Testing.Core;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Core.Domain.Tests.Model.Recipes
{
	public class RecipeShould
	{
		private RecipeBuilder GetBuilder()
		{
			return new RecipeBuilder();
		}

		[Fact]
		public void AddIngredients()
		{
			var recipe = GetBuilder()
				.WithName(nameof(AddIngredients))
				.Build();
			var ingredients = Enumerable.Range(0, 10).Select(x => new IngredientBuilder().WithName("Test" + x)).Select(x => x.Build()).ToArray();
			recipe.Ingredients.Should().BeEmpty();
			recipe.AddIngredients(ingredients);
			recipe.Ingredients.Should().HaveCount(ingredients.Length);
			recipe.Ingredients.Should().Contain(ingredients.Select(x => x.Id));
		}

		[Fact]
		public void RemoveIngredients()
		{
			var recipe = GetBuilder()
				.WithName(nameof(RemoveIngredients))
				.Build();

			var ingredients = Enumerable.Range(0, 10).Select(x => new IngredientBuilder().WithName("Test" + x)).Select(x => x.Build()).ToArray();
			const int removeCount = 5;
			recipe.AddIngredients(ingredients);
			recipe.Ingredients.Should().HaveCount(ingredients.Length);
			recipe.RemoveIngredients(ingredients.Take(removeCount).ToArray());
			recipe.Ingredients.Should().HaveCount(ingredients.Length - removeCount);
			recipe.Ingredients.Should().Contain(ingredients.Skip(removeCount).Take(removeCount).Select(x => x.Id));
			recipe.RemoveIngredients(ingredients.Skip(removeCount).Take(removeCount).ToArray());
			recipe.Ingredients.Should().BeEmpty();
		}

		[Fact]
		public void Creates()
		{
			const string name = "NewName";
			const string author = "NewAuthor";

			var recipe = GetBuilder()
				.WithName(name)
				.WithAuthor(author)
				.Build();

			recipe.Name.Should().Be(name);
			recipe.Author.Should().Be(author);
		}

		[Fact]
		public void Update()
		{
			const string name = "NewName";
			const string author = "NewAuthor";

			var recipe = GetBuilder()
				.WithName(nameof(Update))
				.Build();

			recipe.Name.Should().NotBe(name);
			recipe.Author.Should().NotBe(author);

			recipe.Update(name: name, author: author);

			recipe.Name.Should().Be(name);
			recipe.Author.Should().Be(author);

			Assert.Throws<ArgumentException>(() => recipe.Update(name: ""));
			Assert.Throws<ArgumentException>(() => recipe.Update(author: ""));
		}

		[Fact]
		public void Throws()
		{
			Assert.Throws<ArgumentNullException>(() => GetBuilder().WithName(null).Build());
			Assert.Throws<ArgumentException>(() => GetBuilder().WithName("").Build());
			Assert.Throws<ArgumentNullException>(() => GetBuilder().WithAuthor(null).Build());
			Assert.Throws<ArgumentException>(() => GetBuilder().WithAuthor("").Build());
		}
	}
}