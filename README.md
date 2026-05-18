# Hotel Genérico API

API REST de gestión hotelera desarrollada con **ASP.NET Core 10**, diseñada para administrar habitaciones, huéspedes, ventas, comprobantes electrónicos (SUNAT) y reportes operativos.

Frontend: [HotelGenerico.Web](https://github.com/MarcosZumaran/HotelGenerico.Web)

---

## Stack

| Tecnología | Propósito |
|---|---|
| .NET 10 / ASP.NET Core | Framework principal |
| Entity Framework Core 10 | ORM y acceso a datos |
| SQL Server 2022 Express | Base de datos |
| JWT Bearer | Autenticación stateless |
| Swagger + Scalar | Documentación interactiva |
| ClosedXML | Exportación Excel |
| QuestPDF | Generación de PDF (comprobantes) |
| NLua | Reglas de negocio dinámicas (impuestos) |
| Riok.Mapperly | Mapeo automático de objetos (compile-time) |
| BCrypt.Net-Next | Hashing de contraseñas |
| xUnit + Moq | Tests unitarios y de integración |

---

## Arquitectura

```
┌─────────────┐     ┌──────────────┐     ┌──────────┐
│  Frontend   │────▶│  API (esta)  │────▶│ SQL Svr  │
│  (Vercel)   │     │  :5000       │     │  :1433   │
└─────────────┘     └──────┬───────┘     └──────────┘
                           │
                    ┌──────┴──────┐
                    │  VerificaPE │
                    │  (RENIEC)   │
                    └─────────────┘
```

```
HotelGenericoApi/
├── Controllers/        # Endpoints REST
├── Services/
│   ├── Interfaces/     # Contratos
│   └── Implementations/# Lógica de negocio
├── Models/             # Entidades (POCOs)
├── Data/               # DbContext + Fluent API
├── DTOs/
│   ├── Request/        # Payloads de entrada
│   └── Response/       # Payloads de salida
├── Mappings/           # Mappers (Riok.Mapperly)
├── Middleware/          # ExceptionMiddleware
├── JsonConverters/     # Custom JSON converters
├── Hubs/               # SignalR (HabitacionHub)
├── Migrations/         # Migraciones EF Core
└── Scripts/            # Scripts Lua (impuestos)
```

---

## Requisitos

- .NET SDK 10.0+
- SQL Server (local o Docker)
- (Opcional) Docker Desktop

---

## Inicio rápido (local)

```bash
git clone https://github.com/MarcosZumaran/HotelGenericoApi.git
cd HotelGenericoApi

# restaurar dependencias
dotnet restore

# configurar cadena de conexión en appsettings.Development.json
# o usar variables de entorno

# aplicar migraciones
dotnet ef database update

# ejecutar
dotnet run
```

La API arranca en `http://localhost:5000`.  
Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger)  
Scalar: [http://localhost:5000/scalar/v1](http://localhost:5000/scalar/v1)

### Seed automático

En entorno `Development` se crean usuarios por defecto al iniciar:
- **Admin** — `admin` / `Admin123!`
- **Cajero** — `cajero` / `Cajero123!`

---

## Docker (producción local)

```bash
# clonar
git clone https://github.com/MarcosZumaran/HotelGenericoApi.git
cd HotelGenericoApi

# iniciar SQL Server + API
SA_PASSWORD=MiClaveSegura123 \
JWT_KEY="<base64-de-32-bytes>" \
VERIFICAPE_API_KEY=tu-api-key \
CORS_ORIGIN=https://tudominio.vercel.app \
docker compose up -d
```

La API queda en `http://localhost:5000`.

Variables requeridas:

| Variable | Descripción |
|---|---|
| `SA_PASSWORD` | Contraseña del SA de SQL Server |
| `JWT_KEY` | Clave secreta JWT (256 bits en base64) |
| `VERIFICAPE_API_KEY` | API key de VerificaPE (RENIEC) |
| `CORS_ORIGIN` | URL del frontend (Vercel) |

---

## Endpoints principales

| Método | Ruta | Módulo |
|---|---|---|
| POST | `/api/usuario/login` | Autenticación |
| GET/POST | `/api/habitacion` | Habitaciones (CRUD) |
| GET/POST | `/api/estancia` | Estancias / Check-in |
| POST | `/api/estancia/{id}/checkout` | Check-out |
| GET/POST | `/api/venta` | Ventas |
| GET | `/api/reporte/cierre-caja` | Reporte diario |
| GET | `/api/reporte/estado-habitaciones` | Estado de habitaciones |
| GET | `/api/cliente/{doc}/reniec` | Consulta RENIEC |
| GET | `/health` | Health check |
| POST | `/api/setup` | Inicialización del sistema |

Ver documentación completa en Swagger o Scalar.

---

## Testing

```bash
# tests unitarios + integración
dotnet test
```

---

## License

Proyecto académico sin fines comerciales.
