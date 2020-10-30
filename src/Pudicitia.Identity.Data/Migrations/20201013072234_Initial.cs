using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pudicitia.Identity.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "Identity");

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
                name: "Permission",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 32, nullable: false),
                    Salt = table.Column<string>(maxLength: 64, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 32, nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Identity",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshToken",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RefreshToken = table.Column<string>(type: "char(24)", nullable: false),
                    ExpireOn = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Code",
                schema: "Identity",
                table: "Permission",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "Identity",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "Identity",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                schema: "Identity",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_RefreshToken",
                schema: "Identity",
                table: "UserRefreshToken",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_UserId",
                schema: "Identity",
                table: "UserRefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Identity",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                schema: "Identity",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLog",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRefreshToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Identity");
        }
    }
}
