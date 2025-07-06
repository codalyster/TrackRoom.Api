using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrackRoom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionIdPropertyForTheMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "271a8fb0-72c1-4c0a-951d-777e739030a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fecc2d3-0ace-4cbd-b996-a3ac0166749d");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0eb1433d-a383-45d8-8e97-502ee2624b21", "2", "User", "User" },
                    { "67034602-2f47-4b08-8863-c28552bb11ab", "1", "Host", "Host" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0eb1433d-a383-45d8-8e97-502ee2624b21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67034602-2f47-4b08-8863-c28552bb11ab");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Members");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "271a8fb0-72c1-4c0a-951d-777e739030a9", "1", "Host", "Host" },
                    { "2fecc2d3-0ace-4cbd-b996-a3ac0166749d", "2", "User", "User" }
                });
        }
    }
}
