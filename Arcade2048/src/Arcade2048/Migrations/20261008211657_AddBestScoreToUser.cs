using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arcade2048.Migrations
{
    /// <inheritdoc />
    public partial class AddBestScoreToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leaderboard_entries");

            migrationBuilder.DropTable(
                name: "scores");

            migrationBuilder.AddColumn<int>(
                name: "best_score",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "best_score_achieved_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_best_score_best_score_achieved_at",
                table: "users",
                columns: new[] { "best_score", "best_score_achieved_at" },
                descending: new[] { true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_best_score_best_score_achieved_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "best_score",
                table: "users");

            migrationBuilder.DropColumn(
                name: "best_score_achieved_at",
                table: "users");

            migrationBuilder.CreateTable(
                name: "leaderboard_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    best_score = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    last_modified_by = table.Column<string>(type: "text", nullable: true),
                    last_modified_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_leaderboard_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scores",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    achieved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_best_score = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    last_modified_by = table.Column<string>(type: "text", nullable: true),
                    last_modified_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scores", x => x.id);
                });
        }
    }
}
