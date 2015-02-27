using Nhanderu.Belizas.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nhanderu.Belizas
{
    /// <summary>
    /// Represents a truth table generated by a formula.
    /// </summary>
    public class TruthTable : ITruthTable
    {
        private String _formula;
        private Dictionary<String, Char> _operators = new Dictionary<String, Char>();
        private readonly List<Char> _defaultOperators = new List<Char> { '\'', '.', '+', ':', '>', '<', '-', '(', ')' };
        private const Int32 _er = 13312;

        #region Operators
        /// <summary>
        /// Gets and sets the character that represents the operator "not".
        /// </summary>
        public Char Not
        {
            get { return _operators["Not"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["Not"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the operator "and".
        /// </summary>
        public Char And
        {
            get { return _operators["And"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["And"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the operator "or".
        /// </summary>
        public Char Or
        {
            get { return _operators["Or"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["Or"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the operator "xor".
        /// </summary>
        public Char Xor
        {
            get { return _operators["Xor"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["Xor"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the operator "if then".
        /// </summary>
        public Char IfThen
        {
            get { return _operators["IfThen"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["IfThen"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the operator "then if".
        /// </summary>
        public Char ThenIf
        {
            get { return _operators["ThenIf"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["ThenIf"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the operator "if and only if".
        /// </summary>
        public Char IfAndOnlyIf
        {
            get { return _operators["IfAndOnlyIf"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["IfAndOnlyIf"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the opening bracket.
        /// </summary>
        public Char OpeningBracket
        {
            get { return _operators["OpeningBracket"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["OpeningBracket"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the character that represents the opening bracket.
        /// </summary>
        public Char ClosingBracket
        {
            get { return _operators["ClosingBracket"]; }
            set
            {
                if (!_operators.ContainsValue(value) && Convert.ToInt32(value) < _er)
                    _operators["ClosingBracket"] = value;
            }
        }
        #endregion

        #region Truth table data properties
        /// <summary>
        /// Gets and sets the formula that rules the truth table and calculates the table automatically.
        /// </summary>
        public String Formula
        {
            get { return _formula; }
            set
            {
                _formula = value;
                Calculate();
            }
        }

        /// <summary>
        /// Gets a list of the arguments in the formula.
        /// </summary>
        public IList<Char> Arguments { get; private set; }

        /// <summary>
        /// Gets the values of the arguments in the formula.
        /// </summary>
        public Boolean[,] ArgumentsValues { get; private set; }

        /// <summary>
        /// Gets a list of the expressions in the formula.
        /// </summary>
        public IList<String> Expressions { get; private set; }

        /// <summary>
        /// Gets the values of the arguments in the formula.
        /// </summary>
        public IList<Boolean[]> ExpressionsValues { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the TruthTable class to the value indicated by the formula.
        /// </summary>
        /// <param name="formula">The formula that will rule the truth table.</param>
        /// <param name="calculate">A boolean value that indicates if the table will be calculated automatically in the constructor or it will be calculated by calling the method Calculate.</param>
        /// <param name="characters">The characters that will represent the operators.</param>
        public TruthTable(String formula, Boolean calculate = false, IEnumerable<Char> characters = null)
        {
            //Gets the enumarator of the characters that will represent the operators.
            IEnumerator<Char> charactersEnumerator = (characters ?? _defaultOperators).GetEnumerator();
            String[] operatorsKeys = new String[9] { "Not", "And", "Or", "Xor", "IfThen", "ThenIf", "IfAndOnlyIf", "OpeningBracket", "ClosingBracket" };
            Int32 index = 0;

            //Iterates through the enumerator until it reaches its end or the ninth position and adds the current character in the operators list.
            while (charactersEnumerator.MoveNext() && index < 9)
                _operators.Add(operatorsKeys[index++], charactersEnumerator.Current);

            //Verifies it the enumerator had less than 9 characters and adds the default operators to the rest of the positions in the list.
            while (index < 9)
                _operators.Add(operatorsKeys[index], _defaultOperators[index++]);

            //Sets the formula as the argument.
            _formula = formula;

            //Initializes the TruthTable properties.
            Arguments = new List<Char>();
            Expressions = new List<String>();
            ExpressionsValues = new List<Boolean[]>();

            //Verifies if it's necessary to calculate automatically.
            if (calculate) Calculate();
        }

        /// <summary>
        /// Verifies the formula, if it is under all the conditions to be a consistent truth table formula. 
        /// </summary>
        /// <param name="formula">The formula to be validated. If null, validate the formula that rules the instance.</param>
        /// <returns>True if the formula is under all the conditions, false if not.</returns>
        public Boolean ValidateFormula(String formula = null)
        {
            //Uses the Formula property if the parameter is null.
            //Removes all white spaces.
            String sentence = (formula ?? Formula).Replace(" ", "");

            //Verifies if the sentence is empty.
            if (String.IsNullOrEmpty(sentence))
                return false;

            //Iterates through the sentence to verify if it has any disallowed character.
            foreach (Char character in sentence)
                if (!Char.IsLetter(character) && !IsAnOperator(character))
                    return false;

            //Verifies if the sentence has the same amount of opening and closing brackets.
            if (sentence.Contains(OpeningBracket.ToString()) || sentence.Contains(ClosingBracket.ToString()))
            {
                Int32[] parenthesisCount = new Int32[2];

                foreach (Char character in sentence)
                    //If it is an opening bracket, sum 1 to opening brackets count.
                    if (character == OpeningBracket)
                        parenthesisCount[0]++;
                    //If it is an closing bracket, sum 1 to closing brackets count.
                    else if (character == ClosingBracket)
                        parenthesisCount[1]++;

                //Verifies if the amount of brackets is different.
                if (parenthesisCount[0] != parenthesisCount[1])
                    return false;
            }

            //Iterates through the sentence to verify the sequence of characters.
            for (Int32 index = 0; index < sentence.Length; index++)
                if (index == 0)
                {
                    //Verifies if the first character isn't a letter and an opening bracket.
                    if (!Char.IsLetter(sentence[index]) && sentence[index] != OpeningBracket)
                        return false;

                    //Verifies if the letters have, on its right, a character that isn't an operator (including not) and a closing bracket.
                    else if (Char.IsLetter(sentence[index]) && !IsAnOperator(sentence[index + 1], true, false) && sentence[index + 1] != ClosingBracket)
                        return false;

                    //Verifies if the opening bracket have, on its right, a character that isn't a letter nor opening bracket.
                    else if (sentence[index] == OpeningBracket && !Char.IsLetter(sentence[index + 1]) && sentence[index + 1] != OpeningBracket)
                        return false;
                }
                else if (index == sentence.Length - 1)
                {
                    //Verifies if the last character isn't a letter, not operator and a closing bracket.
                    if (!Char.IsLetter(sentence[index]) && sentence[index] != Not && sentence[index] != ClosingBracket)
                        return false;

                    //Verifies if the letters have, on its left, a character that isn't an operator (except not) and an opening bracket.
                    else if (Char.IsLetter(sentence[index]) && !IsAnOperator(sentence[index - 1], false, false) && sentence[index - 1] != OpeningBracket)
                        return false;

                    //Verifies if the not operators have, on its left, a character that isn't a not operator, a letter and a closing bracket.
                    else if (sentence[index] == Not && !Char.IsLetter(sentence[index - 1]) && sentence[index - 1] != Not && sentence[index - 1] != ClosingBracket)
                        return false;

                    //Verifies if the closing bracket have, on its left, a character that isn't a closing bracket, a letter and a not operator.
                    else if (sentence[index] == ClosingBracket && !Char.IsLetter(sentence[index - 1]) && sentence[index - 1] != Not && sentence[index - 1] != ClosingBracket)
                        return false;
                }
                else
                {
                    //Verifies if the letters have, on its left, a character that isn't an operator (except not) and an opening bracket.
                    //Also verifies if the letters have, on its right, a character that isn't an operator (including not) and a closing bracket.
                    if (Char.IsLetter(sentence[index]) && ((!IsAnOperator(sentence[index - 1], false, false) && sentence[index - 1] != OpeningBracket) || (!IsAnOperator(sentence[index + 1], true, false) && sentence[index + 1] != ClosingBracket)))
                        return false;

                    //Verifies if the not operators have, on its left, a character that isn't a letter, a not operator and a closing bracket.
                    //Also verifies if the not operators have, on its right, a character that isn't an operator (including not) and a closing bracket.
                    else if (sentence[index] == Not && ((!Char.IsLetter(sentence[index - 1]) && sentence[index - 1] != Not && sentence[index - 1] != ClosingBracket) || (!IsAnOperator(sentence[index + 1], true, false) && sentence[index + 1] != ClosingBracket)))
                        return false;

                    //Verifies if the opening bracket have, on its left, a character that isn't a operator (except not) and an opening bracket.
                    //Also verifies if the opening bracket have, on its right, a character that isn't a letter and opening bracket.
                    else if (sentence[index] == OpeningBracket && ((!IsAnOperator(sentence[index - 1], false, false) && sentence[index - 1] != OpeningBracket) || (!Char.IsLetter(sentence[index + 1]) && sentence[index + 1] != OpeningBracket)))
                        return false;

                    //Verifies if the closing bracket have, on its left, a character that isn't a letter, a not operator and a closing bracket.
                    //Also verifies if the closing bracket have, on its right, a character that isn't an operator (including not) and a closing bracket.
                    else if (sentence[index] == ClosingBracket && ((!Char.IsLetter(sentence[index - 1]) && sentence[index - 1] != Not && sentence[index - 1] != ClosingBracket) || (!IsAnOperator(sentence[index + 1], true, false) && sentence[index + 1] != ClosingBracket)))
                        return false;

                    //Verifies if the operators (except not) have, on its left, a character that isn't a have a letter, a closing bracket and a not operator.
                    //Also verifies if the operators (except not) have, on its right, a character that isn't a letter and an opening bracket.
                    else if (IsAnOperator(sentence[index], false, false) && ((!Char.IsLetter(sentence[index - 1]) && sentence[index - 1] != ClosingBracket && sentence[index - 1] != Not) || (!Char.IsLetter(sentence[index + 1]) && sentence[index + 1] != OpeningBracket)))
                        return false;
                }

            return true;
        }

        /// <summary>
        /// Calculates the truth table using the formula.
        /// </summary>
        public void Calculate()
        {
            //Verifies if the formula is valid.
            if (!ValidateFormula()) throw new InvalidFormulaException();
            else
            {
                //Gets the arguments.
                foreach (Char item in Formula)
                    if (Char.IsLetter(item) && !Arguments.Contains(item))
                        Arguments.Add(item);

                //Calculates the arguments and the expressions.
                CalculateArguments();
                CalculateExpressions();
            }
        }

        /// <summary>
        /// Returns all the operators.
        /// </summary>
        /// <param name="includeNot">If the not operator will be included in the enumeration.</param>
        /// <param name="includeBrackets">If the brackets will be included in the enumeration.</param>
        /// <returns>A list with all the operators.</returns>
        public IList<Char> EnumerateOperators(Boolean includeNot = true, Boolean includeBrackets = true)
        {
            List<Char> operators = new List<Char>();

            //Verifies if the Not operator is included in the enumaration.
            if (includeNot) operators.Add(Not);

            //Includes the operators in the enumaration.
            operators.AddRange(new List<Char>() { And, Or, Xor, IfThen, ThenIf, IfAndOnlyIf });

            //Verifies if the brackets are included in the enumaration.
            if (includeBrackets)
            {
                operators.Add(OpeningBracket);
                operators.Add(ClosingBracket);
            }

            return operators;
        }

        /// <summary>
        /// Verifies if the following character represents any operator.
        /// </summary>
        /// <param name="character">The character to be verified.</param>
        /// <param name="includeNot">If the not operator will be included in the verification.</param>
        /// <param name="includeBrackets">If the brackets will be included in the verification.</param>
        /// <returns>True if the character is an operator, false if it isn't.</returns>
        public Boolean IsAnOperator(Char character, Boolean includeNot = true, Boolean includeBrackets = true)
        {
            //Iterates through all the operators and verifies if the character is a operator.
            foreach (Char item in EnumerateOperators(includeNot, includeBrackets))
                if (character == item) return true;

            return false;
        }

        /// <summary>
        /// Generates a text that represents the truth table.
        /// </summary>
        /// <returns>The truth table in a text.</returns>
        public override String ToString()
        {
            //Initializes the string that will represent the table.
            StringBuilder table = new StringBuilder();

            //Creates the first line: the arguments and the expressions.
            //Iterates until it reaches the sum of the quantities of the arguments and the expressions to write them.
            for (Int32 index = 0; index < Arguments.Count + ExpressionsValues.Count; index++)
                //Writes the arguments.
                if (index < Arguments.Count)
                    TryAppend(table, Arguments[index] + " ");
                //If the iteration reach the argument limit, starts to write the expressions.
                else
                    TryAppend(table, Expressions[index - Arguments.Count] + " ");

            TryAppend(table, "\n");

            //Create the other lines: the logical values.
            //Iterates until it reaches 2 power the number of arguments to make the lines.
            for (Int32 line = 0; line < Math.Pow(2, Arguments.Count); line++)
            {
                //Iterates until it reaches the sum of the quantities of the arguments and the expressions to make the columns.
                for (Int32 column = 0; column < Arguments.Count + ExpressionsValues.Count; column++)
                    //Writes the arguments values.
                    if (column < Arguments.Count)
                        TryAppend(table, Convert.ToInt32(ArgumentsValues[line, column]).ToString() + " ");
                    //If the iteration reach the argument limit, starts to write the expressions values.
                    else
                    {
                        //Verifies if the size of the expression string is even to add proportionals black spaces to justify the text.
                        if (Expressions[column - Arguments.Count].Length % 2 == 0)
                            for (Int32 i = 0; i < Expressions[column - Arguments.Count].Length / 2 - 1; i++)
                                TryAppend(table, " ");
                        else
                            for (Int32 i = 0; i < Expressions[column - Arguments.Count].Length / 2; i++)
                                TryAppend(table, " ");

                        //Writes the current expression value.
                        TryAppend(table, Convert.ToInt32(ExpressionsValues[column - Arguments.Count][line]).ToString() + " ");
                        
                        //Writes the rest of the blank spaces to justify 
                        for (Int32 i = 0; i < Expressions[column - Arguments.Count].Length / 2; i++)
                            TryAppend(table, " ");
                    }

                TryAppend(table, "\n");
            }

            return table.ToString();
        }

        #region Private helper methods
        private void CalculateArguments()
        {
            try
            {
                ArgumentsValues = new Boolean[(Int32)Math.Pow(2, Arguments.Count), Arguments.Count];
            }
            catch (OutOfMemoryException) { throw new TooMuchArgumentsInTruthTableException(Arguments.Count); }

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

        private void CalculateExpressions()
        {
            String expression = "", pseudoformula = "";
            Dictionary<String, String> snips = SnipFormula();

            Int32 count = 0, counter = 0, depth = 0;
            foreach (Char item in Formula)
                if (item == OpeningBracket)
                {
                    count++;
                    counter++;
                    if (counter > depth)
                        depth = counter;
                }
                else if (item == ClosingBracket)
                    counter--;

            Int32[] actualID = new Int32[depth + 1], fatherID = new Int32[depth + 1];

            actualID[0] = 0;
            for (Int32 index = 1; index <= depth; index++)
                actualID[index] = -1;
            for (Int32 index = 0; index <= depth; index++)
                fatherID[index] = -1;

            while (snips.Count != 0)
            {
                while (!snips.ContainsKey(ConvertKey(actualID)))
                {
                    actualID[counter--] = -1;
                    fatherID[counter] = -1;
                }

                while (snips[ConvertKey(actualID)].Contains(OpeningBracket.ToString()))
                {
                    fatherID[counter] = actualID[counter];
                    actualID[++counter] = 0;
                }

                pseudoformula = snips[ConvertKey(actualID)];
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

                    pseudoformula = ReplaceFirst(pseudoformula, expression, Convert.ToChar(ExpressionsValues.Count + _er).ToString());
                    CalculateExpression(expression, pseudoformula.Length == 1);
                }

                if (snips.Count > 1)
                {
                    if (HasER(snips[ConvertKey(actualID)]))
                        for (Int32 index = 0; index < Expressions.Count; index++)
                            if (snips[ConvertKey(actualID)].Contains(Convert.ToChar(_er + index).ToString()))
                                snips[ConvertKey(actualID)] = snips[ConvertKey(actualID)].Replace(Convert.ToChar(_er + index).ToString(), Expressions[index]);
                    snips[ConvertKey(fatherID)] = ReplaceFirst(snips[ConvertKey(fatherID)], OpeningBracket + snips[ConvertKey(actualID)] + ClosingBracket, pseudoformula);
                }

                snips.Remove(ConvertKey(actualID));

                if (snips.Count > 0)
                    actualID[counter]++;

            }
        }

        private void CalculateExpression(String expression, Boolean hasParenthesis)
        {
            Boolean[] expressionValues = new Boolean[(Int32)Math.Pow(2, Arguments.Count)];
            Char expressionOperator = expression[1];
            Boolean value1 = true, value2 = true;

            for (Int32 line = 0; line < expressionValues.Length; line++)
            {
                if (expression[0] >= _er)
                    value1 = ExpressionsValues[expression[0] - _er][line];
                else
                    value1 = ArgumentsValues[line, Arguments.IndexOf(expression[0])];

                if (expression.Length == 3)
                    if (expression[2] >= _er)
                        value2 = ExpressionsValues[expression[2] - _er][line];
                    else
                        value2 = ArgumentsValues[line, Arguments.IndexOf(expression[2])];

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
            while (HasER(expression))
            {
                foreach (Char item in expression)
                    if (item >= _er)
                        churros = ReplaceFirst(expression, item.ToString(), Expressions[item - _er]);
                expression = churros;
            }

            if (hasParenthesis)
                expression = OpeningBracket + expression + ClosingBracket;

            try
            {
                Expressions.Add(expression);
                ExpressionsValues.Add(expressionValues);
            }
            catch (OutOfMemoryException) { throw new TooMuchExpressionsInTruthTableException(); }
        }

        private Dictionary<String, String> SnipFormula()
        {
            Int32 counter = 0, count = 0, depth = 0;
            foreach (Char item in Formula)
                if (item == OpeningBracket)
                {
                    count++;
                    counter++;
                    if (counter > depth)
                        depth = counter;
                }
                else if (item == ClosingBracket)
                    counter--;

            Int32[] treecode = new Int32[depth + 1];
            if (depth > 0)
                for (Int32 index = 1; index <= depth; index++)
                    treecode[index] = -1;
            treecode[0] = 0;

            List<String> pseudoformulas = new List<String>(count + 1);
            List<Int32> snipStart = new List<Int32>(count), snipEnd = new List<Int32>(count);
            List<Int32[]> treecodes = new List<Int32[]>(depth);
            pseudoformulas.Add(Formula);
            treecodes.Add(treecode);

            treecode = new Int32[depth + 1];
            treecodes[treecodes.Count - 1].CopyTo(treecode, 0);

            while (snipStart.Count != count)
                for (Int32 index = 0; index < Formula.Length; index++)
                    if (Formula[index] == OpeningBracket)
                    {
                        snipStart.Add(index + 1);
                        snipEnd.Add(-1);
                        treecode[++counter]++;
                        while (ContainsCode(treecodes, treecode))
                            treecode[counter]++;
                        treecodes.Add(treecode);
                        treecode = new Int32[depth + 1];
                        treecodes[treecodes.Count - 1].CopyTo(treecode, 0);
                    }
                    else if (Formula[index] == ClosingBracket)
                    {
                        for (Int32 position = snipStart.Count - 1; position > -1; position--)
                            if (snipEnd[position] == -1)
                            {
                                snipEnd[position] = index;
                                break;
                            }
                        treecode[counter--] = -1;
                    }

            for (Int32 index = 0; index < snipStart.Count; index++)
                pseudoformulas.Add(Formula.Substring(snipStart[index], snipEnd[index] - snipStart[index]));

            return OrganizeSnips(treecodes, pseudoformulas);
        }

        private Dictionary<String, String> OrganizeSnips(List<Int32[]> codes, List<String> texts)
        {
            Dictionary<String, String> snips = null;

            if (codes.Count == texts.Count)
            {
                snips = new Dictionary<String, String>();
                for (Int32 index = 0; index < codes.Count; index++)
                    snips.Add(ConvertKey(codes[index]), texts[index]);
            }

            return snips;
        }

        private Boolean HasER(String expression)
        {
            Boolean hasChurros = false;

            foreach (Char item in expression)
                if (!hasChurros)
                    hasChurros = Convert.ToInt32(item) >= _er;
                else
                    break;

            return hasChurros;
        }

        private Boolean ContainsCode(List<Int32[]> list, Int32[] code)
        {
            Boolean contains = false;

            for (Int32 listIndex = 0; listIndex < list.Count; listIndex++)
                if (!contains)
                    for (Int32 codeIndex = 0; codeIndex < code.Length; codeIndex++)
                        if (contains || codeIndex == 0)
                            contains = list[listIndex][codeIndex] == code[codeIndex];
                        else
                            break;

            return contains;
        }

        private String ConvertKey(Int32[] key)
        {
            StringBuilder newKey = new StringBuilder();

            for (Int32 index = 0; index < key.Length; index++)
            {
                newKey.Append(key[index].ToString());
                if (index != key.Length - 1)
                    newKey.Append('.');
            }

            return newKey.ToString();
        }

        private String ReplaceFirst(String text, String oldValue, String newValue)
        {
            return text.Substring(0, text.IndexOf(oldValue)) + newValue + text.Substring(text.IndexOf(oldValue) + oldValue.Length);
        }

        private void TryAppend(StringBuilder stringBuilder, String text)
        {
            try
            {
                stringBuilder.Append(text);
            }
            catch (OutOfMemoryException)
            {
                throw new TooMuchInformationInTruthTableException("Trying to convert the table to a text, the program has exceeded the limits of memory that the OS has reserved.");
            }
        }
        #endregion
    }
}