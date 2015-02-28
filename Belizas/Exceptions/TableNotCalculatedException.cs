using System;

namespace Nhanderu.Belizas.Exceptions
{
    public class TableNotCalculatedException : Exception
    {
        public TableNotCalculatedException() : base("An error ocurred while trying to access a truth table property before it was calculated.") { }
    }
}
