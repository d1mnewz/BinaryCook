﻿using System;
using System.Threading.Tasks;
using BinaryCook.Application.Services.Ingredients.Contracts;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Ingredients;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Services.Ingredients.Handlers
{
	public class IngredientCreateCommandHandler : ICommandHandler<IngredientCreateCommand>
	{
		private readonly ISave<Ingredient, Guid> _save;

		public IngredientCreateCommandHandler(ISave<Ingredient, Guid> save)
		{
			_save = save;
		}

		public async Task<ICommandResult> HandleAsync(CommandContext<IngredientCreateCommand> ctx)
		{
			var ingredient = new Ingredient(ctx.Command.Name);
			await _save.InsertAsync(ingredient);
			return ctx.Succeeded(ingredient.Id);
		}
	}
}