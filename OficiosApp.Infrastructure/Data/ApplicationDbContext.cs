using Microsoft.EntityFrameworkCore;
using OficiosApp.Domain.Entities;

namespace OficiosApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Identidad y Perfiles
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Profesional> Profesionales { get; set; }

    // Clasificación y Geografía
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Zona> Zonas { get; set; }
    public DbSet<ProfesionalCategoria> ProfesionalesCategorias { get; set; }
    public DbSet<ProfesionalZona> ProfesionalesZonas { get; set; }

    // Disponibilidad y Portfolio
    public DbSet<HorarioAtencion> HorariosAtencion { get; set; }
    public DbSet<FotoPortfolio> FotosPortfolio { get; set; }

    // Transacciones y Citas
    public DbSet<EstadoCita> EstadosCita { get; set; }
    public DbSet<Cita> Citas { get; set; }
    public DbSet<HistorialEstadoCita> HistorialEstadosCita { get; set; }

    // Valoraciones y Preferencias
    public DbSet<Resena> Resenas { get; set; }
    public DbSet<Favorito> Favoritos { get; set; }

    // Operación y Moderación
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<ReporteUsuario> ReportesUsuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Claves Primarias Compuestas
        modelBuilder.Entity<ProfesionalCategoria>()
            .HasKey(pc => new { pc.IdUsuarioProfesional, pc.IdCategoria });

        modelBuilder.Entity<ProfesionalZona>()
            .HasKey(pz => new { pz.IdUsuarioProfesional, pz.IdZona });

        modelBuilder.Entity<Favorito>()
            .HasKey(f => new { f.IdUsuarioCliente, f.IdUsuarioProfesional });

        // 2. Relaciones 1 a 1 (Usuario -> Cliente / Profesional)
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Usuario)
            .WithOne()
            .HasForeignKey<Cliente>(c => c.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Profesional>()
            .HasOne(p => p.Usuario)
            .WithOne()
            .HasForeignKey<Profesional>(p => p.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // 3. Relación 1 a 1 (Cita -> Reseña)
        modelBuilder.Entity<Resena>()
            .HasOne(r => r.Cita)
            .WithOne(c => c.Resena)
            .HasForeignKey<Resena>(r => r.IdCita)
            .OnDelete(DeleteBehavior.Cascade);

        // 4. Prevención de ciclos de borrado en cascada (Restrict)
        // Evita errores de PostgreSQL al tener múltiples FK apuntando a la misma tabla
        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Cliente)
            .WithMany()
            .HasForeignKey(c => c.IdUsuarioCliente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Profesional)
            .WithMany()
            .HasForeignKey(c => c.IdUsuarioProfesional)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReporteUsuario>()
            .HasOne(r => r.Denunciante)
            .WithMany()
            .HasForeignKey(r => r.IdUsuarioDenunciante)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReporteUsuario>()
            .HasOne(r => r.Denunciado)
            .WithMany()
            .HasForeignKey(r => r.IdUsuarioDenunciado)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReporteUsuario>()
            .HasOne(r => r.AdministradorResolutor)
            .WithMany()
            .HasForeignKey(r => r.IdAdministradorResolutor)
            .OnDelete(DeleteBehavior.SetNull);
    }
}