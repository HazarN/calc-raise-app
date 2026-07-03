using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CalcRaise.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Methods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeekStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    WeekNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutWeeks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    MethodId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Methods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseMuscleGroups",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    MuscleGroupId = table.Column<int>(type: "integer", nullable: false),
                    Involvement = table.Column<int>(type: "integer", nullable: false),
                    InvolvementPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscleGroups", x => new { x.ExerciseId, x.MuscleGroupId });
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleGroups_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleGroups_MuscleGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgramExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramDayId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TargetSetsMin = table.Column<int>(type: "integer", nullable: false),
                    TargetSetsMax = table.Column<int>(type: "integer", nullable: false),
                    TargetRepsMin = table.Column<int>(type: "integer", nullable: false),
                    TargetRepsMax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_ProgramDays_ProgramDayId",
                        column: x => x.ProgramDayId,
                        principalTable: "ProgramDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSetLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramExerciseId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutWeekId = table.Column<int>(type: "integer", nullable: false),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Rir = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSetLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSetLogs_ProgramExercises_ProgramExerciseId",
                        column: x => x.ProgramExerciseId,
                        principalTable: "ProgramExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutSetLogs_WorkoutWeeks_WorkoutWeekId",
                        column: x => x.WorkoutWeekId,
                        principalTable: "WorkoutWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleGroups_MuscleGroupId",
                table: "ExerciseMuscleGroups",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MethodId",
                table: "Exercises",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Methods_Name",
                table: "Methods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MuscleGroups_Name",
                table: "MuscleGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDays_DayOfWeek",
                table: "ProgramDays",
                column: "DayOfWeek");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ExerciseId",
                table: "ProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ProgramDayId_Order",
                table: "ProgramExercises",
                columns: new[] { "ProgramDayId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetLogs_ProgramExerciseId_WorkoutWeekId_SetNumber",
                table: "WorkoutSetLogs",
                columns: new[] { "ProgramExerciseId", "WorkoutWeekId", "SetNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetLogs_WorkoutWeekId",
                table: "WorkoutSetLogs",
                column: "WorkoutWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutWeeks_WeekStartDate",
                table: "WorkoutWeeks",
                column: "WeekStartDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseMuscleGroups");

            migrationBuilder.DropTable(
                name: "WorkoutSetLogs");

            migrationBuilder.DropTable(
                name: "MuscleGroups");

            migrationBuilder.DropTable(
                name: "ProgramExercises");

            migrationBuilder.DropTable(
                name: "WorkoutWeeks");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "ProgramDays");

            migrationBuilder.DropTable(
                name: "Methods");
        }
    }
}
