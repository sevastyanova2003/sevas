using CodeAnalyzerServer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
  public class DatabaseHandlerTests
  {
    private DatabaseHandler _handler;
    public DatabaseHandlerTests()
    {
      string connectionString = "Host=localhost;Username=postgres;Password=fbje22;Database=codeanalyzertestdb";
      using var con = new NpgsqlConnection(connectionString);
      con.Open();
      using var command = new NpgsqlCommand("", con);
      command.CommandText = "DROP TABLE IF EXISTS teachers";
      command.ExecuteNonQuery();
      command.CommandText = "DROP TABLE IF EXISTS groups";
      command.ExecuteNonQuery();
      command.CommandText = "DROP TABLE IF EXISTS students";
      command.ExecuteNonQuery();
      command.CommandText = "DROP TABLE IF EXISTS entries";
      command.ExecuteNonQuery();
      _handler = new DatabaseHandler(connectionString);
    }

    [Fact]
    public void TeacherAuthTest()
    {
      // Arrange
      _handler.addTeacher("teacher", "password");
      // Act & Assert
      Assert.True(_handler.authTeacher("teacher", "password"));
      Assert.False(_handler.authTeacher("teacher", "passwordWrong"));
      Assert.False(_handler.authTeacher("teacherWrong", "password"));
    }

    [Fact]
    public void StudentAuthTest()
    {
      // Arrange
      _handler.addTeacher("feeny", "bmw");
      _handler.addGroup("group1", "feeny");
      _handler.addStudent("cory", "cory123", "group1");
      // Act & Assert
      Assert.True(_handler.authStudent("cory", "cory123"));
      Assert.False(_handler.authStudent("cory", "cory234"));
      Assert.False(_handler.authTeacher("Cory", "cory123"));
    }

    [Fact]
    public void TeacherAddTest()
    {
      // Arrange
      _handler.addTeacher("white", "password");
      // Act & Assert
      Assert.Throws<Npgsql.PostgresException>(() => _handler.addTeacher("white", "password"));
    }

    [Fact]
    public void GroupAddTest()
    {
      // Arrange
      _handler.addTeacher("walter", "password");
      _handler.addGroup("group2", "walter");
      // Act & Assert
      Assert.Throws<Npgsql.PostgresException>(() => _handler.addGroup("group2", "walter"));
      Assert.Throws<ArgumentException>(() => _handler.addGroup("group3", "notATeacher"));
    }

    [Fact]
    public void StudentAddTest()
    {
      // Arrange
      _handler.addTeacher("averina", "123");
      _handler.addGroup("vesna", "averina");
      _handler.addStudent("polina", "123", "vesna");
      // Act & Assert
      Assert.Throws<Npgsql.PostgresException>(() => _handler.addStudent("polina", "123", "vesna"));
      Assert.Throws<ArgumentException>(() => _handler.addStudent("sasha", "123", "notAGroup"));
    }

    [Fact]
    public void GroupUpdateTest()
    {
      // Arrange
      _handler.addTeacher("walterWhite", "password");
      _handler.addTeacher("walterBlack", "123");
      _handler.addGroup("groupUpdate", "walterWhite");
      // Act & Assert
      Assert.Throws<ArgumentException>(() => _handler.updateGroupTeacher("groupUpdate", "notATeacher"));
      _handler.updateGroupTeacher("groupUpdate", "walterBlack");
      Assert.Equal("groupUpdate\n", _handler.GetGroups("walterBlack"));
      Assert.Equal("", _handler.GetGroups("walterWhite"));
    }

    [Fact]
    public void StudentUpdateTest()
    {
      // Arrange
      _handler.addTeacher("dumbledor", "password");
      _handler.addGroup("griffindor", "dumbledor");
      _handler.addGroup("slytherin", "dumbledor");
      _handler.addStudent("potter", "249", "griffindor");
      // Act & Assert
      Assert.Throws<ArgumentException>(() => _handler.updateStudentGroup("potter", "notAGroup"));
      _handler.updateStudentGroup("potter", "slytherin");
      Assert.Equal("slytherin", _handler.GetStudentGroup("potter"));
    }

    [Fact]
    public void StudentDeleteTest()
    {
      // Arrange
      _handler.addTeacher("snape", "password");
      _handler.addGroup("ravenclaw", "snape");
      _handler.addStudent("luna", "249", "ravenclaw");
      // Act
      _handler.deleteStudent("luna");
      // Assert
      Assert.False(_handler.authStudent("luna", "249"));
    }

    [Fact]
    public void GroupDeleteTest()
    {
      // Arrange
      _handler.addTeacher("minerva", "password");
      _handler.addGroup("huff", "minerva");
      _handler.addStudent("cedric", "249", "huff");
      // Act & Assert
      Assert.Throws<ArgumentException>(() => _handler.deleteGroup("huff"));
      _handler.deleteStudent("cedric");
      _handler.deleteGroup("huff");
      Assert.Equal("", _handler.GetGroups("minerva"));
    }

    [Fact]
    public void TeacherDeleteTest()
    {
      // Arrange
      _handler.addTeacher("flitwick", "password");
      _handler.addGroup("ravens", "flitwick");
      // Act & Assert
      Assert.Throws<ArgumentException>(() => _handler.deleteTeacher("flitwick"));
      _handler.deleteGroup("ravens");
      _handler.deleteTeacher("flitwick");
      Assert.False(_handler.authTeacher("flitwick", "password"));
    }

    [Fact]
    public void EntryAddTest()
    {
      // Arrange
      _handler.addTeacher("chris", "password");
      _handler.addGroup("chrestomanci", "chris");
      _handler.addStudent("cat", "249", "chrestomanci");
      // Act & Assert
      Assert.Throws<ArgumentException>(() => _handler.addEntry("notAStudent", "", "", "", 0));
      _handler.addEntry("cat", "A", "", "", 0);
      Assert.NotEqual("", _handler.GetStudentEntries("cat"));
    }

    [Fact]
    public void GetStudentsTest()
    {
      // Arrange
      _handler.addTeacher("howl", "243");
      _handler.addGroup("castle", "howl");
      _handler.addStudent("michael", "123", "castle");
      _handler.addStudent("sophie", "123", "castle");
      _handler.addGroup("bakery", "howl");
      _handler.addStudent("martha", "123", "bakery");

      // Act & Assert
      Assert.Equal("castle michael\ncastle sophie\nbakery martha\n", _handler.GetStudents("howl"));
      Assert.Equal("", _handler.GetStudents("notATeacher"));
    }

    [Fact]
    public void GetFileTest()
    {
      // Arrange
      _handler.addTeacher("jiminy", "password");
      _handler.addTeacher("james", "password");
      _handler.addGroup("corner", "jiminy");
      _handler.addStudent("pinocchio", "235", "corner");
      _handler.addEntry("pinocchio", "A", "text", "mistakeList", 1);
      string entries = _handler.GetStudentEntries("pinocchio");
      int id = int.Parse(entries.Split(" ")[0]);
      // Act & Assert
      Assert.Equal("Access to entry denied", _handler.getFile(id, "notP", "StudentMode"));
      Assert.Equal("Access to entry denied", _handler.getFile(id, "james", "TeacherMode"));
      Assert.Equal("text", _handler.getFile(id, "pinocchio", "StudentMode"));
      Assert.Equal("text", _handler.getFile(id, "jiminy", "TeacherMode"));
    }

    [Fact]
    public void GetMistakesTest()
    {
      // Arrange
      _handler.addTeacher("stacy", "password");
      _handler.addTeacher("phillips", "password");
      _handler.addGroup("avonlea", "stacy");
      _handler.addStudent("shirley", "235", "avonlea");
      _handler.addEntry("shirley", "A", "text", "mistakeList", 1);
      string entries = _handler.GetStudentEntries("shirley");
      int id = int.Parse(entries.Split(" ")[0]);
      // Act & Assert
      Assert.Equal("Access to entry denied", _handler.getMistakes(id, "notShirley", "StudentMode"));
      Assert.Equal("Access to entry denied", _handler.getMistakes(id, "phillips", "TeacherMode"));
      Assert.Equal("mistakeList", _handler.getMistakes(id, "shirley", "StudentMode"));
      Assert.Equal("mistakeList", _handler.getMistakes(id, "stacy", "TeacherMode"));
    }

    [Fact]
    public void EntryDeleteTest()
    {
      // Arrange
      _handler.addTeacher("shifu", "password");
      _handler.addGroup("kungfu", "shifu");
      _handler.addStudent("po", "235", "kungfu");
      _handler.addEntry("po", "A", "text", "mistakeList", 1);
      string entries = _handler.GetStudentEntries("po");
      int id = int.Parse(entries.Split(" ")[0]);
      // Act & Assert
      Assert.Equal("Access to entry denied", _handler.deleteEntry(id, "tigress", "StudentMode"));
      Assert.Equal("Entry deleted", _handler.deleteEntry(id, "po", "StudentMode"));
      Assert.Equal("", _handler.GetStudentEntries("po"));
    }

    [Fact]
    public void CheckStudentTeacherTest()
    {
      // Arrange
      _handler.addTeacher("arefieva", "password");
      _handler.addGroup("4a", "arefieva");
      _handler.addStudent("liza", "235", "4a");
      _handler.addTeacher("svetlana", "password");
      _handler.addGroup("3a", "svetlana");
      _handler.addStudent("alena", "235", "3a");
      // Act & Assert
      Assert.True(_handler.CheckStudentTeacher("arefieva", "liza"));
      Assert.False(_handler.CheckStudentTeacher("svetlana", "liza"));
      Assert.False(_handler.CheckStudentTeacher("arefieva", "alena"));
    }

    [Fact]
    public void GetStudentStatsTest()
    {
      // Act & Assert
      Assert.Equal("No entries found", _handler.GetStudentStats("notAStudent"));
    }

    [Fact]
    public void GetStudentGroup()
    {
      // Arrange
      _handler.addTeacher("glushko", "password");
      _handler.addGroup("239", "glushko");
      _handler.addStudent("olya", "password", "239");
      // Act & Assert
      Assert.Equal("239", _handler.GetStudentGroup("olya"));
    }

    [Fact]
    public void GetGroupStatsTest()
    {
      // Act & Assert
      Assert.Equal("No entries found", _handler.GetGroupStats("notAGroup"));
    }

    [Fact]
    public void GetStudentEntriesTest()
    {
      // Act & Assert
      Assert.Equal("", _handler.GetStudentEntries("notAStudent"));
    }
  }
}
