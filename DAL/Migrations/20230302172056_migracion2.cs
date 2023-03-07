using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    public partial class migracion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dwh_torrecontrol");

            migrationBuilder.CreateTable(
                name: "tdc_cat_estados_devolucion_pedido",
                schema: "dwh_torrecontrol",
                columns: table => new
                {
                    Cod_estado_devolucion = table.Column<string>(type: "text", nullable: false),
                    Md_uuid = table.Column<string>(type: "text", nullable: false),
                    Md_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Des_estado_devolucion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tdc_cat_estados_devolucion_pedido", x => x.Cod_estado_devolucion);
                });

            migrationBuilder.CreateTable(
                name: "Tdc_cat_estados_envio_pedido",
                schema: "dwh_torrecontrol",
                columns: table => new
                {
                    Cod_estado_envio = table.Column<string>(type: "text", nullable: false),
                    Md_uuid = table.Column<string>(type: "text", nullable: false),
                    Md_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Des_estado_envio = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tdc_cat_estados_envio_pedido", x => x.Cod_estado_envio);
                });

            migrationBuilder.CreateTable(
                name: "Tdc_cat_estados_pago_pedido",
                schema: "dwh_torrecontrol",
                columns: table => new
                {
                    Cod_estado_pago = table.Column<string>(type: "text", nullable: false),
                    Md_uuid = table.Column<string>(type: "text", nullable: false),
                    Md_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Des_estado_pago = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tdc_cat_estados_pago_pedido", x => x.Cod_estado_pago);
                });

            migrationBuilder.CreateTable(
                name: "Tdc_cat_lineas_distribucion",
                schema: "dwh_torrecontrol",
                columns: table => new
                {
                    Cod_linea = table.Column<string>(type: "text", nullable: false),
                    Md_uuid = table.Column<string>(type: "text", nullable: false),
                    Md_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cod_provincia = table.Column<string>(type: "text", nullable: false),
                    Cod_municipio = table.Column<string>(type: "text", nullable: false),
                    Cod_barrio = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tdc_cat_lineas_distribucion", x => x.Cod_linea);
                });

            migrationBuilder.CreateTable(
                name: "Tdc_tch_estado_pedidos",
                schema: "dwh_torrecontrol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Md_uuid = table.Column<string>(type: "text", nullable: false),
                    Md_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Cod_estado_envio = table.Column<string>(type: "text", nullable: true),
                    Cod_estado_pago = table.Column<string>(type: "text", nullable: true),
                    Cod_estado_devolucion = table.Column<string>(type: "text", nullable: true),
                    Cod_pedido = table.Column<string>(type: "text", nullable: false),
                    Cod_linea = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tdc_tch_estado_pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tdc_tch_estado_pedidos_tdc_cat_estados_devolucion_pedido_Co~",
                        column: x => x.Cod_estado_devolucion,
                        principalSchema: "dwh_torrecontrol",
                        principalTable: "tdc_cat_estados_devolucion_pedido",
                        principalColumn: "Cod_estado_devolucion");
                    table.ForeignKey(
                        name: "FK_Tdc_tch_estado_pedidos_Tdc_cat_estados_envio_pedido_Cod_est~",
                        column: x => x.Cod_estado_envio,
                        principalSchema: "dwh_torrecontrol",
                        principalTable: "Tdc_cat_estados_envio_pedido",
                        principalColumn: "Cod_estado_envio");
                    table.ForeignKey(
                        name: "FK_Tdc_tch_estado_pedidos_Tdc_cat_estados_pago_pedido_Cod_esta~",
                        column: x => x.Cod_estado_pago,
                        principalSchema: "dwh_torrecontrol",
                        principalTable: "Tdc_cat_estados_pago_pedido",
                        principalColumn: "Cod_estado_pago");
                    table.ForeignKey(
                        name: "FK_Tdc_tch_estado_pedidos_Tdc_cat_lineas_distribucion_Cod_linea",
                        column: x => x.Cod_linea,
                        principalSchema: "dwh_torrecontrol",
                        principalTable: "Tdc_cat_lineas_distribucion",
                        principalColumn: "Cod_linea",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tdc_tch_estado_pedidos_Cod_estado_devolucion",
                schema: "dwh_torrecontrol",
                table: "Tdc_tch_estado_pedidos",
                column: "Cod_estado_devolucion");

            migrationBuilder.CreateIndex(
                name: "IX_Tdc_tch_estado_pedidos_Cod_estado_envio",
                schema: "dwh_torrecontrol",
                table: "Tdc_tch_estado_pedidos",
                column: "Cod_estado_envio");

            migrationBuilder.CreateIndex(
                name: "IX_Tdc_tch_estado_pedidos_Cod_estado_pago",
                schema: "dwh_torrecontrol",
                table: "Tdc_tch_estado_pedidos",
                column: "Cod_estado_pago");

            migrationBuilder.CreateIndex(
                name: "IX_Tdc_tch_estado_pedidos_Cod_linea",
                schema: "dwh_torrecontrol",
                table: "Tdc_tch_estado_pedidos",
                column: "Cod_linea");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tdc_tch_estado_pedidos",
                schema: "dwh_torrecontrol");

            migrationBuilder.DropTable(
                name: "tdc_cat_estados_devolucion_pedido",
                schema: "dwh_torrecontrol");

            migrationBuilder.DropTable(
                name: "Tdc_cat_estados_envio_pedido",
                schema: "dwh_torrecontrol");

            migrationBuilder.DropTable(
                name: "Tdc_cat_estados_pago_pedido",
                schema: "dwh_torrecontrol");

            migrationBuilder.DropTable(
                name: "Tdc_cat_lineas_distribucion",
                schema: "dwh_torrecontrol");
        }
    }
}
