namespace CodeAnalyzerWinForms
{
  partial class TeacherForm
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
      comboBox1 = new ComboBox();
      button1 = new Button();
      comboBox2 = new ComboBox();
      button2 = new Button();
      button3 = new Button();
      dataGridView1 = new DataGridView();
      Id = new DataGridViewTextBoxColumn();
      Problem = new DataGridViewTextBoxColumn();
      SendingDate = new DataGridViewTextBoxColumn();
      MistakeCount = new DataGridViewTextBoxColumn();
      LoadFile = new DataGridViewButtonColumn();
      ViewMistakes = new DataGridViewButtonColumn();
      ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
      SuspendLayout();
      // 
      // comboBox1
      // 
      comboBox1.FormattingEnabled = true;
      comboBox1.Location = new Point(28, 31);
      comboBox1.Name = "comboBox1";
      comboBox1.Size = new Size(182, 33);
      comboBox1.TabIndex = 0;
      comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
      // 
      // button1
      // 
      button1.Location = new Point(241, 31);
      button1.Name = "button1";
      button1.Size = new Size(157, 34);
      button1.TabIndex = 1;
      button1.Text = "Get Group Stats";
      button1.UseVisualStyleBackColor = true;
      button1.Click += button1_Click;
      // 
      // comboBox2
      // 
      comboBox2.FormattingEnabled = true;
      comboBox2.Location = new Point(28, 82);
      comboBox2.Name = "comboBox2";
      comboBox2.Size = new Size(182, 33);
      comboBox2.TabIndex = 2;
      comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
      // 
      // button2
      // 
      button2.Location = new Point(241, 82);
      button2.Name = "button2";
      button2.Size = new Size(157, 34);
      button2.TabIndex = 3;
      button2.Text = "Get Student Stats";
      button2.UseVisualStyleBackColor = true;
      button2.Click += button2_Click;
      // 
      // button3
      // 
      button3.Location = new Point(431, 81);
      button3.Name = "button3";
      button3.Size = new Size(203, 34);
      button3.TabIndex = 4;
      button3.Text = "View Student History";
      button3.UseVisualStyleBackColor = true;
      button3.Click += button3_Click;
      // 
      // dataGridView1
      // 
      dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Id, Problem, SendingDate, MistakeCount, LoadFile, ViewMistakes });
      dataGridView1.Location = new Point(1, 139);
      dataGridView1.Name = "dataGridView1";
      dataGridView1.ReadOnly = true;
      dataGridView1.RowHeadersWidth = 62;
      dataGridView1.RowTemplate.Height = 33;
      dataGridView1.Size = new Size(959, 225);
      dataGridView1.TabIndex = 6;
      dataGridView1.CellContentClick += dataGridView1_CellContentClick;
      // 
      // Id
      // 
      Id.HeaderText = "Id";
      Id.MinimumWidth = 8;
      Id.Name = "Id";
      Id.ReadOnly = true;
      Id.Width = 150;
      // 
      // Problem
      // 
      Problem.HeaderText = "Problem";
      Problem.MinimumWidth = 8;
      Problem.Name = "Problem";
      Problem.ReadOnly = true;
      Problem.Width = 150;
      // 
      // SendingDate
      // 
      SendingDate.HeaderText = "SendingDate";
      SendingDate.MinimumWidth = 8;
      SendingDate.Name = "SendingDate";
      SendingDate.ReadOnly = true;
      SendingDate.Width = 150;
      // 
      // MistakeCount
      // 
      MistakeCount.HeaderText = "MistakeCount";
      MistakeCount.MinimumWidth = 8;
      MistakeCount.Name = "MistakeCount";
      MistakeCount.ReadOnly = true;
      MistakeCount.Width = 150;
      // 
      // LoadFile
      // 
      LoadFile.HeaderText = "Load File";
      LoadFile.MinimumWidth = 8;
      LoadFile.Name = "LoadFile";
      LoadFile.ReadOnly = true;
      LoadFile.Width = 150;
      // 
      // ViewMistakes
      // 
      ViewMistakes.HeaderText = "View Mistakes";
      ViewMistakes.MinimumWidth = 8;
      ViewMistakes.Name = "ViewMistakes";
      ViewMistakes.ReadOnly = true;
      ViewMistakes.Width = 150;
      // 
      // TeacherForm
      // 
      AutoScaleDimensions = new SizeF(10F, 25F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(960, 450);
      Controls.Add(dataGridView1);
      Controls.Add(button3);
      Controls.Add(button2);
      Controls.Add(comboBox2);
      Controls.Add(button1);
      Controls.Add(comboBox1);
      Name = "TeacherForm";
      Text = "TeacherForm";
      ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private ComboBox comboBox1;
    private Button button1;
    private ComboBox comboBox2;
    private Button button2;
    private Button button3;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn Id;
    private DataGridViewTextBoxColumn Problem;
    private DataGridViewTextBoxColumn SendingDate;
    private DataGridViewTextBoxColumn MistakeCount;
    private DataGridViewButtonColumn LoadFile;
    private DataGridViewButtonColumn ViewMistakes;
  }
}