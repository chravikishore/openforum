using System.Collections.Generic;
using System.Linq;

namespace OpenForum.Core
{
	public static class Extenstions
	{
		// TODO: unit test
		public static string WithMaxLength(this string self, int maxLength)
		{
			if (self == null || self.Length < maxLength)
			{
				return self;
			}
			else
			{
				return self.Substring(0, maxLength) + "...";
			}
		}

		// TODO: unit test
		public static IEnumerable<T> GetPage<T>(this IEnumerable<T> self, int? page, int itemsPerPage)
		{
			return self.Skip(itemsPerPage * page ?? 0).Take(itemsPerPage);
		}
	}
}