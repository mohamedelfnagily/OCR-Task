using Microsoft.EntityFrameworkCore;
using OCR.DAL.Data.Context;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Repository.Non_Generic.UsersRepo
{
	public class UserRepository:BaseRepository<User>,IUserRepository
	{
		private readonly ApplicationDbContext _context;
		public UserRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public async Task<User> GetUserByIdIncludingCarData(string Id)
		{
			if (Id == null) { return null; }
			return await _context.Users.Include(e => e.CarData).FirstOrDefaultAsync(e=>e.Id==Id);
		}
	}
}
