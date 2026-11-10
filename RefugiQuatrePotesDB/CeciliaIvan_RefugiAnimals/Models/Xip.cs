using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CeciliaIvan_RefugiAnimals.Models;

[Index(nameof(Codi), IsUnique = true)]
public class Xip
{
    [Key]
    public int XipID { get; set; }
    [Required, StringLength(15, MinimumLength = 15)]
    public string Codi{ get; set; }
    [Required]
    public DateTime DataImplantacio{ get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public Xip()
    {
        DataImplantacio = DateTime.Now;
    }
    
}