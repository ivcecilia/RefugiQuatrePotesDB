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
    //Ej4
    public bool CanviarMare(string codiXip, string? codiXipMare)
    {
        if (codiXip.Length != 15 || !codiXip.All(char.IsDigit)||string.IsNullOrEmpty(codiXip))
        {
            Console.WriteLine($"El codi {codiXip} no és vàlid: han de ser 15 dígits");
            return false;
        }
        Animal? animal = _context.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);    
        if (animal == null)
        {
            Console.WriteLine($"No hi ha cap animal amb el xip {codiXip}");
            return false;
        }

        if (codiXipMare == null)
        {
            if (animal.Mare == null)
            {
                Console.WriteLine("L'animal no tenia mare ja");
                return false;
            }
            animal.Mare = null;
            animal.MareID = null;
            if (animal.Mare != null)
            {
                animal.Mare.Cries.Remove(animal);
            }
            Console.WriteLine("L'animal s'ha quedat sense mare");
            _context.SaveChanges();
            return true;
        }
        Animal? mare = _context.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipMare); 
        if (mare == null)
        {
            Console.WriteLine($"No hi ha cap mare animal amb el xip {codiXipMare}");
            return false;
        }
        if (!mare.EsFemella)
        {
            Console.WriteLine("La mare no es femella");
            return false;
        }
        
        
        if (mare == animal.Mare)
        {
            Console.WriteLine("Aquesta mare ja es la mateixa assignada");
            return false;
        }

        animal.Mare = mare;
        animal.MareID = mare.AnimalID;
        mare.Cries.Add(animal);
        Console.WriteLine("S'ha assignat la mare al fill");
        _context.SaveChanges();
        return true;
    }
    //Ej4.2
    public bool RegistrarCria(string codiXipMare, Animal cria)
    {
        if (codiXipMare.Length != 15 || !codiXipMare.All(char.IsDigit)||string.IsNullOrEmpty(codiXipMare))
        {
            Console.WriteLine($"El codi {codiXipMare} no és vàlid: han de ser 15 dígits");
            return false;
        }
        Animal? mare = _context.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipMare);    
        if (mare == null)
        {
            Console.WriteLine($"No hi ha cap animal amb el xip {codiXipMare}");
            return false;
        }
        if (!mare.EsFemella)
        {
            Console.WriteLine("La mare no es femella");
            return false;
        }

        if (cria.Especie != mare.Especie)
        {
            Console.WriteLine("La cria no es de la mateixa especie");
            return false;
        }

        if (cria.DataNaixement <= mare.DataNaixement)
        {
            Console.WriteLine("La cria es mes adulta que la mare");
            return false;
        }
        Animal? animal = _context.Animals.FirstOrDefault(a => a.Xip.Codi == cria.Xip.Codi);
        if (animal != null)
        {
            Console.WriteLine("Un animal existent ja te aquest xip");
            return false;
        }
        cria.Mare = mare;
        cria.MareID = mare.AnimalID;
        mare.Cries.Add(cria);
        Console.WriteLine("S'ha registrat la cria a la mare");
        _context.SaveChanges();
        return true;
    }
    //Ej4.3
}