using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace CeciliaIvan_RefugiAnimals.Models;

public class Animal
{
    [Key]
    public int AnimalID { get; set; }
    [Required,MaxLength(50)]
    public string Nom{ get; set; } ="";
    [Required]
    public Especie Especie{ get; set; }
    [Required]
    public bool EsFemella{ get; set; }
    [Required]
    public DateTime DataNaixement{ get; set; }
    [Required]
    public DateTime DataEntrada{ get; set; } 
    [Range(0.1,100)]
    public double Pes{ get; set; }
    public bool EstaAdoptat{ get; set; }
    public DateTime? DataAdopcio{ get; set; }
    [Precision(8, 2)]  
    public decimal QuotaAdopcio{ get; set; }
    [Required]
    public int XipID{ get; set; }
    public virtual Xip Xip { get; set; } = null!;
    public int? AdoptantID{ get; set; }
    public virtual Adoptant? Adoptant{ get; set; }
    public virtual ICollection<Voluntari> Voluntaris { get; set; }

    public override string ToString()
    {
        return $"Animal: {{AnimalId = {AnimalID} ,Nom = {Nom} ,Especie = {Especie} ,EsFemella = {EsFemella} ,DataNaixement = {DataNaixement} ,DataEntrada = {DataEntrada} ,Pes = {Pes} ,EstaAdoptat = {EstaAdoptat} , DataAdopcio = {DataAdopcio} , QuotaAdopcio = {QuotaAdopcio} , XipId = {XipID} , Xip = {Xip}}}";
    }

    public Animal()
    {
        DataEntrada = DateTime.Now;
        EstaAdoptat = false;
        DataAdopcio = null;
        QuotaAdopcio = 0;
        Voluntaris = new HashSet<Voluntari>();
    }

    
}