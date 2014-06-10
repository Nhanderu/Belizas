using System;
using System.Collections.Generic;
using System.Linq;

namespace Nhanderu.Belizas
{
    class Program
    {
        static void Main(String[] args)
        {
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
            else if (TruthTable.HasDisallowedCharacters(formula, TruthTable.Operators.Enumerate()))
                Console.WriteLine("Fórmula inválida.");
            else
            {
                Console.Title = "Carregando";
                Console.WriteLine("Fórmula digitada:");
                Console.WriteLine(formula + "\n");

                #region Arguments
                Console.WriteLine("Argumentos da tabela verdade:");

                List<Char> arguments = new List<Char>();
                foreach (Char item in formula.ToCharArray())
                    if (Char.IsLetter(item) && !arguments.Contains(item))
                        arguments.Add(item);

                foreach (Char argument in arguments)
                    Console.Write(argument.ToString() + " ");
                Console.WriteLine("");

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
                
                #region Negation expressions
                Console.WriteLine("Argumentos negados da tabela verdade:");

                //Gets arguments in formula by the position of the negation symbol.
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

                List<Char[]> andArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Operators.And);

                Boolean[,] andValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), andArguments.Count];
                if (andArguments.Count > 0)
                {
                    String[] table = TruthTable.GetTable(arguments, values, TruthTable.Operators.And, andArguments, out andValues);
                    for (int i = 0; i < table.Length; i++)
                        Console.WriteLine(table[i]);
                }
                else
                    Console.WriteLine("Não há expressões de e lógico.");
                #endregion
                Console.WriteLine("");

                #region Logical or expressions
                Console.WriteLine("Expressões de ou lógico:");

                List<Char[]> orArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Operators.Or);

                Boolean[,] orValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), orArguments.Count];
                if (orArguments.Count > 0)
                {
                    String[] table = TruthTable.GetTable(arguments, values, TruthTable.Operators.Or, orArguments, out orValues);
                    for (int i = 0; i < table.Length; i++)
                        Console.WriteLine(table[i]);
                }
                else
                    Console.WriteLine("Não há expressões de ou lógico.");
                #endregion
                Console.WriteLine("");

                #region Logical exclusive or expressions
                Console.WriteLine("Expressões de ou exclusivo lógico:");

                List<Char[]> xorArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Operators.Xor);

                Boolean[,] xorValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), xorArguments.Count];
                if (xorArguments.Count > 0)
                {
                    String[] table = TruthTable.GetTable(arguments, values, TruthTable.Operators.Xor, xorArguments, out xorValues);
                    for (int i = 0; i < table.Length; i++)
                        Console.WriteLine(table[i]);
                }
                else
                    Console.WriteLine("Não há expressões de ou exclusivo lógico.");
                #endregion
                Console.WriteLine("");

                #region If/then expressions
                Console.WriteLine("Expressões de se/então:");

                List<Char[]> ifThenArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Operators.IfThen);

                Boolean[,] ifThenValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), ifThenArguments.Count];
                if (ifThenArguments.Count > 0)
                {
                    String[] table = TruthTable.GetTable(arguments, values, TruthTable.Operators.IfThen, ifThenArguments, out ifThenValues);
                    for (int i = 0; i < table.Length; i++)
                        Console.WriteLine(table[i]);
                }
                else
                    Console.WriteLine("Não há expressões de se/então.");
                #endregion
                Console.WriteLine("");

                #region Then/if expressions
                Console.WriteLine("Expressões de então/se:");

                List<Char[]> thenIfArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Operators.ThenIf);

                Boolean[,] thenIfValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), thenIfArguments.Count];
                if (thenIfArguments.Count > 0)
                {
                    String[] table = TruthTable.GetTable(arguments, values, TruthTable.Operators.ThenIf, thenIfArguments, out thenIfValues);
                    for (int i = 0; i < table.Length; i++)
                        Console.WriteLine(table[i]);
                }
                else
                    Console.WriteLine("Não há expressões de então/se.");
                #endregion
                Console.WriteLine("");

                #region If and only if expressions
                Console.WriteLine("Expressões de se e somente se:");

                List<Char[]> ifAndOnlyIfArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Operators.IfAndOnlyIf);

                Boolean[,] ifAndOnlyIfValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), ifAndOnlyIfArguments.Count];
                if (ifAndOnlyIfArguments.Count > 0)
                {
                    String[] table = TruthTable.GetTable(arguments, values, TruthTable.Operators.IfAndOnlyIf, ifAndOnlyIfArguments, out ifAndOnlyIfValues);
                    for (int i = 0; i < table.Length; i++)
                        Console.WriteLine(table[i]);
                }
                else
                    Console.WriteLine("Não há expressões de se e somente se.");
                #endregion
                Console.WriteLine("");
            }

            Console.Title = "Belizas";
            Console.ReadKey();
        }
    }
}
