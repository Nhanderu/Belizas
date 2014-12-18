using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nhanderu.TheRealTable.TruthTableData;
using Nhanderu.TheRealTable.TruthTableData.Exceptions;

namespace Nhanderu.TheRealTable.Test
{
    [TestClass]
    public class TruthTableTest
    {
        TruthTable _exampleTable = new TruthTable("");

        #region Constructor
        [TestMethod]
        [ExpectedException(typeof(InvalidFormulaException))]
        public void Constructor_EmptyStringAsFormula_InvalidFormulaExceptionThrown()
        {
            TruthTable table = new TruthTable("", true);
        }
        #endregion

        #region Calculate
        [TestMethod]
        [ExpectedException(typeof(InvalidFormulaException))]
        public void Calculate_EmptyStringAsFormula_InvalidFormulaExceptionThrown()
        {
            TruthTable table = new TruthTable("");
            table.Calculate();
        }
        #endregion

        #region Formula
        [TestMethod]
        [ExpectedException(typeof(InvalidFormulaException))]
        public void Formula_EmptyStringAsFormula_InvalidFormulaExceptionThrown()
        {
            TruthTable table = new TruthTable("");
            table.Formula = "";
        }
        #endregion

        #region ValidateFormula
        [TestMethod]
        public void ValidateFormula_EmptyStringAsFormula_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula(""));
        }
        [TestMethod]
        public void ValidateFormula_FormulaWithUnclosedBrackets_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.(b.(c.(d.(e.f)"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithFollowingOperators_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.b+c>d<f--g"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaEndingWithOperator_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.b+c>d<f-"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithDisallowedCharacter_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.b+c>d<f?g"));
        }
        #endregion
    }
}
