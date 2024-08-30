using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCardAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalUpdatedFieldToMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Updated",
                table: "Movies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Movies");
        }
    }
}
