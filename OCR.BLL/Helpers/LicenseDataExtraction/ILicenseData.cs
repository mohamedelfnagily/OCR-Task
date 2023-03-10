using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using IronOcr;
using IronSoftware.Drawing;
using OCR.DAL.Data.Models;
using OCR.BLL.DTOs;

namespace OCR.BLL.Helpers.LicenseDataExtraction
{
	public interface ILicenseData
	{
		CarDataReadDto GetLicenseData(byte[] licenseImage);
		Bitmap ResizeImage(Image image, int width, int height);
	}
}
