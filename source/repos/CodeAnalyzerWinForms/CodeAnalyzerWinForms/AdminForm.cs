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
  public partial class AdminForm : Form
  {
    private string _username;
    private string _password;
    private Client client;

    public AdminForm(string username, string password, Client client)
    {
      _username = username;
      _password = password;
      this.client = client;
      InitializeComponent();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
      StringEnterForm usernameForm = new StringEnterForm("Please, enter teacher username");
      if (usernameForm.ShowDialog() == DialogResult.OK)
      {
        StringEnterForm passwordForm = new StringEnterForm("Please, enter teacher password");
        if (passwordForm.ShowDialog() == DialogResult.OK)
        {
          string res = await client.AddTeacher(_username, _password, usernameForm.resultString, passwordForm.resultString);
          MessageBox.Show(
            res,
            "Action results");
        }
      }
    }

    private async void button2_Click(object sender, EventArgs e)
    {
      StringEnterForm nameForm = new StringEnterForm("Please, enter group name");
      if (nameForm.ShowDialog() == DialogResult.OK)
      {
        StringEnterForm teacherForm = new StringEnterForm("Please, enter teacher of group");
        if (teacherForm.ShowDialog() == DialogResult.OK)
        {
          string res = await client.AddGroup(_username, _password, nameForm.resultString, teacherForm.resultString);
          MessageBox.Show(
            res,
            "Action results");
        }
      }
    }

    private async void button3_Click(object sender, EventArgs e)
    {
      StringEnterForm usernameForm = new StringEnterForm("Please, enter student username");
      if (usernameForm.ShowDialog() == DialogResult.OK)
      {
        StringEnterForm passwordForm = new StringEnterForm("Please, enter student password");
        if (passwordForm.ShowDialog() == DialogResult.OK)
        {
          StringEnterForm groupNameForm = new StringEnterForm("Please, enter group of student");
          if (groupNameForm.ShowDialog() == DialogResult.OK)
          {
            string res = await client.AddStudent(_username, _password,
              usernameForm.resultString, passwordForm.resultString, groupNameForm.resultString);
            MessageBox.Show(
              res,
              "Action results");
          }
        }
      }
    }

    private async void button4_Click(object sender, EventArgs e)
    {
      StringEnterForm nameForm = new StringEnterForm("Please, enter group name");
      if (nameForm.ShowDialog() == DialogResult.OK)
      {
        StringEnterForm teacherForm = new StringEnterForm("Please, enter new teacher of group");
        if (teacherForm.ShowDialog() == DialogResult.OK)
        {
          string res = await client.EditGroup(_username, _password, nameForm.resultString, teacherForm.resultString);
          MessageBox.Show(
            res,
            "Action results");
        }
      }
    }

    private async void button5_Click(object sender, EventArgs e)
    {
      StringEnterForm usernameForm = new StringEnterForm("Please, enter student username");
      if (usernameForm.ShowDialog() == DialogResult.OK)
      {
        StringEnterForm groupNameForm = new StringEnterForm("Please, enter new group of student");
        if (groupNameForm.ShowDialog() == DialogResult.OK)
        {
          string res = await client.EditStudent(_username, _password, usernameForm.resultString, groupNameForm.resultString);
          MessageBox.Show(
            res,
            "Action results");
        }
      }
    }

    private async void button6_Click(object sender, EventArgs e)
    {
      StringEnterForm usernameForm = new StringEnterForm("Please, enter teacher username");
      if (usernameForm.ShowDialog() == DialogResult.OK)
      {
        string res = await client.DeleteTeacher(_username, _password, usernameForm.resultString);
        MessageBox.Show(
          res,
          "Action results");
      }
    }

    private async void button7_Click(object sender, EventArgs e)
    {
      StringEnterForm nameForm = new StringEnterForm("Please, enter group name");
      if (nameForm.ShowDialog() == DialogResult.OK)
      {
        string res = await client.DeleteGroup(_username, _password, nameForm.resultString);
        MessageBox.Show(
          res,
          "Action results");
      }
    }

    private async void button8_Click(object sender, EventArgs e)
    {
      StringEnterForm usernameForm = new StringEnterForm("Please, enter student username");
      if (usernameForm.ShowDialog() == DialogResult.OK)
      {
        string res = await client.DeleteStudent(_username, _password, usernameForm.resultString);
        MessageBox.Show(
          res,
          "Action results");
      }
    }
  }
}
