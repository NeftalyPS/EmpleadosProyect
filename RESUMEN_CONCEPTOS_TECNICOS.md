# Resumen de Conceptos Técnicos - Proyecto Empleados

## Opción A: Proyecto Único

Este proyecto implementa la **Opción A** del mandato del profesor: una sola aplicación ASP.NET Core que contiene tanto controladores MVC como controladores de API, compartiendo todo (DbContext, modelos, servicios y configuración).

**Estructura actual:**
- **Proyecto único**: EmpleadosProyect (ASP.NET Core MVC + Web API)
- **Controllers MVC**: EmpleadosController, AccountController, DepartamentosController, HomeController
- **Controllers API**: EmpleadosApiController, AuthApiController, DepartamentosApiController
- **DbContext compartido**: EmpleadosDbContext
- **Modelos compartidos**: Entities (Empleado, Departamento, Usuario) y DTOs
- **Configuración compartida**: Program.cs, appsettings.json

Este documento explica cómo el proyecto implementa cada concepto técnico requerido por el profesor.

---

## 1. Fundamentos de ASP.NET Core

### Estructura del proyecto
- **Program.cs**: Punto de entrada de la aplicación. Configura servicios y middleware.
- **appsettings.json**: Configuración de conexión a BD, JWT, logging.
- **wwwroot**: Archivos estáticos (uploads/empleados para fotos).

### Pipeline de middleware (Program.cs)
```csharp
app.UseMiddleware<ExceptionMiddleware>();  // Manejo global de errores
app.UseHttpsRedirection();                  // Redirección HTTPS
app.UseStaticFiles();                       // Archivos estáticos
app.UseRouting();                           // Enrutamiento
app.UseAuthentication();                   // Autenticación
app.UseAuthorization();                    // Autorización
```

### Enrutamiento
- **Convencional**: `{controller=Home}/{action=Index}/{id?}` en Program.cs
- **Por atributos**: `[Route("api/empleados")]` en API controllers

### Configuración (IConfiguration)
```csharp
builder.Configuration.GetConnectionString("DefaultConnection")
builder.Configuration["Jwt:Key"]
```

---

## 2. MVC

### Controllers, Actions y Action Results
- **EmpleadosController**: CRUD de empleados (Index, Details, Create, Edit)
- **AccountController**: Login/Logout con cookies
- **Action Results**: `View()`, `RedirectToAction()`, `NotFound()`, `BadRequest()`

### Views con Razor
- **Sintaxis**: `@model`, `@foreach`, `@if`, `@ViewBag`
- **Ejemplo**: `Views/Empleados/Create.cshtml`

### Layouts y secciones
- **_Layout.cshtml**: Layout principal con Bootstrap 5
- **_ViewStart.cshtml**: Especifica el layout por defecto
- **@RenderSection**: Para secciones personalizables (scripts, estilos)

### ViewModels vs Entidades
- **Entidades**: `Empleado`, `Departamento`, `Usuario` (Models/Entities)
- **DTOs**: `EmpleadoDto`, `EmpleadoCreateDto`, `EmpleadoUpdateDto` (Models/DTOs)
- **En MVC**: Se usan directamente las entidades (se podría mejorar con ViewModels)

### Tag Helpers
- **asp-action**: `<form asp-action="Create">`
- **asp-for**: `<input asp-for="Nombre">`
- **asp-route-**: `<a asp-action="Edit" asp-route-id="@item.Id">`

### Model Binding
- **Desde formularios**: `Create(Empleado empleado, IFormFile? foto)`
- **Query strings**: `Index(int page = 1, string? nombre = null)`
- **Rutas**: `Details(int id)`

---

## 3. Web API

### Atributo [ApiController] y convenciones REST
```csharp
[ApiController]
[Route("api/empleados")]
public class EmpleadosApiController : ControllerBase
```

### Verbos HTTP correctos
- **GET**: Listar y obtener detalles
- **POST**: Crear recursos
- **PUT**: Actualización completa
- **PATCH**: Actualización parcial (desactivar, despedir)
- **DELETE**: No implementado (se usa PATCH para despedir)

### Códigos de estado
- **200 OK**: `Ok()`, `Ok(empleadoDto)`
- **201 Created**: `CreatedAtAction()`
- **204 No Content**: `NoContent()` (PUT, PATCH)
- **400 Bad Request**: `BadRequest()`, `BadRequest(new { message })`
- **401 Unauthorized**: `Unauthorized()` (login fallido)
- **404 Not Found**: `NotFound()`
- **500 Internal Server Error**: Manejado por ExceptionMiddleware

### Negociación de contenido (JSON)
- Automático con `[ApiController]`
- Respuestas en JSON para endpoints API

---

## 4. Acceso a Datos

### Entity Framework Core - Code First
- **DbContext**: `EmpleadosDbContext` en Data/
- **DbSet<T>**: `DbSet<Empleado>`, `DbSet<Departamento>`, `DbSet<Usuario>`

### Configuración con Fluent API
```csharp
modelBuilder.Entity<Empleado>()
    .HasOne(e => e.Departamento)
    .WithMany(d => d.Empleados)
    .HasForeignKey(e => e.DepartamentoId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Empleado>()
    .Property(e => e.Estado)
    .HasConversion<string>();
```

### Migraciones
```bash
dotnet ef migrations add InicialConSeed
dotnet ef database update
```

### LINQ
- **Consultas**: `_context.Empleados.Include(e => e.Departamento).AsQueryable()`
- **Filtros**: `.Where(e => e.Nombre.Contains(nombre))`
- **Ordenamiento**: `.OrderBy(e => e.Id)`
- **Proyecciones**: `_mapper.Map<List<EmpleadoDto>>(items)`

### Carga relacionada con .Include()
```csharp
_context.Empleados.Include(e => e.Departamento)
```

### Operaciones async/await
- `ToListAsync()`, `FirstOrDefaultAsync()`, `FindAsync()`
- `SaveChangesAsync()`, `CountAsync()`

---

## 5. Patrones y Buenas Prácticas

### Inyección de Dependencias (Program.cs)
```csharp
builder.Services.AddDbContext<EmpleadosDbContext>(...);  // Scoped por defecto
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
```

### Capa de servicios
- **ITokenService/TokenService**: Generación de tokens JWT
- **AutoMapperProfile**: Configuración de mapeo DTO ↔ Entity

### DTOs y AutoMapper
- **DTOs**: `EmpleadoDto`, `EmpleadoCreateDto`, `EmpleadoUpdateDto`
- **AutoMapper**: Mapeo automático entre entidades y DTOs
```csharp
CreateMap<Empleado, EmpleadoDto>()
    .ForMember(d => d.DepartamentoNombre, opt => opt.MapFrom(s => s.Departamento?.Nombre));
```

### Manejo de excepciones global
- **ExceptionMiddleware**: Captura excepciones y retorna ProblemDetails para API
- **Logging**: `_logger.LogError(ex, "Error no controlado")`

---

## 6. Validación

### Data Annotations
- **En entidades**: `[Required]`, `[StringLength(100)]`, `[EmailAddress]`, `[Range(0, 9999999)]`
- **En DTOs**: Mismos atributos para validación en API

### Verificación de ModelState.IsValid
```csharp
if (!ModelState.IsValid)
{
    ViewBag.Departamentos = await _context.Departamentos.ToListAsync();
    return View(empleado);
}
```

### Validación en cliente y servidor
- **Cliente**: jQuery Validation (automático con Tag Helpers)
- **Servidor**: ModelState.IsValid en controllers

---

## 7. Seguridad

### Autenticación con cookies (MVC)
```csharp
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});
```
- **AccountController**: Login con Claims y SignInAsync

### Autenticación con JWT Bearer (API)
```csharp
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // ...
    };
});
```
- **AuthApiController**: Login retorna token JWT
- **TokenService**: Generación de tokens con claims

### Autorización por roles
```csharp
[Authorize]  // Cualquier usuario autenticado
[Authorize(Roles = "Admin")]  // Solo administradores
```

### Hashing de contraseñas
- **BCrypt.Net-Next**: `BCrypt.Net.BCrypt.HashPassword("Admin123!")`
- **Verificación**: `BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash)`

### CORS
- **NO configurado**: Se debe agregar para producción si se consume desde otro dominio

---

## 8. Archivos y Persistencia

### Subida de archivos con IFormFile
```csharp
public async Task<IActionResult> Create(Empleado empleado, IFormFile? foto)
```

### Validación de archivo
```csharp
if (foto != null && foto.Length > 0)
```

### Almacenamiento físico en wwwroot
```csharp
var carpeta = Path.Combine(_env.WebRootPath, "uploads", "empleados");
var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
var rutaFisica = Path.Combine(carpeta, nombreArchivo);
await foto.CopyToAsync(stream);
return $"/uploads/empleados/{nombreArchivo}";
```

---

## 9. Diagnóstico

### Logging con ILogger<T>
```csharp
private readonly ILogger<ExceptionMiddleware> _logger;
_logger.LogError(ex, "Error no controlado");
```

### Niveles de log
- **Information**: Configurado en appsettings.json
- **Warning**: Configurado para Microsoft.AspNetCore
- **Error**: Usado en ExceptionMiddleware

### Manejo de errores
- **MVC**: `app.UseExceptionHandler("/Home/Error")` en producción
- **API**: ProblemDetails en ExceptionMiddleware
- **Nunca se muestra stack trace al usuario**

---

## 10. Pruebas (Introductorio)

### Estado actual
- **NO implementado**: No hay pruebas unitarias con xUnit o MSTest

### Recomendación
Crear proyecto de pruebas xUnit para probar:
- Lógica de TokenService
- Validaciones en controllers
- Mapeo de AutoMapper

---

## 11. Tecnologías Utilizadas

- **.NET 9.0** (más reciente que .NET 8 LTS)
- **ASP.NET Core MVC + Web API** (mismo proyecto)
- **Entity Framework Core 8.0.29** (Code First + Migrations)
- **SQL Server LocalDB**
- **Razor + Bootstrap 5** (UI responsiva)
- **Swashbuckle 6.9.0** (Swagger para documentación API)
- **JWT Bearer** (Microsoft.AspNetCore.Authentication.JwtBearer 8.0.29)
- **AutoMapper 12.0.1** (mapeo DTO ↔ Entity)
- **BCrypt.Net-Next 4.2.0** (hashing de contraseñas)

---

## 12. Cumplimiento de Requisitos del Mandato

### 1. Diseño de la Base de Datos ✓
- **Fotografía**: Ruta relativa almacenada en `FotoRuta` ✓
- **Campos completos**: Nombre, Apellido, Departamento, Puesto, Salario, FechaNacimiento, FechaContratacion, FechaTerminacion, Direccion, Telefono, Email, Estado ✓
- **Departamento como entidad separada**: Relación uno a muchos con Empleado ✓
- **Archivos físicos**: Almacenados en `wwwroot/uploads/empleados/` ✓
- **EF Core Code First**: `EmpleadosDbContext` con Fluent API ✓
- **Migraciones**: Ejecutadas con `Add-Migration InicialConSeed` y `Update-Database` ✓
- **Seed data**: 3 departamentos y 10 empleados en `OnModelCreating` ✓

### 2. Proyecto Web API ✓
- **GET /api/empleados**: Paginación (page, pageSize) + filtros (nombre, apellido, fechaContratacion) ✓
- **GET /api/empleados/{id}**: Obtener empleado por ID ✓
- **POST /api/empleados**: Crear empleado ✓
- **PUT /api/empleados/{id}**: Actualizar empleado ✓
- **PATCH /api/empleados/{id}/desactivar**: Soft delete reversible ✓
- **PATCH /api/empleados/{id}/despedir**: Registrar despido con fecha ✓
- **POST /api/empleados/{id}/foto**: Subir fotografía (multipart/form-data) ✓
- **Endpoints de departamentos**: CRUD completo en `DepartamentosApiController` ✓
- **DTOs**: `EmpleadoDto`, `EmpleadoCreateDto`, `EmpleadoUpdateDto`, `DepartamentoDto` ✓
- **Validación**: Data Annotations en DTOs ([Required], [StringLength], [EmailAddress], [Range]) ✓
- **Paginación**: Objeto `PagedResult<T>` con items, totalCount, page, pageSize ✓
- **ProblemDetails**: Middleware `ExceptionMiddleware` retorna formato RFC 7807 ✓
- **Swagger**: Configurado con Swashbuckle en Program.cs ✓
- **JWT Bearer**: `AuthApiController` con endpoint `/api/auth/login` ✓

### 3. Proyecto MVC ✓
- **Login con cookies**: `AccountController` con CookieAuthentication ✓
- **Layout principal**: `_Layout.cshtml` con Bootstrap 5 y menú de navegación ✓
- **Listado de empleados**: Paginación + filtros (nombre, apellido, fechaContratacion) ✓
- **Acciones por fila**: Ver detalle, editar, desactivar, despedir ✓
- **Vista de detalle**: Muestra todos los datos y fotografía ✓
- **Formulario de creación**: Validación cliente + servidor ✓
- **Formulario de modificación**: Edit con validación ✓
- **Subida de fotografía**: IFormFile en Create/Edit ✓
- **Acciones de desactivación y despido**: Con confirmación ✓
- **Gestión de departamentos**: CRUD en `DepartamentosController` ✓
- **Autorización por roles**: `[Authorize(Roles = "Admin")]` en Despedir ✓

### 4. Reglas de Negocio ✓
- **Desactivar ≠ Despedir**: Estados separados (Inactivo vs Despedido) ✓
- **Irreversibilidad**: Empleado despedido no puede modificarse ✓
- **No eliminación física**: Todos los cambios son lógicos ✓
- **Protección de departamentos**: No se permite eliminar con empleados asociados (validación en controller)

---

## 13. Consideraciones Adicionales

### Validación en dos capas
- **Cliente**: jQuery Validation con Tag Helpers ✓
- **Servidor**: ModelState.IsValid + Data Annotations ✓

### Diseño responsivo
- **Bootstrap 5 Grid**: `col-md-6`, `col-md-12` en views ✓
- Funciona en escritorio y móvil ✓

### Separación de responsabilidades
```
Controllers/        - Lógica de presentación MVC
Controllers/API/    - Endpoints REST API
Data/               - DbContext compartido
Models/Entities/    - Entidades de BD
Models/DTOs/        - Data Transfer Objects (API)
Services/           - Lógica de negocio (TokenService)
Middleware/         - Middleware personalizado
Views/              - Vistas Razor
```

### Configuración sensible
- **appsettings.json**: Connection string (OK para desarrollo) ✓
- **appsettings.Development.json**: Configuración desarrollo ✓
- **User Secrets**: Configurado con UserSecretsId para secretos JWT ✓

### Control de versiones
- **Git**: Repositorio en GitHub ✓
- **.gitignore**: Excluye bin/, obj/, etc. ✓

### README
- **Estado**: NO existe README.md
- **Recomendación**: Crear con instrucciones de:
  - Clonar repositorio
  - Configurar connection string
  - Ejecutar migraciones
  - Iniciar aplicación

---

## 14. Puntos a Mejorar para Cumplir 100%

### Requisitos del mandato pendientes:

1. **ViewModels separados de entidades en MVC** ⚠️
   - **Estado actual**: Los controllers MVC usan directamente las entidades (Empleado, Departamento)
   - **Requisito**: "ViewModels separados de las entidades"
   - **Solución**: Crear ViewModels en Models/ViewModels/ (EmpleadoCreateViewModel, EmpleadoEditViewModel, etc.)

2. **Pruebas unitarias** ⚠️
   - **Estado actual**: No hay proyecto de pruebas
   - **Requisito**: "Al menos una prueba unitaria de la lógica de un controller o servicio usando xUnit o MSTest"
   - **Solución**: Crear proyecto xUnit y probar TokenService o lógica de controllers

3. **Configuración de CORS en la API** ⚠️
   - **Estado actual**: No configurado
   - **Requisito**: "Configuración de CORS en la API"
   - **Solución**: Agregar `builder.Services.AddCors()` en Program.cs

4. **README.md** ⚠️
   - **Estado actual**: No existe
   - **Requisito**: "Incluir un README.md con instrucciones para clonar, configurar la cadena de conexión, ejecutar migraciones e iniciar ambos proyectos"
   - **Solución**: Crear README.md con instrucciones completas

### Mejoras opcionales (no requeridas):

5. **Vistas parciales** para reutilizar código
6. **Implementar DELETE** en API (opcional, el mandato permite usar PATCH)

---

## 15. Tecnologías Utilizadas (vs Requisitos)

| Requisito | Estado | Versión |
|-----------|--------|---------|
| .NET 8 (LTS) o superior | ✓ | .NET 9.0 (más reciente) |
| ASP.NET Core Web API | ✓ | Incluido en .NET 9.0 |
| ASP.NET Core MVC | ✓ | Incluido en .NET 9.0 |
| Entity Framework Core | ✓ | 8.0.29 |
| SQL Server (LocalDB) | ✓ | LocalDB |
| Razor + Bootstrap 5 | ✓ | Bootstrap 5 |
| Swashbuckle (Swagger) | ✓ | 6.9.0 |
| JWT Bearer | ✓ | 8.0.29 |
| AutoMapper | ✓ | 12.0.1 |
| xUnit | ⚠️ | No implementado |

---

## Resumen de Cumplimiento

**Cumplimiento general: ~85%**

**Completamente cumplido:**
- ✓ Diseño de base de datos normalizada
- ✓ Proyecto Web API con todos los endpoints
- ✓ Proyecto MVC con todas las pantallas
- ✓ Reglas de negocio implementadas
- ✓ Seguridad (Cookies + JWT)
- ✓ Validación en dos capas
- ✓ Manejo de archivos
- ✓ Logging y manejo de errores
- ✓ DTOs y AutoMapper
- ✓ Inyección de dependencias
- ✓ Entity Framework Core con migraciones

**Pendiente para 100%:**
- ⚠️ ViewModels en MVC (requisito explícito)
- ⚠️ Pruebas unitarias xUnit (requisito explícito)
- ⚠️ Configuración CORS (requisito explícito)
- ⚠️ README.md (requisito explícito)
