using System;

namespace Alterius.SimpleOption
{
    public struct ExceptionHelper<TResult>
    {
        private readonly Exception _ex;

        private TResult _result;
        private bool _hasResult;

        public ExceptionHelper(Exception ex)
        {
            _ex = ex;
            _result = default(TResult);
            _hasResult = false;
        }

        public ExceptionHelper<TResult> IsTypeOf<T>(Func<Exception, TResult> func) where T : Exception
        {
            if (!_hasResult && _ex is T)
            {
                _result = func(_ex);
                _hasResult = true;
            }

            return this;
        }

        public TResult Else(Func<TResult> func)
        {
            if (!_hasResult)
            {
                _result = func();
            }

            return _result;
        }
    }
}