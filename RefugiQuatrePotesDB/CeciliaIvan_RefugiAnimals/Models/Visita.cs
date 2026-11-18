using System.ComponentModel.DataAnnotations;

namespace CeciliaIvan_RefugiAnimals.Models;

public class Visita
{
    [Key]
    public int VisitaID { get; set; }
    public DateTime Data{ get; set; }
    [Required]
    public Motiu Motiu{ get; set; }
    [Required]
    public int Cost { get; set; }
    [Required]
    public virtual Animal AnimalVisitat { get; set; }
    
    public Visita()
    {
        Data = DateTime.Now;
        
    }
}