using System;
using System.Collections.Generic;

namespace Nhanderu.Belizas
{
    class Program
    {
        static String ReplaceFirst(String text, String oldValue, String newValue)
        {
            return text.Substring(0, text.IndexOf(oldValue)) + newValue + text.Substring(text.IndexOf(oldValue) + oldValue.Length);
        }

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
                Console.WriteLine("Fórmula inválida.");
            else
            {
                Console.Title = "Carregando";
                Console.WriteLine("Fórmula digitada:");
                Console.WriteLine(formula + "\n");

                Exception error = null;
                try
                {
                    Boolean[,] test = new Boolean[(Int32)Math.Pow(2, table.Arguments.Count), table.Arguments.Count];
                    test = null;
                }
                catch (OutOfMemoryException exception)
                {
                    Console.Clear();
                    Console.WriteLine("ERRO");
                    Console.WriteLine("Sua memória é um lixo e não aguenta a quantidade de valores que a tabela resultante possui.");
                    Console.WriteLine("");
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

                    error = exception;
                }

                if (error == null)
                {
                    table.CalculateArguments();

                    String expression = "";
                    while (formula.Length != 1)
                    {
                        if (formula.IndexOf(TruthTable.Operators.Not) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.Not) - 1, 2);
                        else if (formula.IndexOf(TruthTable.Operators.And) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.And) - 1, 3);
                        else if (formula.IndexOf(TruthTable.Operators.Or) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.Or) - 1, 3);
                        else if (formula.IndexOf(TruthTable.Operators.Xor) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.Xor) - 1, 3);
                        else if (formula.IndexOf(TruthTable.Operators.IfThen) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.IfThen) - 1, 3);
                        else if (formula.IndexOf(TruthTable.Operators.ThenIf) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.ThenIf) - 1, 3);
                        else if (formula.IndexOf(TruthTable.Operators.IfAndOnlyIf) > 0)
                            expression = formula.Substring(formula.IndexOf(TruthTable.Operators.IfAndOnlyIf) - 1, 3);

                        formula = ReplaceFirst(formula, expression, Convert.ToChar(table.ExpressionsValues.Count + table.Churros).ToString());
                        table.CalculateExpression(expression);
                    }

                    Console.WriteLine("Tabela gerada:");
                    Console.WriteLine(table.ToString());
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
