using Ehrms.Administration.API.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ehrms.Administration.API.Database.ModelConfiguration;

public class PaymentCriteriaConfiguration : IEntityTypeConfiguration<PaymentCriteria>
{
    public void Configure(EntityTypeBuilder<PaymentCriteria> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");
    }
}