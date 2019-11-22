using System;
using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expressions.Task3.E3SQueryProvider.Test
{
    [TestClass]
    public class FTSRequestTranslatorTests
    {
        #region SubTask 1 : operands order

        [TestMethod]
        public void TestBinaryBackOrder()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => "EPRUIZHW006" == employee.Workstation;

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(EPRUIZHW006)", translated);
        }

        #endregion

        #region SubTask 2: inclusion operations

        [TestMethod]
        public void TestBinaryEqualsQueryable()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation == "EPRUIZHW006");

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(EPRUIZHW006)", translated);
        }

        [TestMethod]
        public void TestBinaryEquals()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation == "EPRUIZHW006";

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(EPRUIZHW006)", translated);
        }

        [TestMethod]
        public void TestMethodEquals()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.Equals("EPRUIZHW006");

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(EPRUIZHW006)", translated);
        }

        [TestMethod]
        public void TestStartsWith()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.StartsWith("EPRUIZHW006");
            
            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(EPRUIZHW006*)", translated);
        }

        [TestMethod]
        public void TestEndsWith()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.EndsWith("IZHW0060");

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(*IZHW0060)", translated);
        }

        [TestMethod]
        public void TestContains()
        {
            var translator = new ExpressionToFTSRequestTranslator();
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.EndsWith("IZHW006");

            string translated = translator.Translate(expression);
            Assert.AreEqual("Workstation:(*IZHW006*)", translated);
        }

        #endregion
    }
}
