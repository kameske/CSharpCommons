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
using System.ComponentModel;

namespace Commons
{
    /// <summary>
    /// DIコンテナクラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static partial class DIContainer
    {
        /*
         * 名前空間の代わりに定義した static partial クラス。
         * 名前空間に擬似的にメソッドやプロパティを持たせつつ、
         * 名前空間外に見せないクラスも定義したいためにこのような実装をとる。
         */

        /// <summary>コンテナ</summary>
        private static ContainerImpl Container { get; } = new ContainerImpl();

        /// <summary>コンテナ設定</summary>
        public static Config ContainerConfig { get; } = new Config();

        /// <summary>
        /// 実装クラスを登録する。登録済みの型でも上書き可能。
        /// </summary>
        /// <param name="createMethod">インスタンス生成メソッド</param>
        /// <param name="lifetime">ライフタイム</param>
        /// <param name="key">
        ///     登録コンテナ名<br/>
        ///     <c>null</c>の場合、<see cref="Config.DefaultKey"/>を使用する。
        /// </param>
        /// <typeparam name="T">登録型</typeparam>
        public static void Register<T>(Func<T> createMethod, Lifetime lifetime, string? key = null)
            where T : IInjectable<T>
            => Container.Register(createMethod, lifetime, key ?? ContainerConfig.DefaultKey);

        /// <summary>
        /// 実装クラスからインスタンスを生成して返却する。
        /// </summary>
        /// <param name="key">
        ///     取得コンテナ名<br/>
        ///     <c>null</c>の場合、<see cref="Config.DefaultKey"/>を使用する。
        /// </param>
        /// <typeparam name="T">インスタンス型</typeparam>
        /// <returns>インスタンス</returns>
        /// <exception cref="ContainerNotRegistrationException">登録されていない型を指定した場合</exception>
        public static T Resolve<T>(string? key = null) where T : IInjectable<T>
            => Container.Resolve<T>(key ?? ContainerConfig.DefaultKey);

        /// <summary>
        /// 指定したキー名のコンテナ内に指定したクラスの生成メソッドが登録されているかどうかを返す。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <typeparam name="T">チェック対象のクラス型</typeparam>
        /// <returns>生成メソッドが登録されている場合true</returns>
        /// <exception cref="ArgumentNullException">keyがnullの場合</exception>
        public static bool HasCreateMethod<T>(string key = "default")
            => Container.HasCreateMethod<T>(key);
    }
}