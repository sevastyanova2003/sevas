using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerServer
{
  /// <summary>
  /// Class for reading through parsed words and symbols from code
  /// </summary>
  public class Parser
  {
    private string[] _lines;
    private int _cur_line;
    private int _cur_id;
    private List<List<bool>> _meaningful;

    public int CurLine { get { return _cur_line; } }

    public int CurId { get { return _cur_id; } }

    /// <summary>
    /// Creates Parser of <paramref name="lines"/> where index is <paramref name="meaningful"/>
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="meaningful"></param>
    public Parser(string[] lines, List<List<bool>> meaningful)
    {
      _lines = lines;
      _cur_line = 0;
      _cur_id = 0;
      _meaningful = meaningful;
    }

    /// <summary>
    /// Checks if char <paramref name="c"/> is part of an identifier
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool IsIdentifierSymbol(char c)
    {
      return c == '_' || Char.IsLetterOrDigit(c);
    }

    /// <summary>
    /// Skips to next symbol
    /// </summary>
    private void IncreaseId()
    {
      _cur_id++;
      while (_cur_id >= _lines[_cur_line].Length)
      {
        _cur_line++;
        _cur_id = 0;
        if (_cur_line >= _lines.Length)
        {
          break;
        }
      }
    }

    /// <summary>
    /// Return next meaningful word or symbol in text and moves index of reading
    /// </summary>
    /// <returns></returns>
    public string? GetWord()
    {
      if (_cur_line >= _lines.Length)
      {
        return null;
      }
      if (_cur_id >= _lines[_cur_line].Length)
      {
        IncreaseId();
      }
      if (_cur_line >= _lines.Length)
      {
        return null;
      }
      if (!_meaningful[_cur_line][_cur_id])
      {
        IncreaseId();
        return GetWord();
      }
      if (!IsIdentifierSymbol(_lines[_cur_line][_cur_id]))
      {
        string ans = _lines[_cur_line][_cur_id].ToString();
        IncreaseId();
        if (Char.IsWhiteSpace(ans, 0))
        {
          return GetWord();
        }
        return ans;
      }
      StringBuilder stringBuilder = new StringBuilder();
      while (_cur_line < _lines.Length && IsIdentifierSymbol(_lines[_cur_line][_cur_id]) && _meaningful[_cur_line][_cur_id])
      {
        stringBuilder.Append(_lines[_cur_line][_cur_id]);
        IncreaseId();
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns next meaningful char in text, index of reading remains the same
    /// </summary>
    /// <returns></returns>
    public char? PeakChar()
    {
      int cur_line = _cur_line;
      int cur_id = _cur_id;
      while (_cur_line < _lines.Length && (_cur_id >= _lines[_cur_line].Length || !_meaningful[_cur_line][_cur_id] || Char.IsWhiteSpace(_lines[_cur_line][_cur_id])))
      {
        IncreaseId();
      }
      if (_cur_line < _lines.Length)
      {
        char ans = _lines[_cur_line][_cur_id];
        _cur_line = cur_line;
        _cur_id = cur_id;
        return ans;
      }
      _cur_line = cur_line;
      _cur_id = cur_id;
      return null;
    }

  }
}
