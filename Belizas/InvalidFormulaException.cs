using System;

namespace Nhanderu.Belizas
{
    class InvalidFormulaException : Exception
    {
        public InvalidFormulaException() : base("Can't operate this function because the formula used is invalid. Always validate using the \"ValidateFormula\" method before setting the formula.") { }
    }
}
