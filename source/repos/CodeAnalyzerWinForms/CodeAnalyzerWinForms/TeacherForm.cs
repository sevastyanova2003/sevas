using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAnalyzerWinForms
{
  public partial class TeacherForm : Form
  {
    private string _username = "";
    private string _password = "";
    private string _selected_group = "";
    private string _selected_student = "";
    private Dictionary<string, List<string>> _students_by_group;
    private Client _client;

    public TeacherForm(string username, string password, Client client)
    {
      _username = username;
      _password = password;
      _client = client;
      InitializeComponent();
      FillComboBox();
    }

    public async void FillComboBox()
    {
      List<string> groups = await _client.GetGroups(_username, _password);
      _students_by_group = await _client.GetStudents(_username, _password);

      foreach (string group in groups)
      {
        if (group != null && group != "")
        {
          comboBox1.Items.Add(group);
        }
      }
    }

    private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (IsANonHeaderButtonCell(e))
      {
        object? val = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
        if (val == null)
        {
          return;
        }
        string? id = val.ToString();
        if (id == null || id == "")
        {
          return;
        }
        if (e.ColumnIndex == 5)
        {
          string mistakes = await _client.GetMistakes(_username, _password, Mode.TeacherMode, id);
          MessageBox.Show(mistakes, "Mistakes found in entry");
        }
        else if (e.ColumnIndex == 4)
        {
          string fileText = await _client.GetFile(_username, _password, Mode.TeacherMode, id);
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = "C# files (.cs) | *.cs";
          if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
            return;
          string filename = saveFileDialog.FileName;
          System.IO.File.WriteAllText(filename, fileText);
          MessageBox.Show($"Saved file as {filename}");
        }
      }
    }

    private bool IsANonHeaderButtonCell(DataGridViewCellEventArgs cellEvent)
    {
      if (dataGridView1.Columns[cellEvent.ColumnIndex] is
          DataGridViewButtonColumn &&
          cellEvent.RowIndex != -1)
      { return true; }
      else { return (false); }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBox1.SelectedItem == null)
      {
        return;
      }
      _selected_group = comboBox1.SelectedItem.ToString() ?? "";
      comboBox2.Items.Clear();
      if (_students_by_group.ContainsKey(_selected_group))
      {
        foreach (string student in _students_by_group[_selected_group])
        {
          comboBox2.Items.Add(student);
        }
      }
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBox2.SelectedItem == null)
      {
        return;
      }
      _selected_student = comboBox2.SelectedItem.ToString() ?? "";
    }

    private async void button1_Click(object sender, EventArgs e)
    {
      string groupStats = await _client.GetGroupStats(_username, _password, _selected_group);
      MessageBox.Show(
        groupStats,
        $"Group {_selected_group} Stats");
    }

    private async void button2_Click(object sender, EventArgs e)
    {
      string userStats = await _client.GetStudentStats(_username, _password, Mode.TeacherMode, _selected_student);
      MessageBox.Show(
        userStats,
        $"{_selected_student} stats");
    }

    private async void button3_Click(object sender, EventArgs e)
    {
      List<Entry> entries = await _client.GetStudentEntries(_username, _password, Mode.TeacherMode,
        _selected_student);
      dataGridView1.Rows.Clear();
      foreach (Entry x in entries)
      {
        dataGridView1.Rows.Add(x.id, x.problem, x.dateTime, x.mistakeCnt);
      }
    }
  }
}
