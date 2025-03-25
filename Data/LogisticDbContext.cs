using Microsoft.EntityFrameworkCore;
using LogisticWebApp.Models;    
namespace LogisticWebApp.Data;
public class LogisticDbContext : DbContext
{
    public LogisticDbContext(DbContextOptions<LogisticDbContext> options)
        : base(options)
    {
    }

  public DbSet<Cliente> Clienti { get; set; }
    public DbSet<Corriere> Corrieri { get; set; }
    public DbSet<Spedizione> Spedizioni { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Notifica> Notifiche { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Corriere>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Spedizione)
            .WithOne(s => s.Feedback)
            .HasForeignKey<Feedback>(f => f.SpedizioneId);
    }
}