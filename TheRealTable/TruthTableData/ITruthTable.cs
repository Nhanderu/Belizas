using System;
using System.Collections.Generic;

namespace Nhanderu.TheRealTable.TruthTableData
{
    public interface ITruthTable
    {
        #region Operators
        Char Not { get; set; }

        Char And { get; set; }

        Char Or { get; set; }

        Char Xor { get; set; }

        Char IfThen { get; set; }

        Char ThenIf { get; set; }

        Char IfAndOnlyIf { get; set; }

        Char OpeningBracket { get; set; }

        Char ClosingBracket { get; set; }
        #endregion

        #region Truth table data properties
        String Formula { get; }

        IList<Char> Arguments { get; }

        Boolean[,] ArgumentsValues { get; }

        IList<String> Expressions { get; }

        IList<Boolean[]> ExpressionsValues { get; }
        #endregion

        Boolean ValidateFormula(String formula = null);

        IList<Char> EnumerateOperators();

        void Calculate();

        String ToString();
    }
}
