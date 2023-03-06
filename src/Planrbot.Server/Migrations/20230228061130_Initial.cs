﻿using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planrbot.Server.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "ToDoItems",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Date = table.Column<DateTime>(type: "datetime2", nullable: false),
				Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
				IsComplete = table.Column<bool>(type: "bit", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_ToDoItems", x => x.Id);
			});

		migrationBuilder.CreateIndex(
			name: "IX_ToDoItems_Id",
			table: "ToDoItems",
			column: "Id",
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "ToDoItems");
	}
}