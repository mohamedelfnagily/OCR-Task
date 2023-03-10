using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OCR.BLL.DTOs;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Non_Generic.UsersRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.BLL.Managers.ManagingUsers
{
	public class ManageUser : IManageUser
	{
		private readonly IUserRepository _userrepository;
		private readonly UserManager<User> _usermanager;
		private readonly IMapper _mapper;
		public ManageUser(IUserRepository userrepository, IMapper mapper, UserManager<User> usermanaer)
		{
			_userrepository= userrepository;
			_mapper= mapper;
			_usermanager= usermanaer;
		}

		public async Task<bool> CheckUserExistance(string Id)
		{
			var UserExists = await _usermanager.FindByIdAsync(Id);
			if (UserExists == null) { return false; }
			return true;
		}

		public async Task<UserReadDto> DeleteUser(string Id)
		{
			var user = await _usermanager.FindByIdAsync(Id);
			if (user == null)
			{
				return new UserReadDto() { ErrorMessage = "Invalid Id, User not found." };
			}
			UserReadDto DeletedUser = _mapper.Map<UserReadDto>(user);
			await _usermanager.DeleteAsync(user);
			return DeletedUser;
		}

		public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
		{
			var Users = await _userrepository.GetAllAsync();
			var RetrievedUsers = _mapper.Map<IEnumerable<UserReadDto>>(Users);	
			return RetrievedUsers;
		}

		public async Task<UserReadDto> GetUserByIdAsync(string Id)
		{
			var user = await _userrepository.GetUserByIdIncludingCarData(Id);
			if (user == null)
			{
				return new UserReadDto() { ErrorMessage="Invalid Id, User not found."};
			}
			UserReadDto RetrievedUser = _mapper.Map<UserReadDto>(user);
			return RetrievedUser;
		}
	}
}
