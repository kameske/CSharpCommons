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
    public static partial class Validator
    {
        /// <summary>
        /// 引数が<c>null</c>でも空文字でもないことの検証処理を行う。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="funcMakeNullException">
        ///     <paramref name="target"/>が<c>null</c>の場合に発生させる例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="NullReferenceException"/>を使用する。
        /// </param>
        /// <param name="funcMakeEmptyException">
        ///     <paramref name="target"/>が空文字の場合に発生させる例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="Exception"/>を使用する。
        /// </param>
        /// <exception cref="Exception">検証エラー時</exception>
        public static void ValidateNotNullOrEmpty(string? target,
            Func<Exception>? funcMakeNullException, Func<Exception>? funcMakeEmptyException)
        {
            ValidateNotNull(target, funcMakeNullException);

            if (!target!.Equals(string.Empty)) return;

            if (funcMakeEmptyException != null) throw funcMakeEmptyException();
            throw new Exception();
        }

        /// <summary>
        /// <see cref="ValidateNotNullOrEmpty"/>を実行する<see cref="Action"/>を返す。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="funcMakeNullException">
        ///     <paramref name="target"/>が<c>null</c>の場合に発生させる例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="NullReferenceException"/>を使用する。
        /// </param>
        /// <param name="funcMakeEmptyException">
        ///     <paramref name="target"/>が空文字の場合に発生させる例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="Exception"/>を使用する。
        /// </param>
        /// <returns><see cref="ValidateNotNullOrEmpty"/>実行<see cref="Action"/></returns>
        public static Action NotNullOrEmpty(string? target,
            Func<Exception>? funcMakeNullException, Func<Exception>? funcMakeEmptyException)
            => () => ValidateNotNullOrEmpty(target, funcMakeNullException, funcMakeEmptyException);
    }
}