using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerWinForms
{
  internal class WhiteSpaceRemoval
  {
    private string _result;
    public WhiteSpaceRemoval(string s)
    {
      StringBuilder sb = new StringBuilder();
      foreach (char c in s)
      {
        if (!Char.IsWhiteSpace(c))
        {
          sb.Append(c);
        }
      }
      _result = sb.ToString();
    }

    public string GetResult()
    {
      return _result;
    }
  }
}
