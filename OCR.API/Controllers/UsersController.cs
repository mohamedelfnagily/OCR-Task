using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCR.BLL.DTOs;
using OCR.BLL.Managers.ManagingUsers;

namespace OCR.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IManageUser _usermanager;
		public UsersController(IManageUser usermanager)
		{
			_usermanager = usermanager;
		}
		[HttpGet("GetUserById/{Id}")]
		public async Task<ActionResult<UserReadDto>> GetUser(string Id)
		{
			if(Id== null) { return BadRequest(); }
			UserReadDto RetrievedUser = await _usermanager.GetUserByIdAsync(Id);
			if(RetrievedUser.ErrorMessage!=string.Empty) { return BadRequest(RetrievedUser.ErrorMessage); }
			return Ok(RetrievedUser);
		}

		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
		{
			var RetrievedUsers = await _usermanager.GetAllUsersAsync();	
			return Ok(RetrievedUsers);
		}
		[HttpDelete("DeleteUser/{Id}")]
		public async Task<ActionResult<UserReadDto>> DeleteUserById(string Id)
		{
			if(Id== null) { return BadRequest(); }
			UserReadDto DeletedUser = await _usermanager.DeleteUser(Id);
			if (DeletedUser.ErrorMessage != string.Empty) { return BadRequest(DeletedUser.ErrorMessage); }
			return Ok(DeletedUser);
		}
	}
}
