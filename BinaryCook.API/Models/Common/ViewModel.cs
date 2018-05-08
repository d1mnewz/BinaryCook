﻿using BinaryCook.Application.Web.Models;

namespace BinaryCook.API.Models.Common
{
	public abstract class ViewModel<T> : IViewModel<T>
	{
		/// <summary>
		/// Id
		/// </summary>
		public T Id { get; set; }

		public object GetId() => Id;
	}
}