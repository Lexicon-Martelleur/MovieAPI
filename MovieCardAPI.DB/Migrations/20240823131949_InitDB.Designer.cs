﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieCardAPI.DB.Contexts;

#nullable disable

namespace MovieCardAPI.DB.Migrations
{
    [DbContext(typeof(MovieContext))]
    [Migration("20240823131949_InitDB")]
    partial class InitDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieCardAPI.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<long>("TimeStamp")
                        .HasMaxLength(1000)
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "d1",
                            Rating = 1,
                            TimeStamp = 1724414949L,
                            Title = "T1"
                        },
                        new
                        {
                            Id = 2,
                            Description = "d2",
                            Rating = 2,
                            TimeStamp = 1724414949L,
                            Title = "T2"
                        },
                        new
                        {
                            Id = 3,
                            Description = "d3",
                            Rating = 3,
                            TimeStamp = 1724414949L,
                            Title = "T3"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
