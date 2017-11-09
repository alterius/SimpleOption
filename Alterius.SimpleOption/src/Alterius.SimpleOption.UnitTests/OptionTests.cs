using System;
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
            var option = Option<string>.None();

            //Act
            var result = option.Match(
                s => some,
                n => none);

            //Assert
            Assert.Equal(none, result);
        }

        [Fact]
        public void None_WithException_ReturnsNoneAndException()
        {
            //Arrange
            var option = Option<string>.None(new NullReferenceException());

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
            Assert.Throws<ArgumentNullException>(() => Option<string>.None(null));
        }

        [Fact]
        public void Some_ReturnsSome()
        {
            //Arrange
            var option = Option<string>.Some("test");

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
            var option = Option<string>.Some(null);

            //Act
            var result = option.Match(
                s => some,
                e => none);

            //Assert
            Assert.Equal(none, result);
        }
    }
}
