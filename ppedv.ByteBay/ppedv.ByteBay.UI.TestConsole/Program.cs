// See https://aka.ms/new-console-template for more information
using Autofac;
using ppedv.ByteBay.Core;
using ppedv.ByteBay.Data.EfCore;
using ppedv.ByteBay.Model;
using ppedv.ByteBay.Model.Contracts;

string conString = "Server=(localdb)\\mssqllocaldb;Database=ByteBay_test;Trusted_Connection=true";

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Hello, World!");


var builder = new ContainerBuilder();
builder.RegisterType<EfRepository>()
       .AsImplementedInterfaces()
       .WithParameter("conString", conString);

var container = builder.Build();

var repo = container.Resolve<IRepository>();
var os = new OrderService(repo);

//var repo = new EfRepository(conString);
//var os = new OrderService(repo);

foreach (var best in repo.Query<Bestellung>().OrderBy(x=>x.BestellDatum).ToList())
{
    Console.WriteLine($"{best.BestellDatum:g} {best.Lieferadresse.Zeile1}");
    foreach (var pos in best.Positionen)
    {
        Console.WriteLine($"\t{pos.Menge}x {pos.Produkt.Name} {pos.Preis:c}");
    }
}
var beste = os.GetMostExpensiveOrderOfToday(repo.Query<Bestellung>().First().BestellDatum);

Console.WriteLine($"Beste: {beste.Id}");
