using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzerWinForms
{
  public class Client
  {
    public Client() { }

    public async Task<string> SendRequestAsync(string message)
    {
      using TcpClient tcpClient = new TcpClient();
      try
      {
        await tcpClient.ConnectAsync("127.0.0.1", 8888);

        var stream = tcpClient.GetStream();
        byte[] data = Encoding.UTF8.GetBytes(message);
        byte[] size = BitConverter.GetBytes(data.Length);
        await stream.WriteAsync(size);
        await stream.WriteAsync(data);

        byte[] sizeBuffer = new byte[4];
        await stream.ReadExactlyAsync(sizeBuffer, 0, sizeBuffer.Length);
        int sizeResponse = BitConverter.ToInt32(sizeBuffer, 0);

        byte[] dataResponse = new byte[sizeResponse];

        int bytes = await stream.ReadAsync(dataResponse);
        var response = Encoding.UTF8.GetString(dataResponse, 0, bytes);
        return response;
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Request to server failed. {ex.Message}");
        return $"FAILED\n{ex.Message}";
      }
    }

    public async Task<string> Auth(string username, string password, Mode mode)
    {
      string res = await SendRequestAsync("AUTH\n" + username + "\n" + password + "\n" + mode);
      return res;
    }

    public async Task<string> AddTeacher(string username, string password, string added_user, string added_password)
    {
      string res = await SendRequestAsync("AddTeacher\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + added_user + "\n" + added_password);
      return res;
    }

    public async Task<string> AddGroup(string username, string password, string groupName, string teacherName)
    {
      string res = await SendRequestAsync("AddGroup\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + groupName + "\n" + teacherName);
      return res;
    }

    public async Task<string> EditGroup(string username, string password, string groupName, string teacherName)
    {
      string res = await SendRequestAsync("EditGroup\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + groupName + "\n" + teacherName);
      return res;
    }

    public async Task<string> AddStudent(string username, string password, string added_user, string added_password, string group_name)
    {
      string res = await SendRequestAsync("AddStudent\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + added_user + "\n" + added_password + "\n" + group_name);
      return res;
    }

    public async Task<string> EditStudent(string username, string password, string studentName, string groupName)
    {
      string res = await SendRequestAsync("EditStudent\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + studentName + "\n" + groupName);
      return res;
    }

    public async Task<string> DeleteTeacher(string username, string password, string teacherUsername)
    {
      string res = await SendRequestAsync("DeleteTeacher\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + teacherUsername);
      return res;
    }

    public async Task<string> DeleteGroup(string username, string password, string groupName)
    {
      string res = await SendRequestAsync("DeleteGroup\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + groupName);
      return res;
    }

    public async Task<string> DeleteStudent(string username, string password, string studentUsername)
    {
      string res = await SendRequestAsync("DeleteStudent\n" + username + "\n" + password + "\n" + Mode.AdminMode
        + "\n" + studentUsername);
      return res;
    }

    public async Task<string> SendEntry(string username, string password, string problem, string text)
    {
      string res = await SendRequestAsync("GetEntryAdded\n" + username + "\n" + password + "\n"
        + Mode.StudentMode + "\n" + problem + "\n" + text);
      return res;
    }

    public async Task<string> GetStudentStats(string username, string password, Mode mode, string studentUsername)
    {
      string res = await SendRequestAsync("GetStudentStats\n" + username + "\n" + password + "\n" +
        mode + "\n" + studentUsername);
      return res;
    }

    public async Task<string> GetGroupStats(string username, string password)
    {
      string res = await SendRequestAsync("GetGroupStats\n" + username + "\n" + password + "\n" +
        Mode.StudentMode);
      return res;
    }

    public async Task<string> GetGroupStats(string username, string password, string group)
    {
      string res = await SendRequestAsync("GetGroupStats\n" + username + "\n" + password + "\n" +
        Mode.TeacherMode + "\n" + group);
      return res;
    }

    public async Task<List<string>> GetGroups(string username, string password)
    {
      string res = await SendRequestAsync("GetGroups\n" + username + "\n" + password + "\n" + Mode.TeacherMode);
      List<string> resList = res.Split("\n").ToList();
      if (resList.Count > 0 && resList[0] == "FAILED")
      {
        MessageBox.Show(res);
        return new List<string>();
      }
      return resList;
    }

    public async Task<Dictionary<string, List<string>>> GetStudents(string username, string password)
    {
      string res = await SendRequestAsync("GetStudents\n" + username + "\n" + password + "\n" + Mode.TeacherMode);
      Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
      List<string> resList = res.Split("\n").ToList();
      if (resList.Count > 0 && resList[0] == "FAILED")
      {
        return dict;
      }
      foreach (string pair in resList)
      {
        List<string> pairList = pair.Split(" ").ToList();
        if (pairList.Count == 2)
        {
          if (!dict.ContainsKey(pairList[0]))
          {
            dict[pairList[0]] = new List<string>();
          }
          dict[pairList[0]].Add(pairList[1]);
        }
      }
      return dict;
    }

    public async Task<string> GetFile(string username, string password, Mode mode, string id)
    {
      string res = await SendRequestAsync("GetFile\n" + username + "\n" + password + "\n" + mode + "\n" + id);
      return res;
    }

    public async Task<string> GetMistakes(string username, string password, Mode mode, string id)
    {
      string res = await SendRequestAsync("GetMistakes\n" + username + "\n" + password + "\n" + mode + "\n" + id);
      return res;
    }

    public async Task<string> DeleteEntry(string username, string password, Mode mode, string id)
    {
      string res = await SendRequestAsync("DeleteEntry\n" + username + "\n" + password + "\n" + mode + "\n" + id);
      return res;
    }

    public async Task<List<Entry>> GetStudentEntries(string username, string password, Mode mode, string studentUsername)
    {
      string response = await SendRequestAsync("GetStudentEntries\n" + username + "\n" + password + "\n" + mode + "\n" +
        studentUsername);
      List<Entry> result = new List<Entry>();
      var responseArr = response.Split('\n');
      if (response == "" || responseArr.Length == 0)
      {
        return result;
      }
      if (responseArr[0] == "FAILED")
      {
        MessageBox.Show(
            $"Locating student's entries {response}",
            "Entry results");
        return result;
      }
      foreach (string entry in response.Split('\n'))
      {
        try
        {
          result.Add(new Entry(entry));
        }
        catch (Exception ex) { }
      }
      return result;
    }
  }
}
