using System.Collections.Generic;
using System.Text;

namespace Assets.scripts.Exstensions
{
	public static class IEnumerableExstension
	{
		public static string ListToString<T>(this IEnumerable<T> list)
		{
			StringBuilder text = new StringBuilder(string.Empty);
			foreach (var item in list)
				text.Append(item.ToString() + " ");

			return text.ToString();
		}
	}
}
