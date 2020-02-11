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
using System.Linq;

namespace Commons
{
    public static partial class DIContainer
    {
        /// <summary>
        /// コンテナ実装クラス
        /// </summary>
        private partial class ContainerImpl
        {
            private Dictionary<string, Dictionary<Type, CreateInfo>> ContainerDic { get; }

            internal ContainerImpl()
            {
                ContainerDic = new Dictionary<string, Dictionary<Type, CreateInfo>>();
            }

            /// <summary>
            /// 実装クラスを登録する。登録済みの型でも上書き可能。
            /// </summary>
            /// <param name="createMethod">インスタンス生成メソッド</param>
            /// <param name="lifetime">ライフタイム</param>
            /// <param name="key">
            ///     登録コンテナ名<br/>
            ///     <c>null</c>の場合、<see cref="DIContainer.ContainerConfig"/>の<see cref="Config.DefaultKey"/>を使用する。
            /// </param>
            /// <typeparam name="T">登録型</typeparam>
            public void Register<T>(Func<T> createMethod, Lifetime lifetime,
                string? key)
                where T : IInjectable<T>
            {
                Validator.ValidateAndAlso(
                    Validator.NotNull(createMethod, () => new ArgumentNullException(nameof(createMethod))),
                    Validator.NotNull(lifetime, () => new ArgumentNullException(nameof(lifetime)))
                );
                // 登録情報作成
                var createObjMethod = new Func<object>(() => (object) createMethod());
                var createInfo = new CreateInfo(createObjMethod, lifetime);
                var keyName = key ?? ContainerConfig.DefaultKey;

                // 登録先コンテナ取得
                var containerKv = ContainerDic.FirstOrDefault(kv => kv.Key.Equals(keyName));
                if (IsNull(containerKv))
                {
                    // コンテナが存在しないので、先にコンテナを作ってから
                    var newContainer = new Dictionary<Type, CreateInfo>
                    {
                        {typeof(T), createInfo}
                    };
                    ContainerDic.Add(keyName, newContainer);
                }
                else
                {
                    // コンテナに登録
                    var container = containerKv.Value;
                    if (container.ContainsKey(typeof(T))) container.Remove(typeof(T));
                    container.Add(typeof(T), createInfo);
                }
            }

            /// <summary>
            /// 実装クラスからインスタンスを生成して返却する。
            /// </summary>
            /// <param name="key">
            ///     コンテナ名<br/>
            ///     <c>null</c>の場合、<see cref="DIContainer.ContainerConfig"/>の<see cref="Config.DefaultKey"/>を使用する。
            /// </param>
            /// <typeparam name="T">インスタンス型</typeparam>
            /// <returns>インスタンス</returns>
            /// <exception cref="ContainerNotRegistrationException">登録されていない型を指定した場合</exception>
            public T Resolve<T>(string? key = null) where T : IInjectable<T>
            {
                // コンテナ取得
                var keyName = key ?? ContainerConfig.DefaultKey;
                if (!ContainerDic.ContainsKey(keyName))
                    throw new ContainerNotRegistrationException(typeof(T));
                var container = ContainerDic.First(kv => kv.Key.Equals(keyName)).Value;

                // インスタンス生成情報取得
                if (!container.ContainsKey(typeof(T)))
                    throw new ContainerNotRegistrationException(typeof(T));
                var createInfo = container.First(kv => kv.Key == typeof(T)).Value;

                return (T) createInfo.GetInstance();
            }

            private static bool IsNull(KeyValuePair<string, Dictionary<Type, CreateInfo>> src)
            {
                return src.Equals(default(KeyValuePair<string, Dictionary<Type, CreateInfo>>));
            }

            /// <summary>
            /// 指定したキー名のコンテナ内に指定したクラスの生成メソッドが登録されているかどうかを返す。
            /// </summary>
            /// <param name="key">
            ///     キー名<br/>
            ///     <c>null</c>の場合、<see cref="DIContainer.ContainerConfig"/>の<see cref="Config.DefaultKey"/>を使用する。
            /// </param>
            /// <typeparam name="T">チェック対象のクラス型</typeparam>
            /// <returns>生成メソッドが登録されている場合true</returns>
            public bool HasCreateMethod<T>(string? key = null)
            {
                var keyName = key ?? ContainerConfig.DefaultKey;

                // キー名のコンテナ存在チェック
                var containerKv = ContainerDic.FirstOrDefault(kv => kv.Key.Equals(keyName));
                if (IsNull(containerKv)) return false;

                // コンテナ内の情報チェック
                var container = containerKv.Value;
                return container.ContainsKey(typeof(T));
            }
        }
    }
}