using System;

namespace Nhanderu.TheRealTable.TruthTable
{
    class InvalidFormulaException : Exception
    {
        public InvalidFormulaException() : base("Can't operate this function because the used formula is invalid. Always validate using the \"ValidateFormula\" method before setting the formula.") { }
    }
}