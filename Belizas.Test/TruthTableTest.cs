using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nhanderu.Belizas;
using Nhanderu.Belizas.Exceptions;

namespace Nhanderu.Belizas.Test
{
    [TestClass]
    public class TruthTableTest
    {
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

        [TestMethod]
        public void Calculate_LowerAndUpperCaseArguments_ShouldCountAsDifferentArguments()
        {
            TruthTable table = new TruthTable("a.A.b");

            table.Calculate();
            Assert.AreEqual(3, table.Arguments.Count);
        }
        #endregion

        #region ValidateFormula
        [TestMethod]
        public void ValidateFormula_FormulaIsEmpty_InvalidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(false, table.ValidateFormula(""));
            Assert.AreEqual(false, table.ValidateFormula("              "));
        }
        [TestMethod]
        public void ValidateFormula_FormulaWithUnclosedBrackets_InvalidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(false, table.ValidateFormula("a.(b.(c.(d.(e.f)"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithFollowingOperators_InvalidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(false, table.ValidateFormula("a.b+c>d<f--g"));
            Assert.AreEqual(false, table.ValidateFormula("a.a+a:a>a<a-a'.a+a:a>a<a-a'.a+a:a>>a<a-a'"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaStartingAndEndingWithOperator_InvalidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(false, table.ValidateFormula("a.b+c>d<f-"));
            Assert.AreEqual(false, table.ValidateFormula(".b+c>d<f-a"));
            Assert.AreEqual(false, table.ValidateFormula(".b+c>d<f-"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithDisallowedCharacter_InvalidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(false, table.ValidateFormula("a.b+c>d<f?g"));
        }

        [TestMethod]
        public void ValidateFormula_FormulaWithBracketsOnly_InvalidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(false, table.ValidateFormula("()"));
            Assert.AreEqual(false, table.ValidateFormula("((((()))))"));
            Assert.AreEqual(false, table.ValidateFormula("((((()()()()))))"));
        }

        [TestMethod]
        public void ValidateFormula_SimpleFormula_ValidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(true, table.ValidateFormula("a.b"));
        }

        [TestMethod]
        public void ValidateFormula_ComplexFormula_ValidFormula()
        {
            TruthTable table = new TruthTable("");

            Assert.AreEqual(true, table.ValidateFormula("a+(b.c>(a'+b')'<b':((c-c')-a))'"));
        }
        #endregion
    }
}
