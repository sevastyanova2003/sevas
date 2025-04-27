using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeAnalyzerServer
{
  /// <summary>
  /// Class for handling string requests and generating string responses
  /// </summary>
  public class RequestHandler
  {
    DatabaseHandler databaseHandler;
    QueryText queryText;
    string adminUsername = "admin";
    string adminPassword = "123";

    /// <summary>
    /// Creates RequestHandler with <paramref name="handler"/>
    /// </summary>
    /// <param name="handler"></param>
    public RequestHandler(DatabaseHandler handler)
    {
      databaseHandler = handler;
      queryText = new QueryText();
    }

    /// <summary>
    /// Handles request <paramref name="message"/> and generates string response
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public string GetResponse(string message)
    {
      if (message == null || message.Length == 0)
      {
        return "Incorrect request";
      } else
      {
        List<string> messageList = message.Split('\n').ToList();
        if (messageList.Count == 0)
        {
          return "Incorrect request";
        }
        switch (messageList[0])
        {
          case "AUTH":
            bool check = GetAuth(messageList);
            if (check)
            {
              return "OK";
            } else
            {
              return "Authorization failed: incorrect username or password";
            }
          case "AddTeacher":
            return GetTeacherAdded(messageList);
          case "AddGroup":
            return GetGroupAdded(messageList);
          case "AddStudent":
            return GetStudentAdded(messageList);
          case "EditGroup":
            return GetGroupUpdated(messageList);
          case "EditStudent":
            return GetStudentUpdated(messageList);
          case "DeleteTeacher":
            return GetTeacherDeleted(messageList);
          case "DeleteGroup":
            return GetGroupDeleted(messageList);
          case "DeleteStudent":
            return GetStudentDeleted(messageList);
          case "GetStudentEntries":
            return GetStudentEntries(messageList);
          case "GetEntryAdded":
            return GetEntryAdded(messageList, message);
          case "GetFile":
            return GetFile(messageList);
          case "GetMistakes":
            return GetMistakes(messageList);
          case "GetStudentStats":
            return GetStudentStats(messageList);
          case "GetGroupStats":
            return GetGroupStats(messageList);
          case "GetGroups":
            return GetGroups(messageList);
          case "GetStudents":
            return GetStudents(messageList);
          case "DeleteEntry":
            return DeleteEntry(messageList);
        }
      }
      return "Incorrect request";
    }

    /// <summary>
    /// Handles GetStudentStats request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetStudentStats(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "Authorization failed. Try logging in again";
      }
      if (messageList.Count != 5)
      {
        return "Incorrect request";
      }
      try
      {
        if (messageList[3] == "StudentMode")
        {
          return databaseHandler.GetStudentStats(messageList[1]);
        }
        else if (messageList[3] == "TeacherMode")
        {
          Console.WriteLine("In teacher mode");
          if (!databaseHandler.CheckStudentTeacher(messageList[1], messageList[4]))
          {
            return "FAILED\nStudent not in teacher's group";
          }
          Console.WriteLine("checked student teacher");
          return databaseHandler.GetStudentStats(messageList[4]);
        }
      }
      catch (Exception e)
      {
        return $"FAILED\n{e.Message}";
      }
      return "Incorrect request";
    }

    /// <summary>
    /// Handles getFile request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetFile(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "Authorization failed. Try logging in again";
      }
      try
      {
        int id = Int32.Parse(messageList[4]);
        return databaseHandler.getFile(id, messageList[1], messageList[3]);
      } catch (Exception e)
      {
        return $"Failed to get file {e.Message}";
      }
    }

    /// <summary>
    /// Handles getMistakes request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetMistakes(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "Authorization failed. Try logging in again";
      }
      try
      {
        int id = Int32.Parse(messageList[4]);
        return databaseHandler.getMistakes(id, messageList[1], messageList[3]);
      }
      catch (Exception e)
      {
        return $"Failed to get mistakes {e.Message}";
      }
    }

    /// <summary>
    /// Handles DeleteEntry request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string DeleteEntry(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "Authorization failed. Try logging in again";
      }
      try
      {
        int id = Int32.Parse(messageList[4]);
        return databaseHandler.deleteEntry(id, messageList[1], messageList[3]);
      }
      catch (Exception e)
      {
        return $"Failed to delete entry {e.Message}";
      }
    }

    /// <summary>
    /// Handles get teacher's groups request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetGroups(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "FAILED\nCould not get group list.\nAuthorization failed. Try logging in again";
      }
      try
      {
        return databaseHandler.GetGroups(messageList[1]);
      } catch (Exception e)
      {
        return $"FAILED\nCould not get group list.\n{e.Message}";
      }
    }

    /// <summary>
    /// Handles get teacher's students request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetStudents(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "FAILED\nCould not get student list.\nAuthorization failed. Try logging in again";
      }
      try
      {
        return databaseHandler.GetStudents(messageList[1]);
      }
      catch (Exception e)
      {
        return $"FAILED\nCould not get student list.\n{e.Message}";
      }
    }

    /// <summary>
    /// Handles entry addition requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public string GetEntryAdded(List<string> messageList, string message)
    {
      if (!GetAuth(messageList))
      {
        return "Authorization failed. Try logging in again";
      }
      if (messageList[3] != "StudentMode" || messageList.Count < 6)
      {
        return "Incorrect request";
      }
      try
      {
        string fileText = message.Substring(messageList[0].Length + messageList[1].Length
        + messageList[2].Length + messageList[3].Length + messageList[4].Length + "\n".Length * 5);
        Analyzer analyzer = new Analyzer(fileText);
        List<string> checkRes = analyzer.Analyze();
        string mistakes = String.Join("\n", checkRes);
        databaseHandler.addEntry(messageList[1], messageList[4], queryText.TextToQuery(fileText), mistakes, checkRes.Count);
        return mistakes;
      }
      catch (Exception e)
      {
        return $"Entry Addition Failed. {e.Message}";
      }
    }

    /// <summary>
    /// Handles get group's stats request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetGroupStats(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "Authorization failed. Try logging in again";
      }
      if (messageList[3] == "StudentMode")
      {
        if (messageList.Count != 4)
        {
          return "Incorrect request";
        }
        try
        {
          string groupName = databaseHandler.GetStudentGroup(messageList[1]);
          return databaseHandler.GetGroupStats(groupName);
        } catch (Exception e)
        {
          return $"FAILED\n{e.Message}";
        }
      } else if (messageList[3] == "TeacherMode")
      {
        if (messageList.Count != 5)
        {
          return "Incorrect request";
        }
        try
        {
          return databaseHandler.GetGroupStats(messageList[4]);
        } catch (Exception e)
        {
          return $"FAILED\n{e.Message}";
        }
      }
      return "Incorrect request";
    }

    /// <summary>
    /// Handles getStudentEntries request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetStudentEntries(List<string> messageList)
    {
      if (!GetAuth(messageList))
      {
        return "FAILED\nAuthorization failed. Try logging in again";
      }
      if (messageList.Count != 5)
      {
        return "FAILED\nIncorrect request";
      }
      try
      {
        if (messageList[3] == "StudentMode")
        {
          return databaseHandler.GetStudentEntries(messageList[1]);
        }
        else if (messageList[3] == "TeacherMode")
        {
          if (!databaseHandler.CheckStudentTeacher(messageList[1], messageList[4]))
          {
            return "FAILED\nStudent not in teacher's group";
          }
          return databaseHandler.GetStudentEntries(messageList[4]);
        }
      } catch (Exception e)
      {
        return $"FAILED\n{e.Message}";
      }
      return "FAILED\nIncorrect request";
    }

    /// <summary>
    /// Handles teacher addition requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetTeacherAdded(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 6)
      {
        return "Incorrect request";
      }
      try
      {
        databaseHandler.addTeacher(messageList[4], messageList[5]);
      } catch (Exception e)
      {
        return $"Teacher not added. {e.Message}";
      }
      return "Teacher added";
    }

    /// <summary>
    /// Handles GetTeacherDeleted request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetTeacherDeleted(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 5)
      {
        return "Incorrect request";
      }
      try
      {
        databaseHandler.deleteTeacher(messageList[4]);
      }
      catch (Exception e)
      {
        return $"Teacher not deleted. {e.Message}";
      }
      return "Teacher deleted";
    }

    /// <summary>
    /// Handles GetGroupDeleted requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetGroupDeleted(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 5)
      {
        return "Incorrect request";
      }
      try
      {
        databaseHandler.deleteGroup(messageList[4]);
      }
      catch (Exception e)
      {
        return $"Group not deleted. {e.Message}";
      }
      return "Group deleted";
    }

    /// <summary>
    /// Handles GetStudentDeleted requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetStudentDeleted(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 5)
      {
        return "Incorrect request";
      }
      try
      {
        databaseHandler.deleteStudent(messageList[4]);
      }
      catch (Exception e)
      {
        return $"Student not deleted. {e.Message}";
      }
      return "Student deleted";
    }

    /// <summary>
    /// Handles student addition requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetStudentAdded(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 7)
      {
        return "Incorrect request";
      }
      try
      {
      databaseHandler.addStudent(messageList[4], messageList[5], messageList[6]);
      }
      catch (Exception e)
      {
        return $"Student not added. {e.Message}";
      }
      return "Student added";
    }

    /// <summary>
    /// Handles group addition requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetGroupAdded(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 6)
      {
        return "Incorrect request";
      }
      try
      {
      databaseHandler.addGroup(messageList[4], messageList[5]);
      }
      catch (Exception e)
      {
        return $"Group not added. {e.Message}";
      }
      return "Group added";
    }

    /// <summary>
    /// Handles update of group's teacher requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetGroupUpdated(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 6)
      {
        return "Incorrect request";
      }
      try
      {
        databaseHandler.updateGroupTeacher(messageList[4], messageList[5]);
      }
      catch (Exception e)
      {
        return $"Group teacher not updated. {e.Message}";
      }
      return "Group teacher updated";
    }

    /// <summary>
    /// Handles update of student's group requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public string GetStudentUpdated(List<string> messageList)
    {
      if (!GetAdminAuth(messageList))
      {
        return "Admin authorization failed. Try logging in again";
      }
      if (messageList.Count != 6)
      {
        return "Incorrect request";
      }
      try
      {
        databaseHandler.updateStudentGroup(messageList[4], messageList[5]);
      }
      catch (Exception e)
      {
        return $"Student group not updated. {e.Message}";
      }
      return "Student group updated";
    }

    /// <summary>
    /// Handles admin authorization request
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public bool GetAdminAuth(List<string> messageList)
    {
      return GetAuth(messageList) && messageList[3] == "AdminMode";
    }

    /// <summary>
    /// Handles authorization requests
    /// </summary>
    /// <param name="messageList"></param>
    /// <returns></returns>
    public bool GetAuth(List<string> messageList)
    {
      if (messageList.Count < 4)
      {
        return false;
      }
      bool res = false;
      switch (messageList[3])
      {
        case "AdminMode":
          res = messageList[1] == adminUsername && messageList[2] == adminPassword;
          break;
        case "StudentMode":
          try
          {
            res = databaseHandler.authStudent(messageList[1], messageList[2]);
          }
          catch (Exception e)
          {
            res = false;
          }
          break;
        case "TeacherMode":
          try
          {
            res = databaseHandler.authTeacher(messageList[1], messageList[2]);
          } catch (Exception e)
          {
            res = false;
          }
          break;
      }
      return res;
    }
  }
}
