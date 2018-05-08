﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BinaryCook.Application.Core.Quering;
using BinaryCook.Application.Services.Recipes.Contracts;
using BinaryCook.API.Infrastructure;
using BinaryCook.API.Infrastructure.Controllers;
using BinaryCook.API.Infrastructure.Extensions;
using BinaryCook.API.Models.Recipes;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data;
 using BinaryCook.Core.Domain.Model.Recipes;
using BinaryCook.Core.Messaging;
using BinaryCook.Infrastructure.AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BinaryCook.API.Controllers
{
	public class RecipesController : ApiController,
		IGetController<RecipeFilterModel, Guid>,
		IEditController<RecipeEditModel, Guid>
	{
		private readonly IBus _bus;
		private readonly IMapper<Recipe, RecipeViewModel> _mapper;

		public RecipesController(
			IBus bus,
			IMapper<Recipe, RecipeViewModel> mapper)
		{
			_bus = bus;
			_mapper = mapper;
		}

		/// <summary>
		/// Get Recipe by Id
		/// </summary>
		/// <param name="id">Recipe Id</param>
		/// <returns>A <see cref="RecipeViewModel"/></returns>
		[ProducesResponseCodeType(typeof(RecipeViewModel), HttpStatusCode.OK)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var items = await _bus.Find(id).Fetch<Recipe>().MapWith(_mapper);
			return Ok(items.FirstOrDefault());
		}

		/// <summary>
		/// Get Recipes by Ids
		/// </summary>
		/// <param name="ids">Recipe Ids</param>
		/// <returns>A <see cref="List{RecipeViewModel}"/> that corresponds to given Ids</returns>
		[ProducesResponseCodeType(typeof(List<RecipeViewModel>), HttpStatusCode.OK)]
		[HttpGet("lookup")]
		public async Task<IActionResult> GetByIds([FromQuery] Guid[] ids) =>
			Ok(await _bus.Find(ids).Fetch<Recipe>().MapWith(_mapper));

		/// <summary>
		/// Get Recipes by filter
		/// </summary>
		/// <param name="filter">Filter</param>
		/// <returns>A <see cref="List{RecipeViewModel}"/>  that corresponds to given filter</returns>
		[ProducesResponseCodeType(typeof(IPagedList<RecipeViewModel>), HttpStatusCode.OK)]
		[HttpGet("")]
		public async Task<IActionResult> GetByFilter([FromQuery] RecipeFilterModel filter) =>
			Ok(await _bus.Filter(filter).Fetch<Recipe>().MapWith(_mapper));

		/// <summary>
		/// Create Recipe
		/// </summary>
		/// <param name="model">Model</param>
		/// <returns><see cref="Guid"/>Id of Recipe that was created</returns>
		[ProducesResponseCodeType(typeof(Guid), HttpStatusCode.Created)]
		[ProducesResponseCodeType(typeof(IValidationResult), HttpStatusCode.BadRequest)]
		[HttpPost]
		public Task<IActionResult> Create([FromBody] RecipeEditModel model) => _bus
			.Publish(new RecipeCreateCommand(model.Name, model.Description))
			.ToCreatedAtResult();

		/// <summary>
		/// Update Recipe by Id
		/// </summary>
		/// <param name="id">Recipe Id</param>
		/// <param name="model">Model</param>
		/// <returns><see cref="Guid"/>Id of Recipe that was updated</returns>
		[ProducesResponseCodeType(typeof(Guid), HttpStatusCode.Accepted)]
		[ProducesResponseCodeType(typeof(IValidationResult), HttpStatusCode.BadRequest)]
		[HttpPut("{id}")]
		public Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RecipeEditModel model) =>
			_bus.Publish(new RecipeUpdateCommand(id, model.Name, model.Description)).ToAcceptedAtResult();
	}
}