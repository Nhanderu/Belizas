using System;

namespace Nhanderu.TheRealTable.TruthTable
{
    class TooMuchArgumentsInTruthTableException : OutOfMemoryException
    {
        public TooMuchArgumentsInTruthTableException() : base("An error has occurred due to the size of the truth table. There's a lot of arguments in the formula, which is more than the memory of this computer can handle or than the OS has reserved for this program.") { }

        public TooMuchArgumentsInTruthTableException(Int32 numberOfArguments) : base(String.Format("An error has occurred due to the size of the truth table. There's {0} arguments in the formula, so it needs {1}KBs for the values of each argument and {2}GBs for all the arguments values, which is more than the memory of this computer can handle or than the OS has reserved for this program.", numberOfArguments, Math.Pow(2, numberOfArguments) / 1024, numberOfArguments * Math.Pow(2, numberOfArguments) / 1073741824)) { }
    }
}