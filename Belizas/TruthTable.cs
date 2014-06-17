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

            Not = '\'';
            And = '.';
            Or = '+';
            Xor = ':';
            IfThen = '>';
            ThenIf = '<';
            IfAndOnlyIf = '-';
        }

        public Char Not { get; set; }
        public Char And { get; set; }
        public Char Or { get; set; }
        public Char Xor { get; set; }
        public Char IfThen { get; set; }
        public Char ThenIf { get; set; }
        public Char IfAndOnlyIf { get; set; }

        public String Formula { get; private set; }
        public List<Char> Arguments { get; private set; }
        public Boolean[,] ArgumentsValues { get; private set; }
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
                    for (Int32 index = 0; index < EnumerateOperators().Length; index++)
                        if (character == EnumerateOperators()[index])
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
                    else if (Formula.ToCharArray()[index] == Not)
                        isValid = Char.IsLetter(Formula.ToCharArray()[index - 1]) && index != 0;
                    else
                        foreach (Char item in EnumerateOperators())
                            if (isValid)
                                if (item == Not)
                                    isValid = Formula.ToCharArray()[index + 1] != item && index != 0 && index != Formula.Length - 1;
                                else
                                    isValid = Formula.ToCharArray()[index - 1] != item && Formula.ToCharArray()[index + 1] != item && index != 0 && index != Formula.Length - 1;
                }

            return isValid;
        }
        public Char[] EnumerateOperators()
        {
            return new Char[] { Not, And, Or, Xor, IfThen, ThenIf, IfAndOnlyIf };
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
            String expression = "", formula = Formula;
            while (formula.Length != 1)
            {
                if (Formula.IndexOf(Not) > 0)
                    expression = formula.Substring(formula.IndexOf(Not) - 1, 2);
                else if (Formula.IndexOf(And) > 0)
                    expression = formula.Substring(formula.IndexOf(And) - 1, 3);
                else if (Formula.IndexOf(Or) > 0)
                    expression = formula.Substring(formula.IndexOf(Or) - 1, 3);
                else if (Formula.IndexOf(Xor) > 0)
                    expression = formula.Substring(formula.IndexOf(Xor) - 1, 3);
                else if (Formula.IndexOf(IfThen) > 0)
                    expression = formula.Substring(formula.IndexOf(IfThen) - 1, 3);
                else if (Formula.IndexOf(ThenIf) > 0)
                    expression = formula.Substring(formula.IndexOf(ThenIf) - 1, 3);
                else if (Formula.IndexOf(IfAndOnlyIf) > 0)
                    expression = formula.Substring(formula.IndexOf(IfAndOnlyIf) - 1, 3);

                formula = ReplaceFirst(formula, expression, Convert.ToChar(ExpressionsValues.Count + Churros).ToString());
                CalculateExpression(expression);
            }
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

            Expressions.Add(expression);
            ExpressionsValues.Add(expressionValues);
        }
        public override String ToString()
        {
            String table = "";

            for (Int32 index = 0; index < Arguments.Count + ExpressionsValues.Count; index++)
                if (index < Arguments.Count)
                    table += Arguments[index] + " ";
                else if (index == Arguments.Count + ExpressionsValues.Count - 1)
                    table += Formula;
                else
                    table += "x ";
            table += "\n";

            for (Int32 line = 0; line < Math.Pow(2, Arguments.Count); line++)
            {
                for (Int32 column = 0; column < Arguments.Count + ExpressionsValues.Count; column++)
                {
                    if (column < Arguments.Count)
                        table += Convert.ToInt32(ArgumentsValues[line, column]).ToString() + " ";
                    else
                        table += Convert.ToInt32(ExpressionsValues[column - Arguments.Count][line]).ToString() + " ";
                }
                table += "\n";
            }
            table += "\n";

            return table;
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