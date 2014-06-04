﻿using System;
using System.Collections.Generic;

namespace Nhanderu.Belizas
{
    public static class TruthTable
    {
        public static class Expressions
        {
            public static Char Negation = '\'',
            And = '.',
            Or = '+',
            Xor = ':',
            IfThen = '>',
            ThenIf = '<',
            IfAndOnlyIf = '-';

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
                    IfAndOnlyIf
                };
            }
            public static Boolean DoXor(Boolean value1, Boolean value2)
            {
                if ((value1 && !value2) || (!value1 && value2))
                    return true;
                else
                    return false;
            }
            public static Boolean DoIfThen(Boolean value1, Boolean value2)
            {
                if (value1 && !value2)
                    return false;
                else
                    return true;
            }
            public static Boolean DoThenIf(Boolean value1, Boolean value2)
            {
                if (!value1 && value2)
                    return false;
                else
                    return true;
            }
            public static Boolean DoIfAndOnlyIf(Boolean value1, Boolean value2)
            {
                if (value1 == value2)
                    return true;
                else
                    return false;
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
            }

            foreach (Boolean status in charactersStatus)
                if (status)
                    hasDisallowedCharacters = true;

            return hasDisallowedCharacters;
        }
        public static List<Char> GetArgumentsInExpression(String formula, Int32 quantityOfArguments)
        {
            List<Char> arguments = new List<Char>();

            if (formula.IndexOf(Expressions.Negation) >= 0)
            {
                List<Int32> symbolPositions = new List<Int32>();
                symbolPositions.Add(formula.IndexOf(Expressions.Negation));

                for (Int32 i = 1; i < quantityOfArguments; i++)
                    if (formula.IndexOf(Expressions.Negation, symbolPositions[symbolPositions.Count - 1] + 1) > 0)
                        symbolPositions.Add(formula.IndexOf(Expressions.Negation, symbolPositions[symbolPositions.Count - 1] + 1));

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
    }
}
