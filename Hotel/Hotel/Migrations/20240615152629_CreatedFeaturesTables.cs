using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class CreatedFeaturesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectsComplete = table.Column<int>(type: "int", nullable: false),
                    LuxuryRooms = table.Column<int>(type: "int", nullable: false),
                    Beaches = table.Column<int>(type: "int", nullable: false),
                    RegularGuests = table.Column<int>(type: "int", nullable: false),
                    FitnessCenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jacuzzi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SwimmingPool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SPATreatments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodRestaurants = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transportation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Features");
        }
    }
}
