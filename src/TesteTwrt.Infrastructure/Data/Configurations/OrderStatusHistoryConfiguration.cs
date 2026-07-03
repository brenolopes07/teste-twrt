using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteTwrt.Domain.Entities;

namespace TesteTwrt.Infrastructure.Data.Configurations;

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ToTable("OrderStatusHistories");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).ValueGeneratedNever();

        builder.Property(h => h.OrderId).IsRequired();
        builder.Property(h => h.PreviousStatus).IsRequired().HasConversion<int>();
        builder.Property(h => h.NewStatus).IsRequired().HasConversion<int>();
        builder.Property(h => h.ChangedAt).IsRequired();
        builder.Property(h => h.Reason).HasMaxLength(500);
    }
}
