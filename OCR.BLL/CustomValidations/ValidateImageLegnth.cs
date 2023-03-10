using Microsoft.AspNetCore.Http;
using OCR.BLL.Helpers.ImagesSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.CustomValidations
{
	public class ValidateImageLegnth:ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if (value != null)
			{
				if(((IFormFile)value).Length > ImageValiedLength.Length)
				{
					return false;
				}
			}
			return true;
		}
	}
}
