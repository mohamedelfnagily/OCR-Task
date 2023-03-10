using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Helpers.ImagesSettings
{
	public static class ImageValiedExtensions
	{
		public static List<string> Extensions { get;} = new List<string> { ".jpg",".png",".jpeg"};
	}
}
