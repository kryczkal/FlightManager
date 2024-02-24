using NUnit.Framework;
using DataTransformation;
using DataTransformation.StringFormatter;

namespace Formatter.Tests
{
    public class FormatterTests
    {
        [Test]
        public void FormatArray_ReturnsFormattedArray()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 4, 5 };
            var expected = "[1;2;3;4;5]";

            // Act
            var result = FTRFormatter.FormatArray<int>(values);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}