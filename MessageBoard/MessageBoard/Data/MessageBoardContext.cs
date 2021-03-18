﻿using MessageBoard.Data.Configurations;
using MessageBoard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Data
{
    public class MessageBoardContext : DbContext
    {
        public MessageBoardContext(DbContextOptions<MessageBoardContext> options)
            :base(options)
        {
            
        }

        public DbSet<Guestbook> Guestbooks { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemberConfiguration).Assembly);

            //modelBuilder.Entity<Member>()
            //    .HasMany(c => c.Guestbooks)
            //    .WithOne(e => e.Member)
            //    .IsRequired();

            modelBuilder.Entity<Guestbook>()
                .HasOne<Member>(x => x.Member)
                .WithMany(x => x.Guestbooks)
                .HasForeignKey(x => x.MemberId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
