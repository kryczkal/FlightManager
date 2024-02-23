using NUnit.Framework;
using FileParser;

[TestFixture]
public class FTRParserTest
{
    [Test]
    public void ParseArray_IntArray_ReturnsCorrectArray()
    {
        // Arrange
        string line = "[1;2;3;4;5]";

        // Act
        int[] result = FTRParser.ParseArray<int>(line);

        // Assert
        Assert.That(result, Is.EquivalentTo(new int[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void ParseArray_StringArray_ReturnsCorrectArray()
    {
        // Arrange
        string line = "[apple;banana;cherry]";

        // Act
        string[] result = FTRParser.ParseArray<string>(line);

        // Assert
        Assert.That(result, Is.EquivalentTo(new string[] { "apple", "banana", "cherry" }));
    }
}