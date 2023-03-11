using IronOcr;
using OCR.BLL.DTOs;
using OCR.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCR.BLL.Helpers.LicenseDataExtraction
{
	public class LicenseData : ILicenseData
	{
		public CarDataReadDto GetLicenseData(byte[] licenseImage)
		{
			IronOcr.License.LicenseKey = "IRONOCR.MELFNAGILY96.599-8601A9F524-HCJG5KBE6O7KR-63E3CKIHCLKR-WUMPG2XGZKIB-X22Z72XZ7GP6-XKZEQP5C6NVN-PMOKTY-TE7KDFE5MVGJEA-DEPLOYMENT.TRIAL-7ZJN3P.TRIAL.EXPIRES.09.APR.2023";
			System.Drawing.Image img;
			using (var ms = new MemoryStream(licenseImage))
			{
				img = Image.FromStream(ms);
			}
			//System.Drawing.Image img = System.Drawing.Image.FromFile(@"C:\Users\mohamed_elfnagily\Downloads\0_1657216282.jpg");
			var myIgmg = ResizeImage(img, ImageProperSize.Width, ImageProperSize.Height);
			var ocr = new IronTesseract();
			var CarDataList = new List<string>();
			CarDataReadDto EnteredCarData = new CarDataReadDto();
			using (var input = new OcrInput(myIgmg))
			{
				int counter = 0;
				input.Contrast();
				input.EnhanceResolution();
				var result = ocr.Read(input).Text;
				var str = result.Replace("\n", ",").Replace("\r", "").Replace(" ", ",");
				var str2 = Regex.Replace(str, @"[^0-9,]+", "");
				var myList = str2.Split(',').ToList();
				if (myList.Count >= 2)
				{
					foreach (var item in myList)
					{
						if (item != string.Empty && item.Length >= 4)
						{
							CarDataList.Add(item);
							counter++;
						}
						if (counter == 2) { 
							EnteredCarData = new CarDataReadDto { ChassisNumber = CarDataList[0] , MotorNumber= CarDataList[1] };
							break;
						}
					}
				}
			}
			if (EnteredCarData.MotorNumber == string.Empty || EnteredCarData.ChassisNumber == string.Empty)
			{
				var NewIgmg = ResizeImage(img, ImageProperSize.SubstitutableWidth, ImageProperSize.SubstitutableHeight);
				ocr.Language = OcrLanguage.Arabic;
				using (var input = new OcrInput(NewIgmg))
				{
					int counter = 0;
					input.Contrast();
					input.EnhanceResolution();
					var result = ocr.Read(input).Text;
					var str = result.Replace("\n", ",").Replace("\r", "").Replace(" ", ",");
					var str2 = Regex.Replace(str, @"[^0-9,]+", "");
					var myList = str2.Split(',').ToList();
					if (myList.Count >= 2)
					{
						foreach (var item in myList)
						{
							if (item != string.Empty && item.Length >= 4)
							{
								CarDataList.Add(item);
								counter++;
							}
							if (counter == 2)
							{
								EnteredCarData = new CarDataReadDto { ChassisNumber = CarDataList[0], MotorNumber = CarDataList[1] };
								break;
							}
						}
					}
				}
			}
			if (EnteredCarData.MotorNumber == string.Empty || EnteredCarData.ChassisNumber == string.Empty)
			{
				return new CarDataReadDto() { ErrorMessage = "Image resolution is not good enough, please upload a new image" };
			}
			return EnteredCarData;
		}

		public Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
