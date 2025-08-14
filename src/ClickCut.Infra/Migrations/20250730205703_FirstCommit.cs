using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickCut.Infra.Migrations
{
	/// <inheritdoc />
	public partial class FirstCommit : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Link_User_UserId",
				table: "Link");

			migrationBuilder.DropPrimaryKey(
				name: "PK_User",
				table: "User");

			migrationBuilder.RenameTable(
				name: "User",
				newName: "Users");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Users",
				table: "Users",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Link_Users_UserId",
				table: "Link",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Link_Users_UserId",
				table: "Link");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Users",
				table: "Users");

			migrationBuilder.RenameTable(
				name: "Users",
				newName: "User");

			migrationBuilder.AddPrimaryKey(
				name: "PK_User",
				table: "User",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Link_User_UserId",
				table: "Link",
				column: "UserId",
				principalTable: "User",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
