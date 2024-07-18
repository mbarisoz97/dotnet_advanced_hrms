using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.ProjectManagement.API.Database.Configuration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
	public void Configure(EntityTypeBuilder<Skill> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Name)
			.HasMaxLength(Consts.MaxProjectNameLength);
	}
}