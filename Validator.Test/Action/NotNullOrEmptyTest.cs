using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class NotNullOrEmptyTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateNotNullOrEmptyTestCaseSource =
        {
            new object[]
            {
                null, ValidateFailureException.FuncMake,
                ValidateFailureException.FuncMake, false, false
            },
            new object[]
            {
                "", ValidateFailureException.FuncMake,
                ValidateFailureException.FuncMake, false, false
            },
            new object[]
            {
                "abc", ValidateFailureException.FuncMake,
                ValidateFailureException.FuncMake, false, true
            },
        };

        [TestCaseSource(nameof(ValidateNotNullOrEmptyTestCaseSource))]
        public static void ValidateNotNullOrEmptyTest(
            string target,
            Func<Exception> funcMakeNullException,
            Func<Exception> funcMakeEmptyException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateNotNullOrEmpty(
                    target, funcMakeNullException, funcMakeEmptyException);
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


        private static readonly object[] NotNullOrEmptyTestCaseSource =
        {
            new object[] {null, null, null, false },
            new object[] {null, ValidateFailureException.FuncMake, null, false},
            new object[] {"a", null, ValidateFailureException.FuncMake, false},
            new object[] {"a", ValidateFailureException.FuncMake,
                ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(NotNullOrEmptyTestCaseSource))]
        public static void NotNullOrEmptyTest_(string target,
            Func<Exception> funcMakeNullException,
            Func<Exception> funcMakeEmptyException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.NotNullOrEmpty(target,
                    funcMakeNullException, funcMakeEmptyException);
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