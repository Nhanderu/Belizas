using System;

namespace Nhanderu.Belizas.Exceptions
{
    public class TooMuchInformationInTruthTableException : OutOfMemoryException
    {
        public TooMuchInformationInTruthTableException(String message = null) : base("An error has occurred due to the size of the truth table.{0}" + message == null ? "" : " " + message) { }
    }
}
