using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SupportTicket.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AssigneeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 20, 10, 0, 0, 0, DateTimeKind.Utc), "alice@support.com", "Alice Agent" },
                    { 2, new DateTime(2026, 7, 20, 10, 0, 0, 0, DateTimeKind.Utc), "bob@support.com", "Bob Agent" },
                    { 3, new DateTime(2026, 7, 20, 10, 0, 0, 0, DateTimeKind.Utc), "carol@support.com", "Carol Manager" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssigneeId", "CreatedAt", "Description", "Priority", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 7, 20, 10, 0, 0, 0, DateTimeKind.Utc), "User reports 401 error on login page after password reset.", 2, 0, "Cannot login to portal", new DateTime(2026, 7, 20, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, new DateTime(2026, 7, 20, 11, 0, 0, 0, DateTimeKind.Utc), "Office printer on floor 3 is not responding to print jobs.", 1, 1, "Printer not working", new DateTime(2026, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 1, new DateTime(2026, 7, 20, 13, 0, 0, 0, DateTimeKind.Utc), "Outlook emails are delayed by 15-20 minutes.", 0, 2, "Email sync delay", new DateTime(2026, 7, 20, 15, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 3, new DateTime(2026, 7, 19, 10, 0, 0, 0, DateTimeKind.Utc), "VPN disconnects every 30 minutes for remote users.", 2, 3, "VPN connection drops", new DateTime(2026, 7, 20, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 2, new DateTime(2026, 7, 18, 10, 0, 0, 0, DateTimeKind.Utc), "Employee requested a 27-inch monitor for home office setup.", 0, 4, "Request new monitor", new DateTime(2026, 7, 20, 11, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Body", "CreatedAt", "TicketId" },
                values: new object[,]
                {
                    { 1, 2, "Checking authentication logs for this user account.", new DateTime(2026, 7, 20, 10, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, 2, "Restarted the print spooler service. Monitoring.", new DateTime(2026, 7, 20, 12, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, 1, "Mail queue was backed up. Cleared and sync is normal now.", new DateTime(2026, 7, 20, 15, 0, 0, 0, DateTimeKind.Utc), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssigneeId",
                table: "Tickets",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status",
                table: "Tickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
