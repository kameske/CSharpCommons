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
using System.Diagnostics.CodeAnalysis;

namespace Commons
{
    public static partial class Validator
    {
        /// <summary>
        /// 同値検証処理を行う。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="other">比較対象</param>
        /// <param name="funcMakeException">
        ///     検証エラー時の例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="Exception"/>を使用する。
        /// </param>
        /// <exception cref="Exception">
        ///     <paramref name="target"/>が<paramref name="other"/>と異なる場合
        /// </exception>
        public static void ValidateEqual<T>(IEquatable<T>? target,
            [AllowNull] T other,
            Func<Exception>? funcMakeException)
        {
            var exception = funcMakeException != null ? funcMakeException() : new Exception();

            if (ReferenceEquals(target, other)) return;

            if (ReferenceEquals(target, null) ^ ReferenceEquals(other, null)) throw exception;

            if (target!.Equals(other)) return;

            if (funcMakeException != null) throw funcMakeException();
            throw new Exception();
        }

        /// <summary>
        /// <see cref="ValidateEqual{T}"/>を実行する<see cref="Action"/>を返す。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="other">比較対象</param>
        /// <param name="funcMakeException">
        ///     検証エラー時の例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="NullReferenceException"/>を使用する。
        /// </param>
        /// <returns><see cref="ValidateEqual{T}"/>実行<see cref="Action"/></returns>
        public static Action Equal<T>(IEquatable<T>? target,
            [AllowNull] T other,
            Func<Exception>? funcMakeException)
            => () => ValidateEqual(target, other, funcMakeException);
    }
}