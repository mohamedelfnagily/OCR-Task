using OCR.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingUsers
{
	public interface IManageUser
	{
		Task<bool> CheckUserExistance(string Id);
		Task<UserReadDto> GetUserByIdAsync(string Id);
		Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
		Task<UserReadDto> DeleteUser(string Id);
	}
}
