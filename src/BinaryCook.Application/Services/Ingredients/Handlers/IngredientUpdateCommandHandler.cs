﻿using System;
using System.Threading.Tasks;
using BinaryCook.Application.Services.Ingredients.Contracts;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Ingredients;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Services.Ingredients.Handlers
{
	public class IngredientUpdateCommandHandler : ICommandHandler<IngredientUpdateCommand>
	{
		private readonly IRead<Ingredient, Guid> _read;
		private readonly ISave<Ingredient, Guid> _save;

		public IngredientUpdateCommandHandler(IRead<Ingredient, Guid> read, ISave<Ingredient, Guid> save)
		{
			_read = read;
			_save = save;
		}

		public async Task<ICommandResult> HandleAsync(CommandContext<IngredientUpdateCommand> ctx)
		{
			var ingredient = await _read.GetAsync(ctx.Command.Id);
			ingredient.Update(name: ctx.Command.Name);
			await _save.UpdateAsync(ingredient);
			return ctx.Succeeded(ingredient.Id);
		}
	}
}