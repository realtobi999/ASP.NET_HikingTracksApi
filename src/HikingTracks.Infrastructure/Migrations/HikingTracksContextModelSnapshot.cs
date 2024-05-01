﻿// <auto-generated />
using System;
using HikingTracks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HikingTracks.Infrastructure.Migrations
{
    [DbContext(typeof(HikingTracksContext))]
    partial class HikingTracksContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HikingTracks.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<Guid>("Token")
                        .HasColumnType("uuid")
                        .HasColumnName("token");

                    b.Property<double>("TotalDistance")
                        .HasColumnType("double precision")
                        .HasColumnName("total_distance");

                    b.Property<int>("TotalHikes")
                        .HasColumnType("integer")
                        .HasColumnName("total_hikes");

                    b.Property<TimeSpan>("TotalMovingTime")
                        .HasColumnType("interval")
                        .HasColumnName("total_moving_time");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("HikingTracks.Domain.Hike", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountID")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<double>("AverageSpeed")
                        .HasColumnType("double precision")
                        .HasColumnName("average_speed");

                    b.Property<string>("CoordinatesString")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("coordinates");

                    b.Property<double>("Distance")
                        .HasColumnType("double precision")
                        .HasColumnName("distance");

                    b.Property<double>("ElevationGain")
                        .HasColumnType("double precision")
                        .HasColumnName("elevation_gain");

                    b.Property<double>("ElevationLoss")
                        .HasColumnType("double precision")
                        .HasColumnName("elevation_loss");

                    b.Property<double>("MaxSpeed")
                        .HasColumnType("double precision")
                        .HasColumnName("max_speed");

                    b.Property<TimeSpan>("MovingTime")
                        .HasColumnType("interval")
                        .HasColumnName("moving_time");

                    b.Property<Guid?>("account_id")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("account_id");

                    b.ToTable("Hikes", t =>
                        {
                            t.Property("account_id")
                                .HasColumnName("account_id1");
                        });
                });

            modelBuilder.Entity("HikingTracks.Domain.Hike", b =>
                {
                    b.HasOne("HikingTracks.Domain.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("account_id");

                    b.Navigation("Account");
                });
#pragma warning restore 612, 618
        }
    }
}
