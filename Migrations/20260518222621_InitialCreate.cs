using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelGenericoApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "afectacion_igv",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "char(2)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_afectacion_igv", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "categoria_producto",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria_producto", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "configuracion",
                columns: table => new
                {
                    id_configuracion = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ruc = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    tasa_igv_hotel = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 18.00m),
                    tasa_igv_productos = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 18.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuracion", x => x.id_configuracion);
                });

            migrationBuilder.CreateTable(
                name: "estado_habitacion",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    permite_checkin = table.Column<bool>(type: "bit", nullable: false),
                    permite_checkout = table.Column<bool>(type: "bit", nullable: false),
                    es_estado_final = table.Column<bool>(type: "bit", nullable: false),
                    color_ui = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estado_habitacion", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "estado_sunat",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    descripcion_larga = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estado_sunat", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "login_attempt",
                columns: table => new
                {
                    id_login_attempt = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    attempted_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    succeeded = table.Column<bool>(type: "bit", nullable: false),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_attempt", x => x.id_login_attempt);
                });

            migrationBuilder.CreateTable(
                name: "metodo_pago",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "char(3)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metodo_pago", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "rol_usuario",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_usuario", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "temporada",
                columns: table => new
                {
                    id_temporada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    multiplicador = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 1.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temporada", x => x.id_temporada);
                });

            migrationBuilder.CreateTable(
                name: "tipo_comprobante",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "char(2)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_comprobante", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "tipo_documento",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "char(1)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_documento", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "tipo_habitacion",
                columns: table => new
                {
                    id_tipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    capacidad = table.Column<int>(type: "int", nullable: true, defaultValue: 2),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    precio_base = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 50.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_habitacion", x => x.id_tipo);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_sunat = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    id_afectacion_igv = table.Column<string>(type: "char(2)", nullable: false, defaultValue: "10"),
                    id_categoria = table.Column<int>(type: "int", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    stock_minimo = table.Column<int>(type: "int", nullable: true, defaultValue: 5),
                    unidad_medida = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "NIU"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto", x => x.id_producto);
                    table.ForeignKey(
                        name: "fk_producto_afectacion",
                        column: x => x.id_afectacion_igv,
                        principalTable: "afectacion_igv",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fk_producto_categoria",
                        column: x => x.id_categoria,
                        principalTable: "categoria_producto",
                        principalColumn: "id_categoria");
                });

            migrationBuilder.CreateTable(
                name: "transicion_estado",
                columns: table => new
                {
                    id_transicion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estado_actual = table.Column<int>(type: "int", nullable: false),
                    id_estado_siguiente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transicion_estado", x => x.id_transicion);
                    table.ForeignKey(
                        name: "fk_transicion_actual",
                        column: x => x.id_estado_actual,
                        principalTable: "estado_habitacion",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "fk_transicion_siguiente",
                        column: x => x.id_estado_siguiente,
                        principalTable: "estado_habitacion",
                        principalColumn: "id_estado");
                });

            migrationBuilder.CreateTable(
                name: "cierre_caja_envio",
                columns: table => new
                {
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    id_estado_sunat = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    fecha_envio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    intentos_envio = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    hash_xml = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cierre_caja_envio", x => x.fecha);
                    table.ForeignKey(
                        name: "fk_cierre_estado_sunat",
                        column: x => x.id_estado_sunat,
                        principalTable: "estado_sunat",
                        principalColumn: "codigo");
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    esta_activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    debe_cambiar_password = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                    table.ForeignKey(
                        name: "fk_usuario_rol",
                        column: x => x.id_rol,
                        principalTable: "rol_usuario",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_documento = table.Column<string>(type: "char(1)", nullable: false),
                    documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nacionalidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "PERUANA"),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    fecha_verificacion_reniec = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id_cliente);
                    table.ForeignKey(
                        name: "fk_cliente_tipo_documento",
                        column: x => x.tipo_documento,
                        principalTable: "tipo_documento",
                        principalColumn: "codigo");
                });

            migrationBuilder.CreateTable(
                name: "tarifa",
                columns: table => new
                {
                    id_tarifa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_tipo_habitacion = table.Column<int>(type: "int", nullable: false),
                    id_temporada = table.Column<int>(type: "int", nullable: true),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tarifa", x => x.id_tarifa);
                    table.ForeignKey(
                        name: "fk_tarifa_temporada",
                        column: x => x.id_temporada,
                        principalTable: "temporada",
                        principalColumn: "id_temporada");
                    table.ForeignKey(
                        name: "fk_tarifa_tipo_habitacion",
                        column: x => x.id_tipo_habitacion,
                        principalTable: "tipo_habitacion",
                        principalColumn: "id_tipo");
                });

            migrationBuilder.CreateTable(
                name: "habitacion",
                columns: table => new
                {
                    id_habitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numero_habitacion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    piso = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    id_tipo = table.Column<int>(type: "int", nullable: false),
                    precio_noche = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 50.00m),
                    id_estado = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    fecha_ultimo_cambio = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    usuario_cambio = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_habitacion", x => x.id_habitacion);
                    table.ForeignKey(
                        name: "fk_habitacion_estado",
                        column: x => x.id_estado,
                        principalTable: "estado_habitacion",
                        principalColumn: "id_estado");
                    table.ForeignKey(
                        name: "fk_habitacion_tipo",
                        column: x => x.id_tipo,
                        principalTable: "tipo_habitacion",
                        principalColumn: "id_tipo");
                    table.ForeignKey(
                        name: "fk_habitacion_usuario",
                        column: x => x.usuario_cambio,
                        principalTable: "usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    id_venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: true),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    fecha_venta = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    metodo_pago = table.Column<string>(type: "char(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.id_venta);
                    table.ForeignKey(
                        name: "fk_venta_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "fk_venta_metodo_pago",
                        column: x => x.metodo_pago,
                        principalTable: "metodo_pago",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fk_venta_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "historial_estado_habitacion",
                columns: table => new
                {
                    id_historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_habitacion = table.Column<int>(type: "int", nullable: false),
                    id_estado_anterior = table.Column<int>(type: "int", nullable: true),
                    id_estado_nuevo = table.Column<int>(type: "int", nullable: true),
                    fecha_cambio = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    observacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_estado_habitacion", x => x.id_historial);
                    table.ForeignKey(
                        name: "fk_historial_habitacion",
                        column: x => x.id_habitacion,
                        principalTable: "habitacion",
                        principalColumn: "id_habitacion");
                    table.ForeignKey(
                        name: "fk_historial_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "reserva",
                columns: table => new
                {
                    id_reserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_habitacion = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    fecha_entrada_prevista = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_salida_prevista = table.Column<DateTime>(type: "datetime", nullable: false),
                    monto_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pendiente"),
                    observaciones = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    es_no_show = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reserva", x => x.id_reserva);
                    table.ForeignKey(
                        name: "fk_reserva_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "fk_reserva_habitacion",
                        column: x => x.id_habitacion,
                        principalTable: "habitacion",
                        principalColumn: "id_habitacion");
                    table.ForeignKey(
                        name: "fk_reserva_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "item_venta",
                columns: table => new
                {
                    id_item = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false, computedColumnSql: "(cantidad * precio_unitario)", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_venta", x => x.id_item);
                    table.ForeignKey(
                        name: "fk_item_venta_producto",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "fk_item_venta_venta",
                        column: x => x.id_venta,
                        principalTable: "venta",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateTable(
                name: "estancia",
                columns: table => new
                {
                    id_estancia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_reserva = table.Column<int>(type: "int", nullable: true),
                    id_habitacion = table.Column<int>(type: "int", nullable: false),
                    id_cliente_titular = table.Column<int>(type: "int", nullable: false),
                    fecha_checkin = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_checkout_prevista = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_checkout_real = table.Column<DateTime>(type: "datetime", nullable: true),
                    monto_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Activa"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estancia", x => x.id_estancia);
                    table.ForeignKey(
                        name: "fk_estancia_cliente",
                        column: x => x.id_cliente_titular,
                        principalTable: "cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "fk_estancia_habitacion",
                        column: x => x.id_habitacion,
                        principalTable: "habitacion",
                        principalColumn: "id_habitacion");
                    table.ForeignKey(
                        name: "fk_estancia_reserva",
                        column: x => x.id_reserva,
                        principalTable: "reserva",
                        principalColumn: "id_reserva");
                });

            migrationBuilder.CreateTable(
                name: "comprobante",
                columns: table => new
                {
                    id_comprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estancia = table.Column<int>(type: "int", nullable: true),
                    id_venta = table.Column<int>(type: "int", nullable: true),
                    tipo_comprobante = table.Column<string>(type: "char(2)", nullable: false),
                    serie = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    correlativo = table.Column<int>(type: "int", nullable: false),
                    fecha_emision = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    monto_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    igv_monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    cliente_documento_tipo = table.Column<string>(type: "char(1)", nullable: true),
                    cliente_documento_num = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cliente_nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    metodo_pago = table.Column<string>(type: "char(3)", nullable: true),
                    id_estado_sunat = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    xml_firmado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdr_zip = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    fecha_envio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    intentos_envio = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    hash_xml = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comprobante", x => x.id_comprobante);
                    table.ForeignKey(
                        name: "fk_comprobante_cliente_tipo",
                        column: x => x.cliente_documento_tipo,
                        principalTable: "tipo_documento",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fk_comprobante_estado_sunat",
                        column: x => x.id_estado_sunat,
                        principalTable: "estado_sunat",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fk_comprobante_estancia",
                        column: x => x.id_estancia,
                        principalTable: "estancia",
                        principalColumn: "id_estancia");
                    table.ForeignKey(
                        name: "fk_comprobante_metodo_pago",
                        column: x => x.metodo_pago,
                        principalTable: "metodo_pago",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fk_comprobante_tipo",
                        column: x => x.tipo_comprobante,
                        principalTable: "tipo_comprobante",
                        principalColumn: "codigo");
                    table.ForeignKey(
                        name: "fk_comprobante_venta",
                        column: x => x.id_venta,
                        principalTable: "venta",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateTable(
                name: "huesped",
                columns: table => new
                {
                    id_huesped = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estancia = table.Column<int>(type: "int", nullable: false),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    es_titular = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_huesped", x => x.id_huesped);
                    table.ForeignKey(
                        name: "fk_huesped_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente");
                    table.ForeignKey(
                        name: "fk_huesped_estancia",
                        column: x => x.id_estancia,
                        principalTable: "estancia",
                        principalColumn: "id_estancia");
                });

            migrationBuilder.CreateTable(
                name: "item_estancia",
                columns: table => new
                {
                    id_item = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_estancia = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false, computedColumnSql: "(cantidad * precio_unitario)", stored: true),
                    fecha_registro = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_estancia", x => x.id_item);
                    table.ForeignKey(
                        name: "fk_item_estancia_estancia",
                        column: x => x.id_estancia,
                        principalTable: "estancia",
                        principalColumn: "id_estancia");
                    table.ForeignKey(
                        name: "fk_item_estancia_producto",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cierre_caja_envio_id_estado_sunat",
                table: "cierre_caja_envio",
                column: "id_estado_sunat");

            migrationBuilder.CreateIndex(
                name: "uq_cliente_documento",
                table: "cliente",
                columns: new[] { "tipo_documento", "documento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_comprobante_cliente",
                table: "comprobante",
                columns: new[] { "cliente_documento_tipo", "cliente_documento_num" });

            migrationBuilder.CreateIndex(
                name: "ix_comprobante_fecha_emision",
                table: "comprobante",
                column: "fecha_emision");

            migrationBuilder.CreateIndex(
                name: "IX_comprobante_id_estado_sunat",
                table: "comprobante",
                column: "id_estado_sunat");

            migrationBuilder.CreateIndex(
                name: "IX_comprobante_id_estancia",
                table: "comprobante",
                column: "id_estancia");

            migrationBuilder.CreateIndex(
                name: "IX_comprobante_id_venta",
                table: "comprobante",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_comprobante_metodo_pago",
                table: "comprobante",
                column: "metodo_pago");

            migrationBuilder.CreateIndex(
                name: "IX_comprobante_tipo_comprobante",
                table: "comprobante",
                column: "tipo_comprobante");

            migrationBuilder.CreateIndex(
                name: "uq_comprobante_serie_correlativo",
                table: "comprobante",
                columns: new[] { "serie", "correlativo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estancia_id_cliente_titular",
                table: "estancia",
                column: "id_cliente_titular");

            migrationBuilder.CreateIndex(
                name: "IX_estancia_id_habitacion",
                table: "estancia",
                column: "id_habitacion");

            migrationBuilder.CreateIndex(
                name: "IX_estancia_id_reserva",
                table: "estancia",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "IX_habitacion_id_estado",
                table: "habitacion",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_habitacion_id_tipo",
                table: "habitacion",
                column: "id_tipo");

            migrationBuilder.CreateIndex(
                name: "IX_habitacion_usuario_cambio",
                table: "habitacion",
                column: "usuario_cambio");

            migrationBuilder.CreateIndex(
                name: "uq_habitacion_numero",
                table: "habitacion",
                column: "numero_habitacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historial_estado_habitacion_id_usuario",
                table: "historial_estado_habitacion",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "ix_historial_habitacion_fecha",
                table: "historial_estado_habitacion",
                columns: new[] { "id_habitacion", "fecha_cambio" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_huesped_id_cliente",
                table: "huesped",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_huesped_id_estancia",
                table: "huesped",
                column: "id_estancia");

            migrationBuilder.CreateIndex(
                name: "ix_item_estancia_estancia",
                table: "item_estancia",
                column: "id_estancia");

            migrationBuilder.CreateIndex(
                name: "IX_item_estancia_id_producto",
                table: "item_estancia",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_item_venta_id_producto",
                table: "item_venta",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "ix_item_venta_venta",
                table: "item_venta",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "ix_login_attempt_ip_fecha",
                table: "login_attempt",
                columns: new[] { "ip_address", "attempted_at" });

            migrationBuilder.CreateIndex(
                name: "IX_login_attempt_username_at",
                table: "login_attempt",
                columns: new[] { "username", "attempted_at" });

            migrationBuilder.CreateIndex(
                name: "ix_producto_codigo_sunat",
                table: "producto",
                column: "codigo_sunat");

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_afectacion_igv",
                table: "producto",
                column: "id_afectacion_igv");

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_categoria",
                table: "producto",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_id_cliente",
                table: "reserva",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_id_habitacion",
                table: "reserva",
                column: "id_habitacion");

            migrationBuilder.CreateIndex(
                name: "IX_reserva_id_usuario",
                table: "reserva",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_tarifa_id_temporada",
                table: "tarifa",
                column: "id_temporada");

            migrationBuilder.CreateIndex(
                name: "IX_tarifa_id_tipo_habitacion",
                table: "tarifa",
                column: "id_tipo_habitacion");

            migrationBuilder.CreateIndex(
                name: "IX_transicion_estado_id_estado_siguiente",
                table: "transicion_estado",
                column: "id_estado_siguiente");

            migrationBuilder.CreateIndex(
                name: "uq_transicion",
                table: "transicion_estado",
                columns: new[] { "id_estado_actual", "id_estado_siguiente" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_id_rol",
                table: "usuario",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_venta_id_cliente",
                table: "venta",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_venta_id_usuario",
                table: "venta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_venta_metodo_pago",
                table: "venta",
                column: "metodo_pago");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cierre_caja_envio");

            migrationBuilder.DropTable(
                name: "comprobante");

            migrationBuilder.DropTable(
                name: "configuracion");

            migrationBuilder.DropTable(
                name: "historial_estado_habitacion");

            migrationBuilder.DropTable(
                name: "huesped");

            migrationBuilder.DropTable(
                name: "item_estancia");

            migrationBuilder.DropTable(
                name: "item_venta");

            migrationBuilder.DropTable(
                name: "login_attempt");

            migrationBuilder.DropTable(
                name: "tarifa");

            migrationBuilder.DropTable(
                name: "transicion_estado");

            migrationBuilder.DropTable(
                name: "estado_sunat");

            migrationBuilder.DropTable(
                name: "tipo_comprobante");

            migrationBuilder.DropTable(
                name: "estancia");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "temporada");

            migrationBuilder.DropTable(
                name: "reserva");

            migrationBuilder.DropTable(
                name: "afectacion_igv");

            migrationBuilder.DropTable(
                name: "categoria_producto");

            migrationBuilder.DropTable(
                name: "metodo_pago");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "habitacion");

            migrationBuilder.DropTable(
                name: "tipo_documento");

            migrationBuilder.DropTable(
                name: "estado_habitacion");

            migrationBuilder.DropTable(
                name: "tipo_habitacion");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "rol_usuario");
        }
    }
}
