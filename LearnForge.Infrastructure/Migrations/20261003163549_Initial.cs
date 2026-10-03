using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:activity_type", "article,code_challenge,debug_challenge,multiple_choice,fill_in_blank,code_ordering,output_prediction")
                .Annotation("Npgsql:Enum:lesson_resource_type", "article,video,docs,repos,website")
                .Annotation("Npgsql:Enum:material_status", "draft,published");

            migrationBuilder.CreateTable(
                name: "activity_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lesson_resource_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_resource_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Settings_HeartsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_MaxHearts = table.Column<int>(type: "integer", nullable: false),
                    Settings_StreaksEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_WeeklyLeaderboardEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Settings_XpPerExercise = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.Id);
                    table.CheckConstraint("CK_Course_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.ForeignKey(
                        name: "FK_courses_material_statuses_Status",
                        column: x => x.Status,
                        principalTable: "material_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "course_modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_modules", x => x.Id);
                    table.CheckConstraint("CK_CourseModule_Order_Positive", "\"Order\" >= 1");
                    table.CheckConstraint("CK_CourseModule_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.ForeignKey(
                        name: "FK_course_modules_courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_course_modules_material_statuses_Status",
                        column: x => x.Status,
                        principalTable: "material_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ContentMarkdown = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessons", x => x.Id);
                    table.CheckConstraint("CK_Lesson_Order_Positive", "\"Order\" >= 1");
                    table.CheckConstraint("CK_Lesson_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.ForeignKey(
                        name: "FK_lessons_course_modules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "course_modules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_lessons_material_statuses_Status",
                        column: x => x.Status,
                        principalTable: "material_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    ContentJson = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{}"),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.Id);
                    table.CheckConstraint("CK_Activity_Order_Positive", "\"Order\" >= 1");
                    table.CheckConstraint("CK_Activity_Prompt_NotEmpty", "LENGTH(TRIM(\"Prompt\")) > 0");
                    table.CheckConstraint("CK_Activity_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.ForeignKey(
                        name: "FK_activities_activity_types_Type",
                        column: x => x.Type,
                        principalTable: "activity_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_activities_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_activities_material_statuses_Status",
                        column: x => x.Status,
                        principalTable: "material_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lesson_resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_resources", x => x.Id);
                    table.CheckConstraint("CK_LessonResource_Order_Positive", "\"Order\" >= 1");
                    table.CheckConstraint("CK_LessonResource_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.CheckConstraint("CK_LessonResource_Url_NotEmpty", "LENGTH(TRIM(\"Url\")) > 0");
                    table.ForeignKey(
                        name: "FK_lesson_resources_lesson_resource_types_Type",
                        column: x => x.Type,
                        principalTable: "lesson_resource_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lesson_resources_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_lesson_resources_material_statuses_Status",
                        column: x => x.Status,
                        principalTable: "material_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "activity_types",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 1, "Article" },
                    { 2, "CodeChallenge" },
                    { 3, "DebugChallenge" },
                    { 4, "MultipleChoice" },
                    { 5, "FillInBlank" },
                    { 6, "CodeOrdering" },
                    { 7, "OutputPrediction" }
                });

            migrationBuilder.InsertData(
                table: "lesson_resource_types",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 1, "Article" },
                    { 2, "Video" },
                    { 3, "Docs" },
                    { 4, "Repos" },
                    { 5, "Website" }
                });

            migrationBuilder.InsertData(
                table: "material_statuses",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Published" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_activities_DeletedAt",
                table: "activities",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_activities_LessonId_Order",
                table: "activities",
                columns: new[] { "LessonId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_activities_Status",
                table: "activities",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_activities_Type",
                table: "activities",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_activity_types_Code",
                table: "activity_types",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_modules_CourseId_Order",
                table: "course_modules",
                columns: new[] { "CourseId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_modules_DeletedAt",
                table: "course_modules",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_course_modules_Status",
                table: "course_modules",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_courses_DeletedAt",
                table: "courses",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_courses_InstructorId",
                table: "courses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_courses_Status",
                table: "courses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_resource_types_Code",
                table: "lesson_resource_types",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_resources_DeletedAt",
                table: "lesson_resources",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_resources_LessonId_Order",
                table: "lesson_resources",
                columns: new[] { "LessonId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_resources_Status",
                table: "lesson_resources",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_resources_Type",
                table: "lesson_resources",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_lessons_CourseModuleId_Order",
                table: "lessons",
                columns: new[] { "CourseModuleId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lessons_DeletedAt",
                table: "lessons",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_lessons_Status",
                table: "lessons",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_material_statuses_Code",
                table: "material_statuses",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "lesson_resources");

            migrationBuilder.DropTable(
                name: "activity_types");

            migrationBuilder.DropTable(
                name: "lesson_resource_types");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "course_modules");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "material_statuses");
        }
    }
}
