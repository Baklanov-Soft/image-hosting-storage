using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageHosting.Storage.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AddColumn<List<string>>(
                name: "Categories",
                table: "Images",
                type: "varchar(200)[]",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Images",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ForbiddenCategories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForbiddenCategories", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_Hidden",
                table: "Images",
                column: "Hidden");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForbiddenCategories");

            migrationBuilder.DropIndex(
                name: "IX_Images_Hidden",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Forbidden = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageCategories",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCategories", x => new { x.ImageId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ImageCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageCategories_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageCategories_CategoryId",
                table: "ImageCategories",
                column: "CategoryId");
        }
    }
}
