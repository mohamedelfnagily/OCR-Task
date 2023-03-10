using OCR.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingCarsData
{
	public interface ICarDataManager
	{
		Task<CarDataReadDto> AddCarData(CarDataAddDto model);
	}
}
