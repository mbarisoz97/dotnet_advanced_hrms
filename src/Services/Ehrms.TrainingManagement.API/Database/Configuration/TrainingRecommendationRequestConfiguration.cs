using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.TrainingManagement.API.Database.Configuration;

public class TrainingRecommendationRequestConfiguration : IEntityTypeConfiguration<TrainingRecommendationRequest>
{
    public void Configure(EntityTypeBuilder<TrainingRecommendationRequest> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Title)
            .HasMaxLength(150);
    }
}