using System;
using System.Threading.Tasks;
using Xunit;

namespace Alterius.SimpleOption.UnitTests
{
    public class OptionTests
    {
        private const string some = "SOME";
        private const string none = "NONE";

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
        public void None_WithException_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(new NullReferenceException());

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
            Assert.IsType<NullReferenceException>(exResult);
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
            var option = Option.Some("test");

            //Act
            var result = option.Match(
                s => some,
                e => none);

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
                e => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void ImplicitCastValue_ReturnsNone()
        {
            //Arrange
            Option<string> option = (string)null;

            //Act
            var result = option.Match(
                s => some,
                e => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void ImplicitCastValue_ReturnsSome()
        {
            //Arrange
            Option<string> option = "test";

            //Act
            var result = option.Match(
                s => some,
                e => none);

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void ImplicitCastException_ReturnsNoneAndException()
        {
            //Arrange
            Option<string> option = new NullReferenceException();

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
            Assert.IsType<NullReferenceException>(exResult);
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
            var option = Option.Some("test");

            //Act
            string result = null;
            option
                .Some(s => { result = some; })
                .None(e => { result = none; });

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public void FluentInterface_None_ReturnsSome()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            string result = null;
            option
                .Some(s => { result = some; })
                .None(e => { result = none; });

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void FluentInterface_WithException_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option.None<string>(new NullReferenceException());

            //Act
            string result = null;
            Exception exResult = null;
            option
                .Some(s => { result = some; })
                .None(e => { result = none; exResult = e; });

            //Assert
            Assert.Equal(none, result);
            Assert.IsType<NullReferenceException>(exResult);
        }

        [Fact]
        public async Task MatchAsyncSome_ReturnsSome()
        {
            //Arrange
            var option = Option.Some("test");

            //Act
            var result = await option.Match(
                s => Task.FromResult(some),
                e => Task.FromResult(none));

            //Assert
            Assert.Equal(some, result);
        }

        [Fact]
        public async Task MatchAsyncNone_ReturnsNone()
        {
            //Arrange
            var option = Option.None<string>();

            //Act
            var result = await option.Match(
                s => Task.FromResult(some),
                e => Task.FromResult(none));

            //Assert
            Assert.Equal(none, result);
        }
    }
}
