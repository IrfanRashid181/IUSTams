using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMSProj.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditorium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoriums",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Auditorium_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoriums", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Auditoriums_Floors_FloorID",
                        column: x => x.FloorID,
                        principalTable: "Floors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoriums_FloorID",
                table: "Auditoriums",
                column: "FloorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoriums");
        }
    }
}
