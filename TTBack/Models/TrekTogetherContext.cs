﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TTBack.Models
{
    public partial class TrekTogetherContext : DbContext
    {
        public TrekTogetherContext()
        {
        }

        public TrekTogetherContext(DbContextOptions<TrekTogetherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Trip> Trips { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public DbSet<UserTrip> UserTrips { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>()
                .HasKey(ut => new { ut.UserId, ut.TripId });

            modelBuilder.Entity<UserTrip>()
                .HasOne(t => t.Trip)
                .WithMany(ut => ut.UserTrips)
                .HasForeignKey(t => t.TripId);

            modelBuilder.Entity<UserTrip>()
                .HasOne(u => u.User)
                .WithMany(ut => ut.UserTrips)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GosNomer)
                    .HasMaxLength(50)
                    .HasColumnName("carMake"); // Кажется в базе он остался как carMake ну а тут он GosNomer. Прикольно, наверное оставлю так)) ^_^, Но пожалуй так делать не советую. Сам себе комментарии пишу, ахах))

                entity.Property(e => e.CarModel)
                    .HasMaxLength(50)
                    .HasColumnName("carModel");

                entity.Property(e => e.CarYear)
                    .HasMaxLength(10)
                    .HasColumnName("carYear")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Car_User");
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArrivalCity)
                    .HasMaxLength(50)
                    .HasColumnName("arrivalCity");

                entity.Property(e => e.AvailableSeats).HasColumnName("availableSeats");

                entity.Property(e => e.DepartureCity)
                    .HasMaxLength(50)
                    .HasColumnName("departureCity");

                entity.Property(e => e.DepartureData)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("departureData");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_Trip_User");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK_Trip_Car");

                entity.Property(t => t.IsCompleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsDriver).HasColumnName("isDriver");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phoneNumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
