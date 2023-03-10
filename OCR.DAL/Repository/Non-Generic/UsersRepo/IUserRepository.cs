using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Repository.Non_Generic.UsersRepo
{
	public interface IUserRepository:IBaseRepository<User>
	{
		Task<User> GetUserByIdIncludingCarData(string Id);
	}
}
