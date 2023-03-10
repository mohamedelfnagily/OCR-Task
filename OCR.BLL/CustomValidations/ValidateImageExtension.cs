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
	public class ValidateImageExtension:ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if(value !=null)
			{
					if (!ImageValiedExtensions.Extensions.Contains(Path.GetExtension(((IFormFile)value).FileName).ToLower()))
					{
						return false;
					}
				
			}
			return true;
		}
	}
}
