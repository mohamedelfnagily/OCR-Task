using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCR.BLL.DTOs;
using OCR.BLL.Managers.ManagingAuthentication;

namespace OCR.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationManager _authenticationmanager;
		public AuthenticationController(IAuthenticationManager authenticationmanager)
		{
			_authenticationmanager = authenticationmanager;
		}
		[HttpPost("SignInUser")]
		public async Task<ActionResult<TokenDto>> SignIn(UserSignInDto model)
		{
			if(model == null)
			{
				return BadRequest(new TokenDto() { Message="No Email or password Entered"});
			}
			TokenDto UsersToken = await _authenticationmanager.SignInUser(model);
			if(UsersToken.Message!="Success")
			{
				return BadRequest(UsersToken.Message);
			}
			return Ok(UsersToken);
		}

		[HttpPost("RegisterUser")]
		public async Task<ActionResult<UserReadDto>> Register(UserAddDto model)
		{
			if (model == null)
			{
				return BadRequest();
			}
			UserReadDto AddedUser = await _authenticationmanager.RegisterNewUser(model);
			if(AddedUser.ErrorMessage!=string.Empty)
			{
				return BadRequest(AddedUser.ErrorMessage);
			}
			return Ok(AddedUser);
		}
	}
}
