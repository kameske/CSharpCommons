using System;
using Commons.Linq.Extension;

namespace Commons
{
    public static partial class Validator
    {
        /// <summary>
        /// <paramref name="actions"/>の処理を順次実行する。
        /// 一つでも例外が発生した時点で、発生した例外をそのままスローする。
        /// </summary>
        /// <param name="actions">実行する処理</param>
        /// <exception cref="Exception">処理によって発生した例外</exception>
        public static void ValidateAndAlso(params Action[] actions)
        {
            actions.ForEach((action, _) => action.Invoke());
        }

        /// <summary>
        /// <see cref="ValidateAndAlso"/>を実行する<see cref="Action"/>を返す。
        /// </summary>
        /// <param name="actions">実行する処理</param>
        /// <returns><see cref="ValidateAndAlso"/>実行<see cref="Action"/></returns>
        public static Action AndAlso(params Action[] actions)
            => () => ValidateAndAlso(actions);
    }
}