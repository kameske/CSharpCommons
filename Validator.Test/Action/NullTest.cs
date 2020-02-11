using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class NullTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateNullTestCaseSource =
        {
            new object[] {null, ValidateFailureException.FuncMake, false, true},
            new object[] {10, ValidateFailureException.FuncMake, false, false},
        };

        [TestCaseSource(nameof(ValidateNullTestCaseSource))]
        public static void ValidateNullTest(
            int? target,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateNull(
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


        private static readonly object[] NullTestCaseSource =
        {
            new object[] {null, null, false},
            new object[] {null, ValidateFailureException.FuncMake, false},
            new object[] {10, null, false},
            new object[] {10, ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(NullTestCaseSource))]
        public static void NullTest_(int? target,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.Null(target, funcMakeException);
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