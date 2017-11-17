using System;

namespace Alterius.SimpleOption
{
    public class OptionResult<T, TResult>
    {
        private readonly Option<T> _option;
        private Func<T, TResult> _some;

        internal OptionResult(Option<T> option, Func<T, TResult> some)
        {
            _option = option;
            _some = some;
        }

        public TResult None(Func<Exception, TResult> none)
        {
            return _option.Match(_some, none);
        }

        public TResult None(Func<TResult> none)
        {
            return _option.Match(_some, none);
        }
    }
}
