using MessageBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessageBoard.Data.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Account)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(e => e.Password)
                .IsRequired()
                .HasColumnType("varchar(MAX)");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(e => e.AuthCode)
                .IsRequired()
                .HasColumnType("char(10)");
        }
    }
}
