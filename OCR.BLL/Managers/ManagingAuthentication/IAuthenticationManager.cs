using Microsoft.IdentityModel.Tokens;
using OCR.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingAuthentication
{
	public interface IAuthenticationManager
	{
		Task<TokenDto> SignInUser(UserSignInDto model);
		Task<UserReadDto> RegisterNewUser(UserAddDto model);
		string TokenGenerator(IEnumerable<Claim> claims, SigningCredentials credentials);
	}
}
