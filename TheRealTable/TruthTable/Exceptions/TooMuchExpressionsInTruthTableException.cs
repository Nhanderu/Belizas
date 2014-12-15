using System;

namespace Nhanderu.TheRealTable.TruthTable.Exceptions
{
    class TooMuchExpressionsInTruthTableException : TooMuchInformationInTruthTableException
    {
        public TooMuchExpressionsInTruthTableException() : base("There's a lot of expressions in the formula, which is more than the memory of this computer can handle or than the OS has reserved for this program.") { }
    }
}