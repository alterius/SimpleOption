using System;
using System.Threading.Tasks;

namespace Alterius.SimpleOption
{
    public struct Option<T>
    {
        private readonly T _value;
        private readonly Exception _ex;
        private readonly bool _some;

        public bool HasSome => _some && _value != null;

        internal Option(T value) : this()
        {
            _value = value;
            _some = true;
        }

        internal Option(Exception ex) : this()
        {
            _ex = ex ?? throw new ArgumentNullException(nameof(ex));
            _some = false;
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<Exception, TResult> none)
        {
            return HasSome ? some(_value) : none(_ex);
        }

        public Task<TResult> Match<TResult>(Func<T, Task<TResult>> some, Func<Exception, Task<TResult>> none)
        {
            return HasSome ? some(_value) : none(_ex);
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            return HasSome ? some(_value) : none();
        }

        public Task<TResult> Match<TResult>(Func<T, Task<TResult>> some, Func<Task<TResult>> none)
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

        public Option<T> Some(Action<T> some)
        {
            if (HasSome) some(_value);
            return this;
        }

        public OptionResult<T, TResult> Some<TResult>(Func<T, TResult> some)
        {
            return new OptionResult<T, TResult>(this, some);
        }

        public OptionResult<T, Task<TResult>> Some<TResult>(Func<T, Task<TResult>> some)
        {
            return new OptionResult<T, Task<TResult>>(this, some);
        }

        public Option<T> None(Action none)
        {
            if (!HasSome) none();
            return this;
        }

        public Option<T> None(Action<Exception> none)
        {
            if (!HasSome) none(_ex);
            return this;
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
        private Option()
        { }

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
