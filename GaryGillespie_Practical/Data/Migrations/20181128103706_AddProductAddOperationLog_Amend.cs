using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GaryGillespie_Practical.Data.Migrations
{
    public partial class AddProductAddOperationLog_Amend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Logs_LogId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "LogId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Logs_LogId",
                table: "Products",
                column: "LogId",
                principalTable: "Logs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Logs_LogId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "LogId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Logs_LogId",
                table: "Products",
                column: "LogId",
                principalTable: "Logs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
