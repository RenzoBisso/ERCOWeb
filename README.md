# 🛒 ERCO Web — Sitio Institucional y Panel de Administración

Proyecto web desarrollado para **ERCO**, una distribuidora de productos de consumo masivo. El sistema consta de dos módulos principales: un **sitio público** con información institucional, catálogo de productos, marcas y promociones; y un **portal de administración** protegido que permite gestionar todo el contenido de la empresa desde un panel centralizado.

---

## 🌐 Pagina web

[ERCO](https://www.ercosrl.com.ar/)


## ✨ Funcionalidades

### Sitio Público
- Página de inicio con información institucional
- Catálogo de productos con filtros por categoría y búsqueda por nombre
- Sección de marcas representadas
- Página de promociones con fechas de vigencia y descarga de PDF
- Mapa de sucursales con ubicación geográfica (latitud/longitud)
- Formulario de consultas con envío de email automático
- Suscripción a newsletter con envío masivo de emails

### Panel de Administración (acceso restringido)
- Login seguro con autenticación basada en ASP.NET Core Identity
- Gestión completa (ABM) de:
  - 📦 Productos (nombre, categoría, marca, imagen, estado activo/inactivo)
  - 🏷️ Marcas (nombre, logo, estado)
  - 🗂️ Categorías
  - 🎯 Promociones (título, descripción, fechas de inicio/fin, imagen, PDF adjunto)
  - 📍 Sucursales (nombre, ubicación, coordenadas, sucursal principal)
  - 👥 Usuarios
- Filtros y búsqueda en todas las secciones
- Servicio de email para comunicaciones con clientes y suscriptores

---

## 🛠️ Tecnologías utilizadas

| Capa | Tecnología |
|------|-----------|
| Backend | ASP.NET Core 8.0 (MVC), C# |
| ORM | Entity Framework Core 8.0 |
| Base de datos | SQL Server |
| Autenticación | ASP.NET Core Identity |
| Email | SMTP / MimeKit / Resend |
| Frontend | HTML, CSS, JavaScript (Razor Views) |
| Control de versiones | Git / GitHub |

---

## 📁 Estructura del proyecto

```
ERCOWeb/
├── Controllers/
│   ├── HomeController.cs          # Página principal
│   ├── ProductosController.cs     # Catálogo público
│   ├── PromocionesController.cs   # Promociones públicas
│   ├── SucursalesController.cs    # Mapa de sucursales
│   ├── NuestrasMarcas.cs          # Marcas públicas
│   ├── NosotrosController.cs      # Info institucional
│   ├── ConsultasController.cs     # Formulario de contacto
│   ├── SuscripcionesController.cs # Newsletter
│   ├── AdminController.cs         # Panel de administración
│   ├── AccountController.cs       # Login / autenticación
│   └── ZonaController.cs          # Zonas de cobertura
├── Models/
│   ├── Producto.cs
│   ├── Marca.cs
│   ├── Categorium.cs
│   ├── Promo.cs
│   ├── Sucursal.cs
│   ├── Zona.cs
│   ├── Usuario.cs
│   └── ErcoContext.cs             # DbContext (EF Core)
├── Views/
│   ├── Admin/                     # Vistas del panel admin
│   ├── Home/
│   ├── Productos/
│   ├── Promociones/
│   ├── Sucursales/
│   └── Shared/
├── Servicios/
│   └── ServicioEmail.cs           # Envío de emails SMTP
├── Migrations/                    # Migraciones EF Core
└── wwwroot/
    ├── images/marcas/             # Logos de marcas
    └── promoPDF/                  # PDFs de promociones
```

---

## ⚙️ Configuración y ejecución local

### Requisitos previos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local o remoto)
- Visual Studio 2022 o VS Code

### Pasos

1. **Clonar el repositorio**
```bash
git clone https://github.com/RenzoBisso/ERCOWeb.git
cd ERCOWeb
```

2. **Configurar `appsettings.json`**

Completar con tus datos de conexión y credenciales:

```json
{
  "ConnectionStrings": {
    "ERCOContext": "Server=TU_SERVIDOR;Database=ERCODb;Trusted_Connection=True;"
  },
  "AdminSettings": {
    "Email": "admin@erco.com",
    "Password": "TuPasswordSegura123"
  },
  "CONFIGURACIONES_EMAIL": {
    "EMAIL": "tu_email@gmail.com",
    "PASSWORD": "tu_app_password",
    "HOST": "smtp.gmail.com",
    "PUERTO": "587"
  }
}
```

3. **Aplicar migraciones y crear la base de datos**
```bash
dotnet ef database update
```

4. **Ejecutar el proyecto**
```bash
dotnet run
```

5. Abrir en el navegador: `https://localhost:5001`

> El administrador se crea automáticamente al iniciar si no existe, usando las credenciales de `AdminSettings`.

---

## 🗄️ Diagrama de base de datos

El archivo `Diagrama BD.drawio` en la raíz del proyecto contiene el diagrama entidad-relación completo. Se puede abrir con [draw.io](https://app.diagrams.net/).

**Entidades principales:**
- `Productos` → relacionado con `Marcas` y `Categorias`
- `Promos` → con imagen y PDF adjunto
- `Sucursales` → con coordenadas geográficas
- `Usuarios` → gestionados via ASP.NET Core Identity

---

## 📦 Paquetes NuGet utilizados

| Paquete | Versión | Uso |
|---------|---------|-----|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.21 | Autenticación y roles |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.25 | ORM con SQL Server |
| Microsoft.EntityFrameworkCore.Tools | 8.0.25 | Migraciones |
| MimeKit | 4.15.1 | Construcción de emails |
| Resend | 0.5.0 | Servicio de envío de emails |

---

## 👨‍💻 Autor

**Renzo Martin Bisso**
Estudiante de Licenciatura en Sistemas de Información — Universidad Nacional de Luján

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=flat&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/renzo-martín-bisso/)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white)](https://github.com/RenzoBisso)
[![Portfolio](https://img.shields.io/badge/Portfolio-000000?style=flat&logo=vercel&logoColor=white)](#)

---

## 📄 Licencia

Este proyecto fue desarrollado como trabajo freelance para uso privado del cliente ERCO. El código se comparte con fines de portfolio.
