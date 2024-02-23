using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataTransformation.FileParser;
using NUnit.Framework.Legacy;


[TestFixture]
public class FTRParserTests
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
    [Test]
    public void ParseFile_ValidFilePath_ReturnsParsedData()
    {
        // Arrange
        string filePath = "../Project/assets/example_data.ftr";
        FTRParser parser = new FTRParser();
        var expectedResult = new List<string[]>
            {
                new string[] { "AI", "0", "Alan Turing Airport", "ALT", "112.05", "-16.09", "1487.52", "USA" },
                new string[] { "AI", "1", "Ada Lovelace Airport", "ADA", "41.5", "52.68", "3676.04", "CAN" },
                new string[] { "AI", "2", "Carl Gauss Airport", "CGA", "74.77", "35.27", "2570.73", "GBR" },
            };

        // Act
        IEnumerable<string[]> result = parser.ParseFile(filePath);

        // Assert
        for (int i = 0; i < expectedResult.Count; i++)
        {
            CollectionAssert.AreEqual(expectedResult[i], result.ElementAt(i), $"Line {i} does not match the expected result.");
        }
    }
}