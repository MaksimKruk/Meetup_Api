using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetupApi.DataAccess.Migrations
{
    public partial class ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscribedUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribedUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventSubscribedUser",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    SubscribedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSubscribedUser", x => new { x.EventsId, x.SubscribedUsersId });
                    table.ForeignKey(
                        name: "FK_EventSubscribedUser_Event_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSubscribedUser_SubscribedUser_SubscribedUsersId",
                        column: x => x.SubscribedUsersId,
                        principalTable: "SubscribedUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSubscribedUser_SubscribedUsersId",
                table: "EventSubscribedUser",
                column: "SubscribedUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSubscribedUser");

            migrationBuilder.DropTable(
                name: "SubscribedUser");
        }
    }
}
