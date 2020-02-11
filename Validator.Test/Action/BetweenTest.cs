using System;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class BetweenTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateBetweenTestCaseSource =
        {
            // Argument Check
            new object[] {null, null, null, null, true, false},
            new object[] {(ValidateTestInt) 10, null, (ValidateTestInt) 20, null, true, false},
            new object[] {null, (ValidateTestInt) 10, null, ValidateFailureException.FuncMake, true, false},
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 10, (ValidateTestInt) 9,
                ValidateFailureException.FuncMake, true, false
            },
            // Boundary Value
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 9, (ValidateTestInt) 9,
                ValidateFailureException.FuncMake, false, false
            },
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 9, (ValidateTestInt) 10,
                ValidateFailureException.FuncMake, false, true
            },
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 9, (ValidateTestInt) 11,
                ValidateFailureException.FuncMake, false, true
            },
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 10, (ValidateTestInt) 10,
                ValidateFailureException.FuncMake, false, true
            },
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 10, (ValidateTestInt) 11,
                ValidateFailureException.FuncMake, false, true
            },
            new object[]
            {
                (ValidateTestInt) 10, (ValidateTestInt) 11, (ValidateTestInt) 11,
                ValidateFailureException.FuncMake, false, false
            },
        };

        [TestCaseSource(nameof(ValidateBetweenTestCaseSource))]
        public static void ValidateBetweenTest(
            ValidateTestInt target, ValidateTestInt from, ValidateTestInt to,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateBetween(
                    target, from, to, funcMakeException);
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

        private static readonly object[] BetweenTestCaseSource =
        {
            // Argument Check
            new object[] {null, null, null, null, false},
            new object[] {(ValidateTestInt)10, null, (ValidateTestInt)20, null, false},
            new object[] {null, (ValidateTestInt)10, null,
                ValidateFailureException.FuncMake, false},
            new object[] {(ValidateTestInt)10, (ValidateTestInt)10,
                (ValidateTestInt)9, ValidateFailureException.FuncMake, false},
        };

        [TestCaseSource(nameof(BetweenTestCaseSource))]
        public static void BetweenTest_(
            ValidateTestInt target, ValidateTestInt from, ValidateTestInt to,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.Between(target, from, to, funcMakeException);
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