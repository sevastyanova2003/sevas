using CodeAnalyzerServer;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject
{
  public class AnalyzerTests
  {
    [Fact]
    public void EmptyTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Empty(res);
    }

    [Fact]
    public void TabTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("Hello\tworld");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("Do not use tab. Use white spaces for indentation. - in line 0 pos 5", res[0]);
    }

    [Fact]
    public void EmptyLineTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("Hello\n\n\nworld");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("Avoid more than one empty line at any time. - in line 2", res[0]);
    }

    [Fact]
    public void WhiteSpacesTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("Hello \nworld");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("Avoid spurious free spaces. For example avoid if (someVar == 0)..., where the dots mark the spurious free spaces - in line 0", res[0]);
    }

    [Fact]
    public void AllmanTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("Hello {\nworld\n}");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("Braces must be in Allman style: any opening brace { must be in a new line - in line 0", res[0]);
    }

    [Fact]
    public void LongFuncTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace CodeAnalyzerTests\r\n{\r\n  internal class ClassLongFunc\r\n  {\r\n    public void Func()\r\n    {\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n      Console.WriteLine();\r\n    }\r\n  }\r\n}\r\n");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("Functions must have no more than 40 lines. - lines 11 to 57", res[0]);
    }

    [Theory]
    [InlineData("internal class classPascalCase;")]
    [InlineData("private class classPascalCase;")]
    public void ClassNameTest(string text)
    {
      // Arrange
      Analyzer analyzer = new Analyzer(text);

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("classPascalCase should be in PascalCase format - in line 0", res[0]);
    }

    [Theory]
    [InlineData("internal interface interfaceName;")]
    [InlineData("private interface interfaceName;")]
    public void InterfaceNameTest(string text)
    {
      // Arrange
      Analyzer analyzer = new Analyzer(text);

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal("interfaceName should be in IPascalCase format - in line 0", res[0]);
    }

    [Theory]
    [InlineData("private int privateInteger;", "privateInteger")]
    [InlineData("private int PrivateInt;", "PrivateInt")]
    public void PrivateNameTest(string text, string varName)
    {
      // Arrange
      Analyzer analyzer = new Analyzer(text);

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Equal(1, res.Count);
      Assert.Equal($"{varName} should be in _camelCase format - in line 0", res[0]);
    }

    [Theory]
    [InlineData("public int publicInteger;")]
    [InlineData("private int _privateInteger;")]
    [InlineData("private void MethodName();")]
    [InlineData("public class ClassName;")]
    [InlineData("private interface IInterfaceName;")]
    public void CorrectNameTest(string text)
    {
      // Arrange
      Analyzer analyzer = new Analyzer(text);

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Empty(res);
    }

    [Fact]
    public void CommentTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("internal class ClassComments\r\n  {\r\n    //public ClassComments() { Console.WriteLine(\"Not allman braces should be ignored\"); }\r\n  }");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Empty(res);
    }

    [Fact]
    public void LongCommentTest()
    {
      // Arrange
      Analyzer analyzer = new Analyzer("internal class ClassComments\r\n  {\r\n    /*public ClassComments() { Console.WriteLine(\"Not allman braces should be ignored\"); }\r\n*/  }");

      // Act
      List<string> res = analyzer.Analyze();

      // Assert
      Assert.Empty(res);
    }
  }
}