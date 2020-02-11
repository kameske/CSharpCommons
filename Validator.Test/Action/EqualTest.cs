using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class EqualTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateEqualTestCaseSource =
        {
            new object[] {null, null, null, false, true},
            new object[] {(ValidateTestInt) 10, null, ValidateFailureException.FuncMake, false, false},
            new object[] {null, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false, false},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 9, ValidateFailureException.FuncMake, false, false},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false, true},
        };

        [TestCaseSource(nameof(ValidateEqualTestCaseSource))]
        public static void ValidateEqualTest(
            ValidateTestInt target, ValidateTestInt other,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateEqual(
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

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {null, null, null, false},
            new object[] {(ValidateTestInt) 10, null, ValidateFailureException.FuncMake, false},
            new object[] {null, (ValidateTestInt) 10, null, false},
            new object[] {(ValidateTestInt) 10, (ValidateTestInt) 10, ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void EqualTest_(
            ValidateTestInt target, ValidateTestInt other,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.Equal(target, other, funcMakeException);
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