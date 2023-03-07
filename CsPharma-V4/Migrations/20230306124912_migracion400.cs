using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsPharma_V4.Migrations
{
    public partial class migracion400 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidosUsuario",
                schema: "dlk_torrecontrol",
                table: "Dlk_cat_acc_empleados");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                schema: "dlk_torrecontrol",
                table: "Dlk_cat_acc_empleados");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApellidosUsuario",
                schema: "dlk_torrecontrol",
                table: "Dlk_cat_acc_empleados",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                schema: "dlk_torrecontrol",
                table: "Dlk_cat_acc_empleados",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
