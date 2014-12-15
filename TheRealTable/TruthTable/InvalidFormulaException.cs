using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhanderu.TheRealTable.TruthTable
{
    class InvalidFormulaException : Exception
    {
        public InvalidFormulaException() : base("Can't operate this function because the formula used is invalid. Always validate using the \"ValidateFormula\" method before setting the formula.") { }
    }
}