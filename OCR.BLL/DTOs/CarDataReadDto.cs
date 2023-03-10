using Microsoft.AspNetCore.Http;
using OCR.BLL.CustomValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.DTOs
{
	public class CarDataReadDto
	{
		public Guid Id { get; set; }
		public string ChassisNumber { get; set; } = "";
		public string MotorNumber { get; set; } = "";
		public byte[]? CarLicenseImage { get; set; }
		public string ErrorMessage { get; set; } = "";

	}
}
