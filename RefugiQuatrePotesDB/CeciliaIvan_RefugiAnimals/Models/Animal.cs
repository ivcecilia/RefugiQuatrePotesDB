using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace CeciliaIvan_RefugiAnimals.Models;

public class Animal
{
    [Key]
    public int AnimalID { get; set; }
    [Required,MaxLength(50)]
    public string Nom{ get; set; }
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
    [Column(TypeName = "decimal(8,2)")]
    public decimal QuotaAdopcio{ get; set; }
    [Required]
    public int XipID{ get; set; }
    public virtual Xip Xip { get; set; } = null;

    public override string ToString()
    {
        return $"Animal: {{AnimalId = {AnimalID} ,Nom = {Nom} ,Especie = {Especie} ,EsFemella = {EsFemella} ,DataNaixement = {DataNaixement} ,DataEntrada = {DataEntrada} ,Pes = {Pes} ,EstaAdoptat = {EstaAdoptat} , DataAdopcio = {DataAdopcio} , QuotaAdopcio = {QuotaAdopcio} , XipId = {XipID} , Xip = {Xip}}}";
    }

    public Animal()
    {
        DataEntrada = DateTime.Now;
        EstaAdoptat = false;
        QuotaAdopcio = 0;
    }

    public Animal(string nom, Especie especie, bool esFemella, DateTime dataNaixement, DateTime dataEntrada, double pes, bool estaAdoptat, DateTime dataAdopcio, decimal quotaAdopcio, int xipId)
    {
        Nom = nom;
        Especie = especie;
        EsFemella = esFemella;
        DataNaixement = dataNaixement;
        DataEntrada = dataEntrada;
        Pes = pes;
        EstaAdoptat = estaAdoptat;
        DataAdopcio = dataAdopcio;
        QuotaAdopcio = quotaAdopcio;
        XipID = xipId;
    }
}