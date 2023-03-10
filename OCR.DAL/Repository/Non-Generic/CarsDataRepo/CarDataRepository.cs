using Microsoft.EntityFrameworkCore;
using OCR.DAL.Data.Context;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Repository.Non_Generic.CarsDataRepo
{
	public class CarDataRepository:BaseRepository<CarData>, ICarDataRepository
	{
		private readonly ApplicationDbContext _context;
		public CarDataRepository(ApplicationDbContext context):base(context)
		{
			_context = context;
		}

		public async Task<bool> CheckForDuplicates(string UserId)
		{
			return await _context.CarsData.AnyAsync(c => c.UserId == UserId);
		}
	}
}
