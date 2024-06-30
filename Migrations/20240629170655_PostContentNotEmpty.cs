using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class PostContentNotEmpty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "character varying(280)",
                maxLength: 280,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(280)",
                oldMaxLength: 280,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "character varying(280)",
                maxLength: 280,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(280)",
                oldMaxLength: 280);
        }
    }
}
