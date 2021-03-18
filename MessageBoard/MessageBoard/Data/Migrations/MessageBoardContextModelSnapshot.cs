﻿// <auto-generated />
using System;
using MessageBoard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MessageBoard.Data.Migrations
{
    [DbContext(typeof(MessageBoardContext))]
    partial class MessageBoardContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MessageBoard.Models.Guestbook", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Reply")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ReplyTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("Guestbooks");
                });

            modelBuilder.Entity("MessageBoard.Models.Member", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("AuthCode")
                        .IsRequired()
                        .HasColumnType("char(10)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(MAX)");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("MessageBoard.Models.Guestbook", b =>
                {
                    b.HasOne("MessageBoard.Models.Member", "Member")
                        .WithMany("Guestbooks")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("MessageBoard.Models.Member", b =>
                {
                    b.Navigation("Guestbooks");
                });
#pragma warning restore 612, 618
        }
    }
}
