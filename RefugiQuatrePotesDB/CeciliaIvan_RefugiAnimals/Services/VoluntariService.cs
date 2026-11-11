using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;

namespace CeciliaIvan_RefugiAnimals.Services;

public class VoluntariService
{
    private RefugiContext _context;

    public VoluntariService(RefugiContext context)
    {
        _context = context;
    }
    public List<Voluntari> GetTots()
    {
        List<Voluntari> llista = _context.Voluntaris
            .OrderBy(a => a.VoluntariID)
            .ToList();
        return llista;
    }
    
    public bool AssignarAnimal(string nomVoluntari, string codiXip)
    {
        Voluntari? v = _context.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);
        if(v == null){ Console.WriteLine("No existeix el voluntari"); return false; }
        Animal? an = _context.Animals.FirstOrDefault(an =>an.Xip.Codi == codiXip);
        if(an==null) { Console.WriteLine("No existeix el animal"); return false; }
        
        if(an.EstaAdoptat){ Console.WriteLine("El animal esta adoptat"); return false; }
        if(v.Animals.Count>=4){ Console.WriteLine("El voluntari ja te el maxim d'animals"); return false; }
        if (v.Animals.Contains(an)) { Console.WriteLine("L'animal ja esta assignat a aquest voluntari"); return false; }
        
        v.Animals.Add(an);
        _context.SaveChanges();
        Console.WriteLine($"S'ha assignat {an.Nom} a {v.Nom}");
        return true;
    }
    //Ej3.2
    public bool DesassignarAnimal(string nomVoluntari, string codiXip)
    {
        Voluntari? v = _context.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);
        if(v == null){ Console.WriteLine("No existeix el voluntari"); return false; }
        Animal? an = _context.Animals.FirstOrDefault(an =>an.Xip.Codi == codiXip);
        if(an==null) { Console.WriteLine("No existeix el animal"); return false; }
        
        if(!v.Animals.Contains(an)) { Console.WriteLine("El voluntari no te aquest animal"); return false; }
        v.Animals.Remove(an);
        _context.SaveChanges();
        Console.WriteLine($"S'ha desassignat {an.Nom} de {v.Nom}");
        return true;
    }
    //Ej3.3
    public bool RegistrarPasseig(string nomVoluntari, string codiXip, int minuts)
    {
        Voluntari? v = _context.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);
        if(v == null){ Console.WriteLine("No existeix el voluntari"); return false; }
        Animal? an = _context.Animals.FirstOrDefault(an =>an.Xip.Codi == codiXip);
        if(an==null) { Console.WriteLine("No existeix el animal"); return false; }
        if(!v.Animals.Contains(an)) { Console.WriteLine("El voluntari no te al animal"); return false; }
        if(an.Especie!=Especie.GOS){Console.WriteLine("L'animal no es un gos"); return false; }
        if (minuts <= 0) { Console.WriteLine("Els minuts han de ser mes grans que 0"); return false; }

        int antiguo = v.MinutsPasseig;
        v.MinutsPasseig += minuts;
        _context.SaveChanges();
        Console.WriteLine($"El voluntari ha passat de {antiguo} minuts a {v.MinutsPasseig} minuts");
        return true;
    }
}