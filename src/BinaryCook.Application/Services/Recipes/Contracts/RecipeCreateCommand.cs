﻿using BinaryCook.Application.Core.Validations.Fluent;
using BinaryCook.Core.Commands;
using FluentValidation;

namespace BinaryCook.Application.Services.Recipes.Contracts
{
	// TODO:
	public class RecipeCreateCommand : Command
	{
		public string Name { get; }
		public string Author { get; }

		public RecipeCreateCommand(string name, string author)
		{
			Name = name;
			Author = author;
		}
	}

	public class RecipeCreateCommandValidator : FluentValidator<RecipeCreateCommand>
	{
		public RecipeCreateCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Author).NotEmpty();
		}
	}
}