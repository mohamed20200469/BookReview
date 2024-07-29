using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReview.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role" },
                values: new object[] { "2abcc456-eb7a-4d44-b910-a7eea4a903ac", "mohamedmagdyy01@hotmail.com", "Idris", "$2b$10$tp08PQRVUrT5v97S8TNuD.ZK/uNMqmBfWLCxNo/HVCJmkK1bP2lbC", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2abcc456-eb7a-4d44-b910-a7eea4a903ac");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}
