using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;
using CeciliaIvan_RefugiAnimals.Services;

namespace CeciliaIvan_RefugiAnimals;

internal class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new RefugiContext())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            var rex = new Animal()
            {
                Nom = "Rex",
                Especie = Especie.GOS,
                EsFemella = false,
                DataNaixement = new DateTime(2019, 3, 10),
                DataEntrada = new DateTime(2025, 1, 15),
                Pes = 28.5,
                Xip = new Xip()
                {
                    Codi = "941000000000001"
                }
            };

            var bruna = new Animal()
            {
                Nom = "Bruna",
                Especie = Especie.GOS,
                EsFemella = true,
                DataNaixement = new DateTime(2021, 7, 2),
                DataEntrada = new DateTime(2025, 6, 20),
                Pes = 18.2,
                Xip = new Xip()
                {
                    Codi = "941000000000002"
                }
            };

            var toby = new Animal()
            {
                Nom = "Toby",
                Especie = Especie.GOS,
                EsFemella = false,
                DataNaixement = new DateTime(2022, 11, 25),
                DataEntrada = new DateTime(2024, 9, 5),
                Pes = 9.8,
                Xip = new Xip()
                {
                    Codi = "941000000000003"
                }
            };

            var kira = new Animal()
            {
                Nom = "Kira",
                Especie = Especie.GOS,
                EsFemella = true,
                DataNaixement = new DateTime(2020, 5, 18),
                DataEntrada = new DateTime(2026, 2, 1),
                Pes = 22.0,
                Xip = new Xip()
                {
                    Codi = "941000000000004"
                }
            };

            var luna = new Animal()
            {
                Nom = "Luna",
                Especie = Especie.GAT,
                EsFemella = true,
                DataNaixement = new DateTime(2023, 4, 9),
                DataEntrada = new DateTime(2025, 3, 12),
                Pes = 3.9,
                Xip = new Xip()
                {
                    Codi = "941000000000005"
                }
            };

            var simba = new Animal()
            {
                Nom = "Simba",
                Especie = Especie.GAT,
                EsFemella = false,
                DataNaixement = new DateTime(2022, 8, 30),
                DataEntrada = new DateTime(2025, 10, 8),
                Pes = 5.1,
                Xip = new Xip()
                {
                    Codi = "941000000000006"
                }
            };

            var mixa = new Animal()
            {
                Nom = "Mixa",
                Especie = Especie.GAT,
                EsFemella = true,
                DataNaixement = new DateTime(2024, 1, 14),
                DataEntrada = new DateTime(2026, 5, 22),
                Pes = 3.2,
                Xip = new Xip()
                {
                    Codi = "941000000000007"
                }
            };

            var bunny = new Animal()
            {
                Nom = "Bunny",
                Especie = Especie.CONILL,
                EsFemella = true,
                DataNaixement = new DateTime(2024, 6, 3),
                DataEntrada = new DateTime(2025, 12, 2),
                Pes = 1.8,
                Xip = new Xip()
                {
                    Codi = "941000000000008"
                }
            };

            var piulet = new Animal()
            {
                Nom = "Piulet",
                Especie = Especie.AU,
                EsFemella = false,
                DataNaixement = new DateTime(2023, 2, 21),
                DataEntrada = new DateTime(2026, 3, 17),
                Pes = 0.4,
                Xip = new Xip()
                {
                    Codi = "941000000000009"
                }
            };

            ctx.Animals.AddRange(rex, bruna, toby, kira, luna, simba, mixa, bunny, piulet);
            ctx.SaveChanges();

            Console.WriteLine("=== Tots els animals ===");
            foreach (var a in ctx.Animals.ToList())
            {
                Console.WriteLine($"{a.AnimalID}. {a.Nom} ({a.Especie}) - Xip: {a.Xip.Codi}");
            }
        }
    }
}