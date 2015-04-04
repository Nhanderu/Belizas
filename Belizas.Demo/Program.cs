using Nhanderu.Belizas;
using System;
using System.Collections.Generic;

namespace Belizas.Demo
{
    class Program
    {
        static void Main(String[] args)
        {
            String formula;
            TruthTable table = new TruthTable("");
            do
            {
                formula = Console.ReadLine();
            } while (!table.ValidateFormula(formula));

            table = new TruthTable(formula, true);

            Console.WriteLine("\n//Tabela verdade");
            Console.Write(table.ToString());
            Console.ReadKey();

            Console.WriteLine("\n//HTML da tabela verdade - Dicionário");
            Console.WriteLine(table.ToHtmlTable(new Dictionary<String, Object>() { { "foo", "bar" }, { "nhan", "deru" } }, trAttributes: new Dictionary<String, Object>() { { "foo", "bar" }, { "nhan", "deru" } }));
            Console.ReadKey();

            Console.WriteLine("\n//HTML da tabela verdade - Objeto");
            Console.WriteLine(table.ToHtmlTable(new { foo = "bar", nhan = "deru", ra_fa = "el" }));
            Console.ReadKey();

            Console.WriteLine("\n//HTML da tabela verdade");
            Console.WriteLine(table.ToHtmlTable());
            Console.ReadKey();
        }
    }
}
