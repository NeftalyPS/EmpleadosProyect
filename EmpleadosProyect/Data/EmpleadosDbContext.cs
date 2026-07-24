using EmpleadosApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpleadosApp.Data
{
    public class EmpleadosDbContext : DbContext
    {
        public EmpleadosDbContext(DbContextOptions<EmpleadosDbContext> options) : base(options) { }

        public DbSet<Empleado> Empleados => Set<Empleado>();
        public DbSet<Departamento> Departamentos => Set<Departamento>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Departamento)
                .WithMany(d => d.Empleados)
                .HasForeignKey(e => e.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.Estado)
                .HasConversion<string>();

         
            modelBuilder.Entity<Departamento>().HasData(
                new Departamento { Id = 1, Nombre = "Recursos Humanos" },
                new Departamento { Id = 2, Nombre = "Tecnología" },
                new Departamento { Id = 3, Nombre = "Ventas" }
            );

            // ---- Seed: Usuarios (password = "Admin123!" y "Rrhh123!") ----
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Rol = "Admin"
                },
                new Usuario
                {
                    Id = 2,
                    Username = "rrhh",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Rrhh123!"),
                    Rol = "RRHH"
                }
            );

           
            var fechaBase = new DateTime(2020, 1, 1);
            var empleados = new List<Empleado>();
            for (int i = 1; i <= 10; i++)
            {
                empleados.Add(new Empleado
                {
                    Id = i,
                    Nombre = $"Empleado{i}",
                    Apellido = $"Apellido{i}",
                    DepartamentoId = (i % 3) + 1,
                    Puesto = i % 2 == 0 ? "Analista" : "Coordinador",
                    Salario = 25000 + (i * 1000),
                    FechaNacimiento = new DateTime(1990, 1, 1).AddYears(i),
                    FechaContratacion = fechaBase.AddMonths(i),
                    Direccion = $"Calle {i}, Ciudad",
                    Telefono = $"809-555-00{i:00}",
                    Email = $"empleado{i}@empresa.com",
                    Estado = EstadoEmpleado.Activo
                });
            }
            modelBuilder.Entity<Empleado>().HasData(empleados);
        }
    }
}