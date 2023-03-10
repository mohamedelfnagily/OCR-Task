using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCR.BLL.DTOs;
using OCR.BLL.Managers.ManagingCarsData;

namespace OCR.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarsDataController : ControllerBase
	{
		private readonly ICarDataManager _cardatamanager;
		public CarsDataController(ICarDataManager cardatamanager)
		{
			_cardatamanager = cardatamanager;
		}

		[HttpPost("AddCarData")]
		public async Task<ActionResult<CarDataReadDto>> AddCarData([FromForm] CarDataAddDto model)
		{
			if (model == null)
			{
				return BadRequest();
			}
			CarDataReadDto AddedCarData = await _cardatamanager.AddCarData(model);
			if(AddedCarData.ErrorMessage!=string.Empty)
			{
				return BadRequest(AddedCarData.ErrorMessage);
			}
			if (!ModelState.IsValid)
			{
				return new CarDataReadDto() { ErrorMessage = string.Join(',', ModelState.Values.SelectMany(e => e.Errors)) };
			}
			return Ok(AddedCarData);
		}
	}
}
