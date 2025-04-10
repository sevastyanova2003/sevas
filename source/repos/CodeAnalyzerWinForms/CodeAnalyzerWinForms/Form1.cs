namespace CodeAnalyzerWinForms
{
  public partial class Form1 : Form
  {
    private string _username = "";
    private string _password = "";
    private Mode _mode = Mode.NotChosenMode;
    public Client client;
    public Form1()
    {
      client = new Client();
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Hide();
      AuthForm newForm = new AuthForm(client);
      if (newForm.ShowDialog() != DialogResult.OK)
      {
        Show();
        return;
      }
      _username = newForm.username;
      _password = newForm.password;
      _mode = newForm.mode;
      if (_mode == Mode.StudentMode)
      {
        StudentForm studentForm = new StudentForm(_username, _password, client);
        studentForm.ShowDialog();
      }
      else if (_mode == Mode.TeacherMode)
      {
        TeacherForm teacherForm = new TeacherForm(_username, _password, client);
        teacherForm.ShowDialog();
      }
      else if (_mode == Mode.AdminMode)
      {
        AdminForm adminForm = new AdminForm(_username, _password, client);
        adminForm.ShowDialog();
      }
      Show();
    }
  }
}