using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace my_books.Migrations
{
    /// <inheritdoc />
    public partial class PublisherIdTypoFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Publishers",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Publishers",
                newName: "id");
        }
    }
}
