using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Helpers.LicenseDataExtraction
{
	public static class ImageProperSize
	{
		public static int Width { get; } = 475;
		public static int Height { get; } = 353;
		public static int SubstitutableWidth { get; set; } = 800;
		public static int SubstitutableHeight { get; set; } = 1000;
	}
}
