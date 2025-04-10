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
  public partial class StringEnterForm : Form
  {
    public string resultString = "";
    public StringEnterForm(string request)
    {
      InitializeComponent();
      label1.Text = request;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      resultString = new WhiteSpaceRemoval(textBox1.Text).GetResult();
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}
