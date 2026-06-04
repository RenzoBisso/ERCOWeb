using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity; 

namespace ERCOWeb.Models;

public partial class ErcoContext : IdentityDbContext<IdentityUser>
{
    public ErcoContext()
    {
    }

    public ErcoContext(DbContextOptions<ErcoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }
    public virtual DbSet<Marca> Marcas { get; set; }
    public virtual DbSet<Promo> Promos { get; set; }
    public virtual DbSet<Sucursal> Sucursals { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Zona> Zonas { get; set; }
    public virtual DbSet<Catalogo> Catalogos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);
            entity.ToTable("CATEGORIA");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca);
            entity.ToTable("MARCA");
            entity.Property(e => e.IdMarca).HasColumnName("idMarca");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Imagen).IsUnicode(false).HasColumnName("imagen");
            entity.Property(e => e.Nombre).HasMaxLength(50).IsFixedLength().HasColumnName("nombre");
            entity.Property(e => e.Prioridad).HasColumnName("Prioridad");
        });


          
        modelBuilder.Entity<Promo>(entity =>
        {
            entity.ToTable("PROMO");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).IsUnicode(false).HasColumnName("descripcion");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.Imagen).IsUnicode(false).HasColumnName("imagen");
            entity.Property(e => e.Pdf).IsUnicode(false).HasColumnName("pdf");
            entity.Property(e => e.Titulo).HasMaxLength(50).IsUnicode(false).HasColumnName("titulo");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.IdSucursal);
            entity.ToTable("SUCURSAL");
            entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Latitud).HasColumnType("decimal(18, 15)").HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasColumnType("decimal(18, 15)").HasColumnName("longitud");
            entity.Property(e => e.Nombre).HasMaxLength(100).IsFixedLength().HasColumnName("nombre");
            entity.Property(e => e.Principal).HasColumnName("principal");
            entity.Property(e => e.Ubicacion).HasColumnName("ubicacion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("USUARIO");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false).HasColumnName("email");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre).HasMaxLength(50).IsUnicode(false).HasColumnName("nombre");
        });

        modelBuilder.Entity<Zona>(entity =>
        {
            entity.HasKey(e => e.IdZona).HasName("PK_Zona");
            entity.ToTable("ZONA");
            entity.Property(e => e.IdZona).HasColumnName("idZona");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre).HasMaxLength(50).IsFixedLength().HasColumnName("nombre");
        });

        modelBuilder.Entity<Catalogo>(entity =>
        {
            entity.HasKey(e => e.IdCatalogo);
            entity.ToTable("CATALOGO");
            entity.Property(e => e.IdCatalogo).HasColumnName("idCatalogo");
            entity.Property(e => e.Tipo).HasMaxLength(20).IsUnicode(false).HasColumnName("tipo");
            entity.Property(e => e.Descripcion).HasMaxLength(100).IsUnicode(false).HasColumnName("descripcion");
            entity.Property(e => e.Imagen).IsUnicode(false).HasColumnName("imagen");
            entity.Property(e => e.Pdf).IsUnicode(false).HasColumnName("pdf");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fechaActualizacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}