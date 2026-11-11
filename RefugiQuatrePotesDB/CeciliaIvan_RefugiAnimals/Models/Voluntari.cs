using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CeciliaIvan_RefugiAnimals.Models;

[Index(nameof(Nom), IsUnique = true)]
public class Voluntari
{
    [Key]
    public int VoluntariID { get; set; }
    [Required, MaxLength(50)]
    public string Nom  { get; set; }
    [Required]
    public Torn Torn { get; set; }
    public int MinutsPasseig { get; set; }
    public virtual ICollection<Animal> Animals { get; set; }
    
    public override string ToString()
    {
        return $"Voluntari: {{VoluntariID = {VoluntariID}, Nom = {Nom}, Torn = {Torn}, MinutsPasseig = {MinutsPasseig}}}";
    }

    public Voluntari()
    {
        MinutsPasseig = 0;
        Animals = new HashSet<Animal>();
    }
}