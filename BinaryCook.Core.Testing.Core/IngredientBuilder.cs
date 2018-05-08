using System;
using BinaryCook.Core.Domain.Model.Ingredients;
using BinaryCook.Core.Domain.Model.Recipes;

namespace BinaryCook.Core.Testing.Core
{
	public class IngredientBuilder : IBuilder<Ingredient>
	{
		private string _name;

		public IngredientBuilder()
		{
			WithName(Guid.NewGuid().ToString());
		}

		public IngredientBuilder WithName(string name)
		{
			_name = name;
			return this;
		}


		public Ingredient Build() => new Ingredient(_name);
	}
}