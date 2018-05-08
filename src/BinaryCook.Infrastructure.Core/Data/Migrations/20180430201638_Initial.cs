using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BinaryCook.Infrastructure.Core.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.CreateTable(
                name: "Ingredient",
                schema: "main",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Metadata_CreatedBy = table.Column<string>(nullable: false),
                    Metadata_CreatedDateAtUtc = table.Column<DateTime>(nullable: false),
                    Metadata_DeletedBy = table.Column<DateTime>(nullable: true),
                    Metadata_DeletedDateAtUtc = table.Column<DateTime>(nullable: true),
                    Metadata_UpdatedBy = table.Column<string>(nullable: true),
                    Metadata_UpdatedDateAtUtc = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                schema: "main",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    Description = table.Column<string>(maxLength: 2048, nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Image_Original = table.Column<string>(maxLength: 512, nullable: true),
                    Image_Thumbnail = table.Column<string>(maxLength: 512, nullable: true),
                    Metadata_CreatedBy = table.Column<string>(nullable: false),
                    Metadata_CreatedDateAtUtc = table.Column<DateTime>(nullable: false),
                    Metadata_DeletedBy = table.Column<DateTime>(nullable: true),
                    Metadata_DeletedDateAtUtc = table.Column<DateTime>(nullable: true),
                    Metadata_UpdatedBy = table.Column<string>(nullable: true),
                    Metadata_UpdatedDateAtUtc = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredient",
                schema: "main",
                columns: table => new
                {
                    IngredientId = table.Column<Guid>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredient", x => new { x.IngredientId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalSchema: "main",
                        principalTable: "Ingredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "main",
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeStep",
                schema: "main",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    DurationInSeconds = table.Column<double>(nullable: false),
                    Metadata_CreatedBy = table.Column<string>(nullable: false),
                    Metadata_CreatedDateAtUtc = table.Column<DateTime>(nullable: false),
                    Metadata_DeletedBy = table.Column<DateTime>(nullable: true),
                    Metadata_DeletedDateAtUtc = table.Column<DateTime>(nullable: true),
                    Metadata_UpdatedBy = table.Column<string>(nullable: true),
                    Metadata_UpdatedDateAtUtc = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeStep_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "main",
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_RecipeId",
                schema: "main",
                table: "RecipeIngredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeStep_RecipeId",
                schema: "main",
                table: "RecipeStep",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeIngredient",
                schema: "main");

            migrationBuilder.DropTable(
                name: "RecipeStep",
                schema: "main");

            migrationBuilder.DropTable(
                name: "Ingredient",
                schema: "main");

            migrationBuilder.DropTable(
                name: "Recipe",
                schema: "main");
        }
    }
}
