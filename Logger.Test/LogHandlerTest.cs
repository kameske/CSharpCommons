using System;
using Commons;
using NUnit.Framework;

namespace Logger.Test
{
    [TestFixture]
    public class LogHandlerTest
    {
        public static void Constructor1Test()
        {
            var _ = new LogHandler();
            Assert.Pass();
        }

        private static readonly object[] Constructor2TestCaseSource =
        {
            new object[]
            {
                (Action<string>) MsgAction, (Action<string>) MsgAction,
                (Action<string>) MsgAction, (Action<string>) MsgAction,
                (Action<Exception>) ExceptionAction, false
            },
            new object[]
            {
                (Action<string>) MsgAction, null,
                (Action<string>) MsgAction, null,
                (Action<Exception>) ExceptionAction, false
            },
            new object[]
            {
                null, (Action<string>) MsgAction,
                null, (Action<string>) MsgAction,
                null, false
            },
            new object[]
            {
                null, null, null, null, null, false
            },
        };

        [TestCaseSource(nameof(Constructor2TestCaseSource))]
        public static void Constructor2Test(
            Action<string> errorAction,
            Action<string> warningAction,
            Action<string> infoAction,
            Action<string> debugAction,
            Action<Exception> exceptionAction,
            bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new LogHandler(
                    errorAction,
                    warningAction,
                    infoAction,
                    debugAction,
                    exceptionAction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static void MsgAction(string msg)
        {
            // 定義だけで何もしない
        }

        private static void ExceptionAction(Exception ex)
        {
            // 定義だけで何もしない
        }
    }
}