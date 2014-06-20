using System;
using System.Collections.Generic;
using System.Text;

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

            Operators = new Dictionary<String, Char>();
            Operators.Add("Not", '\'');
            Operators.Add("And", '.');
            Operators.Add("Or", '+');
            Operators.Add("Xor", ':');
            Operators.Add("IfThen", '>');
            Operators.Add("ThenIf", '<');
            Operators.Add("IfAndOnlyIf", '-');
            Operators.Add("OpeningParenthesis", '(');
            Operators.Add("ClosingParenthesis", ')');
        }

        private Dictionary<String, Char> Operators;
        public Char Not
        {
            get
            {
                return Operators["Not"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["Not"] = value;
            }
        }
        public Char And
        {
            get
            {
                return Operators["And"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["And"] = value;
            }
        }
        public Char Or
        {
            get
            {
                return Operators["Or"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["Or"] = value;
            }
        }
        public Char Xor
        {
            get
            {
                return Operators["Xor"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["Xor"] = value;
            }
        }
        public Char IfThen
        {
            get
            {
                return Operators["IfThen"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["IfThen"] = value;
            }
        }
        public Char ThenIf
        {
            get
            {
                return Operators["ThenIf"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["ThenIf"] = value;
            }
        }
        public Char IfAndOnlyIf
        {
            get
            {
                return Operators["IfAndOnlyIf"];
            }
            set
            {
                if (!Operators.ContainsValue(value) && Convert.ToInt32(value) < Churros)
                    Operators["IfAndOnlyIf"] = value;
            }
        }
        public Char[] Parentheses
        {
            get
            {
                return new Char[] { Operators["OpeningParenthesis"], Operators["ClosingParenthesis"] };
            }
            set
            {
                if (!Operators.ContainsValue(value[0]) && !Operators.ContainsValue(value[1]) && Convert.ToInt32(value[0]) < Churros && Convert.ToInt32(value[1]) < Churros)
                {
                    Operators["OpeningParenthesis"] = value[0];
                    Operators["ClosingParenthesis"] = value[1];
                }
            }
        }

        public String Formula { get; private set; }
        public List<Char> Arguments { get; private set; }
        public Boolean[,] ArgumentsValues { get; private set; }
        public List<String> Expressions { get; private set; }
        public List<Boolean[]> ExpressionsValues { get; private set; }
        private Int32 Churros { get; set; }

        public Boolean ValidateFormula()
        {
            List<Boolean> charactersStatus = new List<Boolean>();
            Boolean isDisallowedCharacter = true, isValid = true;

            foreach (Char character in Formula)
            {
                if (Char.IsLetter(character))
                    isDisallowedCharacter = false;
                else
                    for (Int32 index = 0; index < EnumerateOperators().Length; index++)
                        if (character == EnumerateOperators()[index])
                        {
                            isDisallowedCharacter = false;
                            break;
                        }

                charactersStatus.Add(isDisallowedCharacter);
                isDisallowedCharacter = true;
            }

            foreach (Boolean status in charactersStatus)
                if (status)
                    isValid = false;

            if (isValid && (Formula.Contains(Parentheses[0].ToString()) || Formula.Contains(Parentheses[1].ToString())))
            {
                Int32[] parenthesisCount = new Int32[2];
                foreach (Char character in Formula)
                    if (character == Parentheses[0])
                        parenthesisCount[0]++;
                    else if (character == Parentheses[1])
                        parenthesisCount[1]++;
                if (parenthesisCount[0] != parenthesisCount[1])
                    isValid = false;
            }

            if (isValid)
                for (Int32 index = 0; index < Formula.Length; index++)
                    if (Char.IsLetter(Formula.ToCharArray()[index]))
                    {
                        if (isValid && index != 0)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index - 1]);
                        if (isValid && index != Formula.Length - 1)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index + 1]);
                    }
                    else if (Formula.ToCharArray()[index] == Not)
                    {
                        if (isValid && index != 0)
                            isValid = Char.IsLetter(Formula.ToCharArray()[index - 1]) || Formula.ToCharArray()[index - 1] == Parentheses[0] || Formula.ToCharArray()[index - 1] == Parentheses[1];
                        else if (isValid)
                            isValid = false;
                        if (isValid && index != Formula.Length - 1)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index + 1]);
                    }
                    else if (Formula.ToCharArray()[index] == Parentheses[0])
                    {
                        if (isValid && index != Formula.Length - 1)
                            isValid = Char.IsLetter(Formula.ToCharArray()[index + 1]) || Formula.ToCharArray()[index + 1] == Parentheses[0];
                        else if (isValid)
                            isValid = false;
                        if (isValid && index != 0)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index - 1]);
                    }
                    else if (Formula.ToCharArray()[index] == Parentheses[1])
                    {
                        if (isValid && index != 0)
                            isValid = Char.IsLetter(Formula.ToCharArray()[index - 1]) || Formula.ToCharArray()[index - 1] == Parentheses[1] || Formula.ToCharArray()[index - 1] == Not;
                        else if (isValid)
                            isValid = false;
                        if (isValid && index != Formula.Length - 1)
                            isValid = !Char.IsLetter(Formula.ToCharArray()[index + 1]);
                    }
                    else
                    {
                        isValid = index != 0 && index != Formula.Length - 1;
                        if (isValid)
                            foreach (Char item in EnumerateOperators())
                                if (item != Not && item != Parentheses[0] && item != Parentheses[1])
                                {
                                    isValid = Formula.ToCharArray()[index - 1] != item && Formula.ToCharArray()[index + 1] != item;
                                    if (!isValid)
                                        break;
                                }
                    }

            return isValid;
        }
        public Char[] EnumerateOperators()
        {
            return new Char[] { Not, And, Or, Xor, IfThen, ThenIf, IfAndOnlyIf, Parentheses[0], Parentheses[1] };
        }
        public void CalculateArguments()
        {
            ArgumentsValues = new Boolean[(Int32)Math.Pow(2, Arguments.Count), Arguments.Count];
            for (Int32 line = 0; line < Math.Pow(2, Arguments.Count); line++)
            {
                Int32 calculableLine = line;
                for (Int32 column = 0; column < Arguments.Count; column++)
                {
                    ArgumentsValues[line, column] = calculableLine >= Math.Pow(2, (Arguments.Count - 1) - column);
                    if (ArgumentsValues[line, column])
                        calculableLine -= (Int32)Math.Pow(2, (Arguments.Count - 1) - column);
                }
            }
        }
        public void CalculateExpressions()
        {
            String expression = "", pseudoformula = "";
            List<String> snips = new List<String>();

            snips.Add(Formula);

            while (snips[snips.Count - 1].IndexOf(Parentheses[0]) > -1)
                snips.Add(snips[snips.Count - 1].Substring(snips[snips.Count - 1].IndexOf(Parentheses[0]) + 1, snips[snips.Count - 1].LastIndexOf(Parentheses[1]) - snips[snips.Count - 1].IndexOf(Parentheses[0]) - 1));

            while (snips.Count != 0)
            {
                pseudoformula = snips[snips.Count - 1];
                while (pseudoformula.Length != 1)
                {
                    if (pseudoformula.IndexOf(Not) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(Not) - 1, 2);
                    else if (pseudoformula.IndexOf(And) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(And) - 1, 3);
                    else if (pseudoformula.IndexOf(Or) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(Or) - 1, 3);
                    else if (pseudoformula.IndexOf(Xor) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(Xor) - 1, 3);
                    else if (pseudoformula.IndexOf(IfThen) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(IfThen) - 1, 3);
                    else if (pseudoformula.IndexOf(ThenIf) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(ThenIf) - 1, 3);
                    else if (pseudoformula.IndexOf(IfAndOnlyIf) > 0)
                        expression = pseudoformula.Substring(pseudoformula.IndexOf(IfAndOnlyIf) - 1, 3);

                    pseudoformula = ReplaceFirst(pseudoformula, expression, Convert.ToChar(ExpressionsValues.Count + Churros).ToString());
                    CalculateExpression(expression, pseudoformula.Length == 1);
                }
                if (snips.Count > 1)
                {
                    if (HasChurros(snips[snips.Count - 1]))
                        for (Int32 index = 0; index < Expressions.Count; index++)
                            if (snips[snips.Count - 1].Contains(Convert.ToChar(Churros + index).ToString()))
                                snips[snips.Count - 1] = snips[snips.Count - 1].Replace(Convert.ToChar(Churros + index).ToString(), Expressions[index]);
                    snips[snips.Count - 2] = ReplaceFirst(snips[snips.Count - 2], Parentheses[0] + snips[snips.Count - 1] + Parentheses[1], pseudoformula);
                }
                snips.Remove(snips[snips.Count - 1]);
            }
        }
        public void CalculateExpression(String expression, Boolean hasParenthesis)
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

                if (expressionOperator == Not)
                    expressionValues[line] = !value1;
                else if (expressionOperator == And)
                    expressionValues[line] = value1 && value2;
                else if (expressionOperator == Or)
                    expressionValues[line] = value1 || value2;
                else if (expressionOperator == Xor)
                    expressionValues[line] = value1 ? !value2 : value2;
                else if (expressionOperator == IfThen)
                    expressionValues[line] = value1 ? value2 : true;
                else if (expressionOperator == ThenIf)
                    expressionValues[line] = value2 ? value1 : true;
                else if (expressionOperator == IfAndOnlyIf)
                    expressionValues[line] = value1 == value2;
            }

            String churros = "";
            while (HasChurros(expression))
            {
                foreach (Char item in expression)
                    if (item >= Churros)
                        churros = ReplaceFirst(expression, item.ToString(), Expressions[item - Churros]);
                expression = churros;
            }

            if (hasParenthesis)
                expression = Parentheses[0] + expression + Parentheses[1];

            Expressions.Add(expression);
            ExpressionsValues.Add(expressionValues);
        }
        public override String ToString()
        {
            StringBuilder table = new StringBuilder();

            for (Int32 index = 0; index < Arguments.Count + ExpressionsValues.Count; index++)
                if (index < Arguments.Count)
                    table.Append(Arguments[index] + " ");
                else
                    table.Append(Expressions[index - Arguments.Count] + " ");
            table.AppendLine();

            for (Int32 line = 0; line < Math.Pow(2, Arguments.Count); line++)
            {
                for (Int32 column = 0; column < Arguments.Count + ExpressionsValues.Count; column++)
                    if (column < Arguments.Count)
                        table.Append(Convert.ToInt32(ArgumentsValues[line, column]).ToString() + " ");
                    else
                    {
                        if (Expressions[column - Arguments.Count].Length % 2 == 0)
                            for (Int32 i = 0; i < Expressions[column - Arguments.Count].Length / 2 - 1; i++)
                                table.Append(" ");
                        else
                            for (Int32 i = 0; i < Expressions[column - Arguments.Count].Length / 2; i++)
                                table.Append(" ");

                        table.Append(Convert.ToInt32(ExpressionsValues[column - Arguments.Count][line]).ToString() + " ");
                        for (Int32 i = 0; i < Expressions[column - Arguments.Count].Length / 2; i++)
                            table.Append(" ");
                    }
                table.AppendLine();
            }

            return table.ToString();
        }

        private Boolean HasChurros(String expression)
        {
            Boolean hasChurros = false;

            foreach (Char item in expression)
                if (!hasChurros)
                    hasChurros = Convert.ToInt32(item) >= Churros;

            return hasChurros;
        }
        private String ReplaceFirst(String text, String oldValue, String newValue)
        {
            return text.Substring(0, text.IndexOf(oldValue)) + newValue + text.Substring(text.IndexOf(oldValue) + oldValue.Length);
        }
    }
}