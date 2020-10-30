using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pudicitia.HR.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.CreateTable(
                name: "EventLog",
                schema: "Common",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    EventTypeNamespace = table.Column<string>(maxLength: 256, nullable: false),
                    EventTypeName = table.Column<string>(maxLength: 256, nullable: false),
                    EventContent = table.Column<string>(nullable: false),
                    PublishState = table.Column<int>(nullable: false),
                    PublishCount = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLog", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 32, nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 32, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    MaritalStatus = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    JobId = table.Column<Guid>(nullable: true)
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
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    JobTitleId = table.Column<Guid>(nullable: false),
                    StartOn = table.Column<DateTime>(nullable: false),
                    EndOn = table.Column<DateTime>(nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLog",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "HR");

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
