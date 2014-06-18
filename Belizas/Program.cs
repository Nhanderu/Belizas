using System;
using System.Collections.Generic;

namespace Nhanderu.Belizas
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.Clear();
            Console.Title = "Belizas";

            String formula = "";
            do
            {
                Console.WriteLine("Digite uma WFF para calcular a tabela ou \"?\" para ir às configurações:");
                formula = Console.ReadLine().Replace(" ", "").ToLower();
                Console.Clear();
            }
            while (String.IsNullOrWhiteSpace(formula));

            TruthTable table = new TruthTable(formula);

            if (formula.StartsWith("?"))
                Console.WriteLine("Menu de opções.");
            else if (!table.ValidateFormula())
                Console.WriteLine("Fórmula inválida.\n");
            else
            {
                Console.Title = "Carregando";
                Console.WriteLine("Fórmula digitada:");
                Console.WriteLine(formula + "\n");

                try
                {
                    table.CalculateArguments();
                    table.CalculateExpressions();

                    Console.WriteLine("Tabela gerada:");
                    Console.WriteLine(table.ToString());
                }
                catch (OutOfMemoryException)
                {
                    Console.Clear();
                    Console.WriteLine("ERRO");
                    Console.WriteLine("A tabela resultante ultrapassa o limite de memória reservada para este programa.");
                    Console.WriteLine("Os argumentos vão gerar uma matriz de {0}, que é a quantidade de argumentos, por {1}, que é 2 elevado à quantidade de argumentos.", table.Arguments.Count, Math.Pow(2, table.Arguments.Count));
                    Console.WriteLine("Logo, o total de valores na tabela vai ser de {0} x {1} = {2} bytes.", table.Arguments.Count, Math.Pow(2, table.Arguments.Count), (table.Arguments.Count * Math.Pow(2, table.Arguments.Count)));

                    if ((table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1024) >= 1)
                    {
                        Console.WriteLine("{0} bytes = {1} kilobytes.", (table.Arguments.Count * Math.Pow(2, table.Arguments.Count)), (table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1024));
                        if ((table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1048576) >= 1)
                        {
                            Console.WriteLine("{0} kilobytes = {1} megabytes.", (table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1024), (table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1048576));
                            if ((table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1073741824) >= 1)
                                Console.WriteLine("{0} megabytes = {1} gigabytes.", (table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1048576), (table.Arguments.Count * Math.Pow(2, table.Arguments.Count) / 1073741824));
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("Escreva fórmulas menores da próxima vez.");
                    Console.WriteLine("");
                }
            }

            Console.WriteLine("Precione \"escape\" para fechar o programa.");
            Console.Title = "Belizas";
            ConsoleKeyInfo key = Console.ReadKey(false);
            if (key.Key != ConsoleKey.Escape)
                Main(new String[0]);
        }
    }
}
