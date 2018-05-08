﻿using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Data.Specifications;

namespace BinaryCook.API.Models.Common
{
	public abstract class FilterModel<T> : IFilter<T> where T : class, IAggregateRoot
	{
		/// <summary>
		/// Page Index
		/// </summary>
		public int Page { get; set; }

		/// <summary>
		/// Number of items per page
		/// </summary>
		public int? PageSize { get; set; }

		public abstract ISorter<T> ToSorter();
		public abstract ISpecification<T> ToSpecification();
		public virtual IPagedInfo ToPagedInfo() => new PagedInfo(Page, PageSize);
	}
}