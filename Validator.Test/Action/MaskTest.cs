using System;
using System.Text.RegularExpressions;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class MaskTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateMaskTestCaseSource =
        {
            // Argument Check
            new object[] {null, null, null, true, false},
            new object[] {"", null, ValidateFailureException.FuncMake, true, false},
            new object[] {null, new Regex("."), null, true, false},
            // Validate
            new object[]
            {
                "abc", new Regex("^abc$"),
                ValidateFailureException.FuncMake, false, true
            },
            new object[]
            {
                "abc", new Regex("^aaa$"),
                ValidateFailureException.FuncMake, false, false
            },
        };

        [TestCaseSource(nameof(ValidateMaskTestCaseSource))]
        public static void ValidateMaskTest(
            string target, Regex regex,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateMask(
                    target, regex, funcMakeException);
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


        private static readonly object[] MaskTestCaseSource =
        {
            new object[] {null, null, null, false},
            new object[] {"", null, ValidateFailureException.FuncMake, false},
            new object[] {null, new Regex("."), null, false},
            new object[]
            {
                "abc", new Regex("^abc$"),
                ValidateFailureException.FuncMake, false
            },
        };

        [TestCaseSource(nameof(MaskTestCaseSource))]
        public static void MaskTest_(
            string target, Regex regex,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.Mask(target, regex, funcMakeException);
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