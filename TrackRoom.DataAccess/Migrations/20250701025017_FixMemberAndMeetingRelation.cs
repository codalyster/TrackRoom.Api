using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrackRoom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixMemberAndMeetingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b34afd4-3c2c-4c94-af02-534183cdb6a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3cdca47-0f7f-4c08-89a5-5f5ca520f47a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b9a4d90-19c5-4ee1-aa29-ddac49adf38b", "1", "Host", "Host" },
                    { "a6c7db50-0042-4252-b700-ec313d8602b0", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b9a4d90-19c5-4ee1-aa29-ddac49adf38b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6c7db50-0042-4252-b700-ec313d8602b0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b34afd4-3c2c-4c94-af02-534183cdb6a7", "1", "Host", "Host" },
                    { "f3cdca47-0f7f-4c08-89a5-5f5ca520f47a", "2", "User", "User" }
                });
        }
    }
}
