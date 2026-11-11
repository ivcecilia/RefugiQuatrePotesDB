using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;

namespace CeciliaIvan_RefugiAnimals.Services;

public class AnimalService
{
    private RefugiContext _context;

    public AnimalService(RefugiContext context)
    {
        _context = context;
    }

    public List<Animal> GetTots()
    {
        List<Animal> llista = _context.Animals
            .OrderBy(a => a.AnimalID)
            .ToList();
        return llista;
    }
    //Ej1.2
    public List<Animal> GetDisponibles(Especie especie)
    {
        List<Animal> llista = _context.Animals
            .Where(a => a.Especie == especie && !a.EstaAdoptat)
            .OrderBy(a => a.DataEntrada)
            .ToList();
        return llista;
    }

    public Animal? CercaPerXip(string codiXip)
    {

        if (codiXip.Length != 15 || !codiXip.All(char.IsDigit)||string.IsNullOrEmpty(codiXip))
        {
            Console.WriteLine($"El codi {codiXip} no és vàlid: han de ser 15 dígits");
            return null;
        }

        Animal? animal = _context.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);
        if (animal == null)
        {
            Console.WriteLine($"No hi ha cap animal amb el xip {codiXip}");
            return null;
        }
        return animal;
    }
    //Ej1.3
    public List<Animal> CercaPerNom(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return new List<Animal>();
        string cerca = text.ToLower();
        List<Animal> llista = _context.Animals
            .Where(a => a.Nom.ToLower().Contains(cerca))
            .ToList();
        return llista;

    }
}