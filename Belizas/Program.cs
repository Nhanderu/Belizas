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

                //Gets arguments in formula by the position of the logical and symbol.
                List<Char[]> andArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.And);

                //Verifies is there is at least one argument.
                Boolean[,] andValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), andArguments.Count];
                if (andArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions.
                    List<Int32[]> indexesOfAndArguments = new List<Int32[]>();
                    foreach (Char[] and in andArguments)
                    {
                        Console.Write(and[0].ToString() + TruthTable.Expressions.And + and[1].ToString());
                        indexesOfAndArguments.Add(new Int32[] { arguments.IndexOf(and[0]), arguments.IndexOf(and[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions and its values.
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

                #region Logical or expressions
                Console.WriteLine("Expressões de ou lógico:");

                //Gets arguments in formula by the position of the logical or symbol.
                List<Char[]> orArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.Or);

                //Verifies is there is at least one argument.
                Boolean[,] orValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), orArguments.Count];
                if (orArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions.
                    List<Int32[]> indexesOfOrArguments = new List<Int32[]>();
                    foreach (Char[] or in orArguments)
                    {
                        Console.Write(or[0].ToString() + TruthTable.Expressions.Or + or[1].ToString());
                        indexesOfOrArguments.Add(new Int32[] { arguments.IndexOf(or[0]), arguments.IndexOf(or[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < orArguments.Count; column++)
                        {
                            orValues[line, column] = values[line, indexesOfOrArguments[column][0]] || values[line, indexesOfOrArguments[column][1]];
                            Console.Write(" " + Convert.ToInt32(orValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine("Não há expressões de ou lógico.");
                #endregion
                Console.WriteLine("");

                #region Logical exclusive or expressions
                Console.WriteLine("Expressões de ou exclusivo lógico:");

                //Gets arguments in formula by the position of the logical xor symbol.
                List<Char[]> xorArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.Xor);

                //Verifies is there is at least one argument.
                Boolean[,] xorValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), xorArguments.Count];
                if (xorArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions.
                    List<Int32[]> indexesOfXorArguments = new List<Int32[]>();
                    foreach (Char[] xor in xorArguments)
                    {
                        Console.Write(xor[0].ToString() + TruthTable.Expressions.Xor + xor[1].ToString());
                        indexesOfXorArguments.Add(new Int32[] { arguments.IndexOf(xor[0]), arguments.IndexOf(xor[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < xorArguments.Count; column++)
                        {
                            xorValues[line, column] = TruthTable.Expressions.DoXor(values[line, indexesOfXorArguments[column][0]], values[line, indexesOfXorArguments[column][1]]);
                            Console.Write(" " + Convert.ToInt32(xorValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine("Não há expressões de ou exclusivo lógico.");
                #endregion
                Console.WriteLine("");

                #region If/then expressions
                Console.WriteLine("Expressões de se/então:");

                //Gets arguments in formula by the position of the if/then symbol.
                List<Char[]> ifThenArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.IfThen);

                //Verifies is there is at least one argument.
                Boolean[,] ifThenValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), ifThenArguments.Count];
                if (ifThenArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions.
                    List<Int32[]> indexesOfIfThenArguments = new List<Int32[]>();
                    foreach (Char[] ifThen in ifThenArguments)
                    {
                        Console.Write(ifThen[0].ToString() + TruthTable.Expressions.IfThen + ifThen[1].ToString());
                        indexesOfIfThenArguments.Add(new Int32[] { arguments.IndexOf(ifThen[0]), arguments.IndexOf(ifThen[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < ifThenArguments.Count; column++)
                        {
                            ifThenValues[line, column] = TruthTable.Expressions.DoIfThen(values[line, indexesOfIfThenArguments[column][0]], values[line, indexesOfIfThenArguments[column][1]]);
                            Console.Write(" " + Convert.ToInt32(ifThenValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine("Não há expressões de se/então.");
                #endregion
                Console.WriteLine("");

                #region Then/if expressions
                Console.WriteLine("Expressões de então/se:");

                //Gets arguments in formula by the position of the then/if symbol.
                List<Char[]> thenIfArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.ThenIf);

                //Verifies is there is at least one argument.
                Boolean[,] thenIfValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), thenIfArguments.Count];
                if (thenIfArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions.
                    List<Int32[]> indexesOfThenIfArguments = new List<Int32[]>();
                    foreach (Char[] thenIf in thenIfArguments)
                    {
                        Console.Write(thenIf[0].ToString() + TruthTable.Expressions.ThenIf + thenIf[1].ToString());
                        indexesOfThenIfArguments.Add(new Int32[] { arguments.IndexOf(thenIf[0]), arguments.IndexOf(thenIf[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < thenIfArguments.Count; column++)
                        {
                            thenIfValues[line, column] = TruthTable.Expressions.DoThenIf(values[line, indexesOfThenIfArguments[column][0]], values[line, indexesOfThenIfArguments[column][1]]);
                            Console.Write(" " + Convert.ToInt32(thenIfValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
                }
                else
                    Console.WriteLine("Não há expressões de então/se.");
                #endregion
                Console.WriteLine("");

                #region If and only if expressions
                Console.WriteLine("Expressões de se e somente se:");

                //Gets arguments in formula by the position of the if and only if symbol.
                List<Char[]> ifAndOnlyIfArguments = TruthTable.GetArgumentsInExpression(formula, arguments.Count, TruthTable.Expressions.IfAndOnlyIf);

                //Verifies is there is at least one argument.
                Boolean[,] ifAndOnlyIfValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), ifAndOnlyIfArguments.Count];
                if (ifAndOnlyIfArguments.Count > 0)
                {
                    //Writes the first line of the truth table: the expressions.
                    List<Int32[]> indexesOfIfAndOnlyIfArguments = new List<Int32[]>();
                    foreach (Char[] ifAndOnlyIf in ifAndOnlyIfArguments)
                    {
                        Console.Write(ifAndOnlyIf[0].ToString() + TruthTable.Expressions.IfAndOnlyIf + ifAndOnlyIf[1].ToString());
                        indexesOfIfAndOnlyIfArguments.Add(new Int32[] { arguments.IndexOf(ifAndOnlyIf[0]), arguments.IndexOf(ifAndOnlyIf[1]) });
                    }
                    Console.WriteLine("");

                    //Writes the first columns: the expressions and its values.
                    for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
                    {
                        for (int column = 0; column < ifAndOnlyIfArguments.Count; column++)
                        {
                            ifAndOnlyIfValues[line, column] = TruthTable.Expressions.DoIfAndOnlyIf(values[line, indexesOfIfAndOnlyIfArguments[column][0]], values[line, indexesOfIfAndOnlyIfArguments[column][1]]);
                            Console.Write(" " + Convert.ToInt32(ifAndOnlyIfValues[line, column]).ToString() + "  ");
                        }
                        Console.WriteLine("");
                    }
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
