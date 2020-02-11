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

namespace Commons
{
    /// <summary>
    /// TypeSafeEnum定義クラス.
    /// 列挙するアイテムは必ず静的コンストラクタ内で初期化すること。
    /// </summary>
    /// <typeparam name="T">定義クラス自身の型</typeparam>
    public abstract class TypeSafeEnum<T> where T : TypeSafeEnum<T>
    {
        /// <summary>列挙子管理</summary>
        private static readonly EnumItemsManager<T> EnumItems = new EnumItemsManager<T>();

        /// <summary>全ての要素</summary>
        protected static IEnumerable<T> AllItems => EnumItems.AllEnums;

        /// <summary>列挙子識別子</summary>
        public string Id { get; }

        #region Operator

        /// <summary>
        /// 列挙型のインスタンス同士を比較する。
        /// </summary>
        /// <remarks>
        /// 両者がnull、または<see cref="Id"/>が一致する場合trueを返す。
        /// </remarks>>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺が一致する場合true</returns>
        public static bool operator ==(TypeSafeEnum<T>? left, TypeSafeEnum<T>? right)
        {
            if (ReferenceEquals(left, right)) return true;
            // "left is null ^ right is null" の場合、 left.Id / left!.Id いずれも警告となるためあえて使用しない
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return false;
            return left!.Id == right!.Id;
        }

        /// <summary>
        /// 列挙型のインスタンス同士を比較する。
        /// </summary>
        /// <remarks>
        /// いずれか片方のみが<c>null</c>、または<see cref="Id"/>が一致しない場合<c>true</c>を返す。
        /// </remarks>>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺が一致しない場合<c>true</c></returns>
        public static bool operator !=(TypeSafeEnum<T>? left, TypeSafeEnum<T>? right)
        {
            if (ReferenceEquals(left, right)) return false;
            // "left is null ^ right is null" の場合、 left.Id / left!.Id いずれも警告となるためあえて使用しない
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return true;
            return left!.Id != right!.Id;
        }

        /// <summary>
        /// <see cref="TypeSafeEnum{T}"/>インスタンスと比較する。
        /// </summary>
        /// <paramref name="other"/>が<c>null</c>ではなく<see cref="Id"/>が一致する場合<c>true</c>を返す。
        /// <param name="other">比較対象</param>
        /// <returns>左辺と右辺が一致する場合true</returns>
        public bool Equals(TypeSafeEnum<T>? other)
        {
            if (other == null) return false;
            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeSafeEnum<T>) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        /// <summary>
        /// TypeSafeEnumからTに変換するメソッド.
        /// </summary>
        /// <returns>変換後のメソッド</returns>
        internal T ConvertToClass()
        {
            return (T) this;
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">識別子</param>
        protected TypeSafeEnum(string id)
        {
            Id = id;
            EnumItems.Add(id, this);
        }
    }
}