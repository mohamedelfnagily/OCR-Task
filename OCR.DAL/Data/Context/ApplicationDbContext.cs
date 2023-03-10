using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OCR.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Data.Context
{
	public class ApplicationDbContext:IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions opt):base(opt)
		{
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration<User>(new UserConfiguration());
			builder.ApplyConfiguration<CarData>(new CarDataConfiguration());
		}
		public virtual DbSet<User> Users { get; set; } = null!;
		public virtual DbSet<CarData> CarsData { get; set; } = null!;
	}
}
