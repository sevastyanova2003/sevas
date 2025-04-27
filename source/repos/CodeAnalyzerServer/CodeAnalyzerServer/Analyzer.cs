using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerServer
{

  /// <summary>
  /// Class for code analysis
  /// </summary>
  public class Analyzer
  {
    private string _text;
    private HashSet<string> _nonTypeKeywords = ("abstract\r\nas\r\nbase\r\nbreak\r\ncase\r\ncatch\r\nchecked\r\nconst\r\ncontinue\r\ndefault\r\ndo\r\nelse\r\nexplicit\r\nextern\r\nfalse\r\nfinally\r\nfixed\r\nfor\r\nforeach\r\ngoto\r\nif\r\nimplicit\r\nin\r\ninternal\r\nis\r\nlock\r\nnamespace\r\nnew\r\nnull\r\noperator\r\nout\r\noverride\r\nprivate\r\nprotected\r\npublic\r\nreadonly\r\nref\r\nreturn\r\nsealed\r\nsizeof\r\nstackalloc\r\nstatic\r\nswitch\r\nthis\r\nthrow\r\ntrue\r\ntry\r\ntypeof\r\nunchecked\r\nunsafe\r\nusing\r\nvirtual\r\nvolatile\r\nwhile\r\n" +
      "add\r\nand\r\nalias\r\nascending\r\nasync\r\nawait\r\nby\r\ndescending\r\ndynamic\r\nequals\r\nfrom\r\n\r\nget\r\nglobal\r\ngroup\r\ninit\r\ninto\r\njoin\r\nlet\r\nmanaged\r\nnameof\r\nnint\r\nnot\r\nnotnull\r\non\r\norderby\r\npartial\r\nremove\r\nselect\r\nset\r\nunmanaged\r\nvalue\r\nvar\r\nwhen\r\nwhere\r\nwhere\r\nwith\r\nyield").Split("\r\n").ToHashSet<string>();

    /// <summary>
    /// Creates Analyzer of <paramref name="text"/>
    /// </summary>
    /// <param name="text"></param>
    public Analyzer(string text)
    {
      _text = text;
    }

    /// <summary>
    /// Marks characters in <paramref name="text"/> in /**/ comments as not <paramref name="meaningful"/>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="meaningful"></param>
    private void DeleteComments(string[] text, List<List<bool> > meaningful)
    {
      bool isLongComment = false;
      bool isComment = false;
      for (int i = 0; i < text.Length; ++i)
      {
        isComment = false;
        for (int j = 0; j < text[i].Length; ++j)
        {
          if (!isComment && !isLongComment &&
            text[i][j] == '/' && j + 1 < text[i].Length && text[i][j + 1] == '*')
          {
            isLongComment = true;
          } else if (!isComment && !isLongComment &&
            text[i][j] == '/' && j + 1 < text[i].Length && text[i][j + 1] == '/')
          {
            isComment = true;
          }
          if (isLongComment || isComment)
          {
            meaningful[i][j] = false;
          }
          if (isLongComment && j > 0 && text[i][j - 1] == '*' && text[i][j] == '/')
          {
            isLongComment = false;
          }
        }
      }
    }

    /// <summary>
    /// Checks if <paramref name="s"/>[<paramref name="begin"/>:<paramref name="end"/>] is in Pascal style
    /// </summary>
    /// <param name="s"></param>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsPascal(string s, int begin, int end)
    {
      if (end > s.Length || begin >= end)
      {
        return false;
      }
      for (int i = begin; i < end; ++i)
      {
        if (!Char.IsLetterOrDigit(s[i]))
        {
          return false;
        }
        if (i == begin && !Char.IsUpper(s[i]))
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Checks if <paramref name="s"/>[<paramref name="begin"/>:<paramref name="end"/>] is in camel style
    /// </summary>
    /// <param name="s"></param>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsCamel(string s, int begin, int end)
    {
      if (end > s.Length || begin >= end)
      {
        return false;
      }
      for (int i = begin; i < end; ++i)
      {
        if (!Char.IsLetterOrDigit(s[i]))
        {
          return false;
        }
        if (i == begin && !Char.IsLower(s[i]))
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Checks if <paramref name="s"/>[<paramref name="begin"/>:<paramref name="end"/>] is in IPascal style
    /// </summary>
    /// <param name="s"></param>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsInterface(string s, int begin, int end)
    {
      if (end > s.Length || begin >= end)
      {
        return false;
      }
      if (s.Length == 0 || s[begin] != 'I')
      {
        return false;
      }
      return IsPascal(s, begin + 1, end);
    }

    /// <summary>
    /// Checks if <paramref name="s"/>[<paramref name="begin"/>:<paramref name="end"/>] is in _camel style
    /// </summary>
    /// <param name="s"></param>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsPrivateField(string s, int begin, int end)
    {
      if (end > s.Length || begin >= end)
      {
        return false;
      }
      if (s.Length == 0 || s[begin] != '_')
      {
        return false;
      }
      return IsCamel(s, begin + 1, end);
    }

    /// <summary>
    /// Adds identificator name mistakes in <paramref name="parser"/> to <paramref name="res"/>
    /// </summary>
    /// <param name="parser"></param>
    /// <param name="res"></param>
    private void RunThough(Parser parser, List<string> res)
    {
      bool classExpected = false;
      bool interfaceExpected = false;
      bool privateExpected = false;
      bool typeExpected = false;
      bool inAngleBrackets = false;

      string? cur;
      while ((cur = parser.GetWord()) != null && cur != "")
      {
        if (cur == "<")
        {
          inAngleBrackets = true;
        }
        else if (cur == ">")
        {
          inAngleBrackets = false;
        }
        if (inAngleBrackets) { continue; }
        if (cur == "private")
        {
          privateExpected = true;
          typeExpected = true;
        } else if (cur == "interface")
        {
          interfaceExpected = true;
          typeExpected = true;
        } else if (cur == "class" || cur == "struct" || cur == "record" || cur == "delegate")
        {
          classExpected = true;
          typeExpected = true;
        }
        if (parser.IsIdentifierSymbol(cur[0]))
        {
          if (!typeExpected)
          {
            if (classExpected)
            {
              if (!IsPascal(cur, 0, cur.Length))
              {
                res.Add($"{cur} should be in PascalCase format - in line {parser.CurLine}");
              }
              classExpected = false;
              privateExpected = false;
            } else if (interfaceExpected)
            {
              if (!IsInterface(cur, 0, cur.Length))
              {
                res.Add($"{cur} should be in IPascalCase format - in line {parser.CurLine}");
              }
              interfaceExpected = false;
              privateExpected = false;
            } else if (privateExpected) // not class or interface
            {
              char? c = parser.PeakChar();
              if (c == null || (c != '{' && c != '('))
              {
                if (!IsPrivateField(cur, 0, cur.Length))
                {
                  res.Add($"{cur} should be in _camelCase format - in line {parser.CurLine}");
                }
              }
              privateExpected = false;
            }
          } else if (typeExpected && !_nonTypeKeywords.Contains(cur))
          {
            typeExpected = false;
          }
        }
      }
    }

    /// <summary>
    /// Adds "Extra empty line" mistakes in <paramref name="text"/> to <paramref name="res"/>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="res"></param>
    private void CheckEmptyLines(string[] text, List<string> res)
    {
      for (int i = 1; i < text.Length; ++i)
      {
        if (string.IsNullOrWhiteSpace(text[i]) && string.IsNullOrWhiteSpace(text[i - 1]))
        {
          res.Add($"Avoid more than one empty line at any time. - in line {i}");
        }
      }
    }

    /// <summary>
    /// Adds "using tab" mistakes in <paramref name="text"/> to <paramref name="res"/>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="res"></param>
    private void CheckTabs(string[] text, List<string> res)
    {
      for (int i = 0; i < text.Length; ++i)
      {
        for (int j = 0; j < text[i].Length; ++j)
        {
          if (text[i][j] == '\t')
          {
            res.Add($"Do not use tab. Use white spaces for indentation. - in line {i} pos {j}");
          }
        }
      }
    }

    /// <summary>
    /// Adds Allman mistakes in <paramref name="text"/> where <paramref name="meaningful"/> to <paramref name="res"/>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="meaningful"></param>
    /// <param name="res"></param>
    private void CheckAllman(string[] text, List<List<bool> > meaningful, List<string> res)
    {
      for (int i = 0; i < text.Length; i++)
      {
        for (int j = 1; j < text[i].Length; ++j)
        {
          if (meaningful[i][j] && text[i][j] == '{')
          {
            if (!string.IsNullOrWhiteSpace(text[i].Substring(0, j - 1)))
            {
              res.Add($"Braces must be in Allman style: any opening brace {{ must be in a new line - in line {i}");
            }
          }
        }
      }
    }

    /// <summary>
    /// Adds spurious white spaces mistakes in <paramref name="text"/> to <paramref name="res"/>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="res"></param>
    private void CheckWhiteSpaces(string[] text, List<string> res)
    {
      for (int i = 0; i < text.Length; i++)
      {
        if (text[i].Length >= 1)
        {
          int j = text[i].Length - 1;
          if (text[i][j] == '\r')
          {
            if (j >= 1 && char.IsWhiteSpace(text[i][j - 1]))
            {
              res.Add($"Avoid spurious free spaces. For example avoid if (someVar == 0)..., where the dots mark the spurious free spaces - in line {i}");
            }
          } else if (char.IsWhiteSpace(text[i][j]))
          {
            res.Add($"Avoid spurious free spaces. For example avoid if (someVar == 0)..., where the dots mark the spurious free spaces - in line {i}");
          }
        }
      }
    }

    /// <summary>
    /// Adds "too long function" mistakes in <paramref name="parser"/> to <paramref name="res"/>
    /// </summary>
    /// <param name="parser"></param>
    /// <param name="res"></param>
    private void CheckBrackets(Parser parser, List<string> res)
    {
      List<int> bracketRow = new List<int>();
      bool longBracket = false;
      string? cur;
      while ((cur = parser.GetWord()) != null)
      {
        if (cur == "class" || cur == "struct" || cur == "namespace")
        {
          longBracket = true;
        } else if (cur == "{")
        {
          if (longBracket)
          {
            bracketRow.Add(-1);
            longBracket = false;
          } else
          {
            bracketRow.Add(parser.CurLine);
          }
        } else if (cur == "}")
        {
          if (bracketRow.Count == 0)
          {
            res.Add($"Brackets do not form a correct bracket sequence - extra bracket in line {parser.CurLine}");
          }
          if (bracketRow[bracketRow.Count - 1] != -1 && parser.CurLine - bracketRow[bracketRow.Count - 1] > 40)
          {
            res.Add($"Functions must have no more than 40 lines. - lines {bracketRow[bracketRow.Count - 1]} to {parser.CurLine}");
            bracketRow.RemoveAt(bracketRow.Count - 1);
          }
        }
      }
    }

    /// <summary>
    /// Returns List of styling mistakes in _text
    /// </summary>
    /// <returns></returns>
    public List<string> Analyze()
    {
      List<string> res = new List<string>();
      string[] textArr = _text.Split('\n');
      CheckTabs(textArr, res);
      CheckEmptyLines(textArr, res);
      CheckWhiteSpaces(textArr, res);
      List<List<bool> > meaningful = new List<List<bool>>();
      for (int i = 0; i < textArr.Length; ++i)
      {
        meaningful.Add(new List<bool>());
        for (int j = 0; j < textArr[i].Length; ++j)
        {
          meaningful[i].Add(true);
        }
      }
      DeleteComments(textArr, meaningful);
      CheckAllman(textArr, meaningful, res);
      RunThough(new Parser(textArr, meaningful), res);
      CheckBrackets(new Parser(textArr, meaningful), res);
      return res;
    }
  }
}