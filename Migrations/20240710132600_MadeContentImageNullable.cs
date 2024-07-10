using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class MadeContentImageNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Images_ImageId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Images_ImageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfilePictureImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureImageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Posts",
                newName: "ImageID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ImageId",
                table: "Posts",
                newName: "IX_Posts_ImageID");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Comments",
                newName: "ImageID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ImageId",
                table: "Comments",
                newName: "IX_Comments_ImageID");

            migrationBuilder.AlterColumn<int>(
                name: "ImageID",
                table: "Posts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ImageID",
                table: "Comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfilePictureId",
                table: "AspNetUsers",
                column: "ProfilePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureId",
                table: "AspNetUsers",
                column: "ProfilePictureId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Images_ImageID",
                table: "Comments",
                column: "ImageID",
                principalTable: "Images",
                principalColumn: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Images_ImageID",
                table: "Posts",
                column: "ImageID",
                principalTable: "Images",
                principalColumn: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Images_ImageID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Images_ImageID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfilePictureId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImageID",
                table: "Posts",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ImageID",
                table: "Posts",
                newName: "IX_Posts_ImageId");

            migrationBuilder.RenameColumn(
                name: "ImageID",
                table: "Comments",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ImageID",
                table: "Comments",
                newName: "IX_Comments_ImageId");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureImageId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfilePictureImageId",
                table: "AspNetUsers",
                column: "ProfilePictureImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ProfilePictureImageId",
                table: "AspNetUsers",
                column: "ProfilePictureImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Images_ImageId",
                table: "Comments",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Images_ImageId",
                table: "Posts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
