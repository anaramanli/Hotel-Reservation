using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservations", x => new { x.ReservationId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_UserReservations_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserReservations_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_AppUserId",
                table: "UserReservations",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReservations");
        }
    }
}
