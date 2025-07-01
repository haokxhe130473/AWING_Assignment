using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PirateTreasure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitTreasureMapSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreasureMap",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    MaxChestValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasureMap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreasureCell",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Col = table.Column<int>(type: "int", nullable: false),
                    ChestValue = table.Column<int>(type: "int", nullable: false),
                    TreasureMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasureCell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasureCell_TreasureMap_TreasureMapId",
                        column: x => x.TreasureMapId,
                        principalTable: "TreasureMap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreasureCell_TreasureMapId",
                table: "TreasureCell",
                column: "TreasureMapId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreasureCell");

            migrationBuilder.DropTable(
                name: "TreasureMap");
        }
    }
}
