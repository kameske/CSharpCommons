using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class GreaterThanTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateGreaterThanTestCaseSource =
        {
            // Argument Check
            new object[] {null, null, null, true, false},
            new object[] {(ValidateTestInt) 10, null, ValidateFailureException.FuncMake, true, false},
            new object[] {null, (ValidateTestInt) 10, null, true, false},
            // Boundary Value
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 9, ValidateFailureException.FuncMake, false, true},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false, false},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 11, ValidateFailureException.FuncMake, false, false},
        };

        [TestCaseSource(nameof(ValidateGreaterThanTestCaseSource))]
        public static void ValidateGreaterThanTest(
            ValidateTestInt target, ValidateTestInt other,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateGreaterThan(
                    target, other, funcMakeException);
                validateSuccess = true;
            }
            catch (ValidateFailureException ex)
            {
                logger.Exception(ex);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 検証結果が一致すること
            Assert.AreEqual(isValid, validateSuccess);
        }

        private static readonly object[] GreaterThanTestCaseSource =
        {
            new object[] {null, null, null, false},
            new object[] {(ValidateTestInt) 20, null, ValidateFailureException.FuncMake, false},
            new object[] {null, (ValidateTestInt) 15, null, false},
            new object[] {(ValidateTestInt) 20, (ValidateTestInt) 30, ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(GreaterThanTestCaseSource))]
        public static void GreaterThanTest_(
            ValidateTestInt target, ValidateTestInt other,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.GreaterThan(
                    target, other, funcMakeException);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 関数が取得できること
            Assert.NotNull(act);
        }
    }
}