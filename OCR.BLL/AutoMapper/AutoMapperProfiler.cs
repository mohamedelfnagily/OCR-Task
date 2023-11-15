using AutoMapper;
using OCR.BLL.DTOs;
using OCR.BLL.DTOs.LicensceDataDto;
using OCR.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.AutoMapper
{
	public class AutoMapperProfiler:Profile
	{
		public AutoMapperProfiler()
		{
			CreateMap<UserAddDto, User>();
			CreateMap<User, UserReadDto>();
			CreateMap<CarDataAddDto, CarData>().ForMember(src => src.CarLicenseImage, opt => opt.Ignore());
			CreateMap<CarData, CarDataReadDto>();
			CreateMap<BackLicensceImageDto,CarData>().ForMember(src=>src.CarLicenseImage,opt=>opt.Ignore());
		}
	}
}
