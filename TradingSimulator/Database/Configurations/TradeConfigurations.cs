using Database.Entities;
using Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Database.Configurations
{
    public sealed class TradeConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.ToTable("trades");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Ticker)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 6);

            builder.Property(x => x.Price)
                .HasPrecision(18, 6);

            builder.Property(x => x.Side)
                .HasConversion(
                    v => v.ToString().ToUpperInvariant(),
                    v => v == "BUY" ? TradeSide.Buy : TradeSide.Sell)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .HasColumnType("timestamp with time zone");

            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(x => new { x.UserId, x.Ticker });
            builder.HasIndex(x => x.Timestamp);
        }
    }
}
