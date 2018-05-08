﻿using System;
using BinaryCook.Application.Core.Validations.Fluent;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Ingredients;
using FluentValidation;

namespace BinaryCook.Application.Services.Ingredients.Contracts
{
	public class IngredientUpdateCommand : Command
	{
		public Guid Id { get; }
		public string Name { get; }

		public IngredientUpdateCommand(Guid id, string name)
		{
			Id = id;
			Name = name;
		}
	}

	public class IngredientUpdateCommandValidator : FluentValidator<IngredientUpdateCommand>
	{
		public IngredientUpdateCommandValidator(IRead<Ingredient, Guid> read)
		{
			RuleFor(x => x.Id).MustAsync((id, ct) => read.AnyAsync(id));
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}