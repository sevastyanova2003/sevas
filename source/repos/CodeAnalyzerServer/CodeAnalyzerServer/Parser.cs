﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerServer
{
  public class Parser
  {
    private string[] _lines;
    private int _cur_line;
    private int _cur_id;
    private List<List<bool>> _meaningful;

    public int CurLine { get { return _cur_line; } }

    public Parser(string[] lines, List<List<bool>> meaningful)
    {
      _lines = lines;
      _cur_line = 0;
      _cur_id = 0;
      _meaningful = meaningful;
    }

    public bool IsIdentifierSymbol(char c)
    {
      return c == '_' || Char.IsLetterOrDigit(c);
    }

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

    public string? GetWord()
    {
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
      while (_cur_line < _lines.Length && IsIdentifierSymbol(_lines[_cur_line][_cur_id]))
      {
        stringBuilder.Append(_lines[_cur_line][_cur_id]);
        IncreaseId();
      }
      return stringBuilder.ToString();
    }

    public char? PeakChar()
    {
      int cur_line = _cur_line;
      int cur_id = _cur_id;
      while (_cur_line < _lines.Length && (!_meaningful[cur_line][cur_id] || Char.IsWhiteSpace(_lines[_cur_line][_cur_id])))
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
      return null;
    }

  }
}
