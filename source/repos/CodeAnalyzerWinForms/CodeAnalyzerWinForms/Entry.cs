using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerWinForms
{
  public class Entry
  {
    public int id;
    public string problem;
    public DateTime dateTime;
    public int mistakeCnt;

    public Entry(int id, string problem, DateTime dateTime, int mistakeCnt)
    {
      this.id = id;
      this.problem = problem;
      this.dateTime = dateTime;
      this.mistakeCnt = mistakeCnt;
    }

    public Entry(string s)
    {
      string[] args = s.Split(" ");
      if (args.Length != 5)
      {
        throw new ArgumentException("Wrong argument");
      }
      id = int.Parse(args[0]);
      problem = args[1];
      dateTime = DateTime.Parse(args[2] + " " + args[3]);
      mistakeCnt = int.Parse(args[4]);
    }
  }
}
