using AutoMapper;
using OCR.BLL.DTOs;
using OCR.BLL.Helpers.LicenseDataExtraction;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Non_Generic.CarsDataRepo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingCarsData
{
	public class CarDataManager:ICarDataManager
	{
		private readonly ICarDataRepository _cardatarepository;
		private readonly ILicenseData _licenseData;
		private readonly IMapper _mapper;
		public CarDataManager(ICarDataRepository cardatarepository,ILicenseData licensedata,IMapper mapper)
		{
			_cardatarepository = cardatarepository;
			_licenseData = licensedata;
			_mapper = mapper;
		}

		public async Task<CarDataReadDto> AddCarData(CarDataAddDto model)
		{
			var Errors = string.Empty;
			if(await _cardatarepository.CheckForDuplicates(model.UserId))
			{
				return new CarDataReadDto() { ErrorMessage="This User has already entered a license before."};
			}
			MemoryStream memoryStream = new MemoryStream();
			if (model.CarLicenseImage != null)
			{
				using var dataStream = new MemoryStream();
				await model.CarLicenseImage.CopyToAsync(dataStream);
				memoryStream = dataStream;
			}
			CarData carData = _mapper.Map<CarData>(model);
			CarDataReadDto GeneratedDataFromImage = new CarDataReadDto(); 
			if (model.CarLicenseImage != null)
			{
				carData.CarLicenseImage = memoryStream.ToArray();
				GeneratedDataFromImage = _licenseData.GetLicenseData(carData.CarLicenseImage);
			}
			carData.ChassisNumber = GeneratedDataFromImage.ChassisNumber;
			carData.MotorNumber = GeneratedDataFromImage.MotorNumber;
			carData.Id = Guid.NewGuid();
			await _cardatarepository.AddAsync(carData);
			_cardatarepository.Save();
			CarDataReadDto RetrievedCarData = _mapper.Map<CarDataReadDto>(carData);
			return RetrievedCarData;
		}
	}
}
