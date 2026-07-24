using EmpleadosApp.Data;
using EmpleadosApp.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using EmpleadosApp.Controllers;
using Microsoft.AspNetCore.Mvc;

public class EmpleadosControllerTests
{
    private EmpleadosDbContext CrearContextoEnMemoria()
    {
        var options = new DbContextOptionsBuilder<EmpleadosDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())  // BD nueva y aislada por test
            .Options;
        return new EmpleadosDbContext(options);
    }

    [Fact]
    public async Task Despedir_CambiaEstadoAExtDespedido_YRegistraFechaTerminacion()
    {
        // Arrange
        var context = CrearContextoEnMemoria();
        var departamento = new Departamento { Id = 1, Nombre = "Ventas" };
        var empleado = new Empleado
        {
            Id = 1,
            Nombre = "Juan",
            Apellido = "Pérez",
            DepartamentoId = 1,
            Departamento = departamento,
            Puesto = "Vendedor",
            Email = "juan@test.com",
            Direccion = "Calle 1",
            Telefono = "8095551111",
            Estado = EstadoEmpleado.Activo
        };
        context.Departamentos.Add(departamento);
        context.Empleados.Add(empleado);
        await context.SaveChangesAsync();

        var envMock = new Mock<IWebHostEnvironment>();
        var controller = new EmpleadosController(context, envMock.Object);

        // Act
        var resultado = await controller.Despedir(1);

        // Assert
        var empleadoActualizado = await context.Empleados.FindAsync(1);
        Assert.Equal(EstadoEmpleado.Despedido, empleadoActualizado!.Estado);
        Assert.NotNull(empleadoActualizado.FechaTerminacion);
        Assert.IsType<RedirectToActionResult>(resultado);
    }

    [Fact]
    public async Task Despedir_EmpleadoInexistente_RetornaNotFound()
    {
        var context = CrearContextoEnMemoria();
        var envMock = new Mock<IWebHostEnvironment>();
        var controller = new EmpleadosController(context, envMock.Object);

        var resultado = await controller.Despedir(999);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
