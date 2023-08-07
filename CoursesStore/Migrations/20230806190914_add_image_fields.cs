using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesStore.Migrations
{
    /// <inheritdoc />
    public partial class add_image_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CourseImage",
                table: "Course",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseImage",
                table: "Course");
        }
    }
}
