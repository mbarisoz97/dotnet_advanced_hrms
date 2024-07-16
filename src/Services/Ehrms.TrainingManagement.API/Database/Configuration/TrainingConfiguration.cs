using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.TrainingManagement.API.Database.Configuration;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
	public void Configure(EntityTypeBuilder<Training> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(p => p.Name).HasMaxLength(Consts.MaxTrainingNameLength);
	}
}