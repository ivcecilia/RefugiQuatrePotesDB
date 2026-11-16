using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;
using CeciliaIvan_RefugiAnimals.Services;
using Microsoft.EntityFrameworkCore;

using var db = new RefugiContext();
db.Database.EnsureDeleted();    // primer esborrar...
db.Database.EnsureCreated();    // ...i després crear, amb les taules noves de cada sessió

// ============ Nivell 1 · Seed ============
// Cada animal porta el seu xip: no cal fer db.Xip.Add, EF segueix la navegació.
// No posem DataImplantacio: surt la d'avui, del constructor.
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
    // Ja adoptat: per veure que GetDisponibles el deixa fora
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
db.Animals.AddRange(animals);
db.SaveChanges();               // fins aquí no s'ha fet cap INSERT

AnimalService animalService = new AnimalService(db);

Console.WriteLine("--- Tots els animals ---");
foreach (Animal a in animalService.GetTots())
{
    Console.WriteLine(a);
}

// ============ Nivell 2 ============
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

Console.WriteLine("\n--- Cerca per xip ---");
Animal? trobat = animalService.CercaPerXip("724098100000003");
if (trobat != null)
{
    Console.WriteLine(trobat);
}
animalService.CercaPerXip("724098100000999");   // format bo, però no existeix

// ============ Nivell 3 ============
Console.WriteLine("\n--- Cerca per xip, codis mal formats ---");
animalService.CercaPerXip("12345");             // massa curt
animalService.CercaPerXip("72409810000000A");   // 15 caràcters, però amb una lletra

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

// ============ Extra · CHECK a la BD ============
Console.WriteLine("\n--- Restriccions CHECK ---");
List<Animal> dolents = new List<Animal>()
{
    new Animal() { Nom = "Gegant", Especie = Especie.GOS, EsFemella = false, Pes = 150,     // pes > 100
        DataNaixement = new DateTime(2020, 1, 1), Xip = new Xip() { Codi = "724098100000010" } },
    new Animal() { Nom = "Curt", Especie = Especie.GAT, EsFemella = true, Pes = 4,          // codi de 5
        DataNaixement = new DateTime(2020, 1, 1), Xip = new Xip() { Codi = "12345" } },
    new Animal() { Nom = "Lletra", Especie = Especie.GAT, EsFemella = true, Pes = 4,        // 15, però amb una lletra
        DataNaixement = new DateTime(2020, 1, 1), Xip = new Xip() { Codi = "72409810000001A" } },
};
foreach (Animal dolent in dolents)
{
    try
    {
        db.Animals.Add(dolent);
        db.SaveChanges();
        Console.WriteLine($"{dolent.Nom}: s'ha desat (no hauria!)");
    }
    catch (DbUpdateException e)
    {
        // El missatge de SQLite és a la InnerException: "CHECK constraint failed: CK_..."
        Console.WriteLine($"{dolent.Nom}: no s'ha desat · {e.InnerException?.Message}");
        db.ChangeTracker.Clear();   // si no, l'animal dolent continua pendent i el següent SaveChanges tornaria a petar
    }
}


// ================================================================
// ============ SESSIÓ 2 · ADOPTANTS ==============================
// ================================================================

// ============ Nivell 1 · Dades ============
// El Bruc ja estava adoptat a P1 (EstaAdoptat = true i DataAdopcio): ara li posem adoptant, la Marta.
// Un animal adoptat ha de tenir adoptant: si no, la Marta no tindria cap gos.
// El llegim de la BD: després del ChangeTracker.Clear() de l'extra, la variable de la llista animals ja no la segueix EF
Animal bruc = db.Animals.First(a => a.Xip.Codi == "724098100000004");

Adoptant marta = new Adoptant() { DNI = "12345678Z", Nom = "Marta", Cognoms = "Puig Soler", TeJardi = true };
Adoptant pau = new Adoptant() { DNI = "87654321X", Nom = "Pau", Cognoms = "Ferrer Vidal", TeJardi = false };
bruc.Adoptant = marta;

// Crear l'objecte no n'hi ha prou: cal afegir-lo al context i desar-lo.
// I ABANS de cridar Adoptar, que busca l'adoptant a la BD.
db.Adoptants.AddRange(marta, pau);
db.SaveChanges();

AdoptantService adoptantService = new AdoptantService(db);

// Una funció local per poder mostrar-ho abans i després de cada canvi
void MostrarAdoptants()
{
    // Ho llegim de la BD amb el servei, no de les variables marta i pau
    foreach (Adoptant ad in adoptantService.GetTots())
    {
        Console.WriteLine(ad);
        if (ad.Animals.Count == 0)
        {
            Console.WriteLine("   (cap animal adoptat)");
        }
        foreach (Animal an in ad.Animals)      // cada adoptant AMB els seus animals
        {
            Console.WriteLine($"   {an}");
        }
    }
}

Console.WriteLine("\n--- Adoptants amb els seus animals ---");
MostrarAdoptants();

// ============ Nivell 1 · Adoptar ============
// Una adopció correcta i un cas per cada return false.
// Cada prova ha de fallar NOMÉS per la condició que vols provar.
Console.WriteLine("\n--- Adoptar ---");
adoptantService.Adoptar("12345678Z", "724098100000002");    // bé: la Marta té jardí i adopta el Thor (gos)
adoptantService.Adoptar("87654321X", "724098100000008");    // bé: el Pau no té jardí, però el Floc és un conill
adoptantService.Adoptar("00000000T", "724098100000005");    // false: no hi ha cap adoptant amb aquest DNI
adoptantService.Adoptar("12345678Z", "724098100000999");    // false: no hi ha cap animal amb aquest xip
adoptantService.Adoptar("12345678Z", "724098100000004");    // false: el Bruc ja està adoptat
adoptantService.Adoptar("87654321X", "724098100000003");    // false: la Kira és un gos i el Pau no té jardí
// Per provar el jardí cal un gos NO adoptat: amb el Bruc fallaria abans per "ja adoptat" i mai arribaria al jardí

// ============ Nivell 2 · Màxim 3 animals ============
// La Marta té el Bruc i el Thor: el 3r entra i el 4t no
Console.WriteLine("\n--- Màxim 3 animals per adoptant ---");
adoptantService.Adoptar("12345678Z", "724098100000005");    // bé: la Mixa, 3r animal de la Marta
adoptantService.Adoptar("12345678Z", "724098100000006");    // false: la Lluna seria el 4t

Console.WriteLine("\n--- Adoptants després de les adopcions ---");
MostrarAdoptants();

// ============ Nivell 2 · Retornar ============
Console.WriteLine("\n--- Retornar ---");
adoptantService.Retornar("724098100000002");    // bé: la Marta retorna el Thor
adoptantService.Retornar("724098100000003");    // false: la Kira no està adoptada
adoptantService.Retornar("724098100000999");    // false: no hi ha cap animal amb aquest xip

Console.WriteLine("\n--- Adoptants després de retornar (la Marta ja no té el Thor) ---");
MostrarAdoptants();

// ============ Nivell 3 · Llistat ============
// Ara la Marta en té 2 (Bruc i Mixa) i el Pau 1 (Floc)
Console.WriteLine("\n--- Adoptants amb 1 animal o més ---");
adoptantService.LlistatAdoptants(1);            // Marta (2) i després Pau (1)
Console.WriteLine("\n--- Adoptants amb 3 animals o més ---");
adoptantService.LlistatAdoptants(3);            // cap


// ================================================================
// ============ SESSIÓ 3 · VOLUNTARIS =============================
// ================================================================

// ============ Nivell 1 · Dades ============
// Després de P2: Luna, Thor (retornat) i Kira són gossos no adoptats. La Lluna i el Pelut són gats no adoptats.
// Com amb el Bruc, els animals es LLEGEIXEN de la BD: les variables de la llista animals ja no les segueix EF
Animal luna = db.Animals.First(a => a.Xip.Codi == "724098100000001");
Animal kira = db.Animals.First(a => a.Xip.Codi == "724098100000003");
Animal lluna = db.Animals.First(a => a.Xip.Codi == "724098100000006");

// Variables normals del Program: res de public (això és per als atributs d'una classe, aquí no compila)
Voluntari aina = new Voluntari() { Nom = "Aina", Torn = Torn.MATI };
Voluntari joan = new Voluntari() { Nom = "Joan", Torn = Torn.TARDA };
Voluntari nuria = new Voluntari() { Nom = "Núria", Torn = Torn.CAP_DE_SETMANA };
aina.Animals.Add(luna);         // a les dades, la relació es posa afegint a la col·lecció d'un costat
aina.Animals.Add(kira);
joan.Animals.Add(lluna);

// Crear-los no n'hi ha prou: cal afegir-los al context i desar. I ABANS de cridar el servei, que els busca a la BD
db.Voluntaris.AddRange(aina, joan, nuria);
db.SaveChanges();

VoluntariService voluntariService = new VoluntariService(db);

void MostrarVoluntaris()
{
    // Ho llegim de la BD amb el servei, no de les variables aina, joan i nuria
    foreach (Voluntari v in voluntariService.GetTots())
    {
        Console.WriteLine(v);
        if (v.Animals.Count == 0)
        {
            Console.WriteLine("   (no té cura de cap animal)");
        }
        foreach (Animal an in v.Animals)      // cada voluntari AMB els seus animals
        {
            Console.WriteLine($"   {an}");
        }
    }
}

Console.WriteLine("\n--- Voluntaris amb els seus animals ---");
MostrarVoluntaris();

// ============ Nivell 1 · AssignarAnimal ============
// Una assignació correcta i un cas per cada return false.
// Cada prova ha de fallar NOMÉS per la condició que vols provar: per al màxim de 4 cal un animal
// que no estigui adoptat ni assignat ja a aquell voluntari, si no fallaria abans
Console.WriteLine("\n--- Assignar animals ---");
voluntariService.AssignarAnimal("Núria", "724098100000002");    // bé: la Núria té cura del Thor
voluntariService.AssignarAnimal("Pere", "724098100000002");     // false: no hi ha cap voluntari amb aquest nom
voluntariService.AssignarAnimal("Aina", "724098100000999");     // false: no hi ha cap animal amb aquest xip
voluntariService.AssignarAnimal("Aina", "724098100000004");     // false: el Bruc està adoptat
voluntariService.AssignarAnimal("Aina", "724098100000001");     // false: l'Aina ja té cura de la Luna
voluntariService.AssignarAnimal("Aina", "724098100000007");     // bé: el Pelut, 3r animal de l'Aina
voluntariService.AssignarAnimal("Aina", "724098100000009");     // bé: el Piu, 4t animal de l'Aina
voluntariService.AssignarAnimal("Aina", "724098100000006");     // false: l'Aina ja en té 4 (la Lluna és del Joan, però pot tenir més d'un voluntari)

Console.WriteLine("\n--- Voluntaris després d'assignar ---");
MostrarVoluntaris();

// ============ Nivell 2 · DesassignarAnimal ============
Console.WriteLine("\n--- Desassignar animals ---");
voluntariService.DesassignarAnimal("Aina", "724098100000009");  // bé: l'Aina ja no té cura del Piu (en tenia 4 i en pot treure)
voluntariService.DesassignarAnimal("Pere", "724098100000009");  // false: no hi ha cap voluntari amb aquest nom
voluntariService.DesassignarAnimal("Aina", "724098100000999");  // false: no hi ha cap animal amb aquest xip
voluntariService.DesassignarAnimal("Núria", "724098100000001"); // false: la Núria no té cura de la Luna

// Desassignar no esborra l'animal: el Piu continua a la BD
Animal? piu = animalService.CercaPerXip("724098100000009");
if (piu != null)
{
    Console.WriteLine($"Continua al refugi: {piu}");
}

// ============ Nivell 2 · Adoptar treu els voluntaris ============
// La Marta té el Bruc i la Mixa i té jardí: pot adoptar la Kira, que és de l'Aina
Console.WriteLine("\n--- Adoptar un animal que tenia voluntaris ---");
adoptantService.Adoptar("12345678Z", "724098100000003");        // bé: la Marta adopta la Kira
Console.WriteLine($"Voluntaris de la Kira: {kira.Voluntaris.Count}");

Console.WriteLine("\n--- Voluntaris després de l'adopció (l'Aina ja no té la Kira) ---");
MostrarVoluntaris();

// ============ Nivell 3 · RegistrarPasseig ============
// Ara l'Aina té cura de la Luna (gos) i del Pelut (gat)
Console.WriteLine("\n--- Passejos ---");
voluntariService.RegistrarPasseig("Aina", "724098100000001", 30);   // bé: abans 0, ara 30
voluntariService.RegistrarPasseig("Aina", "724098100000001", 45);   // bé: abans 30, ara 75 (s'acumulen)
voluntariService.RegistrarPasseig("Joan", "724098100000006", 20);   // false: la Lluna és un gat
voluntariService.RegistrarPasseig("Núria", "724098100000001", 20);  // false: la Núria no té cura de la Luna

Console.WriteLine("\n--- Voluntaris al final ---");
MostrarVoluntaris();


// ================================================================
// ============ SESSIÓ 4 · FAMÍLIES ===============================
// ================================================================

// ============ Nivell 1 · Dades ============
// La família es munta en memòria amb les col·leccions de cries i s'afegeix NOMÉS la Nala:
// EF segueix les navegacions i insereix els 4 animals, els 4 xips i les MareID.
// Pes obligatori: amb el CHECK de la BD, un pes de 0 fa petar el SaveChanges
Animal nala = new Animal() { Nom = "Nala", Especie = Especie.GOS, EsFemella = true, Pes = 25,
    DataNaixement = DateTime.Today.AddYears(-9), Xip = new Xip() { Codi = "724098100000011" } };
Animal coco = new Animal() { Nom = "Coco", Especie = Especie.GOS, EsFemella = true, Pes = 21,
    DataNaixement = DateTime.Today.AddYears(-5), Xip = new Xip() { Codi = "724098100000012" } };
Animal rocky = new Animal() { Nom = "Rocky", Especie = Especie.GOS, EsFemella = false, Pes = 6.5,
    DataNaixement = DateTime.Today.AddMonths(-4), Xip = new Xip() { Codi = "724098100000013" } };
Animal bimba = new Animal() { Nom = "Bimba", Especie = Especie.GOS, EsFemella = true, Pes = 5.8,
    DataNaixement = DateTime.Today.AddMonths(-4), Xip = new Xip() { Codi = "724098100000014" } };

coco.Cries.Add(rocky);
coco.Cries.Add(bimba);
nala.Cries.Add(coco);

db.Animals.Add(nala);
db.SaveChanges();

void MostrarFamilies()
{
    // Ho llegim de la BD amb el servei. a.Mare i a.Cries els carrega el lazy loading
    foreach (Animal an in animalService.GetTots())
    {
        string mare;
        if (an.Mare != null)
        {
            mare = an.Mare.Nom;     // an.Mare.Nom sense comprovar el null peta amb els que no tenen mare
        }
        else
        {
            mare = "sense mare";
        }
        Console.WriteLine($"{an.Nom} ({an.Especie}) · mare: {mare} · {an.Cries.Count} cries");
    }
}

Console.WriteLine("\n--- Animals amb la seva mare i les seves cries ---");
MostrarFamilies();      // Nala: sense mare, 1 cria · Coco: mare Nala, 2 cries · Rocky i Bimba: mare Coco, 0 cries

// ============ Nivell 1 · CanviarMare ============
// La Kira (gossa, nascuda el 2023) no té mare. La Luna (gossa, 2020) i el Thor (gos, mascle)
Console.WriteLine("\n--- Canviar la mare ---");
animalService.CanviarMare("724098100000003", "724098100000001");    // bé: la Kira ara és cria de la Luna
animalService.CanviarMare("724098100000003", "724098100000001");    // false: ja té la Luna com a mare
animalService.CanviarMare("724098100000003", null);                 // bé: la Kira es queda sense mare
animalService.CanviarMare("724098100000003", "724098100000002");    // false: el Thor és mascle
animalService.CanviarMare("724098100000999", "724098100000001");    // false: no hi ha cap cria amb aquest xip
animalService.CanviarMare("724098100000003", "724098100000999");    // false: no hi ha cap mare amb aquest xip
animalService.CanviarMare("724098100000003", null);                 // false: la Kira ja no té mare

// ============ Nivell 2 · RegistrarCria ============
// Cada prova ha de fallar NOMÉS per la condició que vols provar: per això cada cria dolenta
// és correcta en tot menys en una cosa
Console.WriteLine("\n--- Registrar cries ---");
Animal toby = new Animal() { Nom = "Toby", Especie = Especie.GOS, EsFemella = false, Pes = 2.1,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000015" } };
animalService.RegistrarCria("724098100000012", toby);               // bé: el Toby és cria de la Coco

Animal criaThor = new Animal() { Nom = "Max", Especie = Especie.GOS, EsFemella = false, Pes = 2,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000016" } };
animalService.RegistrarCria("724098100000002", criaThor);           // false: el Thor és mascle

Animal criaGat = new Animal() { Nom = "Misha", Especie = Especie.GAT, EsFemella = true, Pes = 0.8,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000017" } };
animalService.RegistrarCria("724098100000012", criaGat);            // false: la Coco és un gos i la Misha un gat

Animal criaXip = new Animal() { Nom = "Duc", Especie = Especie.GOS, EsFemella = false, Pes = 2,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000011" } };
animalService.RegistrarCria("724098100000012", criaXip);            // false: el xip ja és de la Nala

// Altres return false (l'enunciat no els demana al Program)
Animal criaVella = new Animal() { Nom = "Avi", Especie = Especie.GOS, EsFemella = false, Pes = 20,
    DataNaixement = DateTime.Today.AddYears(-10), Xip = new Xip() { Codi = "724098100000018" } };
animalService.RegistrarCria("724098100000012", criaVella);          // false: ha nascut abans que la Coco
animalService.RegistrarCria("724098100000999", criaVella);          // false: no hi ha cap mare amb aquest xip

Console.WriteLine("\n--- Famílies després de registrar (la Coco té 3 cries, cap animal nou dels dolents) ---");
MostrarFamilies();

// ============ Nivell 3 · Descendents ============
// Amb les dades inicials, Descendents de la Nala és [Coco, Rocky, Bimba]. Ara també hi surt el Toby
void MostrarDescendents(string codiXip)
{
    List<Animal> descendents = animalService.Descendents(codiXip);
    List<string> noms = new List<string>();
    foreach (Animal an in descendents)
    {
        noms.Add(an.Nom);
    }
    Console.WriteLine($"Descendents de {codiXip}: [{string.Join(", ", noms)}]");
}

Console.WriteLine("\n--- Descendents ---");
MostrarDescendents("724098100000011");     // Nala: [Coco, Rocky, Bimba, Toby]
MostrarDescendents("724098100000012");     // Coco: [Rocky, Bimba, Toby]
MostrarDescendents("724098100000013");     // Rocky: [] (no té cries)
MostrarDescendents("724098100000999");     // no existeix: missatge i []



