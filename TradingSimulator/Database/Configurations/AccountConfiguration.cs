using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.CashBalance)
                .HasPrecision(18, 6);

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("timestamp with time zone");
        }
    }
}
