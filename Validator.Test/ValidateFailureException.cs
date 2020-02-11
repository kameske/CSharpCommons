using System;

namespace Validator.Test
{
    /// <summary>
    /// Validatorテストで用いる、検証失敗時に返却されるべきエラー
    /// </summary>
    public class ValidateFailureException : Exception
    {
        public static Func<ValidateFailureException> FuncMake
            => () => new ValidateFailureException();
    }
}