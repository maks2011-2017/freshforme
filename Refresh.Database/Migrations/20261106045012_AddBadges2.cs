using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Refresh.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBadges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameUserUserBadge_UserBadges_BadgesId",
                table: "GameUserUserBadge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBadges",
                table: "UserBadges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameUserUserBadge",
                table: "GameUserUserBadge");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserBadges");

            migrationBuilder.DropColumn(
                name: "BadgesId",
                table: "GameUserUserBadge");

            migrationBuilder.AddColumn<string>(
                name: "BadgeId",
                table: "UserBadges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "UserBadges",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "BadgesBadgeId",
                table: "GameUserUserBadge",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBadges",
                table: "UserBadges",
                column: "BadgeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameUserUserBadge",
                table: "GameUserUserBadge",
                columns: new[] { "BadgesBadgeId", "UsersUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameUserUserBadge_UserBadges_BadgesBadgeId",
                table: "GameUserUserBadge",
                column: "BadgesBadgeId",
                principalTable: "UserBadges",
                principalColumn: "BadgeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameUserUserBadge_UserBadges_BadgesBadgeId",
                table: "GameUserUserBadge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBadges",
                table: "UserBadges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameUserUserBadge",
                table: "GameUserUserBadge");

            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "UserBadges");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserBadges");

            migrationBuilder.DropColumn(
                name: "BadgesBadgeId",
                table: "GameUserUserBadge");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserBadges",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "BadgesId",
                table: "GameUserUserBadge",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBadges",
                table: "UserBadges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameUserUserBadge",
                table: "GameUserUserBadge",
                columns: new[] { "BadgesId", "UsersUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameUserUserBadge_UserBadges_BadgesId",
                table: "GameUserUserBadge",
                column: "BadgesId",
                principalTable: "UserBadges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
