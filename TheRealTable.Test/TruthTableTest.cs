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
        public void ValidateFormula_FormulaIsEmpty_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula(""));
            Assert.AreEqual(false, _exampleTable.ValidateFormula("              "));
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
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.a+a:a>a<a-a'.a+a:a>a<a-a'.a+a:a>>a<a-a'"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaStartingAndEndingWithOperator_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.b+c>d<f-"));
            Assert.AreEqual(false, _exampleTable.ValidateFormula(".b+c>d<f-a"));
            Assert.AreEqual(false, _exampleTable.ValidateFormula(".b+c>d<f-"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithDisallowedCharacter_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("a.b+c>d<f?g"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithBracketsOnly_InvalidFormula()
        {
            Assert.AreEqual(false, _exampleTable.ValidateFormula("()"));
            Assert.AreEqual(false, _exampleTable.ValidateFormula("((((()))))"));
            Assert.AreEqual(false, _exampleTable.ValidateFormula("((((()()()()))))"));
        }

        [TestMethod]
        public void ValidateFormula_SimpleFormula_ValidFormula()
        {
            Assert.AreEqual(true, _exampleTable.ValidateFormula("a.b"));
        }

        [TestMethod]
        public void ValidateFormula_ComplexFormula_ValidFormula()
        {
            Assert.AreEqual(true, _exampleTable.ValidateFormula("a+(b.c>(a'+b')'<b':((c-c')-a))'"));
        }
        #endregion
    }
}
