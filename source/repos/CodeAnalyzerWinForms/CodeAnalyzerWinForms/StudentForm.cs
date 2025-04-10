using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CodeAnalyzerWinForms
{
  public partial class StudentForm : Form
  {
    private string _username;
    private string _password;
    private Client _client;
    public StudentForm(string username, string password, Client client)
    {
      _username = username;
      _password = password;
      _client = client;
      InitializeComponent();
      FillGrid();
    }

    private async void FillGrid()
    {
      dataGridView1.Rows.Clear();
      List<Entry> entries = await _client.GetStudentEntries(_username, _password, Mode.StudentMode, _username);
      foreach (Entry x in entries)
      {
        dataGridView1.Rows.Add(x.id, x.problem, x.dateTime, x.mistakeCnt);
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
          string mistakes = await _client.GetMistakes(_username, _password, Mode.StudentMode, id);
          MessageBox.Show(mistakes, "Mistakes found in entry");
        } else if (e.ColumnIndex == 4)
        {
          string fileText = await _client.GetFile(_username, _password, Mode.StudentMode, id);
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = "C# files (.cs) | *.cs";
          if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
            return;
          string filename = saveFileDialog.FileName;
          System.IO.File.WriteAllText(filename, fileText);
          MessageBox.Show($"Saved file as {filename}");
        } else if (e.ColumnIndex == 6)
        {
          string res = await _client.DeleteEntry(_username, _password, Mode.StudentMode, id);
          MessageBox.Show(res);
          FillGrid();
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

    private async void button1_Click(object sender, EventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "C# files (.cs) | *.cs";
      dialog.Multiselect = false;
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        string path = dialog.FileName;
        string text = "";
        try
        {
          StreamReader reader = new StreamReader(path);
          text = reader.ReadToEnd();
        }
        catch (Exception ex)
        {
          MessageBox.Show(
          "Failed to read file",
          "Entry results");
          return;
        }
        StringEnterForm problemForm = new StringEnterForm("Please, enter name of problem");
        if (problemForm.ShowDialog() == DialogResult.OK)
        {
          string res = await _client.SendEntry(_username, _password, problemForm.resultString, text);
          MessageBox.Show(
            res,
            "Mistakes in file");
          FillGrid();
        }
      }
    }

    private async void button2_Click(object sender, EventArgs e)
    {
      string userStats = await _client.GetStudentStats(_username, _password, Mode.StudentMode, _username);
      MessageBox.Show(
        userStats,
        "Your Stats");
    }

    private async void button3_Click(object sender, EventArgs e)
    {
      string groupStats = await _client.GetGroupStats(_username, _password);
      MessageBox.Show(
        groupStats,
        "Group Stats");
    }
  }
}
