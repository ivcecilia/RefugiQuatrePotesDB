using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;

namespace CeciliaIvan_RefugiAnimals.Services;

public class VisitaService
{
    private RefugiContext _context;

    public VisitaService(RefugiContext context)
    {
        _context = context;
    }
    
    public List<Visita> GetTots()
    {
        List<Visita> llista = _context.Visites
            .OrderBy(a => a.VisitaID)
            .ToList();
        return llista;
    }

    public void GenerarVisites(int quantes)
    {
        Random rnd = new Random();
        for (int i = 0; i < quantes; i++)
        {
            List<Animal> an = _context.Animals.ToList();
            Animal a = an[rnd.Next(an.Count)];
            DateTime d = DateTime.Today.AddDays(-rnd.Next(0, 365));
            Motiu[] motius = Enum.GetValues<Motiu>();
            Motiu m = motius[rnd.Next(motius.Length)];
            int c = rnd.Next(20, 201);
            
            Console.WriteLine($"Visita assignada animal: {a.Nom}, Data: {d}, Motiu,{m}, Cost{c}$");
            Visita visita = new Visita();
            visita.AnimalVisitat = a;
            visita.Cost = c;
            visita.Data = d;
            visita.Motiu = m;
            
        }
        _context.SaveChanges();
    }
}