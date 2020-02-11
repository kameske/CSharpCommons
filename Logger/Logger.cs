/* ================================================================================
 * This is free and unencumbered software released into the public domain.
 *
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <https://unlicense.org>
 * ================================================================================
 */

using System;

namespace Commons
{
    /// <summary>
    /// ログに関する処理を扱うクラス。
    /// </summary>
    public class Logger : DIContainer.IInjectable<Logger>
    {
        /// <summary>
        /// デフォルト設定キー名
        /// </summary>
        private static string DefaultKeyName => "default";


        /// <summary>
        /// 現在の設定キー名
        /// </summary>
        public static string TargetKeyName { get; private set; } = "";

        /// <summary>
        /// ログハンドラ
        /// </summary>
        public LogHandler LogHandler { get; private set; }


        /// <summary>
        /// メインで使用する設定キーを変更する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="keyName"/>が<c>null</c>の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="keyName"/>が空文字の場合
        /// </exception>
        public static void ChangeTargetKey(string keyName)
        {
            Validator.ValidateNotNullOrEmpty(keyName,
                () => new ArgumentNullException(nameof(keyName)),
                () => new ArgumentException("Cannot Empty", nameof(keyName)));

            TargetKeyName = keyName;

            RegisterInstanceIfNeeded(keyName);
        }

        /// <summary>
        /// 設定キー名からインスタンスを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定キー名<br/>
        ///     <c>null</c>の場合、<see cref="TargetKeyName"/>が設定される。
        /// </param>
        /// <returns>設定インスタンス。</returns>
        public static Logger GetInstance(string? keyName = null)
        {
            var innerKeyName = keyName ?? TargetKeyName;
            RegisterInstanceIfNeeded(innerKeyName!);
            return DIContainer.Resolve<Logger>(innerKeyName!);
        }

        /// <summary>
        /// ログハンドラを設定する。
        /// </summary>
        /// <param name="logHandler">ログ出力ハンドラ</param>
        /// <param name="keyName">
        ///     設定キー名<br/>
        ///     <c>null</c>の場合、<see cref="TargetKeyName"/>が設定される。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///    <paramref name="logHandler"/>が<c>null</c>の場合
        /// </exception>
        public static void SetLogHandler(LogHandler logHandler, string? keyName = null)
        {
            Validator.ValidateNotNull(logHandler,
                () => new ArgumentNullException(nameof(logHandler)));

            var innerKeyName = keyName ?? TargetKeyName;
            var instance = GetInstance(innerKeyName!);
            instance.LogHandler = logHandler;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定した設定キー名の設定インスタンスがコンテナに登録されていなければ登録する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        private static void RegisterInstanceIfNeeded(string keyName)
        {
            if (!DIContainer.HasCreateMethod<Logger>(keyName))
            {
                DIContainer.Register(() => new Logger(LogHandler.Default),
                    DIContainer.Lifetime.Container, keyName);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static Logger()
        {
            ChangeTargetKey(DefaultKeyName);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logHandler">ログハンドラ</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="logHandler"/>が<c>null</c>の場合
        /// </exception>
        public Logger(LogHandler logHandler)
        {
            Validator.ValidateNotNull(logHandler,
                () => new ArgumentNullException(nameof(logHandler)));

            LogHandler = logHandler;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// エラーメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Error(string? message)
        {
            LogHandler.DoError(message);
        }

        /// <summary>
        /// 警告メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Warning(string? message)
        {
            LogHandler.DoWarning(message);
        }

        /// <summary>
        /// 情報メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Info(string? message)
        {
            LogHandler.DoInfo(message);
        }

        /// <summary>
        /// デバッグメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Debug(string? message)
        {
            LogHandler.DoDebug(message);
        }

        /// <summary>
        /// 例外メッセージを処理する。
        /// </summary>
        /// <param name="exception">例外</param>
        public void Exception(Exception exception)
        {
            LogHandler.DoException(exception);
        }
    }
}