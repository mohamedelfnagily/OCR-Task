using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCR.BLL.DTOs.LicensceDataDto;
using OCR.BLL.Managers.ManagingImageProcessing;
using System.Drawing;

namespace OCR.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageProcessingController : ControllerBase
	{
		private readonly IImageProcessingManager _imageProcessingManager;
		public ImageProcessingController(IImageProcessingManager imageProcessingManager)
		{
			_imageProcessingManager = imageProcessingManager;
		}
		[HttpPost("BackLicensceImageData")]
		public async Task<ActionResult> GetBackLicensceImageData([FromForm] BackLicensceImageDto LicensceImage)
		{
			var ImageProcessingResult =await _imageProcessingManager.CutImageToStripes(LicensceImage);
			//Thread.Sleep(10000);
			if(ImageProcessingResult.ErrorMessage!=string.Empty)
			{
				return BadRequest(ImageProcessingResult.ErrorMessage);
			}
			return Ok(ImageProcessingResult);
		}
	}
}
