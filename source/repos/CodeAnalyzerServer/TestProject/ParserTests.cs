using CodeAnalyzerServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
  public class ParserTests
  {
    [Fact]
    public void EmptyTest()
    {
      // Arrange
      string[] lines = { "" };
      Parser parser = new Parser(lines, new List<List<bool>>());

      // Act
      string? res = parser.GetWord();

      // Assert
      Assert.Null(res);
    }

    [Fact]
    public void MultipleEmptyTest()
    {
      // Arrange
      string[] lines = { "", "", "" };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 3; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      Parser parser = new Parser(lines, meaningful);

      // Act
      string? res = parser.GetWord();

      // Assert
      Assert.Null(res);
    }

    [Fact]
    public void GetWordTest()
    {
      // Arrange
      string[] lines = { "Hello{}", "wor l23d " };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 2; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      for (int i = 0; i < 7; ++i)
      {
        meaningful[0].Add(true);
      }
      for (int i = 0; i < 9; ++i)
      {
        meaningful[1].Add(true);

      }
      Parser parser = new Parser(lines, meaningful);

      // Act & Assert
      string[] expected = { "Hello", "{", "}", "wor", "l23d"};
      for (int i = 0; i < 5; ++i)
      {
        Assert.Equal(expected[i], parser.GetWord());
      }
      Assert.Null(parser.GetWord());
    }

    [Fact]
    public void EmptyPeakTest()
    {
      // Arrange
      string[] lines = { "" };
      Parser parser = new Parser(lines, new List<List<bool>>());

      // Act
      char? res = parser.PeakChar();

      // Assert
      Assert.Null(res);
    }

    [Fact]
    public void MultipleEmptyPeakTest()
    {
      // Arrange
      string[] lines = { "", "", "" };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 3; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      Parser parser = new Parser(lines, meaningful);

      // Act
      char? res = parser.PeakChar();

      // Assert
      Assert.Null(res);
    }

    [Fact]
    public void PeakCharTest()
    {
      // Arrange
      string[] lines = { "Hello{}", "wor l23d " };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 2; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      for (int i = 0; i < 7; ++i)
      {
        meaningful[0].Add(true);
      }
      for (int i = 0; i < 9; ++i)
      {
        meaningful[1].Add(true);

      }
      Parser parser = new Parser(lines, meaningful);

      // Act & Assert
      char[] expected = { 'H', '{', '}', 'w', 'l'};
      for (int i = 0; i < 5; ++i)
      {
        Assert.Equal(expected[i], parser.PeakChar());
        parser.GetWord();
      }
      Assert.Null(parser.PeakChar());
    }

    [Fact]
    public void GetWordMeaninglessTest()
    {
      // Arrange
      string[] lines = { "Hello{}", "wor l23d " };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 2; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      for (int i = 0; i < 7; ++i)
      {
        meaningful[0].Add(true);
      }
      for (int i = 0; i < 9; ++i)
      {
        meaningful[1].Add(true);
      }
      meaningful[0][2] = false;
      meaningful[1][0] = false;
      meaningful[1][8] = false;
      Parser parser = new Parser(lines, meaningful);

      // Act & Assert
      string[] expected = { "He", "lo", "{", "}", "or", "l23d" };
      for (int i = 0; i < 6; ++i)
      {
        Assert.Equal(expected[i], parser.GetWord());
      }
      Assert.Null(parser.GetWord());
    }

    [Fact]
    public void PeakCharMeaninglessTest()
    {
      // Arrange
      string[] lines = { "Hello{}", "wor l23d " };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 2; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      for (int i = 0; i < 7; ++i)
      {
        meaningful[0].Add(true);
      }
      for (int i = 0; i < 9; ++i)
      {
        meaningful[1].Add(true);
      }
      meaningful[0][2] = false;
      meaningful[1][0] = false;
      meaningful[1][8] = false;
      Parser parser = new Parser(lines, meaningful);

      // Act & Assert
      char[] expected = { 'H', 'l', '{', '}', 'o', 'l' };
      for (int i = 0; i < 6; ++i)
      {
        Assert.Equal(expected[i], parser.PeakChar());
        parser.GetWord();
      }
      Assert.Null(parser.PeakChar());
    }

    [Fact]
    public void CurLineIdTest()
    {
      // Arrange
      string[] lines = { "Hello{}", "wor l23d " };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 2; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      for (int i = 0; i < 7; ++i)
      {
        meaningful[0].Add(true);
      }
      for (int i = 0; i < 9; ++i)
      {
        meaningful[1].Add(true);
      }
      meaningful[0][2] = false;
      meaningful[1][0] = false;
      meaningful[1][8] = false;
      Parser parser = new Parser(lines, meaningful);

      // Act & Assert
      int[] line = { 0, 0, 0, 0, 1, 1 };
      int[] id = { 0, 2, 5, 6, 0, 3 };
      for (int i = 0; i < 6; ++i)
      {
        Assert.Equal(line[i], parser.CurLine);
        Assert.Equal(id[i], parser.CurId);
        parser.GetWord();
      }
      parser.GetWord();
      Assert.Equal(2, parser.CurLine);
      Assert.Equal(0, parser.CurId);
    }

    [Fact]
    public void CurLineIdPeakTest()
    {
      // Arrange
      string[] lines = { "Hello{}", "wor l23d " };
      List<List<bool>> meaningful = new List<List<bool>>();
      for (int i = 0; i < 2; ++i)
      {
        meaningful.Add(new List<bool>());
      }
      for (int i = 0; i < 7; ++i)
      {
        meaningful[0].Add(true);
      }
      for (int i = 0; i < 9; ++i)
      {
        meaningful[1].Add(true);
      }
      meaningful[0][2] = false;
      meaningful[1][0] = false;
      meaningful[1][8] = false;
      Parser parser = new Parser(lines, meaningful);

      // Act & Assert
      int[] line = { 0, 0, 0, 0, 1, 1 };
      int[] id = { 0, 2, 5, 6, 0, 3 };
      for (int i = 0; i < 6; ++i)
      {
        parser.PeakChar();
        Assert.Equal(line[i], parser.CurLine);
        Assert.Equal(id[i], parser.CurId);
        parser.GetWord();
      }
      parser.GetWord();
      parser.PeakChar();
      Assert.Equal(2, parser.CurLine);
      Assert.Equal(0, parser.CurId);
    }
  }
}
