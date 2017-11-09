using System;

namespace Alterius.SimpleOption
{
    public struct Option<T>
    {
        private readonly T _value;
        private readonly Exception _ex;
        private readonly bool _some;
        public bool HasSome => _some && _value != null;

        private Option(T value)
        {
            _value = value;
            _ex = null;
            _some = true;
        }

        private Option(Exception ex)
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

        public static Option<T> None()
        {
            return new Option<T>();
        }

        public static Option<T> None(Exception ex)
        {
            return new Option<T>(ex);
        }

        public static Option<T> Some(T value)
        {
            return new Option<T>(value);
        }

        public static implicit operator Option<T>(T value)
        {
            return Some(value);
        }

        public static implicit operator Option<T>(Exception ex)
        {
            return None(ex);
        }
    }
}
