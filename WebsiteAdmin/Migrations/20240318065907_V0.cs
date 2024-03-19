﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteAdmin.Migrations
{
    /// <inheritdoc />
    public partial class V0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sach",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tenSach = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tacGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    giaTien = table.Column<double>(type: "float", nullable: true),
                    nxb = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinhVien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tensinhvien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mssv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dienthoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diachi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinhVienSach",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngaymuon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngaytra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVienSach", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sach");

            migrationBuilder.DropTable(
                name: "SinhVien");

            migrationBuilder.DropTable(
                name: "SinhVienSach");
        }
    }
}