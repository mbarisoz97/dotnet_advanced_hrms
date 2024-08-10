using Ehrms.TrainingManagement.API.MessageQueue.StateMachine;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.TrainingManagement.API.Database.Configuration;

public class TrainingRecommendationSagaDataConfiguration : IEntityTypeConfiguration<TrainingRecommendationSagaData>
{
    public void Configure(EntityTypeBuilder<TrainingRecommendationSagaData> builder)
    {
        builder.HasKey(e => e.CorrelationId);
    }
}