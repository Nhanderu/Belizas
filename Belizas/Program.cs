using System;
using System.Collections.Generic;
using System.Threading;

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

            if (formula.StartsWith("?"))
                Console.WriteLine("Menu de opções.");
            else if (!TruthTable.IsValid(formula))
                Console.WriteLine("Fórmula inválida.");
            else
            {
                Console.Title = "Carregando";
                Console.WriteLine("Fórmula digitada:");
                Console.WriteLine(formula + "\n");

                #region Calculate the arguments
                List<Char> arguments = new List<Char>();
                foreach (Char item in formula.ToCharArray())
                    if (Char.IsLetter(item) && !arguments.Contains(item))
                        arguments.Add(item);

                Boolean[,] values = new Boolean[(Int32)Math.Pow(2, arguments.Count), arguments.Count];
                for (Int32 line = 0; line < Math.Pow(2, arguments.Count); line++)
                {
                    Int32 calculableLine = line;
                    for (Int32 column = 0; column < arguments.Count; column++)
                    {
                        if (calculableLine >= Math.Pow(2, (arguments.Count - 1) - column))
                        {
                            values[line, column] = true;
                            calculableLine -= (Int32)Math.Pow(2, (arguments.Count - 1) - column);
                        }
                        else
                            values[line, column] = false;
                    }
                }
                #endregion

                #region Calculate the expressions
                String expression = "";
                TruthTable.ExpressionsValues = new List<Boolean[]>();
                while (formula != "?")
                {
                    if (formula.IndexOf(TruthTable.Operators.And) > 0)
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

                    formula = ReplaceFirst(formula, expression, "?");
                    TruthTable.ExpressionsValues.Add(TruthTable.CalculateExpression(arguments, values, expression));
                }
                #endregion

                #region Write the table
                Console.WriteLine("Tabela gerada:");

                for (Int32 index = 0; index < arguments.Count + TruthTable.ExpressionsValues.Count; index++)
                    if (index < arguments.Count)
                        Console.Write(arguments[index] + " ");
                    else
                        Console.Write("x ");
                Console.WriteLine("");

                for (Int32 line = 0; line < Math.Pow(2, arguments.Count); line++)
                {
                    for (Int32 column = 0; column < arguments.Count + TruthTable.ExpressionsValues.Count; column++)
                    {
                        if (column < arguments.Count)
                            Console.Write(Convert.ToInt32(values[line, column]).ToString() + " ");
                        else
                            Console.Write(Convert.ToInt32(TruthTable.ExpressionsValues[column - arguments.Count][line]).ToString() + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
                #endregion
            }

            Console.WriteLine("Precione \"escape\" para fechar o programa.");
            Console.Title = "Belizas";
            ConsoleKeyInfo key = Console.ReadKey(false);
            if (key.Key != ConsoleKey.Escape)
                Main(new String[0]);
            else
                Thread.Sleep(1000);
        }
    }
}
