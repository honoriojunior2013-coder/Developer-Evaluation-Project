using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(s => s.Customer)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(s => s.Branch)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(s => s.SaleDate)
            .IsRequired();
        
        builder.Property(s => s.IsCancelled)
            .IsRequired();
        
        // Navigation: Sale -> Items (one-to-many)
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Ignore domain events (not persisted)
        builder.Ignore(s => s.DomainEvents);
        
        // Index for performance
        builder.HasIndex(s => s.SaleNumber).IsUnique();
        builder.HasIndex(s => s.Customer);
        builder.HasIndex(s => s.IsCancelled);
    }
}