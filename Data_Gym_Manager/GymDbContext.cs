using Data_Gym_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Gym_Manager
{
    public class GymDbContext:DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public DbSet <Gym> Gyms { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TrainerMember> TrainerMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainerMember>()
                .HasKey(dp=>new {dp.TrainerId, dp.MemberId});

            modelBuilder.Entity<TrainerMember>()
                .HasOne(dp => dp.Member)
                .WithMany(dp => dp.TrainerMembers)
                .HasForeignKey(dp => dp.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrainerMember>()
                .HasOne(dp => dp.Trainer)
                .WithMany(dp => dp.TrainerMembers)
                .HasForeignKey(dp => dp.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
