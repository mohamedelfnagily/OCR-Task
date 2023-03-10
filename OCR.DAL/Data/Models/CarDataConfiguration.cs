using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR.DAL.Data.Models
{
	public class CarDataConfiguration : IEntityTypeConfiguration<CarData>
	{
		public void Configure(EntityTypeBuilder<CarData> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(c=>c.ChassisNumber).HasMaxLength(10);
			builder.Property(c=>c.MotorNumber).HasMaxLength(10);
			
		}
	}
}
