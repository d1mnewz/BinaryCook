﻿using System.Globalization;
using System.Threading.Tasks;
using BinaryCook.Application.Web.Models;
using BinaryCook.Application.Web.Mvc;
using BinaryCook.Core.Data.AggregateRoots;
using Microsoft.AspNetCore.Mvc;

namespace BinaryCook.API.Infrastructure.Controllers
{
	public interface IGetController<in TFilter, in TId>
		where TFilter : IFilter
	{
		Task<IActionResult> GetById(TId id);
		Task<IActionResult> GetByIds(TId[] ids);
		Task<IActionResult> GetByFilter(TFilter filter);
	}

	public interface IEditController<in TEditModel, in TId>
		where TEditModel : IEditModel
	{
		Task<IActionResult> Create(TEditModel model);
		Task<IActionResult> Update(TId id, TEditModel model);
	}

	public interface IDeleteController<in TId>
	{
		Task<IActionResult> Delete(TId id);
	}

	[Route("api/[controller]")]
	[Produces("application/json")]
	[Consumes("application/json")]
	public abstract class ApiController : Controller
	{
		private readonly object _appStateLock = new object();
		private AppState _appState;

		protected AppState Current
		{
			get
			{
				if (_appState == null)
				{
					lock (_appStateLock)
					{
						if (_appState == null)
							_appState = new AppState(User);
					}
				}

				return _appState;
			}
		}

		protected string CurrentLanguage => CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
	}
}