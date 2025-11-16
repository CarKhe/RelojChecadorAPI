using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using relojChecadorAPI.Models;

namespace relojChecadorAPI.Data;

public partial class DbRelojChecadorContext : DbContext
{
    public DbRelojChecadorContext()
    {
    }

    public DbRelojChecadorContext(DbContextOptions<DbRelojChecadorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblArea> TblAreas { get; set; }

    public virtual DbSet<TblAsistencium> TblAsistencia { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblTipoMovimiento> TblTipoMovimientos { get; set; }

    public virtual DbSet<TblUsuario> TblUsuarios { get; set; }

    public virtual DbSet<TblUsuarioArea> TblUsuarioAreas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:DefaultConnection", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_spanish2_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TblArea>(entity =>
        {
            entity.HasKey(e => e.IdArea).HasName("PRIMARY");

            entity.ToTable("tbl_Areas", tb => tb.HasComment("definicion de las Areas donde se pueden registrar el chequeo"));

            entity.Property(e => e.IdArea).HasColumnName("idArea");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("activo");
            entity.Property(e => e.CentroLat)
                .HasPrecision(10, 7)
                .HasColumnName("centroLAT");
            entity.Property(e => e.CentroLon)
                .HasPrecision(10, 7)
                .HasColumnName("centroLON");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Radio).HasColumnName("radio");
        });

        modelBuilder.Entity<TblAsistencium>(entity =>
        {
            entity.HasKey(e => e.IdAsistencia).HasName("PRIMARY");

            entity.ToTable("tbl_Asistencia", tb => tb.HasComment("tabla con el control de los registros  de asistencia"));

            entity.HasIndex(e => e.IdArea, "tbl_Asistencia_tbl_Areas_FK");

            entity.HasIndex(e => e.IdMovimiento, "tbl_Asistencia_tbl_TipoMovimiento_FK");

            entity.HasIndex(e => e.IdUsuario, "tbl_Asistencia_tbl_Usuarios_FK");

            entity.Property(e => e.IdAsistencia).HasColumnName("idAsistencia");
            entity.Property(e => e.DentroZona)
                .HasColumnType("bit(1)")
                .HasColumnName("dentroZona");
            entity.Property(e => e.DistanciaCentro)
                .HasPrecision(10, 2)
                .HasColumnName("distanciaCentro");
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaHora");
            entity.Property(e => e.IdArea).HasColumnName("idArea");
            entity.Property(e => e.IdMovimiento).HasColumnName("idMovimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Latitud)
                .HasPrecision(10, 7)
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasPrecision(10, 7)
                .HasColumnName("longitud");

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.TblAsistencia)
                .HasForeignKey(d => d.IdArea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_Asistencia_tbl_Areas_FK");

            entity.HasOne(d => d.IdMovimientoNavigation).WithMany(p => p.TblAsistencia)
                .HasForeignKey(d => d.IdMovimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_Asistencia_tbl_TipoMovimiento_FK");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TblAsistencia)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_Asistencia_tbl_Usuarios_FK");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("tbl_Roles", tb => tb.HasComment("Tabla encargada de asignar los roles que tendra cada usuario"));

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.RolName)
                .HasMaxLength(20)
                .HasColumnName("rolName");
        });

        modelBuilder.Entity<TblTipoMovimiento>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PRIMARY");

            entity.ToTable("tbl_TipoMovimiento", tb => tb.HasComment("catalogo de los movimientos disponibles"));

            entity.Property(e => e.IdMovimiento).HasColumnName("idMovimiento");
            entity.Property(e => e.Movimiento)
                .HasMaxLength(30)
                .HasColumnName("movimiento");
        });

        modelBuilder.Entity<TblUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("tbl_Usuarios", tb => tb.HasComment("Registro de los Usuarios asignados"));

            entity.HasIndex(e => e.IdRol, "tbl_Usuarios_tbl_Roles_FK");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("activo");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TblUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_Usuarios_tbl_Roles_FK");
        });

        modelBuilder.Entity<TblUsuarioArea>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioArea).HasName("PRIMARY");

            entity.ToTable("tbl_UsuarioArea", tb => tb.HasComment("asigna el area que se desee que se registre el usuario"));

            entity.HasIndex(e => e.IdArea, "tbl_UsuarioArea_tbl_Areas_FK");

            entity.HasIndex(e => e.IdUsuario, "tbl_UsuarioArea_tbl_Usuarios_FK");

            entity.Property(e => e.IdUsuarioArea).HasColumnName("idUsuarioArea");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("activo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdArea).HasColumnName("idArea");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.TblUsuarioAreas)
                .HasForeignKey(d => d.IdArea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_UsuarioArea_tbl_Areas_FK");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TblUsuarioAreas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbl_UsuarioArea_tbl_Usuarios_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
