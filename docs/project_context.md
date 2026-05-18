# Project Context: HotelGenericoApi

## 1. Visión General

API REST para sistema de gestión hotelera genérico. Desarrollada con **.NET 10**, **Entity Framework Core**, **SQL Server**. Proporciona endpoints para gestión de habitaciones, clientes, estancias, ventas, productos, comprobantes electrónicos y reportes. Incluye **JWT** para autenticación, **Swagger + Scalar** para documentación, **Rate Limiting** por políticas, y **WebApplicationFactory** para tests de integración.

Originalmente desarrollada para el hotel "La Rica Noche", refactorizada como **Hotel Genérico** configurable mediante la tabla `configuracion_hotel`.

## 2. Stack Tecnológico

| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| .NET | 10.0 | Framework principal |
| C# | 13 | Lenguaje |
| ASP.NET Core | 10.0 | Framework Web API |
| Entity Framework Core | 10.0.7 | ORM |
| SQL Server Express | - | Base de datos local |
| NLua | 1.7.8 | Scripts Lua para reglas de negocio |
| Riok.Mapperly | 4.3.1 | Mapeo entidad-DTO (source generator) |
| QuestPDF | 2026.2.4 | Generación de PDFs |
| ClosedXML | 0.105.0 | Exportación a Excel |
| BCrypt.Net-Next | 4.1.0 | Hashing de contraseñas |
| Scalar.AspNetCore | 2.14.4 | UI OpenAPI alternativa |
| Swashbuckle.AspNetCore | 10.1.7 | Documentación Swagger |
| JWT Bearer | 10.0.7 | Autenticación |
| SignalR | - | WebSockets para tiempo real |

### Testing

| Herramienta | Versión | Propósito |
|------------|---------|-----------|
| xUnit | 2.9.3 | Framework de testing |
| Moq | 4.20.72 | Mocking de dependencias |
| EF Core InMemory | 10.0.7 | Base de datos en memoria para tests |
| Microsoft.AspNetCore.Mvc.Testing | 10.0.7 | WebApplicationFactory (tests integración) |
| coverlet.collector | 6.0.4 | Cobertura de código |

## 3. Estructura del Proyecto

```
HotelGenericoApi/
├── Controllers/           # Controladores REST (20)
│   ├── CatAfectacionIgvController.cs
│   ├── CatEstadoHabitacionController.cs
│   ├── CatEstadoSunatController.cs
│   ├── CatMetodoPagoController.cs
│   ├── CatRolUsuarioController.cs
│   ├── CatTipoComprobanteController.cs
│   ├── CatTipoDocumentoController.cs
│   ├── ClienteController.cs
│   ├── ComprobanteController.cs
│   ├── ConfiguracionHotelController.cs
│   ├── EstanciaController.cs
│   ├── HabitacionController.cs
│   ├── PdfController.cs
│   ├── ProductoController.cs
│   ├── ReporteController.cs
│   ├── SetupController.cs
│   ├── TiposHabitacionController.cs
│   ├── UsuarioController.cs
│   └── VentaController.cs
├── Data/
│   └── HotelDbContext.cs   # DbContext + Fluent API (snake_case)
├── DTOs/
│   ├── Request/            # DTOs de entrada con Data Annotations
│   └── Response/           # DTOs de salida
├── Extensions/
│   └── IQueryableExtensions.cs
├── Hubs/
│   └── HabitacionHub.cs    # SignalR Hub (/hotelhub)
├── JsonConverters/         # Converters JSON personalizados
│   └── TrimStringConverter.cs
├── Mappings/               # Mapperly (12 mappers)
├── Middleware/
│   └── ExceptionMiddleware.cs  # Manejo global de errores
├── Models/                 # Entidades EF (22 + 3 vistas)
├── Scripts/                # Scripts Lua
│   ├── hotel_tax_rules.lua
│   └── validar_cliente.lua
├── Services/
│   ├── Interfaces/         # 17 interfaces
│   └── Implementations/    # 20 servicios
├── Tests/                  # Tests unitarios + integración
│   ├── HotelGenericoApi.Tests.csproj
│   ├── UnitTest1.cs
│   ├── IntegrationTests.cs
│   ├── TestDbContextFactory.cs
│   └── UsuarioControllerIntegrationTests.cs
├── Dockerfile              # Multi-stage build
├── docker-compose.yml      # SQL Server + API
├── Program.cs              # Punto de entrada
├── HotelGenericoApi.csproj
├── .editorconfig           # Reglas de estilo
└── appsettings.json
```

## 4. Arquitectura

```
┌─────────────┐     ┌──────────────────┐     ┌──────────────┐
│  Frontend    │────▶│  ASP.NET Core    │────▶│  SQL Server  │
│  (Vercel)    │◀────│  Web API         │◀────│  Express     │
└─────────────┘     └──────┬───────────┘     └──────────────┘
                           │
                    ┌──────┴──────┐
                    │  VerificaPE │
                    │  (RENIEC)   │
                    └─────────────┘
```

- **Autenticación**: JWT Bearer con tokens de 8 horas
- **Rate Limiting**: 3 políticas — "login" (5/min/IP), "reniec" (10/min/IP), "global" (100/min/user+IP, queue 10)
- **CORS**: Configurable vía `Cors:AllowedOrigins` (default: `http://localhost:5173`)
- **SignalR**: Hub en `/hotelhub`
- **OpenAPI**: Swagger UI + Scalar UI (solo development)
- **Exception Middleware**: Captura global con respuestas JSON estructuradas
- **ORM**: Entity Framework Core con Fluent API + snake_case
- **Trimming**: `TrimStringConverter` sanitiza strings automáticamente en toda la API

## 5. Base de Datos

### Convenciones
- Nombres en **snake_case** vía Fluent API (no `EFCore.NamingConventions`)
- `DeleteBehavior.NoAccess` global (no cascade)
- Llaves primarias: `id_{tabla}` (ej. `id_habitacion`)
- Timestamps: `created_at`, `fecha_*` en UTC

### Tablas principales

| Tabla | Descripción |
|-------|-------------|
| configuracion_hotel | Datos del hotel (única fila, id=1) |
| cat_tipo_documento | DNI(1), RUC(6), Pasaporte(7), Otros(0) |
| cat_metodo_pago | Efectivo(005), Tarjeta(006), Yape/Plin(008) |
| cat_tipo_comprobante | Boleta(03), Factura(01) |
| cat_afectacion_igv | Gravado(10), Exonerado(20), Inafecto(30), Exportación(40) |
| cat_estado_habitacion | Estados con flags (permite_checkin/checkout) |
| cat_estado_sunat | Pendiente(1), Enviado(2), Aceptado(3), Rechazado(4) |
| cat_rol_usuario | Admin, Recepcionista, Limpieza |
| cat_transicion_estado | Máquina de estados de habitación |
| usuarios | Usuarios del sistema (bcrypt) |
| login_attempts | Histórico de intentos de login |
| clientes | Clientes (incluye anónimo doc 00000000) |
| tipos_habitacion | Tipos de habitación |
| tarifas | Precios por tipo + temporada |
| temporadas | Temporadas con multiplicador |
| habitaciones | Habitaciones del hotel |
| historial_estado_habitacion | Cambios de estado con trazabilidad |
| reservas | Reservas con fechas previstas |
| estancias | Check-in real |
| huespedes | Huéspedes adicionales |
| productos | Productos con categoría y afectación IGV |
| items_estancia | Consumos durante la estancia |
| ventas | Ventas independientes |
| items_venta | Detalle de ventas |
| comprobantes | Comprobantes electrónicos SUNAT |
| cierre_caja_envios | Envíos de cierre de caja |

### Vistas

| Vista | Descripción |
|-------|-------------|
| v_cierre_caja_diario | Ingresos agrupados por fecha + método pago |
| v_estado_habitaciones | Estado actual con datos de estancia activa |
| v_ocupacion_diaria | Ocupación por día |

### Estados de habitación

| ID | Estado | CheckIn | CheckOut | Color UI |
|----|--------|---------|----------|----------|
| 1 | Disponible | ✓ | ✗ | success |
| 2 | Ocupada | ✗ | ✓ | warning |
| 3 | Limpieza | ✗ | ✗ | info |
| 4 | Mantenimiento | ✗ | ✗ | error |

### Transiciones válidas

| Estado Actual → Siguiente |
|--------------------------|
| Disponible (1) → Ocupada (2) |
| Disponible (1) → Mantenimiento (4) |
| Ocupada (2) → Limpieza (3) |
| Limpieza (3) → Disponible (1) |
| Mantenimiento (4) → Disponible (1) |

### Seed automático
En `Development` se ejecuta `SetupService.CrearUsuariosPorDefectoAsync()` al iniciar:
- **Admin**: `admin` / `Admin123!`
- **Cajero**: `cajero` / `Cajero123!`

## 6. Controladores y Endpoints

### Autenticación y Setup
| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | `/api/Usuario/login` | Login, retorna JWT + datos usuario |
| GET | `/api/Setup` | Verifica si existe admin |
| POST | `/api/Setup` | Crea el primer administrador |

### Configuración del Hotel
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/ConfiguracionHotel` | Obtener configuración |
| PUT | `/api/ConfiguracionHotel` | Actualizar configuración |

### Habitaciones
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Habitacion` | Listar todas |
| GET | `/api/Habitacion/{id}` | Obtener por ID |
| POST | `/api/Habitacion` | Crear |
| PUT | `/api/Habitacion/{id}` | Actualizar |
| PATCH | `/api/Habitacion/{id}/estado?idNuevoEstado=X&idUsuario=Y` | Cambiar estado |
| DELETE | `/api/Habitacion/{id}` | Eliminar |

### Estancias
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Estancia` | Listar todas |
| GET | `/api/Estancia/{id}` | Obtener por ID |
| POST | `/api/Estancia` | Check-in (crear estancia) |
| POST | `/api/Estancia/{id}/checkout` | Check-out |
| POST | `/api/Estancia/{id}/huesped` | Agregar huésped |
| POST | `/api/Estancia/{id}/consumo` | Agregar consumo |

### Clientes
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Cliente` | Listar (búsqueda por documento) |
| GET | `/api/Cliente/{id}` | Obtener por ID |
| POST | `/api/Cliente` | Crear |
| PUT | `/api/Cliente/{id}` | Actualizar |
| DELETE | `/api/Cliente/{id}` | Eliminar |
| GET | `/api/Cliente/{doc}/reniec` | Consultar RENIEC |

### Productos
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Producto` | Listar |
| GET | `/api/Producto/{id}` | Obtener |
| POST | `/api/Producto` | Crear |
| PUT | `/api/Producto/{id}` | Actualizar |
| DELETE | `/api/Producto/{id}` | Eliminar |

### Ventas
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Venta` | Listar todas |
| GET | `/api/Venta/{id}` | Obtener por ID |
| POST | `/api/Venta` | Crear (con items) |
| DELETE | `/api/Venta/{id}` | Eliminar |

### Comprobantes
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Comprobante` | Listar |
| GET | `/api/Comprobante/{id}` | Obtener detalle |
| GET | `/api/Comprobante/{id}/pdf` | Generar PDF |
| POST | `/api/Comprobante/{id}/enviar-sunat` | Simular envío SUNAT |

### Reportes
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Reporte/cierre-caja?fecha=AAAA-MM-DD` | Cierre de caja |
| GET | `/api/Reporte/estado-habitaciones` | Estado de habitaciones |
| GET | `/api/Reporte/ocupacion-diaria?fecha=AAAA-MM-DD` | Ocupación diaria |

### Health
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/health` | Health check |

### PDF
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Pdf/CierreCaja` | PDF de cierre de caja |

### Catálogos
CRUD completo para: `CatAfectacionIgv`, `CatEstadoHabitacion`, `CatEstadoSunat`, `CatMetodoPago`, `CatRolUsuario`, `CatTipoComprobante`, `CatTipoDocumento`, `TiposHabitacion`.

## 7. DTOs

- **Request**: `LoginDto`, `CheckInDto`, `ClienteCreateDto`, `ClienteUpdateDto`, y Create/Update DTOs para cada catálogo — todos con Data Annotations de validación.
- **Response**: DTOs de salida para cada entidad, incluyendo `LoginResponseDto`, `PagedResult<T>` para paginación, etc.

## 8. Servicios

### Interfaces (17)
`ICatAfectacionIgvService`, `ICatEstadoHabitacionService`, `ICatEstadoSunatService`, `ICatMetodoPagoService`, `ICatRolUsuarioService`, `ICatTipoComprobanteService`, `ICatTipoDocumentoService`, `ICierreCajaEnvioService`, `IClienteService`, `IComprobanteService`, `IConfiguracionHotelService`, `IEstanciaService`, `IHabitacionService`, `IPdfService`, `IProductoService`, `IReniecService`, `IReporteService`, `ITiposHabitacionService`, `IUsuarioService`, `IValidadorEstadoService`, `IVentaService`

### Implementaciones (20)
Servicios scoped que trabajan directamente con entidades (sin DTOs internos). Cada servicio recibe `HotelDbContext` + `ILogger<T>` vía DI. Destacan:
- **ReniecService**: HttpClient tipificado con `IHttpClientFactory` para consultar VerificaPE
- **SetupService**: Inicialización del sistema (crea admin, seed data)
- **ValidadorEstadoService**: Valida transiciones de máquina de estados

## 9. Mappings (Mapperly)

12 mappers source-generated para los controladores CRUD de catálogos y entidades que aún usan DTOs. Los servicios principales (Habitacion, Estancia, Venta, Reporte) trabajan directamente con entidades sin mappers.

## 10. Scripts Lua

### `hotel_tax_rules.lua`
Función `Calculate_igv_hotel(afectacion_codigo, base_imponible, tipo_comprobante)`:
- Boleta (03) + gravado (10): tasa = **10.5%** (hospedaje)
- Gravado normal: tasa = **18%**
- Exonerado/Inafecto/Exportación: tasa = **0%**
- Retorna `{ tasa, monto }`

### `validar_cliente.lua`
Función `Validar(documento, tipo)`:
- DNI: 8 dígitos
- RUC: 11 dígitos

## 11. Models/Entities (25)

`CatAfectacionIgv`, `CatCategoriaProducto`, `CatEstadoHabitacion`, `CatEstadoSunat`, `CatMetodoPago`, `CatRolUsuario`, `CatTipoComprobante`, `CatTipoDocumento`, `CatTransicionEstado`, `CierreCajaEnvio`, `Cliente`, `Comprobante`, `ConfiguracionHotel`, `Estancia`, `Habitacion`, `HistorialEstadoHabitacion`, `Huesped`, `ItemEstancia`, `ItemVenta`, `LoginAttempt`, `Producto`, `Reserva`, `Tarifa`, `Temporada`, `TiposHabitacion`, `Usuario`, `VCierreCajaDiario`, `VEstadoHabitacion`, `VOcupacionDiaria`, `Venta`

## 12. SignalR

- **Hub**: `HabitacionHub` (hereda de Hub)
- **Endpoint**: `/hotelhub`
- **CORS**: AllowCredentials habilitado con orígenes configurados

## 13. Seguridad

- **Autenticación**: JWT Bearer Token (8h)
- **Claims**: NameIdentifier (id usuario), Role (nombre del rol)
- **CORS**: Desde `Cors:AllowedOrigins` en configuración
- **Rate Limiting**: 3 políticas (login, reniec, global)
- **Contraseñas**: BCrypt.Net-Next
- **Sanitización**: `TrimStringConverter` global para strings de entrada
- **Exception Middleware**: Captura global con códigos HTTP adecuados
- **Endpoints**: Protegidos con `[Authorize]` excepto login y setup

## 14. Configuración

### `appsettings.json` — valores base
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelDB;User Id=sa;Password=***;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "***",
    "Issuer": "HotelGenericoApi",
    "Audience": "HotelGenericoApiClient"
  }
}
```

### `appsettings.Development.json` — credenciales locales
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HotelDB;User Id=sa;Password=Mar703server;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "obTi6lRmI72jFEi+dLHYrKpETi5wm6Dy2H6qMvY0O3A="
  },
  "VerificaPE": {
    "BaseUrl": "https://api.verificape.com/v2",
    "ApiKey": "***"
  }
}
```

### `appsettings.Production.json` — placeholders para CI/CD
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=__DB_SERVER__;..."
  },
  "Jwt": { "Key": "__JWT_SECRET_KEY__" },
  "VerificaPE": { "ApiKey": "__VERIFICAPE_API_KEY__" },
  "Cors": { "AllowedOrigins": ["__FRONTEND_URL__"] }
}
```

### Variables de entorno (producción)
| Variable | Equivalente en appsettings |
|----------|---------------------------|
| `ConnectionStrings__DefaultConnection` | `ConnectionStrings:DefaultConnection` |
| `Jwt__Key` | `Jwt:Key` |
| `VerificaPE__ApiKey` | `VerificaPE:ApiKey` |
| `Cors__AllowedOrigins__0` | `Cors:AllowedOrigins:0` |

## 15. Testing (xUnit)

- **Total tests**: 10 (6 unitarios + 4 integración)
- **Framework**: xUnit 2.9.3 + Moq 4.20.72 + EF Core InMemory + WebApplicationFactory
- **Helpers**:
  - `TestDbContextFactory`: Crea `HotelDbContext` con InMemory database
  - `NoOpClientProxy`: Implementación dummy de `IClientProxy` para SignalR

### Tests unitarios (EstanciaServiceTests)
- `Create_EstanciaConHabitacionDisponible_CreaCorrectamente`
- `Checkout_EstanciaActiva_FinalizaCorrectamente`
- `AddHuesped_EstanciaExistente_AgregaCorrectamente`
- `AddConsumo_EstanciaExistente_AgregaCorrectamente`
- `Transicion_Disponible_Ocupada_EsValida`
- `Transicion_Mantenimiento_Ocupada_NoPermitida`

### Tests de integración (WebApplicationFactory)
- `Health_ReturnsOk`
- `Login_WithValidCredentials_ReturnsToken`
- `Login_WithInvalidCredentials_ReturnsUnauthorized`
- `Swagger_ReturnsJson`

## 16. Decisiones Técnicas

| Decisión | Razón |
|----------|-------|
| Fluent API sin `EFCore.NamingConventions` | Control total sobre nombres de columnas y tablas |
| Servicios simplificados sin DTOs internos | Menor complejidad, las entidades mismas son el contrato |
| `TrimStringConverter` global | Sanitización consistente sin tocar cada controlador |
| `IHttpClientFactory` para Reniec | Evita socket exhaustion, permite manejo centralizado |
| Rate Limiting por política | Protección diferenciada por recurso (login vs API general) |
| Exception Middleware | Respuetas JSON consistentes ante cualquier error |
| WebApplicationFactory para tests E2E | Pruebas reales del pipeline HTTP completo |
| Mapperly solo para catálogos CRUD | Donde realmente hay transformación entidad ↔ DTO |

## 17. Flujo de Operaciones

### Check-In
1. Validar que habitación esté Disponible (id=1)
2. Crear estancia con `FechaCheckin = UtcNow`
3. Cambiar habitación a Ocupada (id=2)

### Check-Out
1. Validar que estancia esté activa
2. Registrar `FechaCheckoutReal = UtcNow`
3. Cambiar estado a "Finalizada"
4. Liberar habitación a Limpieza (id=3)

### Crear Venta
1. Recibir venta con items
2. Guardar venta + items en transacción
3. Calcular total

### Consulta RENIEC
1. Validar tasa "reniec" (10/min/IP)
2. Consultar VerificaPE via HttpClient
3. Cachear/buscar cliente existente por documento

## 18. Puertos

- **API (local)**: `http://localhost:5000`
- **API (Docker)**: `http://localhost:5000` → `:8080` interno
- **SQL Server (Docker)**: `localhost:1433`
- **Swagger**: `http://localhost:5000/swagger`
- **Scalar**: `http://localhost:5000/scalar/v1`
