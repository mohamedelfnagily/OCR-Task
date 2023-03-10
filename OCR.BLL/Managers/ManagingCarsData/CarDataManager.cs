using AutoMapper;
using OCR.BLL.DTOs;
using OCR.BLL.Helpers.LicenseDataExtraction;
using OCR.BLL.Managers.ManagingUsers;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Non_Generic.CarsDataRepo;
using OCR.DAL.Repository.Non_Generic.UsersRepo;
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
		private readonly IManageUser _usermanager;
		private readonly ILicenseData _licenseData;
		private readonly IMapper _mapper;
		public CarDataManager(ICarDataRepository cardatarepository,ILicenseData licensedata,IMapper mapper, IManageUser usermanager)
		{
			_cardatarepository = cardatarepository;
			_licenseData = licensedata;
			_mapper = mapper;
			_usermanager = usermanager;
		}

		public async Task<CarDataReadDto> AddCarData(CarDataAddDto model)
		{
			var Errors = string.Empty;
			if(await _cardatarepository.CheckForDuplicates(model.UserId))
			{
				return new CarDataReadDto() { ErrorMessage="This User has already entered a license before."};
			}
			if(!await _usermanager.CheckUserExistance(model.UserId))
			{
				return new CarDataReadDto() { ErrorMessage = "User doesn't exist, Please check the user Id" };
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
			if(GeneratedDataFromImage.ErrorMessage !=string.Empty)
			{
				return GeneratedDataFromImage;
			}
			carData.ChassisNumber = GeneratedDataFromImage.ChassisNumber;
			carData.MotorNumber = GeneratedDataFromImage.MotorNumber;
			carData.Id = Guid.NewGuid();
			await _cardatarepository.AddAsync(carData);
			_cardatarepository.Save();
			CarDataReadDto RetrievedCarData = _mapper.Map<CarDataReadDto>(carData);
			return RetrievedCarData;
		}

		public async Task<IEnumerable<CarDataReadDto>> GetAllCarDataAsync()
		{
			var RetrivedCarsData = await _cardatarepository.GetAllAsync();
			IEnumerable<CarDataReadDto> CarsDataAvailable = _mapper.Map<IEnumerable<CarDataReadDto>>(RetrivedCarsData);
			return CarsDataAvailable;
		}
	}
}
