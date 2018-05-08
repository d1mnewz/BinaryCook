﻿using System;
using System.Threading.Tasks;
using BinaryCook.Application.Services.Recipes.Contracts;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Domain.Model.Recipes;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Services.Recipes.Handlers
{
	public class RecipeCreateCommandHandler : ICommandHandler<RecipeCreateCommand>
	{
		private readonly ISave<Recipe, Guid> _save;

		public RecipeCreateCommandHandler(ISave<Recipe, Guid> save)
		{
			_save = save;
		}

		public async Task<ICommandResult> HandleAsync(CommandContext<RecipeCreateCommand> ctx)
		{
			var recipe = new Recipe(ctx.Command.Name, ctx.Command.Author);
			await _save.InsertAsync(recipe);
			return ctx.Succeeded(recipe.Id);
		}
	}
}