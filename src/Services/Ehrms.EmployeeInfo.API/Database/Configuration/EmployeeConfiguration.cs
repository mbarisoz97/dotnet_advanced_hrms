using Ehrms.EmployeeInfo.API.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.EmployeeInfo.API.Database.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
	public void Configure(EntityTypeBuilder<Employee> builder)
	{
		builder.HasKey(e => e.Id);
		
		builder.Property(p => p.FirstName)
			.HasMaxLength(50);

		builder.Property(p => p.LastName)	
			.HasMaxLength(50);
	}
}