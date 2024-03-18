﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebsiteAdmin.Data;

#nullable disable

namespace WebsiteAdmin.Migrations
{
    [DbContext(typeof(WebsiteAdminContext))]
    [Migration("20240318050855_guid")]
    partial class guid
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebsiteAdmin.Models.Sach", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("giaTien")
                        .HasColumnType("float");

                    b.Property<string>("nxb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tacGia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tenSach")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sach");
                });

            modelBuilder.Entity("WebsiteAdmin.Models.SinhVien", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("diachi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dienthoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mssv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tensinhvien")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SinhVien");
                });
#pragma warning restore 612, 618
        }
    }
}