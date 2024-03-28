using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace back.Models;

public partial class WotContext : DbContext
{
    public WotContext()
    {
    }

    public WotContext(DbContextOptions<WotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClanWar> ClanWars { get; set; }

    public virtual DbSet<ClanWarJoueur> ClanWarJoueurs { get; set; }

    public virtual DbSet<Joueur> Joueurs { get; set; }

    public virtual DbSet<Tank> Tanks { get; set; }

    public virtual DbSet<TankStatut> TankStatuts { get; set; }

    public virtual DbSet<Tier> Tiers { get; set; }

    public virtual DbSet<TypeTank> TypeTanks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClanWar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClanWar__3213E83F5EAFC08F");

            entity.ToTable("ClanWar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
        });

        modelBuilder.Entity<ClanWarJoueur>(entity =>
        {
            entity.HasKey(e => new { e.IdClanWar, e.IdJoueur }).HasName("PK__ClanWarJ__84B1189D56C2E4CC");

            entity.ToTable("ClanWarJoueur");

            entity.Property(e => e.IdClanWar).HasColumnName("idClanWar");
            entity.Property(e => e.IdJoueur).HasColumnName("idJoueur");
            entity.Property(e => e.IdTank).HasColumnName("idTank");

            entity.HasOne(d => d.IdClanWarNavigation).WithMany(p => p.ClanWarJoueurs)
                .HasForeignKey(d => d.IdClanWar)
                .HasConstraintName("FK__ClanWarJo__idCla__76969D2E");

            entity.HasOne(d => d.IdJoueurNavigation).WithMany(p => p.ClanWarJoueurs)
                .HasForeignKey(d => d.IdJoueur)
                .HasConstraintName("FK__ClanWarJo__idJou__778AC167");

            entity.HasOne(d => d.IdTankNavigation).WithMany(p => p.ClanWarJoueurs)
                .HasForeignKey(d => d.IdTank)
                .HasConstraintName("FK__ClanWarJo__idTan__787EE5A0");
        });

        modelBuilder.Entity<Joueur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Joueur__3213E83F0D3C965C");

            entity.ToTable("Joueur");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EstActiver)
                .HasDefaultValue(1)
                .HasColumnName("estActiver");
            entity.Property(e => e.EstAdmin).HasColumnName("estAdmin");
            entity.Property(e => e.EstStrateur).HasColumnName("estStrateur");
            entity.Property(e => e.IdDiscord)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("idDiscord");
            entity.Property(e => e.Mdp)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("mdp");
            entity.Property(e => e.Pseudo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pseudo");

            entity.HasMany(d => d.IdTanks).WithMany(p => p.IdJoueurs)
                .UsingEntity<Dictionary<string, object>>(
                    "TankJoueur",
                    r => r.HasOne<Tank>().WithMany()
                        .HasForeignKey("IdTank")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__TankJoueu__idTan__7C4F7684"),
                    l => l.HasOne<Joueur>().WithMany()
                        .HasForeignKey("IdJoueur")
                        .HasConstraintName("FK__TankJoueu__idJou__7B5B524B"),
                    j =>
                    {
                        j.HasKey("IdJoueur", "IdTank").HasName("PK__TankJoue__48DC76C8C17C2EF2");
                        j.ToTable("TankJoueur");
                        j.IndexerProperty<int>("IdJoueur").HasColumnName("idJoueur");
                        j.IndexerProperty<int>("IdTank").HasColumnName("idTank");
                    });
        });

        modelBuilder.Entity<Tank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tank__3213E83FB782C6B9");

            entity.ToTable("Tank");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EstVisible)
                .HasDefaultValue(1)
                .HasColumnName("estVisible");
            entity.Property(e => e.IdTankStatut).HasColumnName("idTankStatut");
            entity.Property(e => e.IdTier).HasColumnName("idTier");
            entity.Property(e => e.IdTypeTank).HasColumnName("idTypeTank");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.IdTankStatutNavigation).WithMany(p => p.Tanks)
                .HasForeignKey(d => d.IdTankStatut)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tank__idTankStat__6D0D32F4");

            entity.HasOne(d => d.IdTierNavigation).WithMany(p => p.Tanks)
                .HasForeignKey(d => d.IdTier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tank__idTier__6E01572D");

            entity.HasOne(d => d.IdTypeTankNavigation).WithMany(p => p.Tanks)
                .HasForeignKey(d => d.IdTypeTank)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tank__idTypeTank__6EF57B66");
        });

        modelBuilder.Entity<TankStatut>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TankStat__3213E83F00CE87B6");

            entity.ToTable("TankStatut");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Tier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tier__3213E83FD1A34B67");

            entity.ToTable("Tier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<TypeTank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeTank__3213E83FB21D951C");

            entity.ToTable("TypeTank");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.NomImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nomImage");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
