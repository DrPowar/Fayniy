using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesStore.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDurationAddEffectCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Course");

            migrationBuilder.AddColumn<int>(
                name: "EffectCount",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectCount",
                table: "Course");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Course",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
