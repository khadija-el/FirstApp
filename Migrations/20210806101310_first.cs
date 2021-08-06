using Microsoft.EntityFrameworkCore.Migrations;

namespace FistApi.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelAr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profils", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Profils_IdProfil",
                        column: x => x.IdProfil,
                        principalTable: "Profils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Profils",
                columns: new[] { "Id", "Label", "LabelAr" },
                values: new object[,]
                {
                    { 1, "Administrateur", "مدير" },
                    { 2, "Superviseur", "مشرف" },
                    { 3, "Point focal", "	المخاطب الرئيسي" },
                    { 4, "Propriétaire", "مالك" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Adresse", "Email", "IdProfil", "Nom", "Password", "Prenom", "Username" },
                values: new object[,]
                {
                    { 1, "Temara", "mourabit@angular.io", 1, "mourabit", "123", "mohamed", "mourabit" },
                    { 2, "Temara", "mehdi@angular.io", 2, "mehdi", "123", "mehdi", "mehdi" },
                    { 4, "Temara", "soufiane@angular.io", 3, "soufiane", "123", "soufiane", "soufiane" },
                    { 3, "Temara", "ahmed@angular.io", 4, "ahmed", "123", "ahmed", "ahmed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdProfil",
                table: "Users",
                column: "IdProfil");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Profils");
        }
    }
}
