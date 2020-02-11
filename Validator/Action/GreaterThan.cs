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
        /// 引数 "大なり" 検証処理を行う。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="limit">下限</param>
        /// <param name="funcMakeException">
        ///     検証エラー時の例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="Exception"/>を使用する。
        /// </param>
        /// <exception cref="Exception">
        /// <paramref name="target"/>が<paramref name="limit"/>以下の場合
        /// </exception>
        public static void ValidateGreaterThan<T>(IComparable<T> target, T limit,
            Func<Exception>? funcMakeException)
        {
            ValidateAndAlso(
                NotNull(target, () => new ArgumentNullException(nameof(target))),
                NotNull(limit, () => new ArgumentNullException(nameof(limit)))
            );

            if (target.CompareTo(limit) > 0) return;

            if (funcMakeException != null) throw funcMakeException();
            throw new Exception();
        }

        /// <summary>
        /// <see cref="ValidateGreaterThan{T}"/>を実行する<see cref="Action"/>を返す。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="limit">下限</param>
        /// <param name="funcMakeException">
        ///     検証エラー時の例外生成関数<br/>
        ///     未指定の場合引数無しで生成した<see cref="NullReferenceException"/>を使用する。
        /// </param>
        /// <returns><see cref="ValidateGreaterThan{T}"/>実行<see cref="Action"/></returns>
        public static Action GreaterThan<T>(IComparable<T> target, T limit,
            Func<Exception>? funcMakeException)
            => () => ValidateGreaterThan(target, limit, funcMakeException);
    }
}