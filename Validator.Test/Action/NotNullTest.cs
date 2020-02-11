using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class NotNullTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateNotNullTestCaseSource =
        {
            new object[] {null, ValidateFailureException.FuncMake, false, false},
            new object[] {10, ValidateFailureException.FuncMake, false, true},
        };

        [TestCaseSource(nameof(ValidateNotNullTestCaseSource))]
        public static void ValidateNotNullTest(
            int? target,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateNotNull(
                    target, funcMakeException);
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


        private static readonly object[] NotNullTestCaseSource =
        {
            new object[] {null, null, false},
            new object[] {null, ValidateFailureException.FuncMake, false},
            new object[] {10, null, false},
            new object[] {10, ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(NotNullTestCaseSource))]
        public static void NotNullTest_(int? target,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.NotNull(target, funcMakeException);
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