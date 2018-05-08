using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryCook.Core.Extensions
{
	public static class StringExtensions
	{
		/// <summary>  
		///  Check if string URL is valid  
		/// </summary> 
		public static bool IsValidUrl(this string source) => Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute);

		/// <summary>  
		///  Check if string URL is valid or null  
		/// </summary> 
		public static bool IsValidUrlOrNull(this string source) => source == null || source.IsValidUrl();

		public static bool IsEmptyString(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		public static string SplitCamelCaseToStr(this string stringToSplit, string separator = " ")
		{
			if (string.IsNullOrEmpty(stringToSplit))
				return null;

			var split = SplitCamelCase(stringToSplit);
			if (split == null)
				return null;

			var result = new StringBuilder();
			for (var i = 0; i < split.Count; i++)
			{
				result.Append(split[i]);
				if (i != split.Count - 1)
					result.Append(separator);
			}

			return result.ToString();
		}

		public static List<string> SplitCamelCase(this string stringToSplit)
		{
			if (string.IsNullOrEmpty(stringToSplit))
				return null;

			var result = new List<string>();
			var wasPreviousUppercase = false;
			var current = new StringBuilder();

			foreach (var c in stringToSplit)
			{
				if (char.IsUpper(c))
				{
					if (wasPreviousUppercase)
					{
						current.Append(c);
					}
					else
					{
						if (current.Length > 0)
						{
							result.Add(current.ToString());
							current.Length = 0;
						}

						current.Append(c);
					}

					wasPreviousUppercase = true;
				}
				else // lowercase
				{
					if (wasPreviousUppercase)
					{
						if (current.Length > 1)
						{
							var carried = current[current.Length - 1];
							--current.Length;
							result.Add(current.ToString());
							current.Length = 0;
							current.Append(carried);
						}
					}

					wasPreviousUppercase = false;

					current.Append(current.Length == 0 ? char.ToUpper(c) : c);
				}
			}

			if (current.Length > 0)
			{
				result.Add(current.ToString());
			}

			return result;
		}
	}
}