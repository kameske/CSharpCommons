using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;

namespace Logger.Test
{
    [TestFixture]
    public class LoggerTest
    {
        #region StaticMethods

        private static readonly object[] ChangeTargetKeyTestCaseSource =
        {
            new object[] {null, true},
            new object[] {"", true},
            new object[] {"abc", false},
            new object[] {"あいう", false},
            new object[] {"New\r\nLine", false},
        };

        [TestCaseSource(nameof(ChangeTargetKeyTestCaseSource))]
        public static void ChangeTargetKeyTest(string key, bool isError)
        {
            var errorOccured = false;
            try
            {
                Commons.Logger.ChangeTargetKey(key);
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

        private static readonly object[] GetInstanceTestCaseSource =
        {
            new object[] {null, false},
            new object[] {"", false},
            new object[] {"abc", false},
            new object[] {"あいう", false},
            new object[] {"New\r\nLine", false},
        };

        [TestCaseSource(nameof(GetInstanceTestCaseSource))]
        public static void GetInstanceTest(string key, bool isError)
        {
            Commons.Logger instance = null;

            var errorOccured = false;
            try
            {
                instance = Commons.Logger.GetInstance(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // インスタンスが取得できていること
            Assert.NotNull(instance);
        }

        private static readonly object[] SetLogHandlerTestCaseSource =
        {
            new object[] {null, null, true},
            new object[] {null, "", true},
            new object[] {new LogHandler(), "", false},
            new object[] {null, "abc", true},
            new object[] {new LogHandler(), "abc", false},
            new object[] {null, "あいう", true},
            new object[] {new LogHandler(), "あいう", false},
            new object[] {null, "New\r\nLine", true},
            new object[] {new LogHandler(), "New\r\nLine", false},
        };

        [TestCaseSource(nameof(SetLogHandlerTestCaseSource))]
        public static void SetLogHandlerTest(LogHandler handler,
            string key, bool isError)
        {
            var errorOccured = false;
            try
            {
                Commons.Logger.SetLogHandler(handler, key);
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

        #endregion

        #region ClassMethods

        private static readonly object[] ConstructorTestCaseSource =
        {
            new object[] {null, true},
            new object[] {new LogHandler(), false},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(LogHandler handler, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new Commons.Logger(handler);
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

        private static readonly object[] MsgTestCaseSource =
        {
            new object[] {null, false},
            new object[] {"", false},
            new object[] {"abc", false},
            new object[] {"あいう", false},
            new object[] {"New\r\nLine", false},
        };

        [TestCaseSource(nameof(MsgTestCaseSource))]
        public static void ErrorTest(string message, bool isError)
        {
            var msgList = new List<string>();
            var instance = new Commons.Logger(MakeLogHandlerForMethodTest(msgList));

            var errorOccured = false;
            try
            {
                instance.Error(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // メッセージが出力されていること
            Assert.AreEqual(msgList.Count, 1);
            Assert.IsTrue(msgList[0].Equals(MakeMsg(ActType.Error, message)));
        }

        [TestCaseSource(nameof(MsgTestCaseSource))]
        public static void WarningTest(string message, bool isError)
        {
            var msgList = new List<string>();
            var instance = new Commons.Logger(MakeLogHandlerForMethodTest(msgList));

            var errorOccured = false;
            try
            {
                instance.Warning(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // メッセージが出力されていること
            Assert.AreEqual(msgList.Count, 1);
            Assert.IsTrue(msgList[0].Equals(MakeMsg(ActType.Warning, message)));
        }

        [TestCaseSource(nameof(MsgTestCaseSource))]
        public static void InfoTest(string message, bool isError)
        {
            var msgList = new List<string>();
            var instance = new Commons.Logger(MakeLogHandlerForMethodTest(msgList));

            var errorOccured = false;
            try
            {
                instance.Info(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // メッセージが出力されていること
            Assert.AreEqual(msgList.Count, 1);
            Assert.IsTrue(msgList[0].Equals(MakeMsg(ActType.Info, message)));
        }

        [TestCaseSource(nameof(MsgTestCaseSource))]
        public static void DebugTest(string message, bool isError)
        {
            var msgList = new List<string>();
            var instance = new Commons.Logger(MakeLogHandlerForMethodTest(msgList));

            var errorOccured = false;
            try
            {
                instance.Debug(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // メッセージが出力されていること
            Assert.AreEqual(msgList.Count, 1);
            Assert.IsTrue(msgList[0].Equals(MakeMsg(ActType.Debug, message)));
        }


        private static readonly object[] ExceptionTestCaseSource =
        {
            new object[] {null, true},
            new object[] {new Exception("Error Message."), false},
        };

        [TestCaseSource(nameof(ExceptionTestCaseSource))]
        public static void ErrorTest(Exception exception, bool isError)
        {
            var msgList = new List<string>();
            var instance = new Commons.Logger(MakeLogHandlerForMethodTest(msgList));

            var errorOccured = false;
            try
            {
                instance.Exception(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // メッセージが出力されていること
            Assert.AreEqual(msgList.Count, 1);
            Assert.IsTrue(msgList[0].Equals(MakeExceptionMsg(exception)));
        }

        #endregion

        private static LogHandler MakeLogHandlerForMethodTest(ICollection<string> messages)
        {
            Action<string> MakeMsgAction(ActType actType) => msg => messages.Add(MakeMsg(actType, msg));
            Action<Exception> MakeExceptionAction() => ex => messages.Add(MakeExceptionMsg(ex));

            return new LogHandler(
                MakeMsgAction(ActType.Error),
                MakeMsgAction(ActType.Warning),
                MakeMsgAction(ActType.Info),
                MakeMsgAction(ActType.Debug),
                MakeExceptionAction()
            );
        }

        private static string MakeMsg(ActType type, string message)
            => $"{type}: ${message}";

        private static string MakeExceptionMsg(Exception ex)
            => $"{ActType.Exception}: {ex.Message}";

        private enum ActType
        {
            Error,
            Warning,
            Info,
            Debug,
            Exception
        }
    }
}