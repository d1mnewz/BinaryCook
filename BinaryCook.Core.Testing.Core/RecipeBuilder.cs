using System;
using BinaryCook.Core.Domain.Model.Recipes;

namespace BinaryCook.Core.Testing.Core
{
	public class RecipeBuilder : IBuilder<Recipe>
	{
		private string _name;
		private string _author;

		public RecipeBuilder()
		{
			WithName(Guid.NewGuid().ToString());
			WithAuthor(Guid.NewGuid().ToString());
		}

		public RecipeBuilder WithName(string name)
		{
			_name = name;
			return this;
		}

		public RecipeBuilder WithAuthor(string author)
		{
			_author = author;
			return this;
		}


		public Recipe Build() => new Recipe(_name, _author);
	}
}