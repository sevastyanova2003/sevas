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
  public enum Mode
  {
    AdminMode,
    TeacherMode,
    StudentMode,
    NotChosenMode
  }


  public partial class AuthForm : Form
  {
    public string username = "";
    public string password = "";
    public Mode mode = Mode.NotChosenMode;
    public Client client;


    public AuthForm(Client client)
    {
      this.client = client;
      InitializeComponent();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      username = new WhiteSpaceRemoval(textBox1.Text).GetResult();
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
      password = new WhiteSpaceRemoval(textBox2.Text).GetResult();
    }

    private void radioButton3_CheckedChanged(object sender, EventArgs e)
    {
      mode = Mode.AdminMode;
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
      mode = Mode.StudentMode;
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
      mode = Mode.TeacherMode;
    }

    private async void button1_Click(object sender, EventArgs e)
    {
      if (mode == Mode.NotChosenMode)
      {
        MessageBox.Show(
        "You need to choose mode",
        "Error");
        return;
      }
      string authRes = await client.Auth(username, password, mode);
      if (authRes != "OK")
      {
        MessageBox.Show(
        authRes,
        "Error");
        return;
      }
      this.DialogResult = DialogResult.OK;
      Close();
    }
  }
}
