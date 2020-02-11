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
    /// ログ出力処理ハンドラクラス。
    /// </summary>
    public class LogHandler
    {
        /// <summary>デフォルトハンドラ</summary>
        public static readonly LogHandler Default;


        /// <summary>エラーメッセージ処理</summary>
        private Action<string?>? ErrorAction { get; }

        /// <summary>警告メッセージ処理</summary>
        private Action<string?>? WarningAction { get; }

        /// <summary>情報メッセージ処理</summary>
        private Action<string?>? InfoAction { get; }

        /// <summary>デバッグメッセージ処理</summary>
        private Action<string?>? DebugAction { get; }

        private Action<Exception>? ExceptionAction { get; }


        static LogHandler()
        {
            Default = new LogHandler(
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine
            );
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorAction">エラーメッセージ処理</param>
        /// <param name="warningAction">警告メッセージ処理</param>
        /// <param name="infoAction">情報メッセージ処理</param>
        /// <param name="debugAction">デバッグメッセージ処理</param>
        /// <param name="exceptionAction">例外メッセージ処理</param>
        public LogHandler(Action<string?>? errorAction = null, Action<string?>? warningAction = null,
            Action<string?>? infoAction = null, Action<string?>? debugAction = null,
            Action<Exception>? exceptionAction = null)
        {
            ErrorAction = errorAction;
            WarningAction = warningAction;
            InfoAction = infoAction;
            DebugAction = debugAction;
            ExceptionAction = exceptionAction;
        }


        /// <summary>
        /// エラーメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoError(string? message)
            => ErrorAction?.Invoke(message);

        /// <summary>
        /// 警告メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoWarning(string? message)
            => WarningAction?.Invoke(message);

        /// <summary>
        /// 情報メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoInfo(string? message)
            => InfoAction?.Invoke(message);

        /// <summary>
        /// デバッグメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoDebug(string? message)
            => DebugAction?.Invoke(message);

        /// <summary>
        /// 例外メッセージを処理する。
        /// </summary>
        /// <param name="exception">例外</param>
        internal void DoException(Exception exception)
            => ExceptionAction?.Invoke(exception);
    }
}