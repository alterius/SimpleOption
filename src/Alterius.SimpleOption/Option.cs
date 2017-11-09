using System;

namespace Alterius.SimpleOption
{
    public struct Option<T>
    {
        private readonly T _value;
        private readonly Exception _ex;
        private readonly bool _some;
        public bool HasSome => _some && _value != null;

        public Option(T value)
        {
            _value = value;
            _ex = null;
            _some = true;
        }

        public Option(Exception ex)
        {
            _value = default(T);
            _ex = ex ?? throw new ArgumentNullException(nameof(ex));
            _some = false;
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<Exception, TResult> none)
        {
            return HasSome ? some(_value) : none(_ex);
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return HasSome ? some(_value) : none();
        }

        public void Match(Action<T> some, Action<Exception> none)
        {
            if (HasSome) some(_value); else none(_ex);
        }

        public void Match(Action<T> some, Action none)
        {
            if (HasSome) some(_value); else none();
        }

        public static implicit operator Option<T>(T value)
        {
            return Option.Some(value);
        }

        public static implicit operator Option<T>(Exception ex)
        {
            return Option.None<T>(ex);
        }
    }

    public class Option
    {
        public static Option<T> None<T>()
        {
            return new Option<T>();
        }

        public static Option<T> None<T>(Exception ex)
        {
            return new Option<T>(ex);
        }

        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value);
        }
    }
}
