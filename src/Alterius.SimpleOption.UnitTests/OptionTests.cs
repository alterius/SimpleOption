using System;
using System.Threading.Tasks;
using Xunit;

namespace Alterius.SimpleOption.UnitTests
{
    public class OptionTests
    {
        private const string test = "TEST";
        private const string some = "SOME";
        private const string none = "NONE";
        private readonly Exception exception = new TestException();

        [Fact]
        public void None_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            var result = option.Match(
                s => some,
                () => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void None_NoReturn_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            string result = null;
            option.Match(
                s => { result = some; },
                () => { result = none; });

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void None_WithException_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(exception);

            //Act
            Exception exResult = null;
            var result = option.Match(
                s => some,
                e =>
                {
                    exResult = e;
                    return none;
                });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }

        [Fact]
        public void None_WithExceptionNoReturn_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(exception);

            //Act
            Exception exResult = null;
            string result = null;
            option.Match(
                s => { result = some; },
                e =>
                {
                    exResult = e;
                    result = none;
                });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }

        [Fact]
        public void None_WithNullException_ThrowsNullReferenceException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => Option.None<string>(null));
        }

        [Fact]
        public void Some_ReturnsSome()
        {
            //Arrange
            var option = Option.Some(test);

            //Act
            var result = option.Match(
                s => some,
                () => none);

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void Some_NoReturn_ReturnsSome()
        {
            //Arrange
            var option = Option.Some(test);

            //Act
            string result = null;
            option.Match(
                s => { result = some; },
                () => { result = none; });

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void Some_WhenSomeIsNull_ReturnsNone()
        {
            //Arrange
            var option = Option.Some((string)null);

            //Act
            var result = option.Match(
                s => some,
                () => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void ImplicitCastNull_ReturnsNone()
        {
            //Arrange
            Option<string> option = (string)null;

            //Act
            var result = option.Match(
                s => some,
                () => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void ImplicitCastValue_ReturnsSome()
        {
            //Arrange
            Option<string> option = test;

            //Act
            var result = option.Match(
                s => some,
                () => none);

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void ImplicitCastException_ReturnsNoneAndException()
        {
            //Arrange
            Option<string> option = exception;

            //Act
            Exception exResult = null;
            var result = option.Match(
                s => some,
                e =>
                {
                    exResult = e;
                    return none;
                });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }

        [Fact]
        public void ImplicitCastException_ThrowsNullReferenceException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => { Option<string> result = (Exception)null; });
        }

        [Fact]
        public void FluentInterface_Some_ReturnsSome()
        {
            //Arrange
            var option = Option.Some(test);

            //Act
            string result = null;
            option
                .Some(s => { result = some; })
                .None(() => { result = none; });

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void FluentInterface_WithReturn_Some_ReturnsSome()
        {
            //Arrange
            var option = Option.Some(test);

            //Act
            var result = option
                .Some(s => some)
                .None(() => none);

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public async Task FluentInterface_WithReturn_AsyncSome_ReturnsSome()
        {
            //Arrange
            var option = Option.Some(test);

            //Act
            var result = await option
                .Some(s => Task.FromResult(some))
                .None(() => Task.FromResult(none));

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void FluentInterface_None_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            string result = null;
            option
                .Some(s => { result = some; })
                .None(() => { result = none; });

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void FluentInterface_WithReturn_None_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            var result = option
                .Some(s => some)
                .None(() => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public async Task FluentInterface_WithReturn_AsyncNone_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            var result = await option
                .Some(s => Task.FromResult(some))
                .None(() => Task.FromResult(none));

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void FluentInterface_WithException_None_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(exception);

            //Act
            string result = null;
            Exception exResult = null;
            option
                .Some(s => { result = some; })
                .None(e => { result = none; exResult = e; });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }

        [Fact]
        public void FluentInterface_WithExceptionAndReturn_None_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(exception);

            //Act
            Exception exResult = null;
            var result = option
                .Some(s => some)
                .None(e => { exResult = e; return none; });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }

        [Fact]
        public async Task FluentInterface_WithExceptionAndReturn_AsyncNone_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(exception);

            //Act
            Exception exResult = null;
            var result = await option
                .Some(s => Task.FromResult(some))
                .None(e => { exResult = e; return Task.FromResult(none); });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }

        [Fact]
        public async Task AsyncSome_ReturnsSome()
        {
            //Arrange
            var option = Option.Some(test);

            //Act
            var result = await option.Match(
                s => Task.FromResult(some),
                () => Task.FromResult(none));

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public async Task AsyncNone_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            var result = await option.Match(
                s => Task.FromResult(some),
                () => Task.FromResult(none));

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public async Task AsyncNone_WithException_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>(exception);

            //Act
            Exception exResult = null;
            var result = await option.Match(
                s => Task.FromResult(some),
                e => { exResult = e; return Task.FromResult(none); });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<TestException>(exResult);
            Assert.Equal(exception, exResult);
        }
    }
}
