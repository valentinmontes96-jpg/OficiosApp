using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OficiosApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicialMariaDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CATEGORIAS",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IconoUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIAS", x => x.IdCategoria);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ESTADOS_CITA",
                columns: table => new
                {
                    IdEstadoCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADOS_CITA", x => x.IdEstadoCita);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.IdRol);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ZONAS",
                columns: table => new
                {
                    IdZona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CodigoPostal = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZONAS", x => x.IdZona);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_USUARIOS_ROLES_IdRol",
                        column: x => x.IdRol,
                        principalTable: "ROLES",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CLIENTES",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    DireccionReferencia = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTES", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_CLIENTES_USUARIOS_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NOTIFICACIONES",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdUsuarioDestinatario = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Mensaje = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ReferenciaId = table.Column<int>(type: "int", nullable: true),
                    Leida = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICACIONES", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_NOTIFICACIONES_USUARIOS_IdUsuarioDestinatario",
                        column: x => x.IdUsuarioDestinatario,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PROFESIONALES",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Biografia = table.Column<string>(type: "longtext", nullable: true),
                    TarifaBase = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    DisponibleUrgencia = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UltimaActualizacionDisponibilidad = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFESIONALES", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_PROFESIONALES_USUARIOS_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "REPORTES_USUARIOS",
                columns: table => new
                {
                    IdReporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdUsuarioDenunciante = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioDenunciado = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false),
                    FechaReporte = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    IdAdministradorResolutor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPORTES_USUARIOS", x => x.IdReporte);
                    table.ForeignKey(
                        name: "FK_REPORTES_USUARIOS_USUARIOS_IdAdministradorResolutor",
                        column: x => x.IdAdministradorResolutor,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_REPORTES_USUARIOS_USUARIOS_IdUsuarioDenunciado",
                        column: x => x.IdUsuarioDenunciado,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_REPORTES_USUARIOS_USUARIOS_IdUsuarioDenunciante",
                        column: x => x.IdUsuarioDenunciante,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CITAS",
                columns: table => new
                {
                    IdCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdUsuarioCliente = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioProfesional = table.Column<int>(type: "int", nullable: false),
                    IdEstadoCita = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaServicioPactada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DescripcionProblema = table.Column<string>(type: "longtext", nullable: false),
                    DireccionServicio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    FechaRespuesta = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITAS", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_CITAS_CLIENTES_IdUsuarioCliente",
                        column: x => x.IdUsuarioCliente,
                        principalTable: "CLIENTES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CITAS_ESTADOS_CITA_IdEstadoCita",
                        column: x => x.IdEstadoCita,
                        principalTable: "ESTADOS_CITA",
                        principalColumn: "IdEstadoCita",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CITAS_PROFESIONALES_IdUsuarioProfesional",
                        column: x => x.IdUsuarioProfesional,
                        principalTable: "PROFESIONALES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FAVORITOS",
                columns: table => new
                {
                    IdUsuarioCliente = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioProfesional = table.Column<int>(type: "int", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAVORITOS", x => new { x.IdUsuarioCliente, x.IdUsuarioProfesional });
                    table.ForeignKey(
                        name: "FK_FAVORITOS_CLIENTES_IdUsuarioCliente",
                        column: x => x.IdUsuarioCliente,
                        principalTable: "CLIENTES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FAVORITOS_PROFESIONALES_IdUsuarioProfesional",
                        column: x => x.IdUsuarioProfesional,
                        principalTable: "PROFESIONALES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FOTOS_PORTFOLIO",
                columns: table => new
                {
                    IdFoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdUsuarioProfesional = table.Column<int>(type: "int", nullable: false),
                    ImagenUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    FechaCarga = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOTOS_PORTFOLIO", x => x.IdFoto);
                    table.ForeignKey(
                        name: "FK_FOTOS_PORTFOLIO_PROFESIONALES_IdUsuarioProfesional",
                        column: x => x.IdUsuarioProfesional,
                        principalTable: "PROFESIONALES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HORARIOS_ATENCION",
                columns: table => new
                {
                    IdHorario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdUsuarioProfesional = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    HoraApertura = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    HoraCierre = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HORARIOS_ATENCION", x => x.IdHorario);
                    table.ForeignKey(
                        name: "FK_HORARIOS_ATENCION_PROFESIONALES_IdUsuarioProfesional",
                        column: x => x.IdUsuarioProfesional,
                        principalTable: "PROFESIONALES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PROFESIONALES_CATEGORIAS",
                columns: table => new
                {
                    IdUsuarioProfesional = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFESIONALES_CATEGORIAS", x => new { x.IdUsuarioProfesional, x.IdCategoria });
                    table.ForeignKey(
                        name: "FK_PROFESIONALES_CATEGORIAS_CATEGORIAS_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CATEGORIAS",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROFESIONALES_CATEGORIAS_PROFESIONALES_IdUsuarioProfesional",
                        column: x => x.IdUsuarioProfesional,
                        principalTable: "PROFESIONALES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PROFESIONALES_ZONAS",
                columns: table => new
                {
                    IdUsuarioProfesional = table.Column<int>(type: "int", nullable: false),
                    IdZona = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFESIONALES_ZONAS", x => new { x.IdUsuarioProfesional, x.IdZona });
                    table.ForeignKey(
                        name: "FK_PROFESIONALES_ZONAS_PROFESIONALES_IdUsuarioProfesional",
                        column: x => x.IdUsuarioProfesional,
                        principalTable: "PROFESIONALES",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROFESIONALES_ZONAS_ZONAS_IdZona",
                        column: x => x.IdZona,
                        principalTable: "ZONAS",
                        principalColumn: "IdZona",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HISTORIAL_ESTADOS_CITA",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdCita = table.Column<int>(type: "int", nullable: false),
                    IdEstadoCita = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioAccion = table.Column<int>(type: "int", nullable: false),
                    FechaCambio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observaciones = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORIAL_ESTADOS_CITA", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_HISTORIAL_ESTADOS_CITA_CITAS_IdCita",
                        column: x => x.IdCita,
                        principalTable: "CITAS",
                        principalColumn: "IdCita",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HISTORIAL_ESTADOS_CITA_ESTADOS_CITA_IdEstadoCita",
                        column: x => x.IdEstadoCita,
                        principalTable: "ESTADOS_CITA",
                        principalColumn: "IdEstadoCita",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HISTORIAL_ESTADOS_CITA_USUARIOS_IdUsuarioAccion",
                        column: x => x.IdUsuarioAccion,
                        principalTable: "USUARIOS",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RESENAS",
                columns: table => new
                {
                    IdResena = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdCita = table.Column<int>(type: "int", nullable: false),
                    Puntuacion = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Comentario = table.Column<string>(type: "longtext", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESENAS", x => x.IdResena);
                    table.ForeignKey(
                        name: "FK_RESENAS_CITAS_IdCita",
                        column: x => x.IdCita,
                        principalTable: "CITAS",
                        principalColumn: "IdCita",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CITAS_IdEstadoCita",
                table: "CITAS",
                column: "IdEstadoCita");

            migrationBuilder.CreateIndex(
                name: "IX_CITAS_IdUsuarioCliente",
                table: "CITAS",
                column: "IdUsuarioCliente");

            migrationBuilder.CreateIndex(
                name: "IX_CITAS_IdUsuarioProfesional",
                table: "CITAS",
                column: "IdUsuarioProfesional");

            migrationBuilder.CreateIndex(
                name: "IX_FAVORITOS_IdUsuarioProfesional",
                table: "FAVORITOS",
                column: "IdUsuarioProfesional");

            migrationBuilder.CreateIndex(
                name: "IX_FOTOS_PORTFOLIO_IdUsuarioProfesional",
                table: "FOTOS_PORTFOLIO",
                column: "IdUsuarioProfesional");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIAL_ESTADOS_CITA_IdCita",
                table: "HISTORIAL_ESTADOS_CITA",
                column: "IdCita");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIAL_ESTADOS_CITA_IdEstadoCita",
                table: "HISTORIAL_ESTADOS_CITA",
                column: "IdEstadoCita");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIAL_ESTADOS_CITA_IdUsuarioAccion",
                table: "HISTORIAL_ESTADOS_CITA",
                column: "IdUsuarioAccion");

            migrationBuilder.CreateIndex(
                name: "IX_HORARIOS_ATENCION_IdUsuarioProfesional",
                table: "HORARIOS_ATENCION",
                column: "IdUsuarioProfesional");

            migrationBuilder.CreateIndex(
                name: "IX_NOTIFICACIONES_IdUsuarioDestinatario",
                table: "NOTIFICACIONES",
                column: "IdUsuarioDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_PROFESIONALES_CATEGORIAS_IdCategoria",
                table: "PROFESIONALES_CATEGORIAS",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_PROFESIONALES_ZONAS_IdZona",
                table: "PROFESIONALES_ZONAS",
                column: "IdZona");

            migrationBuilder.CreateIndex(
                name: "IX_REPORTES_USUARIOS_IdAdministradorResolutor",
                table: "REPORTES_USUARIOS",
                column: "IdAdministradorResolutor");

            migrationBuilder.CreateIndex(
                name: "IX_REPORTES_USUARIOS_IdUsuarioDenunciado",
                table: "REPORTES_USUARIOS",
                column: "IdUsuarioDenunciado");

            migrationBuilder.CreateIndex(
                name: "IX_REPORTES_USUARIOS_IdUsuarioDenunciante",
                table: "REPORTES_USUARIOS",
                column: "IdUsuarioDenunciante");

            migrationBuilder.CreateIndex(
                name: "IX_RESENAS_IdCita",
                table: "RESENAS",
                column: "IdCita",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_Email",
                table: "USUARIOS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_IdRol",
                table: "USUARIOS",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAVORITOS");

            migrationBuilder.DropTable(
                name: "FOTOS_PORTFOLIO");

            migrationBuilder.DropTable(
                name: "HISTORIAL_ESTADOS_CITA");

            migrationBuilder.DropTable(
                name: "HORARIOS_ATENCION");

            migrationBuilder.DropTable(
                name: "NOTIFICACIONES");

            migrationBuilder.DropTable(
                name: "PROFESIONALES_CATEGORIAS");

            migrationBuilder.DropTable(
                name: "PROFESIONALES_ZONAS");

            migrationBuilder.DropTable(
                name: "REPORTES_USUARIOS");

            migrationBuilder.DropTable(
                name: "RESENAS");

            migrationBuilder.DropTable(
                name: "CATEGORIAS");

            migrationBuilder.DropTable(
                name: "ZONAS");

            migrationBuilder.DropTable(
                name: "CITAS");

            migrationBuilder.DropTable(
                name: "CLIENTES");

            migrationBuilder.DropTable(
                name: "ESTADOS_CITA");

            migrationBuilder.DropTable(
                name: "PROFESIONALES");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropTable(
                name: "ROLES");
        }
    }
}
