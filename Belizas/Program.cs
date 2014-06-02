using System;
using System.Collections.Generic;
using System.Linq;

namespace Nhanderu.Belizas
{
    class Program
    {
        static void Main(String[] args)
        {
            //Configures the console window.
            Console.Title = "Belizas";

            //Writes the very first informations and gets the formula.
            String formula = "";
            do
            {
                Console.WriteLine("Digite uma WFF para calcular a tabela ou \"?\" para ir às configurações:");
                formula = Console.ReadLine().Replace(" ", "").ToLower();
                Console.Clear();
            }
            while (String.IsNullOrWhiteSpace(formula));

            //Verifies if the formula is valid.
            if (formula.StartsWith("?"))
                Console.WriteLine("Menu de opções.");
            else if (TruthTable.HasDisallowedCharacters(formula, TruthTable.Expressions.Enumerate()))
                Console.WriteLine("Fórmula inválida.");
            else
            {
                //Writes the first lines of the reply.
                Console.Title = "Carregando";
                Console.WriteLine("Fórmula digitada:");
                Console.WriteLine(formula + "\n");

                #region Arguments
                Console.WriteLine("Argumentos da tabela verdade:");

                //Gets the arguments in formula
                List<Char> arguments = new List<Char>();
                foreach (Char item in formula.ToCharArray())
                    if (Char.IsLetter(item) && !arguments.Contains(item))
                        arguments.Add(item);

                //Writes the first line of the truth table: the arguments.
                foreach (Char argument in arguments)
                    Console.Write(argument.ToString() + " ");
                Console.WriteLine("");

                //Writes the first columns: the arguments and its possible values.
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
                        Console.Write(Convert.ToInt32(values[line, column]).ToString() + " ");
                    }
                    Console.WriteLine("");
                }
                #endregion
                Console.WriteLine("");

                #region Negation
                Console.WriteLine("Argumentos negados da tabela verdade:");

                //Gets arguments in formula by the position of the negation symbol (').
                List<Char> negationArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count);

                //Verifies is there is at least one argument.
                Boolean[,] negationValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), negationArguments.Count];
                if (negationArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions (negated arguments).
                    List<Int32> indexesOfNegationArguments = new List<Int32>();
                    foreach (Char negation in negationArguments)
                    {
                        Console.Write(negation.ToString() + "\' ");
                        indexesOfNegationArguments.Add(arguments.IndexOf(negation));
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions (negated arguments) and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < negationArguments.Count; column++)
                        {
                            negationValues[line, column] = !values[line, indexesOfNegationArguments[column]];
                            Console.Write(Convert.ToInt32(negationValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine("Não há argumentos negados.");
                #endregion
                Console.WriteLine("");

                #region Logical and expressions
                Console.WriteLine("Expressões de e lógico:");

                //Gets arguments in formula by the position of the logical and symbol (.).
                List<Char[]> andArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.And);

                //Verifies is there is at least one argument.
                Boolean[,] andValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), andArguments.Count];
                if (andArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions (negated arguments).
                    List<Int32[]> indexesOfAndArguments = new List<Int32[]>();
                    foreach (Char[] and in andArguments)
                    {
                        Console.Write(and[0].ToString() + TruthTable.Expressions.And + and[1].ToString());
                        indexesOfAndArguments.Add(new Int32[] { arguments.IndexOf(and[0]), arguments.IndexOf(and[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions (negated arguments) and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < andArguments.Count; column++)
                        {
                            andValues[line, column] = values[line, indexesOfAndArguments[column][0]] && values[line, indexesOfAndArguments[column][1]];
                            Console.Write(" " + Convert.ToInt32(andValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine("Não há expressões de e lógico.");

                #endregion
                Console.WriteLine("");
            }

            Console.Title = "Belizas";
            Console.ReadKey();
        }
    }
}
