using CeciliaIvan_RefugiAnimals.Data;
using CeciliaIvan_RefugiAnimals.Models;

namespace CeciliaIvan_RefugiAnimals.Services;

public class AnimalService
{
    private RefugiContext ctx;

    public AnimalService(RefugiContext context)
    {
        ctx = context;
    }
    public List<Animal> CercaPerNom(string text)
    {
        
    }
    public Animal? CercaPerXip(string codiXip)
    {
        
    }
}