namespace CeciliaIvan_RefugiAnimals.Models;

public class Animal
{
    public int AnimalID { get; set; }
    public string Nom{ get; set; }
    public Especie Especie{ get; set; }
    public bool EsFemella{ get; set; }
    public DateTime DataNaixement{ get; set; }
    public DateTime DataEntrada{ get; set; }
    public double Pes{ get; set; }
    public bool EstaAdoptat{ get; set; }
    public DateTime DataAdopcio{ get; set; }
    public decimal QuotaAdopcio{ get; set; }
    public int XipID{ get; set; }
}