using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Common
{
    public readonly struct ResultPattern
    {
        public bool Error { get; }
        public string ErrorMessage { get; }

        public ResultPattern(bool error, string errorMessage)
        {
            Error = error;
            ErrorMessage = errorMessage;
        }

        public static ResultPattern Success()
            => new ResultPattern(false, string.Empty);

        public static ResultPattern Failure(string message)
            => new ResultPattern(true, message);
    }

    public readonly struct ResultPattern<T>
    {
        public T Value { get; }
        public bool Error { get; }
        public string ErrorMessage { get; }

        public ResultPattern(T value)
        {
            Value = value;
            Error = false;
            ErrorMessage = string.Empty;
        }

        public ResultPattern(string message)
        {
            Error = true;
            ErrorMessage = message;
        }

        public static ResultPattern<T> Success(T value)
            => new ResultPattern<T>(value);

        public static ResultPattern<T> Failure(string message)
            => new ResultPattern<T>(message);
    }
}
