using System;
using System.Collections.Generic;

namespace ExternalScripts
{
	public static class ArrayExtensions
	{
		public static T[] Slice<T>(this T[] source, int start, int end)
		{
			// Handles negative ends.
			if (end < 0)
			{
				end = source.Length + end;
			}

			int len = end - start;

			// Return new array.
			T[] res = new T[len];
			for (int i = 0; i < len; i++)
			{
				res[i] = source[i + start];
			}

			return res;
		}

		public static string Join<T>(this T[] source, string separator = ",")
		{
			string result = null;

			foreach (var str in source)
			{
				if (result != null)
					result += separator;

				result += str;
			}

			return result;
		}

		public static int IndexOf<T>(this T[] source, object obj)
		{
			return Array.IndexOf(source, obj);
		}
	}

	public static class ListExtensions
	{
		public static List<T> Slice<T>(this List<T> source, int start, int end)
		{
			// Handles negative ends.
			if (end < 0)
			{
				end = source.Count + end;
			}

			int len = end - start;

			// Return new array.
			List<T> res = new List<T>();
			for (int i = 0; i < len; i++)
			{
				res.Add(source[i + start]);
			}

			return res;
		}
	}
}