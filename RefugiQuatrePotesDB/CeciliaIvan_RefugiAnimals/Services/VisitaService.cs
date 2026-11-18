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
        List<Animal> an = _context.Animals.ToList();
        List<Visita> vis = new List<Visita>();
        for (int i = 0; i < quantes; i++)
        {
            
            Animal a = an[rnd.Next(an.Count)];
            DateTime d = DateTime.Today;
            if (a.EstaAdoptat)
            {
                DateTime adopcio = (DateTime)a.DataAdopcio;
                DateTime entrada = a.DataEntrada;
                
                int dif = (adopcio - entrada).Days;
                
                d = adopcio.AddDays(-rnd.Next(0, dif));
            }
            else
            {
                DateTime entrada = a.DataEntrada;
               
                int dif = (DateTime.Today - entrada).Days;
                
                d = DateTime.Today.AddDays(-rnd.Next(0, dif));
            }
            
            Motiu[] motius = Enum.GetValues<Motiu>();
            Motiu m = motius[rnd.Next(motius.Length)];
            int c = 0;
            switch (m)
            {
                case Motiu.CIRURGIA:
                    c= rnd.Next(250, 601);
                    break;
                case Motiu.FERIDA:
                    c= rnd.Next(60, 151);
                    break;
                case Motiu.REVISIO:
                    c= rnd.Next(30, 61);
                    break;
                case Motiu.VACUNA:
                    c= rnd.Next(25, 51);
                    break;
                default:
                    c = 1;
                    break;
            };
            
            
            Console.WriteLine($"Visita assignada animal: {a.Nom}, Data: {d}, Motiu,{m}, Cost{c}$");
            Visita visita = new Visita();
            visita.AnimalVisitat = a;
            visita.Cost = c;
            visita.Data = d;
            visita.Motiu = m;
            
            _context.Add(visita);
        }
        
        _context.SaveChanges();
    }

    public void EstadistiquesPerMotiu()
    {
        
    }
}