using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.TrainingManagement.API.Database.Configuration;

public class TitleConfiguration : IEntityTypeConfiguration<Title>
{
    public void Configure(EntityTypeBuilder<Title> builder)
    {
		builder.Property(p => p.Name).HasMaxLength(Consts.MaxTitleName);
    }
}