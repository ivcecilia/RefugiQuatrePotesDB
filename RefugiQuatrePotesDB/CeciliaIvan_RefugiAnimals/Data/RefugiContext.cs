using CeciliaIvan_RefugiAnimals.Models;
using Microsoft.EntityFrameworkCore;

namespace CeciliaIvan_RefugiAnimals.Data;

public class RefugiContext : DbContext
{
    public RefugiContext(){}
    
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Xip> Xips { get; set; }
    public DbSet<Adoptant> Adoptants { get; set; }
    public DbSet<Voluntari>  Voluntaris { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=RefugiAnimals.db").UseLazyLoadingProxies();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Ej1
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Xip)
            .WithOne(x => x.Animal)
            .HasForeignKey<Animal>("XipID")    
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<Animal>()
            .Property(a => a.Especie)
            .HasConversion<string>();
        //Ej2
        modelBuilder.Entity<Animal>()
            .HasOne(an => an.Adoptant)
            .WithMany(ad => ad.Animals)
            .HasForeignKey(a=>a.AdoptantID)
            .OnDelete(DeleteBehavior.Cascade);
        //Ej3
        modelBuilder.Entity<Animal>()
            .HasMany(v => v.Voluntaris)
            .WithMany(an => an.Animals)
            .UsingEntity(c => c.ToTable("Cura"));


    }
}