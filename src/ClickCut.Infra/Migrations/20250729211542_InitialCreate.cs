﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickCut.Infra.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "User",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
					PasswordHash = table.Column<string>(type: "text", nullable: false),
					Username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_User", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Link",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false),
					Url = table.Column<string>(type: "text", nullable: false),
					UserId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Link", x => x.Id);
					table.ForeignKey(
						name: "FK_Link_User_UserId",
						column: x => x.UserId,
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Link_UserId",
				table: "Link",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Link");

			migrationBuilder.DropTable(
				name: "User");
		}
	}
}
