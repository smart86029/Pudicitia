using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pudicitia.HR.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventPublished",
                schema: "Common",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeNamespace = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EventTypeName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EventContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishState = table.Column<int>(type: "int", nullable: false),
                    PublishCount = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPublished", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "EventSubscribed",
                schema: "Common",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTypeName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EventContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSubscribed", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobChange",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobChange_Person_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentId",
                schema: "HR",
                table: "Department",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobChange_DepartmentId",
                schema: "HR",
                table: "JobChange",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_JobChange_EmployeeId",
                schema: "HR",
                table: "JobChange",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobChange_JobTitleId",
                schema: "HR",
                table: "JobChange",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_DepartmentId",
                schema: "HR",
                table: "Person",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_JobId",
                schema: "HR",
                table: "Person",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EventPublished",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "EventSubscribed",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Job",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "JobChange",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "HR");
        }
    }
}
