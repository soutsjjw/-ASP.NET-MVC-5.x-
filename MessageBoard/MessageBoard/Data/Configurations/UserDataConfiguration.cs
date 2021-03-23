using MessageBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Data.Configurations
{
    public class UserDataConfiguration
    {
        public void Configure(EntityTypeBuilder<UserData> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("nvarchar(20)");

            builder.Property(e => e.UserName)
                .IsRequired()
                .HasColumnType("nvarchar(256)");

            builder.Property(e => e.Email)
                .HasColumnType("nvarchar(256)");

            builder.Property(e => e.IsAdmin)
                .HasColumnType("bit")
                .HasDefaultValueSql("(0)");
        }
    }
}
