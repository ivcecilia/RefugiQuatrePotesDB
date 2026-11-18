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
    public List<Animal> Descendents(string codiXip)
    {
        if (codiXip.Length != 15 || !codiXip.All(char.IsDigit)||string.IsNullOrEmpty(codiXip))
        {
            Console.WriteLine($"El codi {codiXip} no és vàlid: han de ser 15 dígits");
            return null;
        }
        Animal? animal = _context.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);
        if (animal == null)
        {
            Console.WriteLine("No existeix ningun animal amb aquest codi");
            return [];
        }

        List<Animal> cries = new List<Animal>();
        AfegirDescendents(cries, animal);
        return cries;
    }

    public void AfegirDescendents(List<Animal> cries, Animal animal)
    {
        foreach (Animal a in animal.Cries)
        {
            cries.Add(a);
            AfegirDescendents(cries, a);
        }
    }
    //Ej5
    public void CalcularDespesaMes()
    {
        Animal mesCar = null;
        decimal preuMesCar = 0;
        decimal total = 0;
        foreach (Animal a in _context.Animals.ToList())
        {
            if (a.EstaAdoptat) continue;
            decimal previ = a.DespesaAcumulada;
            decimal variable = 0;
            string afe = "";
            switch (a.Especie)
            {
                case(Especie.GOS):
                    variable += 60;
                    break;
                case(Especie.GAT):
                    variable += 40;
                    break;
                case(Especie.CONILL):
                    variable += 15;
                    break;
                case(Especie.AU):
                    variable += 10;
                    break;
                case(Especie.ALTRES):
                    variable += 20;
                    break;
            }
            if (a.Voluntaris.Count>0)
            {
                variable *= 0.9M;
                afe += ", te voluntaris";
            }
            
            if (( DateTime.Today.Year - a.DataNaixement.Year) >= 8)
            {
                variable *= 1.3M;
                afe += ", es senior";
            }

            if (a.Cries.Count > 0)
            {
                
                foreach (Animal ani in a.Cries.ToList())
                {
                    if (!ani.EstaAdoptat)
                    {
                        variable *= 1.1M;
                    }
                }
                afe += ", te "+a.Cries.Count+" cries";
            }

            if (variable > preuMesCar)
            {
                preuMesCar = variable;
                mesCar = a;
            }
            
            total += variable;
            a.DespesaAcumulada += variable;
            _context.SaveChanges();
            Console.WriteLine(a.Nom+": Despesa Previa: "+previ+"$, Despesa Mensual: "+variable+"$, Despesa Actual: "+a.DespesaAcumulada+"$"+afe);
            
        }

        Console.WriteLine("Animal mes car del mes: " + mesCar.Nom+" amb un preu de "+preuMesCar+"$, Gastos mensuals: "+total+"$");
    }
    //Ej6
    public void EstadistiquesPerEspecie()
    {
        var perEspecie = _context.Animals
            .GroupBy(a => a.Especie)
            .Select(e => new
            {
                Especie = e.Key,
                Total = e.Count(),
                Adoptats = e.Count(ag =>ag.EstaAdoptat),
                Disponibles = e.Count(ag =>!ag.EstaAdoptat),
                Pes = e.Average(a => a.Pes)
            })
            .OrderByDescending(e => e.Total)
            .ToList();
        
        foreach (var an in perEspecie)
        {
            Console.WriteLine($"{an.Especie}: {an.Total} Animals, {an.Adoptats} animals adoptats, {an.Disponibles} animals disponibles, {an.Pes} mitja de pes");
        }
    }

}