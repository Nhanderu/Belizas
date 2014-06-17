using System;
using System.Collections.Generic;

namespace Nhanderu.Belizas
{
    public class TruthTable
    {
        public TruthTable(String formula)
        {
            Formula = formula;

            Arguments = new List<Char>();
            foreach (Char item in formula.ToCharArray())
                if (Char.IsLetter(item) && !Arguments.Contains(item))
                    Arguments.Add(item);

            Expressions = new List<String>();
            ExpressionsValues = new List<Boolean[]>();
            Churros = 13312;
        }

        public class Operators
        {
            public static Char Not = '\'',
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
                    Not,
                    And,
                    Or,
                    Xor,
                    IfThen,
                    ThenIf,
                    IfAndOnlyIf,
                };
            }
        }

        public String Formula { get; private set; }
        public List<Char> Arguments { get; private set; }
        public Boolean[,] ArgumentsValues { get; set; }
        public List<String> Expressions { get; private set; }
        public List<Boolean[]> ExpressionsValues { get; private set; }
        public Int32 Churros { get; private set; }

        public Boolean ValidateFormula()
        {
            List<Boolean> charactersStatus = new List<Boolean>();
            Boolean isDisallowedCharacter = true, isValid = true;

            foreach (Char character in Formula)
            {
                if (Char.IsLetter(character))
                    isDisallowedCharacter = false;
                else
                    for (Int32 index = 0; index < Operators.Enumerate().Length; index++)
                        if (character == Operators.Enumerate()[index])
                            isDisallowedCharacter = false;

                charactersStatus.Add(isDisallowedCharacter);
                isDisallowedCharacter = true;
            }

            foreach (Boolean status in charactersStatus)
                if (status)
                    isValid = false;

            if (isValid)
                for (Int32 index = 0; index < Formula.Length; index++)
                {
                    if (Char.IsLetter(Formula.ToCharArray()[index]))
                    {
                        if (isValid && index != 0)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index - 1]);
                        if (isValid && index != Formula.Length - 1)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index + 1]);
                    }
                    else if (Formula.ToCharArray()[index] == Operators.Not)
                        isValid = Char.IsLetter(Formula.ToCharArray()[index - 1]) && index != 0;
                    else
                        foreach (Char item in Operators.Enumerate())
                            if (isValid)
                                if (item == Operators.Not)
                                    isValid = Formula.ToCharArray()[index + 1] != item && index != 0 && index != Formula.Length - 1;
                                else
                                    isValid = Formula.ToCharArray()[index - 1] != item && Formula.ToCharArray()[index + 1] != item && index != 0 && index != Formula.Length - 1;
                }

            return isValid;
        }
        public void CalculateExpression(String expression)
        {
            Boolean[] expressionValues = new Boolean[(Int32)Math.Pow(2, Arguments.Count)];
            Char expressionOperator = expression.ToCharArray()[1];
            Boolean value1 = true, value2 = true;

            for (Int32 line = 0; line < expressionValues.Length; line++)
            {
                if (expression.ToCharArray()[0] >= Churros)
                    value1 = ExpressionsValues[expression.ToCharArray()[0] - Churros][line];
                else
                    value1 = ArgumentsValues[line, Arguments.IndexOf(expression.ToCharArray()[0])];

                if (expression.Length == 3)
                    if (expression.ToCharArray()[2] >= Churros)
                        value2 = ExpressionsValues[expression.ToCharArray()[2] - Churros][line];
                    else
                        value2 = ArgumentsValues[line, Arguments.IndexOf(expression.ToCharArray()[2])];

                if (expressionOperator == Operators.Not)
                    expressionValues[line] = !value1;
                if (expressionOperator == Operators.And)
                    expressionValues[line] = value1 && value2;
                else if (expressionOperator == Operators.Or)
                    expressionValues[line] = value1 || value2;
                else if (expressionOperator == Operators.Xor)
                    expressionValues[line] = value1 ? !value2 : value2;
                else if (expressionOperator == Operators.IfThen)
                    expressionValues[line] = value1 ? value2 : true;
                else if (expressionOperator == Operators.ThenIf)
                    expressionValues[line] = value2 ? value1 : true;
                else if (expressionOperator == Operators.IfAndOnlyIf)
                    expressionValues[line] = value1 == value2;
            }

            ExpressionsValues.Add(expressionValues);
        }
    }
}
