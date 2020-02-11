using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;

namespace Validator.Test.Action
{
    [TestFixture]
    public class ItemNotNullTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ValidateItemNotNullTestCaseSource =
        {
            // Argument Check
            new object[] {null, null, true, false},
            new object[] {null, ValidateFailureException.FuncMake, true, false},
            // Validate
            new object[]
            {
                new ValidateTestInt[] {},
                ValidateFailureException.FuncMake, false, true
            },
            new object[]
            {
                new ValidateTestInt[] {0, null, 2},
                ValidateFailureException.FuncMake, false, false
            },
            new object[]
            {
                new ValidateTestInt[] {1, 2},
                ValidateFailureException.FuncMake, false, true
            },
        };

        [TestCaseSource(nameof(ValidateItemNotNullTestCaseSource))]
        public static void ValidateItemNotNullTest(
            IEnumerable<ValidateTestInt> target,
            Func<Exception> funcMakeException,
            bool isError, bool isValid)
        {
            var errorOccured = false;
            var validateSuccess = false;
            try
            {
                Commons.Validator.ValidateItemNotNull(
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


        private static readonly object[] ItemNotNullTestCaseSource =
        {
            new object[] {null, null, false},
            new object[] {null, ValidateFailureException.FuncMake, false},
            new object[] {new ValidateTestInt[] {}, null, false},
            new object[] {new ValidateTestInt[] {0, null, 2},
                ValidateFailureException.FuncMake, false},
            new object[] {new ValidateTestInt[] {0, 2}, null, false},
        };

        [TestCaseSource(nameof(ItemNotNullTestCaseSource))]
        public static void ItemNotNullTest_(IEnumerable<ValidateTestInt> target,
            Func<Exception> funcMakeException,
            bool isError)
        {
            System.Action act = null;

            var errorOccured = false;
            try
            {
                act = Commons.Validator.ItemNotNull(target, funcMakeException);
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