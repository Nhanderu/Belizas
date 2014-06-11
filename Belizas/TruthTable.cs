using System;
using System.Collections.Generic;

namespace Nhanderu.Belizas
{
    public static class TruthTable
    {
        public static class Operators
        {
            public static Char Negation = '\'',
            And = '.',
            Or = '+',
            Xor = ':',
            IfThen = '>',
            ThenIf = '<',
            IfAndOnlyIf = '-';
            public static Char[] Parentheses = { '(', ')' };

            public static Char[] Enumerate()
            {
                return new Char[]
                {
                    Negation,
                    And,
                    Or,
                    Xor,
                    IfThen,
                    ThenIf,
                    IfAndOnlyIf,
                    Parentheses[0],
                    Parentheses[1]
                };
            }
        }

        public static Boolean HasDisallowedCharacters(String text, Char[] rule)
        {
            List<Boolean> charactersStatus = new List<Boolean>();
            Boolean isDisallowed = true, hasDisallowedCharacters = false;

            foreach (Char character in text)
            {
                if (Char.IsLetter(character))
                    isDisallowed = false;
                else
                    for (Int32 i = 0; i < rule.Length; i++)
                        if (character == rule[i])
                            isDisallowed = false;

                charactersStatus.Add(isDisallowed);
                isDisallowed = true;
            }

            foreach (Boolean status in charactersStatus)
                if (status)
                    hasDisallowedCharacters = true;

            return hasDisallowedCharacters;
        }

        public static List<Char> GetArgumentsInExpression(String formula, Int32 quantityOfArguments)
        {
            List<Char> arguments = new List<Char>();

            if (formula.IndexOf(Operators.Negation) >= 0)
            {
                List<Int32> symbolPositions = new List<Int32>();
                symbolPositions.Add(formula.IndexOf(Operators.Negation));

                for (Int32 i = 1; i < quantityOfArguments; i++)
                    if (formula.IndexOf(Operators.Negation, symbolPositions[symbolPositions.Count - 1] + 1) > 0)
                        symbolPositions.Add(formula.IndexOf(Operators.Negation, symbolPositions[symbolPositions.Count - 1] + 1));

                foreach (Int32 symbolPosition in symbolPositions)
                    arguments.Add(formula.ToCharArray()[symbolPosition - 1]);
            }

            return arguments;
        }
        public static List<Char[]> GetArgumentsInExpression(String formula, Int32 quantityOfArguments, Char expressionOperator)
        {
            List<Char[]> arguments = new List<Char[]>();

            if (formula.IndexOf(expressionOperator) >= 0)
            {
                List<Int32> symbolPositions = new List<Int32>();
                symbolPositions.Add(formula.IndexOf(expressionOperator));

                for (Int32 i = 1; i < quantityOfArguments; i++)
                    if (formula.IndexOf(expressionOperator, symbolPositions[symbolPositions.Count - 1] + 1) > 0)
                        symbolPositions.Add(formula.IndexOf(expressionOperator, symbolPositions[symbolPositions.Count - 1] + 1));

                foreach (Int32 symbolPosition in symbolPositions)
                    arguments.Add(new Char[] { formula.ToCharArray()[symbolPosition - 1], formula.ToCharArray()[symbolPosition + 1] });
            }

            return arguments;
        }
        public static String[] GetTable(List<Char> arguments, Boolean[,] values, Char expressionOperator, List<Char[]> expressionArguments, out Boolean[,] expressionValues)
        {
            expressionValues = new Boolean[(Int32)Math.Pow(2, arguments.Count), expressionArguments.Count];
            String[] table = new String[(Int32)Math.Pow(2, arguments.Count) + 1];

            List<Int32[]> indexes = new List<Int32[]>();
            foreach (Char[] and in expressionArguments)
            {
                table[0] += and[0].ToString() + expressionOperator + and[1].ToString() + " ";
                indexes.Add(new Int32[] { arguments.IndexOf(and[0]), arguments.IndexOf(and[1]) });
            }

            String tableLine;
            for (int line = 0; line < Math.Pow(2, arguments.Count); line++)
            {
                tableLine = "";
                for (int column = 0; column < expressionArguments.Count; column++)
                {
                    if (expressionOperator == Operators.And)
                        expressionValues[line, column] = values[line, indexes[column][0]] && values[line, indexes[column][1]];
                    else if (expressionOperator == Operators.Or)
                        expressionValues[line, column] = values[line, indexes[column][0]] || values[line, indexes[column][1]];
                    else if (expressionOperator == Operators.Xor)
                        expressionValues[line, column] = Xor(values[line, indexes[column][0]], values[line, indexes[column][1]]);
                    else if (expressionOperator == Operators.IfThen)
                        expressionValues[line, column] = IfThen(values[line, indexes[column][0]], values[line, indexes[column][1]]);
                    else if (expressionOperator == Operators.ThenIf)
                        expressionValues[line, column] = ThenIf(values[line, indexes[column][0]], values[line, indexes[column][1]]);
                    else if (expressionOperator == Operators.IfAndOnlyIf)
                        expressionValues[line, column] = IfAndOnlyIf(values[line, indexes[column][0]], values[line, indexes[column][1]]);
                    tableLine += " " + Convert.ToInt32(expressionValues[line, column]).ToString() + "  ";
                }
                table[line + 1] = tableLine;
            }

            return table;
        }

        #region Operators functions
        public static Boolean Xor(Boolean value1, Boolean value2)
        {
            if ((value1 && !value2) || (!value1 && value2))
                return true;
            else
                return false;
        }
        public static Boolean IfThen(Boolean value1, Boolean value2)
        {
            if (value1 && !value2)
                return false;
            else
                return true;
        }
        public static Boolean ThenIf(Boolean value1, Boolean value2)
        {
            if (!value1 && value2)
                return false;
            else
                return true;
        }
        public static Boolean IfAndOnlyIf(Boolean value1, Boolean value2)
        {
            if (value1 == value2)
                return true;
            else
                return false;
        }
        #endregion
    }
}
