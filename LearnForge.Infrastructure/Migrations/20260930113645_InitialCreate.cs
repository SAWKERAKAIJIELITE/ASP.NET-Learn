using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:activity_type", "article,code_challenge,debug_challenge,multiple_choice,fill_in_blank,code_ordering,output_prediction")
                .Annotation("Npgsql:Enum:lesson_resource_type", "article,video,docs,repos,website")
                .Annotation("Npgsql:Enum:material_status", "draft,published");

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
                    Status = table.Column<int>(type: "material_status", nullable: false, defaultValueSql: "'draft'::material_status")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.Id);
                    table.CheckConstraint("CK_Course_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
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
                    Status = table.Column<int>(type: "material_status", nullable: false, defaultValueSql: "'draft'::material_status"),
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
                    Status = table.Column<int>(type: "material_status", nullable: false, defaultValueSql: "'draft'::material_status"),
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
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "activity_type", nullable: false),
                    Prompt = table.Column<string>(type: "text", nullable: false),
                    ContentJson = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{}"),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "material_status", nullable: false, defaultValueSql: "'draft'::material_status"),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.Id);
                    table.CheckConstraint("CK_Activity_Order_Positive", "\"Order\" >= 1");
                    table.CheckConstraint("CK_Activity_Prompt_NotEmpty", "LENGTH(TRIM(\"Prompt\")) > 0");
                    table.CheckConstraint("CK_Activity_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.ForeignKey(
                        name: "FK_activities_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "lesson_resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Type = table.Column<int>(type: "lesson_resource_type", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "material_status", nullable: false, defaultValueSql: "'draft'::material_status"),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_resources", x => x.Id);
                    table.CheckConstraint("CK_LessonResource_Order_Positive", "\"Order\" >= 1");
                    table.CheckConstraint("CK_LessonResource_Title_NotEmpty", "LENGTH(TRIM(\"Title\")) > 0");
                    table.CheckConstraint("CK_LessonResource_Url_NotEmpty", "LENGTH(TRIM(\"Url\")) > 0");
                    table.ForeignKey(
                        name: "FK_lesson_resources_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lessons",
                        principalColumn: "Id");
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
                name: "IX_course_modules_CourseId_Order",
                table: "course_modules",
                columns: new[] { "CourseId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_modules_DeletedAt",
                table: "course_modules",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_courses_DeletedAt",
                table: "courses",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_courses_InstructorId",
                table: "courses",
                column: "InstructorId");

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
                name: "IX_lessons_CourseModuleId_Order",
                table: "lessons",
                columns: new[] { "CourseModuleId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lessons_DeletedAt",
                table: "lessons",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "lesson_resources");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "course_modules");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
