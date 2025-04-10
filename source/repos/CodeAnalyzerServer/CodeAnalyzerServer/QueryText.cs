using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerServer
{
  public class QueryText
  {
    public string TextToQuery(string text)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < text.Length; i++)
      {
        if (text[i] == '\'')
        {
          sb.Append('\'');
        }
        sb.Append(text[i]);
      }
      return sb.ToString();
    }
  }
}
