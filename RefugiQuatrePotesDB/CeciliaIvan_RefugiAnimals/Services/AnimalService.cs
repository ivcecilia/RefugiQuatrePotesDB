using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;

namespace CeciliaIvan_RefugiAnimals.Services;

public class AnimalService
{
    private RefugiContext ctx;

    public AnimalService(RefugiContext context)
    {
        ctx = context;
    }
    public List<Animal> CercaPerNom(Especie especie)
    {
        return ctx.Animals.ToList()
            .Where(a => a.Especie == especie)
            .OrderBy(a => a.DataEntrada)
            .ToList();
    }
    public Animal? CercaPerXip(string codiXip)
    {
        Animal? animal = ctx.Animals
            .FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("No existeix animal amb aquest xip");
        }

        return animal;
    }
}