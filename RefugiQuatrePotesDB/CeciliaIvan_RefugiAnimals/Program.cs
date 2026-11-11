using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;
using CeciliaIvan_RefugiAnimals.Services;
using Microsoft.EntityFrameworkCore;

namespace CeciliaIvan_RefugiAnimals;

internal class Program
{
    static void Main(string[] args)
    {
        using var ctx = new RefugiContext();
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        List<Animal> animals = new List<Animal>()
        {
            new Animal() { Nom = "Luna", Especie = Especie.GOS, EsFemella = true, Pes = 22.5,
                DataNaixement = new DateTime(2020, 3, 14), DataEntrada = new DateTime(2025, 2, 10),
                Xip = new Xip() { Codi = "724098100000001" } },
            new Animal() { Nom = "Thor", Especie = Especie.GOS, EsFemella = false, Pes = 31,
                DataNaixement = new DateTime(2019, 7, 2), DataEntrada = new DateTime(2024, 11, 5),
                Xip = new Xip() { Codi = "724098100000002" } },
            new Animal() { Nom = "Kira", Especie = Especie.GOS, EsFemella = true, Pes = 12.8,
                DataNaixement = new DateTime(2023, 1, 20), DataEntrada = new DateTime(2025, 9, 1),
                Xip = new Xip() { Codi = "724098100000003" } },
            new Animal() { Nom = "Bruc", Especie = Especie.GOS, EsFemella = false, Pes = 18,
                DataNaixement = new DateTime(2021, 5, 9), DataEntrada = new DateTime(2024, 6, 15),
                EstaAdoptat = true, DataAdopcio = new DateTime(2025, 10, 3),
                Xip = new Xip() { Codi = "724098100000004" } },
            new Animal() { Nom = "Mixa", Especie = Especie.GAT, EsFemella = true, Pes = 3.9,
                DataNaixement = new DateTime(2022, 4, 11), DataEntrada = new DateTime(2025, 3, 22),
                Xip = new Xip() { Codi = "724098100000005" } },
            new Animal() { Nom = "Lluna", Especie = Especie.GAT, EsFemella = true, Pes = 4.2,
                DataNaixement = new DateTime(2021, 12, 1), DataEntrada = new DateTime(2024, 12, 30),
                Xip = new Xip() { Codi = "724098100000006" } },
            new Animal() { Nom = "Pelut", Especie = Especie.GAT, EsFemella = false, Pes = 5.6,
                DataNaixement = new DateTime(2018, 8, 17), DataEntrada = new DateTime(2025, 7, 7),
                Xip = new Xip() { Codi = "724098100000007" } },
            new Animal() { Nom = "Floc", Especie = Especie.CONILL, EsFemella = false, Pes = 1.8,
                DataNaixement = new DateTime(2024, 2, 3), DataEntrada = new DateTime(2025, 5, 12),
                Xip = new Xip() { Codi = "724098100000008" } },
            new Animal() { Nom = "Piu", Especie = Especie.AU, EsFemella = true, Pes = 0.3,
                DataNaixement = new DateTime(2023, 9, 25), DataEntrada = new DateTime(2025, 8, 19),
                Xip = new Xip() { Codi = "724098100000009" } },
        };
        ctx.Animals.AddRange(animals);
        ctx.SaveChanges();  

        AnimalService animalService = new AnimalService(ctx);

        Console.WriteLine("--- Tots els animals ---");
        foreach (Animal a in animalService.GetTots())
        {
            Console.WriteLine(a);
        }

        // Ej1.2
        Console.WriteLine("\n--- Gossos disponibles (el Bruc no hi surt) ---");
        foreach (Animal a in animalService.GetDisponibles(Especie.GOS))
        {
            Console.WriteLine(a);
        }

        Console.WriteLine("\n--- ALTRES disponibles ---");
        List<Animal> altres = animalService.GetDisponibles(Especie.ALTRES);
        if (altres.Count == 0)
        {
            Console.WriteLine("No hi ha cap animal disponible d'aquesta espècie");
        }
        else
        {
            foreach (Animal a in altres)
            {
                Console.WriteLine(a);
            }
        }

        Console.WriteLine("\n--- Cerca per xip ---");
        Animal? trobat = animalService.CercaPerXip("724098100000003");
        if (trobat != null)
        {
            Console.WriteLine(trobat);
        }
        animalService.CercaPerXip("724098100000999");   

        // Ej1.3
        Console.WriteLine("\n--- Cerca per xip, codis mal formats ---");
        animalService.CercaPerXip("12345");             
        animalService.CercaPerXip("72409810000000A");   
        
        Console.WriteLine("\n--- Cerca per nom ---");
        List<string> textos = new List<string>() { "lu", "LU", "Lu" };
        foreach (string text in textos)
        {
            Console.WriteLine($"\"{text}\":");
            foreach (Animal a in animalService.CercaPerNom(text))
            {
                Console.WriteLine($"   {a}");
            }
        }
        //----------------------------------------------------------------------------------------------
        //Ej2.1
        List<Adoptant> adoptants = new List<Adoptant>()
        {
            new Adoptant() { DNI = "12345678Z", Nom = "Marta", Cognoms = "Puig Soler", TeJardi = true },
            new Adoptant() { DNI = "87654321X", Nom = "Pau", Cognoms = "Ferrer Vidal", TeJardi = false }
        };
        ctx.Adoptants.AddRange(adoptants);
        ctx.SaveChanges();
        AdoptantService adoptantService = new AdoptantService(ctx);
        
        Console.WriteLine("\n--- Tots els adoptants ---");
        foreach (Adoptant a in adoptantService.GetTots())
        {
            Console.WriteLine(a);
        }
        Console.WriteLine("\n--- Adoptar animals ---");
        adoptantService.Adoptar("12345678Z", "724098100000003");
        adoptantService.Adoptar("6767", "724098100000003");
        adoptantService.Adoptar("12345678Z", "6767");
        adoptantService.Adoptar("87654321X", "724098100000003");
        adoptantService.Adoptar("87654321X", "724098100000002");
        //Ej2.2
        Console.WriteLine("\n--- Animals retornats ---");
        adoptantService.Retornar("724098100000003");   
        adoptantService.Retornar("724098100000003");   
        adoptantService.Retornar("6767");             
        adoptantService.Retornar("724098100000005");  
        Console.WriteLine("\n--- Gossos disponibles ---");
        foreach (Animal a in animalService.GetDisponibles(Especie.GOS))
        {
            Console.WriteLine(a);
        }
        //Ej2.3
        Console.WriteLine("\n--- Llistat d'adoptants amb un minim de 0 animals---");
        adoptantService.LlistatAdoptants(0);
        
        //----------------------------------------------------------------------------------------------
        //Ej3.1
        List<Voluntari> voluntaris = new List<Voluntari>()
        {
            new Voluntari(){ Nom="Aina",  Torn = Torn.MATI,
                Animals = new List<Animal>{ animals[0], animals[1], animals[4], animals[5] } }, 
            new Voluntari(){ Nom="Joan",  Torn = Torn.TARDA,
                Animals = new List<Animal>{ animals[4], animals[7] } },                         
            new Voluntari(){ Nom="Nuria", Torn = Torn.CAP_DE_SETMANA,
                Animals = new List<Animal>{ animals[8] } }                                 
        };
        ctx.Voluntaris.AddRange(voluntaris);
        ctx.SaveChanges();
        VoluntariService voluntariService = new VoluntariService(ctx);
        Console.WriteLine("\n--- Tots els voluntaris ---");
        foreach (Voluntari v in voluntariService.GetTots())
        {
            Console.WriteLine(v);
        }
        Console.WriteLine("\n--- Assignar animals ---");
        voluntariService.AssignarAnimal("Joan", "724098100000007"); 
        voluntariService.AssignarAnimal("Marc", "724098100000007"); 
        voluntariService.AssignarAnimal("Joan", "724098100000999"); 
        voluntariService.AssignarAnimal("Joan", "724098100000004"); 
        voluntariService.AssignarAnimal("Joan", "724098100000005");  
        voluntariService.AssignarAnimal("Aina", "724098100000008");
        //EJ3.2
        Console.WriteLine("\n--- Desassignar animals ---");
        voluntariService.DesassignarAnimal("Joan", "724098100000007");  
        voluntariService.DesassignarAnimal("Marc", "724098100000007"); 
        voluntariService.DesassignarAnimal("Joan", "724098100000999");  
        voluntariService.DesassignarAnimal("Nuria", "724098100000005"); 
        //EJ3.3
        Console.WriteLine("\n--- Registrar passeigs ---");
        voluntariService.RegistrarPasseig("Aina", "724098100000001", 67);  // Luna: 0 -> 30
        voluntariService.RegistrarPasseig("Aina", "724098100000001", 420);  // Luna: 30 -> 75 (s'acumula)
        voluntariService.RegistrarPasseig("Aina", "724098100000006", 6767);
    }
}