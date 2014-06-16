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
                };
            }
        }

        public static List<Boolean[]> ExpressionsValues { get; set; }

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
        public static Boolean[] CalculateExpression(List<Char> arguments, Boolean[,] values, String expression)
        {
            Boolean[] expressionValue = new Boolean[(Int32)Math.Pow(2, arguments.Count)];
            Char expressionOperator = expression.ToCharArray()[1];
            Boolean value1, value2;

            for (Int32 line = 0; line < expressionValue.Length; line++)
            {
                if (expression.ToCharArray()[0] == '?' && expression.ToCharArray()[2] == '?')
                {
                    value1 = ExpressionsValues[ExpressionsValues.Count - 2][line];
                    value2 = ExpressionsValues[ExpressionsValues.Count - 1][line];
                }
                else if (expression.ToCharArray()[0] == '?')
                {
                    value1 = ExpressionsValues[ExpressionsValues.Count - 1][line];
                    value2 = values[line, arguments.IndexOf(expression.ToCharArray()[2])];
                }
                else if (expression.ToCharArray()[2] == '?')
                {
                    value1 = values[line, arguments.IndexOf(expression.ToCharArray()[0])];
                    value2 = ExpressionsValues[ExpressionsValues.Count - 1][line];
                }
                else
                {
                    value1 = values[line, arguments.IndexOf(expression.ToCharArray()[0])];
                    value2 = values[line, arguments.IndexOf(expression.ToCharArray()[2])];
                }

                if (expressionOperator == Operators.And)
                    expressionValue[line] = value1 && value2;
                else if (expressionOperator == Operators.Or)
                    expressionValue[line] = value1 || value2;
                else if (expressionOperator == Operators.Xor)
                    expressionValue[line] = (value1 && !value2) || (!value1 && value2);
                else if (expressionOperator == Operators.IfThen)
                    expressionValue[line] = value1 ? value2 : true;
                else if (expressionOperator == Operators.ThenIf)
                    expressionValue[line] = value2 ? value1 : true;
                else if (expressionOperator == Operators.IfAndOnlyIf)
                    expressionValue[line] = value1 == value2;
            }

            return expressionValue;
        }
    }
}
