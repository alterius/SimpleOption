using System;

namespace Alterius.SimpleOption
{
    public class OptionMatch<T, TResult>
    {
        private readonly Option<T> _option;

        private Func<T, TResult> _some;
        private Func<Exception, TResult> _none;

        public OptionMatch(Option<T> option, Func<T, TResult> some)
        {
            _option = option;
            _some = some;
        }

        public OptionMatch(Option<T> option, Func<Exception, TResult> none)
        {
            _option = option;
            _none = none;
        }

        public TResult Some(Func<T, TResult> some)
        {
            _some = some;
            return _option.Match(_some, _none);
        }

        public TResult None(Func<Exception, TResult> none)
        {
            _none = none;
            return _option.Match(_some, _none);
        }
    }
}
