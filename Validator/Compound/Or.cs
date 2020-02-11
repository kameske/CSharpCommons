using System;
using System.Collections.Generic;
using Commons.Linq.Extension;

namespace Commons
{
    public static partial class Validator
    {
        /// <summary>
        /// <paramref name="actions"/>の処理を順次実行する。
        /// すべての処理を実行した上で、すべての処理で例外が発生していれば
        /// <see cref="AggregateException"/>を発生させる。
        /// </summary>
        /// <param name="actions">実行する処理</param>
        /// <exception cref="AggregateException">実行した処理のすべてで例外が発生した場合</exception>
        public static void ValidateOr(params Action[] actions)
        {
            var errors = new List<Exception>();
            var taskCount = -1;

            actions.ForEach(action =>
            {
                try
                {
                    checked
                    {
                        taskCount++;
                    }

                    action.Invoke();
                }
                catch (Exception e)
                {
                    errors.Add(e);
                }
            });

            if (errors.Count == taskCount) throw new AggregateException(errors);
        }

        /// <summary>
        /// <see cref="ValidateOr"/>を実行する<see cref="Action"/>を返す。
        /// </summary>
        /// <param name="actions">実行する処理</param>
        /// <returns><see cref="ValidateOr"/>実行<see cref="Action"/></returns>
        public static Action Or(params Action[] actions)
            => () => ValidateOr(actions);
    }
}