using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.ProjectManagement.API.Database.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
	public void Configure(EntityTypeBuilder<Project> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e=> e.Name)
			.HasMaxLength(Consts.MaxProjectNameLength);
	}
}