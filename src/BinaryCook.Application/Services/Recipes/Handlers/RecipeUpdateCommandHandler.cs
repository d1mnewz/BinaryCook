﻿using System;
using System.Threading.Tasks;
using BinaryCook.Application.Services.Recipes.Contracts;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Recipes;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Services.Recipes.Handlers
{
	public class RecipeUpdateCommandHandler : ICommandHandler<RecipeUpdateCommand>
	{
		private readonly IRead<Recipe, Guid> _read;
		private readonly ISave<Recipe, Guid> _save;

		public RecipeUpdateCommandHandler(IRead<Recipe, Guid> read, ISave<Recipe, Guid> save)
		{
			_read = read;
			_save = save;
		}

		public async Task<ICommandResult> HandleAsync(CommandContext<RecipeUpdateCommand> ctx)
		{
			var recipe = await _read.GetAsync(ctx.Command.Id);
			recipe.Update(name: ctx.Command.Name, description: ctx.Command.Decription);
			await _save.UpdateAsync(recipe);
			return ctx.Succeeded(recipe.Id);
		}
	}
}