using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.ProcessingImages
{
	public static class IEnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> onItem)
		{
			foreach (var item in enumerable)
				onItem(item);
		}
	}
}
