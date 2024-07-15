using Ehrms.EmployeeInfo.API.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.EmployeeInfo.API.Database.Configuration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
	public void Configure(EntityTypeBuilder<Skill> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(p => p.Name)
			.HasMaxLength(50);
	}
}