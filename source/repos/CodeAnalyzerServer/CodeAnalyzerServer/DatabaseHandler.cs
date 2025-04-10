using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Npgsql;

namespace CodeAnalyzerServer
{
  public class DatabaseHandler
  {
    private string _connectionString;

    public DatabaseHandler(string connectionString)
    {
      _connectionString = connectionString;
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = "CREATE TABLE IF NOT EXISTS teachers(id SERIAL PRIMARY KEY, username VARCHAR(255) UNIQUE, password VARCHAR(255))";
      using var command = new NpgsqlCommand(commandText, con);
      command.ExecuteNonQuery();
      command.CommandText = "CREATE TABLE IF NOT EXISTS groups(id SERIAL PRIMARY KEY, name VARCHAR(255) UNIQUE, teacherName VARCHAR(255))";
      command.ExecuteNonQuery();
      command.CommandText = "CREATE TABLE IF NOT EXISTS students(id SERIAL PRIMARY KEY, username VARCHAR(255) UNIQUE, password VARCHAR(255), groupName VARCHAR(255))";
      command.ExecuteNonQuery();
      command.CommandText = "CREATE TABLE IF NOT EXISTS entries(id SERIAL PRIMARY KEY, studentUsername VARCHAR(255), problem VARCHAR(255), file TEXT, datetime TIMESTAMP, mistakes TEXT, mistakeCount INT)";
      command.ExecuteNonQuery();
      con.Close();
    }

    public bool authTeacher(string username, string password)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM teachers WHERE username = '{username}' AND password = '{password}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      con.Close();
      return res > 0;
    }

    public bool authStudent(string username, string password)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM students WHERE username = '{username}' AND password = '{password}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      con.Close();
      return res > 0;
    }

    public void addTeacher(string username, string password)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"INSERT INTO teachers(username, password) VALUES('{username}','{password}')";
      using var command = new NpgsqlCommand(commandText, con);
      command.ExecuteNonQuery();
      con.Close();
    }

    public void addGroup(string name, string teacher)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM teachers WHERE username = '{teacher}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64) command.ExecuteScalar();
      if (res != 0)
      {
        command.CommandText = $"INSERT INTO groups(name, teacherName) VALUES('{name}','{teacher}')";
        command.ExecuteNonQuery();
      } else
      {
        con.Close();
        throw new ArgumentException("Teacher not found in database");
      }
      con.Close();
    }

    public void updateGroupTeacher(string name, string teacher)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM teachers WHERE username = '{teacher}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64) command.ExecuteScalar();
      if (res != 0)
      {
        command.CommandText = $"UPDATE groups SET teacherName = '{teacher}' WHERE name = '{name}'";
        command.ExecuteNonQuery();
      } else
      {
        con.Close();
        throw new ArgumentException("Teacher not found in database");
      }
      con.Close();
    }

    public void addStudent(string username, string password, string group)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM groups WHERE name = '{group}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      if (res != 0)
      {
        command.CommandText = $"INSERT INTO students(username, password, groupName) VALUES('{username}', '{password}','{group}')";
        command.ExecuteNonQuery();
      }
      else
      {
        con.Close();
        throw new ArgumentException("Group not found in database");
      }
      con.Close();
    }

    public void updateStudentGroup(string username, string group)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM groups WHERE name = '{group}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      if (res != 0)
      {
        command.CommandText = $"UPDATE students SET groupName = '{group}' WHERE username = '{username}'";
        command.ExecuteNonQuery();
      }
      else
      {
        con.Close();
        throw new ArgumentException("Group not found in database");
      }
      con.Close();
    }

    public void deleteStudent(string username)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"DELETE FROM entries WHERE studentUsername='{username}'";
      using var command = new NpgsqlCommand(commandText, con);
      command.ExecuteNonQuery();
      command.CommandText = $"DELETE FROM students WHERE username='{username}'";
      command.ExecuteNonQuery();
      con.Close();
    }

    public void deleteGroup(string name)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM students WHERE groupName = '{name}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      if (res != 0)
      {
        con.Close();
        throw new ArgumentException("Cannot delete group that has students");
      }
      command.CommandText = $"DELETE FROM groups WHERE name='{name}'";
      command.ExecuteNonQuery ();
      con.Close();
    }

    public void deleteTeacher(string username)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM groups WHERE teacherName = '{username}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      if (res != 0)
      {
        con.Close();
        throw new ArgumentException("Cannot delete teacher that has groups");
      }
      command.CommandText = $"DELETE FROM teachers WHERE username = '{username}'";
      command.ExecuteNonQuery();
      con.Close();
    }

    public void addEntry(string student, string problem, string file, string mistakes, int mistakeCnt)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT COUNT(*) FROM students WHERE username = '{student}'";
      using var command = new NpgsqlCommand(commandText, con);
      Int64 res = (Int64)command.ExecuteScalar();
      if (res != 0)
      {
        command.CommandText = $"INSERT INTO entries(studentUsername, problem, file, datetime, mistakes, mistakeCount) VALUES('{student}', '{problem}','{file}', LOCALTIMESTAMP, '{mistakes}', {mistakeCnt})";
        command.ExecuteNonQuery();
      }
      else
      {
        con.Close();
        throw new ArgumentException("Student not found in database");
      }
      con.Close();
    }

    public string GetGroups(string username)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      StringBuilder stringBuilder = new StringBuilder();
      string commandText = $"SELECT * FROM groups WHERE teacherName = '{username}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      while (result.Read())
      {
        stringBuilder.Append(result.GetString(result.GetOrdinal("name")));
        stringBuilder.Append("\n");
      }
      con.Close();
      return stringBuilder.ToString();
    }

    public string GetStudents(string username)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      StringBuilder stringBuilder = new StringBuilder();
      string commandText = $"SELECT students.groupName, students.username FROM students JOIN groups" +
        $" ON students.groupName = groups.name WHERE groups.teacherName = '{username}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      while (result.Read())
      {
        stringBuilder.Append(result.GetString(0) + " ");
        stringBuilder.Append(result.GetString(1) + "\n");
      }
      con.Close();
      return stringBuilder.ToString();
    }

    public string getFile(int id, string username, string mode)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT * FROM entries WHERE id = '{id}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      result.Read();
      string fileText = result.GetString(result.GetOrdinal("file"));
      string student = result.GetString(result.GetOrdinal("studentUsername"));
      if (mode == "StudentMode" && student != username ||
        mode == "TeacherMode" && !CheckStudentTeacher(username, student))
      {
        con.Close();
        return "Access to entry denied";
      }
      con.Close();
      return fileText;
    }

    public string getMistakes(int id, string username, string mode)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT * FROM entries WHERE id = '{id}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      result.Read();
      string mistakes = result.GetString(result.GetOrdinal("mistakes"));
      string student = result.GetString(result.GetOrdinal("studentUsername"));
      if (mode == "StudentMode" && student != username ||
        mode == "TeacherMode" && !CheckStudentTeacher(username, student))
      {
        con.Close();
        return "Access to entry denied";
      }
      con.Close();
      return mistakes;
    }

    public string deleteEntry(int id, string username, string mode)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT * FROM entries WHERE id = '{id}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      result.Read();
      string student = result.GetString(result.GetOrdinal("studentUsername"));
      result.Close();
      if (mode != "StudentMode" || student != username)
      {
        con.Close();
        return "Access to entry denied";
      }
      command.CommandText = $"DELETE FROM entries WHERE id='{id}'";
      command.ExecuteNonQuery();
      con.Close();
      return "Entry deleted";
    }

    public bool CheckStudentTeacher(string teacherUsername, string studentUsername)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT * FROM students WHERE username = '{studentUsername}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      result.Read();
      string group = result.GetString(result.GetOrdinal("groupName"));
      result.Close();
      command.CommandText = $"SELECT * FROM groups WHERE name = '{group}'";
      result = command.ExecuteReader();
      result.Read();
      bool ans = result.GetString(result.GetOrdinal("teacherName")) == teacherUsername;
      con.Close();
      return ans;
    }

    public string GetStudentStats(string studentUsername)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      StringBuilder stringBuilder = new StringBuilder();
      string commandText = $"SELECT problem, AVG(mistakeCount) FROM entries WHERE studentUsername = '{studentUsername}' GROUP BY problem";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      double summ = 0;
      int cnt = 0;
      while (result.Read())
      {
        stringBuilder.Append(result.GetString(0) + " : ");
        double mistakeAvg = result.GetDouble(1);
        stringBuilder.Append(mistakeAvg);
        stringBuilder.Append("\n");
        summ += mistakeAvg;
        cnt++;
      }
      if (cnt == 0)
      {
        con.Close();
        return "No entries found";
      }
      stringBuilder.Append("All : ");
      stringBuilder.Append(summ / cnt);
      con.Close();
      return stringBuilder.ToString();
    }

    public string GetStudentGroup(string studentUsername)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      string commandText = $"SELECT groupName FROM students WHERE username = '{studentUsername}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      result.Read();
      string ans = result.GetString(0);
      con.Close();
      return ans;
    }

    public string GetGroupStats(string groupName)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      StringBuilder stringBuilder = new StringBuilder();
      string commandText = $"SELECT entries.studentUsername, AVG(entries.mistakeCount)" +
        $"FROM entries JOIN students ON entries.studentUsername = students.username WHERE students.groupName = '{groupName}' GROUP BY entries.studentUsername";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      double summ = 0;
      int cnt = 0;
      while (result.Read())
      {
        stringBuilder.Append(result.GetString(0) + " : ");
        double mistakeAvg = result.GetDouble(1);
        stringBuilder.Append(mistakeAvg);
        stringBuilder.Append("\n");
        summ += mistakeAvg;
        cnt++;
      }
      if (cnt == 0)
      {
        con.Close();
        return "No entries found";
      }
      stringBuilder.Append("All : ");
      stringBuilder.Append(summ / cnt);
      con.Close();
      return stringBuilder.ToString();
    }

    public string GetStudentEntries(string studentUsername)
    {
      using var con = new NpgsqlConnection(_connectionString);
      con.Open();
      StringBuilder stringBuilder = new StringBuilder();
      string commandText = $"SELECT * FROM entries WHERE studentUsername = '{studentUsername}'";
      using var command = new NpgsqlCommand(commandText, con);
      var result = command.ExecuteReader();
      while (result.Read())
      {
        stringBuilder.Append(result.GetInt32(result.GetOrdinal("id")));
        stringBuilder.Append(" " + result.GetString(result.GetOrdinal("problem")) + " ");
        stringBuilder.Append(result.GetDateTime(result.GetOrdinal("datetime")));
        stringBuilder.Append(" ");
        stringBuilder.Append(result.GetInt32(result.GetOrdinal("mistakeCount")));
        stringBuilder.Append("\n");
      }
      con.Close();
      return stringBuilder.ToString();
    }
  }
}