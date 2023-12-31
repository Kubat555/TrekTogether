﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TTBack.Models;

#nullable disable

namespace TTBack.Migrations
{
    [DbContext(typeof(TrekTogetherContext))]
    [Migration("20231014175655_Tada")]
    partial class Tada
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TTBack.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CarMake")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("carMake");

                    b.Property<string>("CarModel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("carModel");

                    b.Property<string>("CarYear")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("carYear")
                        .IsFixedLength();

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("TTBack.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ArrivalCity")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("arrivalCity");

                    b.Property<int?>("AvailableSeats")
                        .HasColumnType("int")
                        .HasColumnName("availableSeats");

                    b.Property<string>("DepartureCity")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("departureCity");

                    b.Property<DateTime?>("DepartureData")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("departureData");

                    b.Property<int?>("DriverId")
                        .HasColumnType("int")
                        .HasColumnName("driverId");

                    b.Property<int?>("Price")
                        .HasColumnType("int")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.ToTable("Trip", (string)null);
                });

            modelBuilder.Entity("TTBack.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool?>("IsDriver")
                        .HasColumnType("bit")
                        .HasColumnName("isDriver");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("phoneNumber");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("TTBack.Models.UserTrip", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TripId");

                    b.HasIndex("TripId");

                    b.ToTable("UserTrips");
                });

            modelBuilder.Entity("TTBack.Models.Car", b =>
                {
                    b.HasOne("TTBack.Models.User", "User")
                        .WithMany("Cars")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Car_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TTBack.Models.Trip", b =>
                {
                    b.HasOne("TTBack.Models.User", "Driver")
                        .WithMany("Trips")
                        .HasForeignKey("DriverId")
                        .HasConstraintName("FK_Trip_User");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("TTBack.Models.UserTrip", b =>
                {
                    b.HasOne("TTBack.Models.Trip", "Trip")
                        .WithMany("UserTrips")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TTBack.Models.User", "User")
                        .WithMany("UserTrips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TTBack.Models.Trip", b =>
                {
                    b.Navigation("UserTrips");
                });

            modelBuilder.Entity("TTBack.Models.User", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Trips");

                    b.Navigation("UserTrips");
                });
#pragma warning restore 612, 618
        }
    }
}
