using MessageBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessageBoard.Data.Configurations
{
    public class GuestbookConfiguration
    {
        public void Configure(EntityTypeBuilder<Guestbook> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatorId)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.CreateTime)
                .IsRequired()
                .HasDefaultValueSql("(getDate())");

            builder.Property(e => e.UpdaterId)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId);

            builder.HasOne(p => p.Updater)
                .WithMany()
                .HasForeignKey(p => p.UpdaterId);

            builder.HasOne(p => p.Replier)
                .WithMany()
                .HasForeignKey(p => p.ReplierId);

            builder.Property(e => e.IsDelete)
                .IsRequired()
                .HasDefaultValueSql("(0)");
        }
    }
}
