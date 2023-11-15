using Microsoft.AspNetCore.Http;
using OCR.BLL.CustomValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.DTOs.LicensceDataDto
{
	public class BackLicensceImageDto
	{
		public string UserId { get; set; } = "";
		[ValidateImageExtension(ErrorMessage = "Invalid image extension, Please upload an image with one of those extensions '.jpg ,.png ,.jpeg'")]
		[ValidateImageLegnth(ErrorMessage = "Uploaded image exceeds the maximum limit, Please upload an image less than 1MB.")]
		public IFormFile? CarBackLicenseImage { get; set; }
		public string ChassisNumber { get; set; } = "";
		public string MotorNumber { get; set; } = "";
	}
}
