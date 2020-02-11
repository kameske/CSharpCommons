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
using System.Collections.Generic;

namespace Commons.Linq.Extension
{
    public static partial class LinqExtension
    {
        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行し、
        /// 元の値を後続の処理に流す。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。
        /// この関数の 2 つ目のパラメーターは、ソース要素のインデックスを表す。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <returns><paramref name="source"/>の各要素</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>または<paramref name="action"/>がnullの場合
        /// </exception>
        public static IEnumerable<TSource> Do<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var index = -1;
            foreach (var element in source)
            {
                checked
                {
                    index++;
                }

                action(element, index);

                yield return element;
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行し、
        /// 元の値を後続の処理に流す。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <returns><paramref name="source"/>の各要素</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>または<paramref name="action"/>がnullの場合
        /// </exception>
        public static IEnumerable<TSource> Do<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            return source.Do((elem, _) => action(elem));
        }
    }
}