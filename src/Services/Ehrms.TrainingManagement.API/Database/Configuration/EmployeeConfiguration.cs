using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.TrainingManagement.API.Database.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
	public void Configure(EntityTypeBuilder<Employee> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(p => p.FirstName).HasMaxLength(Consts.MaxEmployeeFirstNameLength);
		builder.Property(p => p.LastName).HasMaxLength(Consts.MaxEmployeeLastNameLength);
	}
}