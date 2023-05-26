// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Hello, World!");

string conString = "Server=(localdb)\\mssqllocaldb;Database=ByteBay_test;Trusted_Connection=true";

var con = new SqlConnection(conString);
con.Open();
var cmd = con.CreateCommand();
cmd.CommandText = "SELECT * FROM BestellPositionen";
var dict = new Dictionary<int, decimal>();

using (var reader = cmd.ExecuteReader())
{
    while (reader.Read())
    {
        var id = reader.GetInt32(reader.GetOrdinal("Id"));
        var price = reader.GetDecimal(reader.GetOrdinal("Preis"));

        dict.Add(id, price);
    }
}

var trans = con.BeginTransaction();

try
{
    foreach (var item in dict)
    {
        var newPrice = item.Value + 1;
        Console.WriteLine($"[{item.Key}] {item.Value:c} -> {newPrice:c}");

        if (item.Key == 2)
            throw new OutOfMemoryException();

        var updCmd = con.CreateCommand();
        updCmd.Transaction = trans;
        updCmd.CommandText = "UPDATE BestellPositionen SET Preis = @newPrice WHERE Id =@id";
        updCmd.Parameters.AddWithValue("@id", item.Key);
        updCmd.Parameters.AddWithValue("@newPrice", newPrice);
        if (updCmd.ExecuteNonQuery() == 1)
            Console.WriteLine("\tOK");
        else
            Console.WriteLine("????");

        Console.WriteLine("Next?");
        Console.ReadKey();
    }
    trans.Commit();
}
catch (Exception e)
{
    Console.WriteLine($"FEHELER: {e.Message}");
    trans.Rollback();
}


