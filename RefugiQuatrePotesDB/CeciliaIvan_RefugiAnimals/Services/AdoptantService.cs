using System.Runtime.InteropServices.JavaScript;
using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;

namespace CeciliaIvan_RefugiAnimals.Services;

public class AdoptantService
{
    private RefugiContext _context;

    public AdoptantService(RefugiContext context)
    {
        _context = context;
    }
    
    public List<Adoptant> GetTots()
    {
        List<Adoptant> llista = _context.Adoptants
            .OrderBy(a => a.AdoptantID)
            .ToList();
        return llista;
    }

    public bool Adoptar(string DNI, string codiXip)
    {
        Adoptant? a = _context.Adoptants.FirstOrDefault(a=>a.DNI==DNI);
        if(a== null) { Console.WriteLine("No existeix el adoptant"); return false; }

        Animal? an = _context.Animals.FirstOrDefault(an =>an.Xip.Codi == codiXip);
        if(an==null) { Console.WriteLine("No existeix el animal"); return false; }
        if (an.EstaAdoptat){ Console.WriteLine("L'animal ja esta adoptat"); return false; }
        if(an.Especie==Especie.GOS&&!a.TeJardi){ Console.WriteLine("Es un gos i l'adoptant no te jardi"); return false; }
        if(a.Animals.Count>=3){ Console.WriteLine("Ja te mes de 3 animals"); return false; }
        
        an.EstaAdoptat = true;
        an.DataAdopcio = DateTime.Now;
        a.Animals.Add(an);
        _context.Animals.Update(an);
        _context.Adoptants.Update(a);
        _context.SaveChanges();
        Console.WriteLine("S'ha adoptat correctament l'animal amb nom "+an.Nom);
        return true;
    }

    public bool Retornar(string codiXip)
    {
        Animal? an = _context.Animals.FirstOrDefault(an =>an.Xip.Codi == codiXip);
        if(an==null) { Console.WriteLine("No existeix el animal"); return false; }
        if (!an.EstaAdoptat){ Console.WriteLine("L'animal no esta adoptat"); return false; }
        Adoptant a = _context.Adoptants.FirstOrDefault(a=>a.DNI==an.Adoptant.DNI);
        an.EstaAdoptat = false;
        an.Adoptant = null;
        an.DataAdopcio = null;
        an.DataEntrada = DateTime.Now;
        a.Animals.Remove(an);
        _context.Animals.Update(an);
        _context.Adoptants.Update(a);
        _context.SaveChanges();
        return true;
    }
}
