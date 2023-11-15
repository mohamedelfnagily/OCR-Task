using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.DTOs.LicensceDataDto
{
	public class LicensceReadDto
	{
		public string DriverFullName { get; set; } = "";
		public string LicensceEndDate { get; set; } = "";
		public string LicensceStartDate { get; set; } = "";
		public string ChassisNumber { get; set; } = "";
		public string MotorNumber { get; set; } = "";
		public string ErrorMessage { get; set; } = "";
	}
}
