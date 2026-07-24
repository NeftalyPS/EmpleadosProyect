using System;
using EmpleadosApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmpleadosProyect.Migrations
{
    [DbContext(typeof(EmpleadosDbContext))]
    partial class EmpleadosDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmpleadosApp.Models.Entities.Departamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Departamentos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "Recursos Humanos"
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "Tecnología"
                        },
                        new
                        {
                            Id = 3,
                            Nombre = "Ventas"
                        });
                });

            modelBuilder.Entity("EmpleadosApp.Models.Entities.Empleado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DepartamentoId")
                        .HasColumnType("int");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaContratacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaTerminacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("FotoRuta")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Puesto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Salario")
                        .HasColumnType("decimal(12,2)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("DepartamentoId");

                    b.ToTable("Empleados");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Apellido = "Apellido1",
                            DepartamentoId = 2,
                            Direccion = "Calle 1, Ciudad",
                            Email = "empleado1@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1991, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado1",
                            Puesto = "Coordinador",
                            Salario = 26000m,
                            Telefono = "809-555-0001"
                        },
                        new
                        {
                            Id = 2,
                            Apellido = "Apellido2",
                            DepartamentoId = 3,
                            Direccion = "Calle 2, Ciudad",
                            Email = "empleado2@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1992, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado2",
                            Puesto = "Analista",
                            Salario = 27000m,
                            Telefono = "809-555-0002"
                        },
                        new
                        {
                            Id = 3,
                            Apellido = "Apellido3",
                            DepartamentoId = 1,
                            Direccion = "Calle 3, Ciudad",
                            Email = "empleado3@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1993, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado3",
                            Puesto = "Coordinador",
                            Salario = 28000m,
                            Telefono = "809-555-0003"
                        },
                        new
                        {
                            Id = 4,
                            Apellido = "Apellido4",
                            DepartamentoId = 2,
                            Direccion = "Calle 4, Ciudad",
                            Email = "empleado4@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1994, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado4",
                            Puesto = "Analista",
                            Salario = 29000m,
                            Telefono = "809-555-0004"
                        },
                        new
                        {
                            Id = 5,
                            Apellido = "Apellido5",
                            DepartamentoId = 3,
                            Direccion = "Calle 5, Ciudad",
                            Email = "empleado5@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1995, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado5",
                            Puesto = "Coordinador",
                            Salario = 30000m,
                            Telefono = "809-555-0005"
                        },
                        new
                        {
                            Id = 6,
                            Apellido = "Apellido6",
                            DepartamentoId = 1,
                            Direccion = "Calle 6, Ciudad",
                            Email = "empleado6@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado6",
                            Puesto = "Analista",
                            Salario = 31000m,
                            Telefono = "809-555-0006"
                        },
                        new
                        {
                            Id = 7,
                            Apellido = "Apellido7",
                            DepartamentoId = 2,
                            Direccion = "Calle 7, Ciudad",
                            Email = "empleado7@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1997, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado7",
                            Puesto = "Coordinador",
                            Salario = 32000m,
                            Telefono = "809-555-0007"
                        },
                        new
                        {
                            Id = 8,
                            Apellido = "Apellido8",
                            DepartamentoId = 3,
                            Direccion = "Calle 8, Ciudad",
                            Email = "empleado8@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1998, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado8",
                            Puesto = "Analista",
                            Salario = 33000m,
                            Telefono = "809-555-0008"
                        },
                        new
                        {
                            Id = 9,
                            Apellido = "Apellido9",
                            DepartamentoId = 1,
                            Direccion = "Calle 9, Ciudad",
                            Email = "empleado9@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(1999, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado9",
                            Puesto = "Coordinador",
                            Salario = 34000m,
                            Telefono = "809-555-0009"
                        },
                        new
                        {
                            Id = 10,
                            Apellido = "Apellido10",
                            DepartamentoId = 2,
                            Direccion = "Calle 10, Ciudad",
                            Email = "empleado10@empresa.com",
                            Estado = "Activo",
                            FechaContratacion = new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaNacimiento = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Nombre = "Empleado10",
                            Puesto = "Analista",
                            Salario = 35000m,
                            Telefono = "809-555-0010"
                        });
                });

            modelBuilder.Entity("EmpleadosApp.Models.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PasswordHash = "$2a$11$wCbMnsaOpoFaL.Q11ya1UuKiyO3KzvaxC4CDRGgWOIrnnXcr4ib26",
                            Rol = "Admin",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            PasswordHash = "$2a$11$KN3dBt5TLjHlaSWuS/2FDel1dNvuSF6Cpt9qtVeYzcMdlpwWWyYge",
                            Rol = "RRHH",
                            Username = "rrhh"
                        });
                });

            modelBuilder.Entity("EmpleadosApp.Models.Entities.Empleado", b =>
                {
                    b.HasOne("EmpleadosApp.Models.Entities.Departamento", "Departamento")
                        .WithMany("Empleados")
                        .HasForeignKey("DepartamentoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Departamento");
                });

            modelBuilder.Entity("EmpleadosApp.Models.Entities.Departamento", b =>
                {
                    b.Navigation("Empleados");
                });
#pragma warning restore 612, 618
        }
    }
}
