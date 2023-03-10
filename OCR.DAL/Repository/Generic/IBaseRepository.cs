using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Repository.Generic
{
	public interface IBaseRepository<T> where T : class
	{
		Task<T> GetByIdAsync(Guid id);
		Task<IEnumerable<T>> GetAllAsync();
		Task AddAsync(T entity);
		Task<T> DeleteById(Guid id);
		void Save();
	}
}
