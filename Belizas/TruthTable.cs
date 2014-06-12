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

            Char[] expressionArguments = new Char[] { expression.ToCharArray()[0], expression.ToCharArray()[2] };
            Char expressionOperator = expression.ToCharArray()[1];

            for (Int32 line = 0; line < expressionValue.Length; line++)
            {
                if (expressionOperator == Operators.And)
                {
                    if (expressionArguments[0] == '?')
                        expressionValue[line] = ExpressionsValues[ExpressionsValues.Count - 1][line] && values[line, arguments.IndexOf(expressionArguments[1])];
                    else if (expressionArguments[1] == '?')
                        expressionValue[line] = values[line, arguments.IndexOf(expressionArguments[0])] && ExpressionsValues[ExpressionsValues.Count - 1][line];
                    else
                        expressionValue[line] = values[line, arguments.IndexOf(expressionArguments[0])] && values[line, arguments.IndexOf(expressionArguments[1])];
                }
                else if (expressionOperator == Operators.Or)
                {
                    if (expressionArguments[0] == '?')
                        expressionValue[line] = ExpressionsValues[ExpressionsValues.Count - 1][line] || values[line, arguments.IndexOf(expressionArguments[1])];
                    else if (expressionArguments[1] == '?')
                        expressionValue[line] = values[line, arguments.IndexOf(expressionArguments[0])] || ExpressionsValues[ExpressionsValues.Count - 1][line];
                    else
                        expressionValue[line] = values[line, arguments.IndexOf(expressionArguments[0])] || values[line, arguments.IndexOf(expressionArguments[1])];
                }
                else if (expressionOperator == Operators.Xor)
                {
                    if (expressionArguments[0] == '?')
                        expressionValue[line] = Xor(ExpressionsValues[ExpressionsValues.Count - 1][line], values[line, arguments.IndexOf(expressionArguments[1])]);
                    else if (expressionArguments[1] == '?')
                        expressionValue[line] = Xor(values[line, arguments.IndexOf(expressionArguments[0])], ExpressionsValues[ExpressionsValues.Count - 1][line]);
                    else
                        expressionValue[line] = Xor(values[line, arguments.IndexOf(expressionArguments[0])], values[line, arguments.IndexOf(expressionArguments[1])]);
                }
                else if (expressionOperator == Operators.IfThen)
                {
                    if (expressionArguments[0] == '?')
                        expressionValue[line] = IfThen(ExpressionsValues[ExpressionsValues.Count - 1][line], values[line, arguments.IndexOf(expressionArguments[1])]);
                    else if (expressionArguments[1] == '?')
                        expressionValue[line] = IfThen(values[line, arguments.IndexOf(expressionArguments[0])], ExpressionsValues[ExpressionsValues.Count - 1][line]);
                    else
                        expressionValue[line] = IfThen(values[line, arguments.IndexOf(expressionArguments[0])], values[line, arguments.IndexOf(expressionArguments[1])]);
                }
                else if (expressionOperator == Operators.ThenIf)
                {
                    if (expressionArguments[0] == '?')
                        expressionValue[line] = ThenIf(ExpressionsValues[ExpressionsValues.Count - 1][line], values[line, arguments.IndexOf(expressionArguments[1])]);
                    else if (expressionArguments[1] == '?')
                        expressionValue[line] = ThenIf(values[line, arguments.IndexOf(expressionArguments[0])], ExpressionsValues[ExpressionsValues.Count - 1][line]);
                    else
                        expressionValue[line] = ThenIf(values[line, arguments.IndexOf(expressionArguments[0])], values[line, arguments.IndexOf(expressionArguments[1])]);
                }
                else if (expressionOperator == Operators.IfAndOnlyIf)
                {
                    if (expressionArguments[0] == '?')
                        expressionValue[line] = ExpressionsValues[ExpressionsValues.Count - 1][line] == values[line, arguments.IndexOf(expressionArguments[1])];
                    else if (expressionArguments[1] == '?')
                        expressionValue[line] = values[line, arguments.IndexOf(expressionArguments[0])] == ExpressionsValues[ExpressionsValues.Count - 1][line];
                    else
                        expressionValue[line] = values[line, arguments.IndexOf(expressionArguments[0])] == values[line, arguments.IndexOf(expressionArguments[1])];
                }
            }

            return expressionValue;
        }

        #region Operators functions
        public static Boolean Xor(Boolean value1, Boolean value2)
        {
            return ((value1 && !value2) || (!value1 && value2));
        }
        public static Boolean IfThen(Boolean value1, Boolean value2)
        {
            return !(value1 && !value2);
        }
        public static Boolean ThenIf(Boolean value1, Boolean value2)
        {
            return !(!value1 && value2);
        }
        #endregion
    }
}
