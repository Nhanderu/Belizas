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

            if (formula.StartsWith("?"))
                Console.WriteLine("Menu de opções.");
            else if (!TruthTable.IsValid(formula))
                Console.WriteLine("Fórmula inválida.");
            else
            {
                Console.Title = "Carregando";
                Console.WriteLine("Fórmula digitada:");
                Console.WriteLine(formula + "\n");
                TruthTable.Formula = formula;

                List<Char> arguments = new List<Char>();
                foreach (Char item in formula.ToCharArray())
                    if (Char.IsLetter(item) && !arguments.Contains(item))
                        arguments.Add(item);

                Exception error = null;
                try
                {
                    Boolean[,] valuesTest = new Boolean[(Int32)Math.Pow(2, arguments.Count), arguments.Count];
                }
                catch (OutOfMemoryException exception)
                {
                    Console.Clear();
                    Console.WriteLine("ERRO");
                    Console.WriteLine("Sua memória é um lixo e não aguenta a quantidade de valores que a tabela resultante possui.");
                    Console.WriteLine("");
                    Console.WriteLine("Os argumentos vão gerar uma matriz de {0}, que é a quantidade de argumentos, por {1}, que é 2 elevado à quantidade de argumentos.", arguments.Count, Math.Pow(2, arguments.Count));
                    Console.WriteLine("Logo, o total de valores na tabela vai ser de {0} x {1} = {2} bytes.", arguments.Count, Math.Pow(2, arguments.Count), (arguments.Count * Math.Pow(2, arguments.Count)));

                    if ((arguments.Count * Math.Pow(2, arguments.Count) / 1024) >= 1)
                    {
                        Console.WriteLine("{0} bytes = {1} kilobytes.", (arguments.Count * Math.Pow(2, arguments.Count)), (arguments.Count * Math.Pow(2, arguments.Count) / 1024));
                        if ((arguments.Count * Math.Pow(2, arguments.Count) / 1048576) >= 1)
                        {
                            Console.WriteLine("{0} kilobytes = {1} megabytes.", (arguments.Count * Math.Pow(2, arguments.Count) / 1024), (arguments.Count * Math.Pow(2, arguments.Count) / 1048576));
                            if ((arguments.Count * Math.Pow(2, arguments.Count) / 1073741824) >= 1)
                                Console.WriteLine("{0} megabytes = {1} gigabytes.", (arguments.Count * Math.Pow(2, arguments.Count) / 1048576), (arguments.Count * Math.Pow(2, arguments.Count) / 1073741824));
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("Escreva fórmulas menores da próxima vez.");
                    Console.WriteLine("");

                    error = exception;
                }

                if (error == null)
                {
                    #region Calculate the arguments
                    Boolean[,] values = new Boolean[(Int32)Math.Pow(2, arguments.Count), arguments.Count];
                    for (Int32 line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        Int32 calculableLine = line;
                        for (Int32 column = 0; column < arguments.Count; column++)
                        {
                            values[line, column] = calculableLine >= Math.Pow(2, (arguments.Count - 1) - column);
                            if (values[line, column])
                                calculableLine -= (Int32)Math.Pow(2, (arguments.Count - 1) - column);
                        }
                    }
                    #endregion

                    #region Calculate the expressions
                    String expression = "";
                    TruthTable.ExpressionsValues = new List<Boolean[]>();
                    while (formula.Length != 1)
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

                        formula = ReplaceFirst(formula, expression, Convert.ToChar(TruthTable.ExpressionsValues.Count + TruthTable.Arroz).ToString());
                        TruthTable.ExpressionsValues.Add(TruthTable.CalculateExpression(arguments, values, expression));
                    }
                    #endregion

                    #region Write the table
                    Console.WriteLine("Tabela gerada:");

                    for (Int32 index = 0; index < arguments.Count + TruthTable.ExpressionsValues.Count; index++)
                        if (index < arguments.Count)
                            Console.Write(arguments[index] + " ");
                        else if (index == arguments.Count + TruthTable.ExpressionsValues.Count - 1)
                            Console.Write(TruthTable.Formula);
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
            }

            Console.WriteLine("Precione \"escape\" para fechar o programa.");
            Console.Title = "Belizas";
            ConsoleKeyInfo key = Console.ReadKey(false);
            if (key.Key != ConsoleKey.Escape)
                Main(new String[0]);
        }
    }
}
