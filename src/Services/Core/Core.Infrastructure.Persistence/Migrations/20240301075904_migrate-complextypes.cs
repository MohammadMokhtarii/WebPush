using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migratecomplextypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientMetadata",
                schema: "Segment",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PushManager",
                schema: "Segment",
                table: "Device");

            migrationBuilder.AddColumn<string>(
                name: "ClientMetadata_OS",
                schema: "Segment",
                table: "Device",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PushManager_Auth",
                schema: "Segment",
                table: "Device",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PushManager_Endpoint",
                schema: "Segment",
                table: "Device",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PushManager_P256DH",
                schema: "Segment",
                table: "Device",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientMetadata_OS",
                schema: "Segment",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PushManager_Auth",
                schema: "Segment",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PushManager_Endpoint",
                schema: "Segment",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PushManager_P256DH",
                schema: "Segment",
                table: "Device");

            migrationBuilder.AddColumn<string>(
                name: "ClientMetadata",
                schema: "Segment",
                table: "Device",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PushManager",
                schema: "Segment",
                table: "Device",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
