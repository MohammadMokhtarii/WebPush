using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Segment");

            migrationBuilder.EnsureSchema(
                name: "Push");

            migrationBuilder.EnsureSchema(
                name: "Base");

            migrationBuilder.CreateSequence(
                name: "notificationseq",
                schema: "Push",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "subscriberseq",
                schema: "Segment",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "Base",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 50, nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_OutboxMessage", x => x.Id);
                },
                comment: "OutboxMessage");

            migrationBuilder.CreateTable(
                name: "Subscriber",
                schema: "Segment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    InActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segment_Subscriber", x => x.Id);
                },
                comment: "مشتریان");

            migrationBuilder.CreateTable(
                name: "Device",
                schema: "Segment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    ClientMetadata_OS = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PushManager_Auth = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PushManager_Endpoint = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PushManager_P256DH = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segment_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segment_Subscriber_Segment_Device",
                        column: x => x.SubscriberId,
                        principalSchema: "Segment",
                        principalTable: "Subscriber",
                        principalColumn: "Id");
                },
                comment: "دستگاه ها");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Push",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    NotificationStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Body_Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Body_Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Push_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segment_Device_Push_Notification",
                        column: x => x.DeviceId,
                        principalSchema: "Segment",
                        principalTable: "Device",
                        principalColumn: "Id");
                },
                comment: "پیام");

            migrationBuilder.CreateTable(
                name: "NotificationActivity",
                schema: "Push",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    NotificationStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Push_NotificationActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Push_Notification_Push_NotificationEvent",
                        column: x => x.NotificationId,
                        principalSchema: "Push",
                        principalTable: "Notification",
                        principalColumn: "Id");
                },
                comment: "لاگ پیام ");

            migrationBuilder.CreateTable(
                name: "NotificationEvent",
                schema: "Push",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    EventTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Push_NotificationEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Push_Notification_Push_NotificationActivity",
                        column: x => x.NotificationId,
                        principalSchema: "Push",
                        principalTable: "Notification",
                        principalColumn: "Id");
                },
                comment: "رویداد های پیام");

            migrationBuilder.CreateIndex(
                name: "IX_Device_SubscriberId",
                schema: "Segment",
                table: "Device",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DeviceId",
                schema: "Push",
                table: "Notification",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationActivity_NotificationId",
                schema: "Push",
                table: "NotificationActivity",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEvent_NotificationId",
                schema: "Push",
                table: "NotificationEvent",
                column: "NotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationActivity",
                schema: "Push");

            migrationBuilder.DropTable(
                name: "NotificationEvent",
                schema: "Push");

            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "Base");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Push");

            migrationBuilder.DropTable(
                name: "Device",
                schema: "Segment");

            migrationBuilder.DropTable(
                name: "Subscriber",
                schema: "Segment");

            migrationBuilder.DropSequence(
                name: "notificationseq",
                schema: "Push");

            migrationBuilder.DropSequence(
                name: "subscriberseq",
                schema: "Segment");
        }
    }
}
