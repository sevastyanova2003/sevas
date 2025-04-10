namespace CodeAnalyzerWinForms
{
  partial class AuthForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      textBox1 = new TextBox();
      textBox2 = new TextBox();
      label1 = new Label();
      label2 = new Label();
      groupBox1 = new GroupBox();
      radioButton3 = new RadioButton();
      radioButton2 = new RadioButton();
      radioButton1 = new RadioButton();
      button1 = new Button();
      groupBox1.SuspendLayout();
      SuspendLayout();
      // 
      // textBox1
      // 
      textBox1.Location = new Point(307, 108);
      textBox1.Name = "textBox1";
      textBox1.Size = new Size(150, 31);
      textBox1.TabIndex = 0;
      textBox1.TextChanged += textBox1_TextChanged;
      // 
      // textBox2
      // 
      textBox2.Location = new Point(307, 170);
      textBox2.Name = "textBox2";
      textBox2.Size = new Size(150, 31);
      textBox2.TabIndex = 1;
      textBox2.TextChanged += textBox2_TextChanged;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(307, 80);
      label1.Name = "label1";
      label1.Size = new Size(134, 25);
      label1.TabIndex = 2;
      label1.Text = "Enter username";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(307, 142);
      label2.Name = "label2";
      label2.Size = new Size(134, 25);
      label2.TabIndex = 3;
      label2.Text = "Enter password";
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add(radioButton3);
      groupBox1.Controls.Add(radioButton2);
      groupBox1.Controls.Add(radioButton1);
      groupBox1.Location = new Point(238, 225);
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new Size(300, 150);
      groupBox1.TabIndex = 4;
      groupBox1.TabStop = false;
      groupBox1.Text = "Choose mode";
      // 
      // radioButton3
      // 
      radioButton3.AutoSize = true;
      radioButton3.Location = new Point(69, 100);
      radioButton3.Name = "radioButton3";
      radioButton3.Size = new Size(90, 29);
      radioButton3.TabIndex = 2;
      radioButton3.TabStop = true;
      radioButton3.Text = "Admin";
      radioButton3.UseVisualStyleBackColor = true;
      radioButton3.CheckedChanged += radioButton3_CheckedChanged;
      // 
      // radioButton2
      // 
      radioButton2.AutoSize = true;
      radioButton2.Location = new Point(69, 65);
      radioButton2.Name = "radioButton2";
      radioButton2.Size = new Size(95, 29);
      radioButton2.TabIndex = 1;
      radioButton2.TabStop = true;
      radioButton2.Text = "Teacher";
      radioButton2.UseVisualStyleBackColor = true;
      radioButton2.CheckedChanged += radioButton2_CheckedChanged;
      // 
      // radioButton1
      // 
      radioButton1.AutoSize = true;
      radioButton1.Location = new Point(69, 30);
      radioButton1.Name = "radioButton1";
      radioButton1.Size = new Size(98, 29);
      radioButton1.TabIndex = 0;
      radioButton1.TabStop = true;
      radioButton1.Text = "Student";
      radioButton1.UseVisualStyleBackColor = true;
      radioButton1.CheckedChanged += radioButton1_CheckedChanged;
      // 
      // button1
      // 
      button1.Location = new Point(325, 381);
      button1.Name = "button1";
      button1.Size = new Size(112, 34);
      button1.TabIndex = 5;
      button1.Text = "OK";
      button1.UseVisualStyleBackColor = true;
      button1.Click += button1_Click;
      // 
      // AuthForm
      // 
      AutoScaleDimensions = new SizeF(10F, 25F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(button1);
      Controls.Add(groupBox1);
      Controls.Add(label2);
      Controls.Add(label1);
      Controls.Add(textBox2);
      Controls.Add(textBox1);
      Name = "AuthForm";
      Text = "AuthForm";
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private TextBox textBox1;
    private TextBox textBox2;
    private Label label1;
    private Label label2;
    private GroupBox groupBox1;
    private RadioButton radioButton3;
    private RadioButton radioButton2;
    private RadioButton radioButton1;
    private Button button1;
  }
}