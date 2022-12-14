using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace back.Models
{
    public partial class WOTContext : DbContext
    {
        public WOTContext()
        {
        }

        public WOTContext(DbContextOptions<WOTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClanWar> ClanWars { get; set; } = null!;
        public virtual DbSet<ClanWarJoueur> ClanWarJoueurs { get; set; } = null!;
        public virtual DbSet<Joueur> Joueurs { get; set; } = null!;
        public virtual DbSet<Tank> Tanks { get; set; } = null!;
        public virtual DbSet<TankStatut> TankStatuts { get; set; } = null!;
        public virtual DbSet<Tier> Tiers { get; set; } = null!;
        public virtual DbSet<TypeTank> TypeTanks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClanWar>(entity =>
            {
                entity.ToTable("ClanWar");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");
            });

            modelBuilder.Entity<ClanWarJoueur>(entity =>
            {
                entity.HasKey(e => new { e.IdClanWar, e.IdJoueur })
                    .HasName("PK__ClanWarJ__84B1189DA2C58C56");

                entity.ToTable("ClanWarJoueur");

                entity.Property(e => e.IdClanWar).HasColumnName("idClanWar");

                entity.Property(e => e.IdJoueur).HasColumnName("idJoueur");

                entity.Property(e => e.IdTank).HasColumnName("idTank");

                entity.HasOne(d => d.IdClanWarNavigation)
                    .WithMany(p => p.ClanWarJoueurs)
                    .HasForeignKey(d => d.IdClanWar)
                    .HasConstraintName("FK__ClanWarJo__idCla__4B7734FF");

                entity.HasOne(d => d.IdJoueurNavigation)
                    .WithMany(p => p.ClanWarJoueurs)
                    .HasForeignKey(d => d.IdJoueur)
                    .HasConstraintName("FK__ClanWarJo__idJou__4C6B5938");

                entity.HasOne(d => d.IdTankNavigation)
                    .WithMany(p => p.ClanWarJoueurs)
                    .HasForeignKey(d => d.IdTank)
                    .HasConstraintName("FK__ClanWarJo__idTan__4D5F7D71");
            });

            modelBuilder.Entity<Joueur>(entity =>
            {
                entity.ToTable("Joueur");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EstActiver)
                    .HasColumnName("estActiver")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EstAdmin).HasColumnName("estAdmin");

                entity.Property(e => e.EstStrateur).HasColumnName("estStrateur");

                entity.Property(e => e.IdDiscord)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("idDiscord");

                entity.Property(e => e.Pseudo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("pseudo");

                entity.HasMany(d => d.IdTanks)
                    .WithMany(p => p.IdJoueurs)
                    .UsingEntity<Dictionary<string, object>>(
                        "TankJoueur",
                        l => l.HasOne<Tank>().WithMany().HasForeignKey("IdTank").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TankJoueu__idTan__51300E55"),
                        r => r.HasOne<Joueur>().WithMany().HasForeignKey("IdJoueur").HasConstraintName("FK__TankJoueu__idJou__503BEA1C"),
                        j =>
                        {
                            j.HasKey("IdJoueur", "IdTank").HasName("PK__TankJoue__48DC76C86DEEC423");

                            j.ToTable("TankJoueur");

                            j.IndexerProperty<int>("IdJoueur").HasColumnName("idJoueur");

                            j.IndexerProperty<int>("IdTank").HasColumnName("idTank");
                        });
            });

            modelBuilder.Entity<Tank>(entity =>
            {
                entity.ToTable("Tank");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EstVisible)
                    .HasColumnName("estVisible")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdTankStatut).HasColumnName("idTankStatut");

                entity.Property(e => e.IdTier).HasColumnName("idTier");

                entity.Property(e => e.IdTypeTank).HasColumnName("idTypeTank");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.HasOne(d => d.IdTankStatutNavigation)
                    .WithMany(p => p.Tanks)
                    .HasForeignKey(d => d.IdTankStatut)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tank__idTankStat__41EDCAC5");

                entity.HasOne(d => d.IdTierNavigation)
                    .WithMany(p => p.Tanks)
                    .HasForeignKey(d => d.IdTier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tank__idTier__42E1EEFE");

                entity.HasOne(d => d.IdTypeTankNavigation)
                    .WithMany(p => p.Tanks)
                    .HasForeignKey(d => d.IdTypeTank)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tank__idTypeTank__43D61337");
            });

            modelBuilder.Entity<TankStatut>(entity =>
            {
                entity.ToTable("TankStatut");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Tier>(entity =>
            {
                entity.ToTable("Tier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<TypeTank>(entity =>
            {
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
}
