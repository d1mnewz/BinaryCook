﻿using BinaryCook.Application.Core.Validations.Fluent;
using BinaryCook.Core.Commands;
using FluentValidation;

namespace BinaryCook.Application.Services.Ingredients.Contracts
{
	public class IngredientCreateCommand : Command
	{
		public string Name { get; }

		public IngredientCreateCommand(string name)
		{
			Name = name;
		}
	}

	public class IngredientCreateCommandValidator : FluentValidator<IngredientCreateCommand>
	{
		public IngredientCreateCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}