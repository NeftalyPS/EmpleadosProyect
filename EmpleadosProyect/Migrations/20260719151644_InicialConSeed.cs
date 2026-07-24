using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmpleadosProyect.Migrations
{
    /// <inheritdoc />
    public partial class InicialConSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FotoRuta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaContratacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaTerminacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Departamentos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Recursos Humanos" },
                    { 2, "Tecnología" },
                    { 3, "Ventas" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "PasswordHash", "Rol", "Username" },
                values: new object[,]
                {
                    { 1, "$2a$11$wCbMnsaOpoFaL.Q11ya1UuKiyO3KzvaxC4CDRGgWOIrnnXcr4ib26", "Admin", "admin" },
                    { 2, "$2a$11$KN3dBt5TLjHlaSWuS/2FDel1dNvuSF6Cpt9qtVeYzcMdlpwWWyYge", "RRHH", "rrhh" }
                });

            migrationBuilder.InsertData(
                table: "Empleados",
                columns: new[] { "Id", "Apellido", "DepartamentoId", "Direccion", "Email", "Estado", "FechaContratacion", "FechaNacimiento", "FechaTerminacion", "FotoRuta", "Nombre", "Puesto", "Salario", "Telefono" },
                values: new object[,]
                {
                    { 1, "Apellido1", 2, "Calle 1, Ciudad", "empleado1@empresa.com", "Activo", new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado1", "Coordinador", 26000m, "809-555-0001" },
                    { 2, "Apellido2", 3, "Calle 2, Ciudad", "empleado2@empresa.com", "Activo", new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado2", "Analista", 27000m, "809-555-0002" },
                    { 3, "Apellido3", 1, "Calle 3, Ciudad", "empleado3@empresa.com", "Activo", new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado3", "Coordinador", 28000m, "809-555-0003" },
                    { 4, "Apellido4", 2, "Calle 4, Ciudad", "empleado4@empresa.com", "Activo", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado4", "Analista", 29000m, "809-555-0004" },
                    { 5, "Apellido5", 3, "Calle 5, Ciudad", "empleado5@empresa.com", "Activo", new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado5", "Coordinador", 30000m, "809-555-0005" },
                    { 6, "Apellido6", 1, "Calle 6, Ciudad", "empleado6@empresa.com", "Activo", new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado6", "Analista", 31000m, "809-555-0006" },
                    { 7, "Apellido7", 2, "Calle 7, Ciudad", "empleado7@empresa.com", "Activo", new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado7", "Coordinador", 32000m, "809-555-0007" },
                    { 8, "Apellido8", 3, "Calle 8, Ciudad", "empleado8@empresa.com", "Activo", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado8", "Analista", 33000m, "809-555-0008" },
                    { 9, "Apellido9", 1, "Calle 9, Ciudad", "empleado9@empresa.com", "Activo", new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado9", "Coordinador", 34000m, "809-555-0009" },
                    { 10, "Apellido10", 2, "Calle 10, Ciudad", "empleado10@empresa.com", "Activo", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Empleado10", "Analista", 35000m, "809-555-0010" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_DepartamentoId",
                table: "Empleados",
                column: "DepartamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Departamentos");
        }
    }
}
