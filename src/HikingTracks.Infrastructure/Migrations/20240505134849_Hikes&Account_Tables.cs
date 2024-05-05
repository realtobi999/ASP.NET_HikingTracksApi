﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HikingTracks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HikesAccount_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    token = table.Column<Guid>(type: "uuid", nullable: false),
                    total_hikes = table.Column<int>(type: "integer", nullable: false),
                    total_distance = table.Column<double>(type: "double precision", nullable: false),
                    total_moving_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Hikes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    distance = table.Column<double>(type: "double precision", nullable: false),
                    elevation_gain = table.Column<double>(type: "double precision", nullable: false),
                    elevation_loss = table.Column<double>(type: "double precision", nullable: false),
                    average_speed = table.Column<double>(type: "double precision", nullable: false),
                    max_speed = table.Column<double>(type: "double precision", nullable: false),
                    moving_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    coordinates = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hikes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Hikes_Accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hikes_account_id",
                table: "Hikes",
                column: "account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hikes");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
