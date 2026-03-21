using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace Database.Configurations
{
    public sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("positions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Symbol)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 6);

            builder.Property(x => x.AvgEntryPrice)
                .HasPrecision(18, 6);

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("timestamp with time zone");

            builder.HasIndex(x => new { x.UserId, x.Symbol })
                .IsUnique();
        }
    }
}
