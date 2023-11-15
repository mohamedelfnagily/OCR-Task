using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Data.Models
{

	public class CarData
	{
		public Guid Id { get; set; }
		public string ChassisNumber { get; set; } = "";
		public string MotorNumber { get; set; } = "";
		public string DriverFullName { get; set; } = "";
		public string LicensceEndDate { get; set; } = "";
		public string LicensceStartDate { get; set; } = "";
		public byte[]? CarLicenseImage { get; set; }
		[ForeignKey("Id")]
		public string UserId { get; set; }
		public User User { get; set; }
	}
}
