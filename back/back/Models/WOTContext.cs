using Microsoft.EntityFrameworkCore;

namespace back.Models;
public partial class WOTContext : DbContext
{
    public WOTContext()
    {
    }

    public WOTContext(DbContextOptions<WOTContext> options)
        : base(options)
    {
    }

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
                    l => l.HasOne<Tank>().WithMany().HasForeignKey("IdTank").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TankJoueu__idTan__71D1E811"),
                    r => r.HasOne<Joueur>().WithMany().HasForeignKey("IdJoueur").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TankJoueu__idJou__70DDC3D8"),
                    j =>
                    {
                        j.HasKey("IdJoueur", "IdTank").HasName("PK__TankJoue__48DC76C800792542");

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
                .HasConstraintName("FK__Tank__idTankStat__6754599E");

            entity.HasOne(d => d.IdTierNavigation)
                .WithMany(p => p.Tanks)
                .HasForeignKey(d => d.IdTier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tank__idTier__68487DD7");

            entity.HasOne(d => d.IdTypeTankNavigation)
                .WithMany(p => p.Tanks)
                .HasForeignKey(d => d.IdTypeTank)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tank__idTypeTank__693CA210");
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
