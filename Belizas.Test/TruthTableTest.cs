using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Nhanderu.Belizas.Exceptions;

namespace Nhanderu.Belizas.Test
{
    [TestClass]
    public class TruthTableTest
    {
        TruthTable _table = new TruthTable("");

        #region Properties
        [TestMethod]
        [ExpectedException(typeof(TableNotCalculatedException))]
        public void Arguments_NotCalculated_TableNotCalculatedExceptionThrown()
        {
            TruthTable table = new TruthTable("a.b");
            Int32 hellhound = table.Arguments.Count;
        }

        [TestMethod]
        [ExpectedException(typeof(TableNotCalculatedException))]
        public void ArgumentsValues_NotCalculated_TableNotCalculatedExceptionThrown()
        {
            TruthTable table = new TruthTable("a.b");
            Int32 on = table.ArgumentsValues.Length;
        }

        [TestMethod]
        [ExpectedException(typeof(TableNotCalculatedException))]
        public void Expressions_NotCalculated_TableNotCalculatedExceptionThrown()
        {
            TruthTable table = new TruthTable("a.b");
            Int32 my = table.Expressions.Count;
        }

        [TestMethod]
        [ExpectedException(typeof(TableNotCalculatedException))]
        public void ExpressionsValues_NotCalculated_TableNotCalculatedExceptionThrown()
        {
            TruthTable table = new TruthTable("a.b");
            Int32 trail = table.ExpressionsValues.Count;
        }
        #endregion

        #region Calculate
        [TestMethod]
        [ExpectedException(typeof(InvalidFormulaException))]
        public void Calculate_EmptyStringAsFormula_InvalidFormulaExceptionThrown()
        {
            _table.Calculate();
        }

        [TestMethod]
        public void Calculate_LowerAndUpperCaseArguments_ShouldCountAsDifferentArguments()
        {
            _table.Formula = "a.A.b";

            _table.Calculate();
            Assert.AreEqual(3, _table.Arguments.Count);

            _table.Formula = "";
        }
        #endregion

        #region ValidateFormula
        // Invalid.
        [TestMethod]
        public void ValidateFormula_EmptyFormula_InvalidFormula()
        {
            Assert.AreEqual(false, _table.ValidateFormula(""));
            Assert.AreEqual(false, _table.ValidateFormula("              "));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithUnclosedBrackets_InvalidFormula()
        {
            Assert.AreEqual(false, _table.ValidateFormula("a.(b.(c.(d.(e.f)"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithFollowingOperators_InvalidFormula()
        {
            Assert.AreEqual(false, _table.ValidateFormula("a.b+c>d<f--g"));
            Assert.AreEqual(false, _table.ValidateFormula("a.a+a:a>a<a-a'.a+a:a>a<a-a'.a+a:a>>a<a-a'"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaStartingAndEndingWithOperator_InvalidFormula()
        {
            Assert.AreEqual(false, _table.ValidateFormula("a.b+c>d<f-"));
            Assert.AreEqual(false, _table.ValidateFormula(".b+c>d<f-a"));
            Assert.AreEqual(false, _table.ValidateFormula(".b+c>d<f-"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithDisallowedCharacter_InvalidFormula()
        {
            Assert.AreEqual(false, _table.ValidateFormula("a.b+c>d<f?g"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithBracketsOnly_InvalidFormula()
        {
            Assert.AreEqual(false, _table.ValidateFormula("()"));
            Assert.AreEqual(false, _table.ValidateFormula("((((()))))"));
            Assert.AreEqual(false, _table.ValidateFormula("((((()()()()))))"));
        }

        // Valid.
        [TestMethod]
        public void ValidateFormula_JustAnArgument_ValidFormula()
        {
            Assert.AreEqual(true, _table.ValidateFormula("d"));
        }

        [TestMethod]
<<<<<<< HEAD
        public void ValidateFormula_FormulaWithJustOneArgument_ValidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(true, table.ValidateFormula("d"));
        }

        [TestMethod]
        public void ValidateFormula_SimpleFormula_ValidFormula()
=======
        public void ValidateFormula_JustANegatedArgument_ValidFormula()
>>>>>>> origin/test
        {
            Assert.AreEqual(true, _table.ValidateFormula("d'"));
        }

        [TestMethod]
        public void ValidateFormula_SimpleFormula_ValidFormula()
        {
            Assert.AreEqual(true, _table.ValidateFormula("a.b"));
            Assert.AreEqual(true, _table.ValidateFormula("a+b"));
            Assert.AreEqual(true, _table.ValidateFormula("a:b"));
            Assert.AreEqual(true, _table.ValidateFormula("a>b"));
            Assert.AreEqual(true, _table.ValidateFormula("a<b"));
            Assert.AreEqual(true, _table.ValidateFormula("a-b"));
        }

        [TestMethod]
        public void ValidateFormula_ComplexFormula_ValidFormula()
        {
            Assert.AreEqual(true, _table.ValidateFormula("a+(b.c>(a'+b')'<b':((c-c')-a))'"));
        }
        #endregion
    }
}
