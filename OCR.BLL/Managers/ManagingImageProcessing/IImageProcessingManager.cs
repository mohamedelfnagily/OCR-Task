using OCR.BLL.DTOs;
using OCR.BLL.DTOs.LicensceDataDto;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingImageProcessing
{
	public interface IImageProcessingManager
	{
		Task<CarDataReadDto> CutImageToStripes(BackLicensceImageDto licensceImage);
		System.Drawing.Image CropImage(System.Drawing.Image licensceImage, System.Drawing.Rectangle cropArea);
		void ImageContainingTextCheck();
		Bitmap ResizeImage(System.Drawing.Image image, int width, int height);

	}
}
