﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebScraper;

#nullable disable

namespace WebScraper.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250115182944_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("WebScraper.GoldDataEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RialPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GoldDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
