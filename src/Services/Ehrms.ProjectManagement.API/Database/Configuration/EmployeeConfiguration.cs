using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.ProjectManagement.API.Database.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
	public void Configure(EntityTypeBuilder<Employee> builder)
	{
		builder.HasKey(e => e.Id);
		
		builder.Property(e=>e.FirstName)
			.HasMaxLength(Consts.MaxEmployeeNameLength);
		
		builder.Property(e=>e.LastName)
			.HasMaxLength(Consts.MaxEmployeeNameLength);
	}
}