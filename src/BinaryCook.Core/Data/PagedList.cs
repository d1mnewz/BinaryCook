using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryCook.Core.Data
{
	// TODO: move into nuget
	public interface IPagedList
	{
		int PageIndex { get; }

		int PageSize { get; }

		int TotalCount { get; }

		int TotalPages { get; }
	}

	public interface IPagedList<T> : IPagedList, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		IPagedList<T2> Convert<T2>(Func<T, T2> func);
	}

	[Serializable]
	public class PagedList<T> : List<T>, IPagedList<T>, IPagedList, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		public PagedList()
		{
		}

		public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
			: this((IEnumerable<T>) source.Skip<T>(pageIndex * pageSize).Take<T>(pageSize).ToList<T>(), pageIndex, pageSize, source.Count<T>())
		{
		}

		public PagedList(IList<T> source, int pageIndex, int pageSize)
			: this((IEnumerable<T>) source.Skip<T>(pageIndex * pageSize).Take<T>(pageSize).ToList<T>(), pageIndex, pageSize, source != null ? source.Count : 0)
		{
		}

		public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
		{
			this.TotalCount = totalCount;
			if (pageSize != 0)
			{
				this.TotalPages = this.TotalCount / pageSize;
				if (this.TotalCount % pageSize > 0)
					++this.TotalPages;
			}

			this.PageSize = pageSize;
			this.PageIndex = pageIndex;
			this.AddRange(source);
		}

		public IPagedList<T2> Convert<T2>(Func<T, T2> func)
		{
			return (IPagedList<T2>) new PagedList<T2>(this.Select<T, T2>(func), this.PageIndex, this.PageSize, this.TotalCount);
		}

		public int PageIndex { get; private set; }

		public int PageSize { get; private set; }

		public int TotalCount { get; private set; }

		public int TotalPages { get; private set; }

		public static PagedList<T> Empty()
		{
			return new PagedList<T>((IEnumerable<T>) new List<T>(), 0, 1, 0);
		}
	}
}