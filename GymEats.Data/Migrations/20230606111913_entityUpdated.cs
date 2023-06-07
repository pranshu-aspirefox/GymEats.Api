using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymEats.Data.Migrations
{
    public partial class entityUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_Survey_SurveyId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_SurveyQuestionMapping_SurveyQuestionMappingId",
                table: "Option");

            migrationBuilder.DropTable(
                name: "SurveyQuestionMapping");

            migrationBuilder.DropIndex(
                name: "IX_Option_QuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_SurveyId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "questionMappingId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "AssociatedWith",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "DietId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "Option");

            migrationBuilder.RenameColumn(
                name: "SurveyQuestionMappingId",
                table: "Option",
                newName: "SurveyQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Option_SurveyQuestionMappingId",
                table: "Option",
                newName: "IX_Option_SurveyQuestionId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedOn",
                table: "Survey",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "SurveyDiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DietId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyDiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyDiet_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyDiet_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyDiet_Diet_DietId",
                        column: x => x.DietId,
                        principalTable: "Diet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyDiet_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyQuestion_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyQuestion_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyQuestion_Survey_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Survey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DietId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyOption_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyOption_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyOption_Diet_DietId",
                        column: x => x.DietId,
                        principalTable: "Diet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyOption_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyOption_SurveyQuestion_SurveyQuestionId",
                        column: x => x.SurveyQuestionId,
                        principalTable: "SurveyQuestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyDiet_CreatedBy",
                table: "SurveyDiet",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyDiet_DietId",
                table: "SurveyDiet",
                column: "DietId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyDiet_OptionId",
                table: "SurveyDiet",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyDiet_UpdatedBy",
                table: "SurveyDiet",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOption_CreatedBy",
                table: "SurveyOption",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOption_DietId",
                table: "SurveyOption",
                column: "DietId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOption_OptionId",
                table: "SurveyOption",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOption_SurveyQuestionId",
                table: "SurveyOption",
                column: "SurveyQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOption_UpdatedBy",
                table: "SurveyOption",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_CreatedBy",
                table: "SurveyQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_QuestionId",
                table: "SurveyQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_SurveyId",
                table: "SurveyQuestion",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_UpdatedBy",
                table: "SurveyQuestion",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_SurveyQuestion_SurveyQuestionId",
                table: "Option",
                column: "SurveyQuestionId",
                principalTable: "SurveyQuestion",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_SurveyQuestion_SurveyQuestionId",
                table: "Option");

            migrationBuilder.DropTable(
                name: "SurveyDiet");

            migrationBuilder.DropTable(
                name: "SurveyOption");

            migrationBuilder.DropTable(
                name: "SurveyQuestion");

            migrationBuilder.RenameColumn(
                name: "SurveyQuestionId",
                table: "Option",
                newName: "SurveyQuestionMappingId");

            migrationBuilder.RenameIndex(
                name: "IX_Option_SurveyQuestionId",
                table: "Option",
                newName: "IX_Option_SurveyQuestionMappingId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedOn",
                table: "Survey",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyId",
                table: "Question",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "questionMappingId",
                table: "Question",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AssociatedWith",
                table: "Option",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "DietId",
                table: "Option",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "Option",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyId",
                table: "Option",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SurveyQuestionMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionMapping", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Option_QuestionId",
                table: "Option",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_SurveyId",
                table: "Option",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Survey_SurveyId",
                table: "Option",
                column: "SurveyId",
                principalTable: "Survey",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_SurveyQuestionMapping_SurveyQuestionMappingId",
                table: "Option",
                column: "SurveyQuestionMappingId",
                principalTable: "SurveyQuestionMapping",
                principalColumn: "Id");
        }
    }
}
