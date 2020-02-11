using System;
using System.Collections.Generic;

namespace Commons
{
    public static partial class Validator
    {
        /// <summary>
        /// <paramref name="actions"/>の処理を順次実行する。
        /// すべての処理で例外が発生していれば
        /// <see cref="AggregateException"/>を発生させる。
        /// 一つでもエラーが発生しなかった処理が存在する場合、例外は発生しない。
        /// </summary>
        /// <param name="actions">実行する処理</param>
        /// <exception cref="AggregateException">実行した処理のすべてで例外が発生した場合</exception>
        public static void ValidateOrElse(params Action[] actions)
        {
            var errors = new List<Exception>();

            foreach (var action in actions)
            {
                try
                {
                    action.Invoke();
                    return;
                }
                catch (Exception e)
                {
                    errors.Add(e);
                }
            }

            throw new AggregateException(errors);
        }

        /// <summary>
        /// <see cref="ValidateOrElse"/>を実行する<see cref="Action"/>を返す。
        /// </summary>
        /// <param name="actions">実行する処理</param>
        /// <returns><see cref="ValidateOrElse"/>実行<see cref="Action"/></returns>
        public static Action OrElse(params Action[] actions)
            => () => ValidateOrElse(actions);
    }
}