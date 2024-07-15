using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.ProjectManagement.API.Database.Configuration;

public class EmploymentConfiguration : IEntityTypeConfiguration<Employment>
{
	public void Configure(EntityTypeBuilder<Employment> builder)
	{
		builder.HasKey(e => e.Id);
	}
}