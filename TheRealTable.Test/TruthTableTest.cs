using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nhanderu.TheRealTable.TruthTableData;
using Nhanderu.TheRealTable.TruthTableData.Exceptions;

namespace Nhanderu.TheRealTable.Test
{
    [TestClass]
    public class TruthTableTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidFormulaException))]
        public void Formula_EmptyStringAsFormula_InvalidFormulaExceptionThrown()
        {
            TruthTable table = new TruthTable("");
            table.Formula = "";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFormulaException))]
        public void Formula_EmptyStringAsFormulaInConstructor_InvalidFormulaExceptionThrown()
        {
            TruthTable table = new TruthTable("", true);
        }
    }
}
