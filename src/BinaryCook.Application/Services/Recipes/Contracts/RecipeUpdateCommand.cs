﻿using System;
using BinaryCook.Application.Core.Validations.Fluent;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Recipes;
using FluentValidation;

namespace BinaryCook.Application.Services.Recipes.Contracts
{
	public class RecipeUpdateCommand : Command
	{
		public Guid Id { get; }
		public string Name { get; }
		public string Decription { get; }

		public RecipeUpdateCommand(Guid id, string name, string decription)
		{
			Id = id;
			Name = name;
			Decription = decription;
		}
	}

	public class RecipeUpdateCommandValidator : FluentValidator<RecipeUpdateCommand>
	{
		public RecipeUpdateCommandValidator(IRead<Recipe, Guid> read)
		{
			RuleFor(x => x.Id).MustAsync((id, ct) => read.AnyAsync(id));
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}