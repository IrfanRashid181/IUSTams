using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMSProj.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepttName",
                table: "Departments",
                newName: "Department_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Department_Name",
                table: "Departments",
                newName: "DepttName");
        }
    }
}
