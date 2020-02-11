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

using System.Collections.Generic;
using System.Linq;

namespace Commons
{
    /// <summary>
    /// 列挙子管理クラス
    /// </summary>
    internal class EnumItemsManager<T> where T : TypeSafeEnum<T>
    {
        private Dictionary<string, TypeSafeEnum<T>> ItemDic { get; }

        /// <summary>
        /// 列挙アイテムの全リスト
        /// </summary>
        public IEnumerable<T> AllEnums => ItemDic.Values.Select(item => item.ConvertToClass());

        public EnumItemsManager()
        {
            ItemDic = new Dictionary<string, TypeSafeEnum<T>>();
        }

        /// <summary>
        /// アイテムを追加する。
        /// </summary>
        /// <param name="id">識別文字列</param>
        /// <param name="item">格納するインスタンス</param>
        /// <exception cref="DuplicateEnumException">IDが重複した場合</exception>
        public void Add(string id, TypeSafeEnum<T> item)
        {
            if (ItemDic.ContainsKey(id)) throw new DuplicateEnumException();
            ItemDic.Add(id, item);
        }
    }
}