using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.Data.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(i => i.Quantity)
            .IsRequired();
        
        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(i => i.Discount)
            .HasColumnType("decimal(5,2)");
        
        builder.Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(i => i.IsCancelled)
            .IsRequired();
        
        // Index for performance
        builder.HasIndex(i => i.SaleId);
    }
}