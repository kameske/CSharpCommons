using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class NotEqualTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateNotEqualTestCaseSource =
        {
            new object[] {null, null, null, false, false},
            new object[] {(ValidateTestInt) 10, null, ValidateFailureException.FuncMake, false, true},
            new object[] {null, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false, true},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 9, ValidateFailureException.FuncMake, false, true},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false, false},
        };

        [TestCaseSource(nameof(ValidateNotEqualTestCaseSource))]
        public static void ValidateNotEqualTest(
            ValidateTestInt target, ValidateTestInt other,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateNotEqual(
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
                if (funcMakeException != null)
                {
                    errorOccured = true;
                }
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 検証結果が一致すること
            Assert.AreEqual(isValid, validateSuccess);
        }

        private static readonly object[] NotEqualTestCaseSource =
        {
            new object[] {null, null, null, false},
            new object[] {(ValidateTestInt) 10, null, ValidateFailureException.FuncMake, false},
            new object[] {null, (ValidateTestInt) 10, null, false},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(NotEqualTestCaseSource))]
        public static void NotEqualTest_(
            ValidateTestInt target, ValidateTestInt other,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.NotEqual(target, other, funcMakeException);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 関数が取得できること
            Assert.NotNull(act);
        }
    }
}