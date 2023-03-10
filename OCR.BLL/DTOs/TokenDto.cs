using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.DTOs
{
	public class TokenDto
	{
		public string Token { get; set; } = "";
		public int ExpiryDuration { get; set; }
		public string Message { get; set; } = "";
	}
}
