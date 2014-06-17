using System;
using System.Collections.Generic;

namespace Nhanderu.Belizas
{
    public static class TruthTable
    {
        public static class Operators
        {
            public static Char And = '.',
            Or = '+',
            Xor = ':',
            IfThen = '>',
            ThenIf = '<',
            IfAndOnlyIf = '-';

            public static Char[] Enumerate()
            {
                return new Char[]
                {
                    And,
                    Or,
                    Xor,
                    IfThen,
                    ThenIf,
                    IfAndOnlyIf,
                };
            }
        }

        public static String Formula { get; set; }
        public static List<Char> Arguments { get; set; }
        public static Boolean[,] ArgumentsValues { get; set; }
        public static List<String> Expressions { get; set; }
        public static List<Boolean[]> ExpressionsValues { get; set; }
        public static Int32 Churros { get { return 13312; } }

        public static Boolean IsValid(String text)
        {
            List<Boolean> charactersStatus = new List<Boolean>();
            Boolean isDisallowedCharacter = true, isValid = true;

            foreach (Char character in text)
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
                for (Int32 index = 0; index < text.Length; index++)
                {
                    if (Char.IsLetter(text.ToCharArray()[index]) && isValid && index != 0)
                        isValid = !Char.IsLetter(text.ToCharArray()[index - 1]);
                    if (Char.IsLetter(text.ToCharArray()[index]) && isValid && index != text.Length - 1)
                        isValid = !Char.IsLetter(text.ToCharArray()[index + 1]);
                }

            return isValid;
        }
        public static Boolean[] CalculateExpression(String expression)
        {
            Boolean[] expressionValues = new Boolean[(Int32)Math.Pow(2, TruthTable.Arguments.Count)];
            Char expressionOperator = expression.ToCharArray()[1];
            Boolean value1, value2;

            for (Int32 line = 0; line < expressionValues.Length; line++)
            {
                if (expression.ToCharArray()[0] >= Churros)
                    value1 = ExpressionsValues[expression.ToCharArray()[0] - Churros][line];
                else
                    value1 = ArgumentsValues[line, Arguments.IndexOf(expression.ToCharArray()[0])];

                if (expression.ToCharArray()[2] >= Churros)
                    value2 = ExpressionsValues[expression.ToCharArray()[2] - Churros][line];
                else
                    value2 = ArgumentsValues[line, Arguments.IndexOf(expression.ToCharArray()[2])];

                if (expressionOperator == Operators.And)
                    expressionValues[line] = value1 && value2;
                else if (expressionOperator == Operators.Or)
                    expressionValues[line] = value1 || value2;
                else if (expressionOperator == Operators.Xor)
                    expressionValues[line] = (value1 && !value2) || (!value1 && value2);
                else if (expressionOperator == Operators.IfThen)
                    expressionValues[line] = value1 ? value2 : true;
                else if (expressionOperator == Operators.ThenIf)
                    expressionValues[line] = value2 ? value1 : true;
                else if (expressionOperator == Operators.IfAndOnlyIf)
                    expressionValues[line] = value1 == value2;
            }

            return expressionValues;
        }
    }
}
