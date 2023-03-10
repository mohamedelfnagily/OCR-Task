using Microsoft.EntityFrameworkCore;
using OCR.DAL.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Repository.Generic
{
	public class BaseRepository<T>:IBaseRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
		}

		public async Task<T> DeleteById(Guid id)
		{
			T Entity = await GetByIdAsync(id);
			_context.Set<T>().Remove(Entity);
			return Entity;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public void Save()
		{
			_context.SaveChanges();
		}

	}
}
