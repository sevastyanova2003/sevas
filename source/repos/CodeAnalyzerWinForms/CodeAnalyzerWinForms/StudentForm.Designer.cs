namespace CodeAnalyzerWinForms
{
  partial class StudentForm
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
      button1 = new Button();
      button2 = new Button();
      button3 = new Button();
      dataGridView1 = new DataGridView();
      Id = new DataGridViewTextBoxColumn();
      Problem = new DataGridViewTextBoxColumn();
      SendingDate = new DataGridViewTextBoxColumn();
      MistakeCount = new DataGridViewTextBoxColumn();
      ViewFile = new DataGridViewButtonColumn();
      ViewMistakes = new DataGridViewButtonColumn();
      DeleteEntry = new DataGridViewButtonColumn();
      ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
      SuspendLayout();
      // 
      // button1
      // 
      button1.Location = new Point(83, 57);
      button1.Name = "button1";
      button1.Size = new Size(112, 34);
      button1.TabIndex = 0;
      button1.Text = "Load File";
      button1.UseVisualStyleBackColor = true;
      button1.Click += button1_Click;
      // 
      // button2
      // 
      button2.Location = new Point(324, 57);
      button2.Name = "button2";
      button2.Size = new Size(136, 34);
      button2.TabIndex = 1;
      button2.Text = "Get My Stats";
      button2.UseVisualStyleBackColor = true;
      button2.Click += button2_Click;
      // 
      // button3
      // 
      button3.Location = new Point(557, 57);
      button3.Name = "button3";
      button3.Size = new Size(153, 34);
      button3.TabIndex = 2;
      button3.Text = "Get Group Stats";
      button3.UseVisualStyleBackColor = true;
      button3.Click += button3_Click;
      // 
      // dataGridView1
      // 
      dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Id, Problem, SendingDate, MistakeCount, ViewFile, ViewMistakes, DeleteEntry });
      dataGridView1.Location = new Point(0, 129);
      dataGridView1.Name = "dataGridView1";
      dataGridView1.ReadOnly = true;
      dataGridView1.RowHeadersWidth = 62;
      dataGridView1.RowTemplate.Height = 33;
      dataGridView1.Size = new Size(1092, 309);
      dataGridView1.TabIndex = 3;
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
      // ViewFile
      // 
      ViewFile.HeaderText = "Load File";
      ViewFile.MinimumWidth = 8;
      ViewFile.Name = "ViewFile";
      ViewFile.ReadOnly = true;
      ViewFile.Width = 150;
      // 
      // ViewMistakes
      // 
      ViewMistakes.HeaderText = "View Mistakes";
      ViewMistakes.MinimumWidth = 8;
      ViewMistakes.Name = "ViewMistakes";
      ViewMistakes.ReadOnly = true;
      ViewMistakes.Width = 150;
      // 
      // DeleteEntry
      // 
      DeleteEntry.HeaderText = "Delete Entry";
      DeleteEntry.MinimumWidth = 8;
      DeleteEntry.Name = "DeleteEntry";
      DeleteEntry.ReadOnly = true;
      DeleteEntry.Width = 150;
      // 
      // StudentForm
      // 
      AutoScaleDimensions = new SizeF(10F, 25F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1094, 450);
      Controls.Add(dataGridView1);
      Controls.Add(button3);
      Controls.Add(button2);
      Controls.Add(button1);
      Name = "StudentForm";
      Text = "StudentForm";
      ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private Button button2;
    private Button button3;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn Id;
    private DataGridViewTextBoxColumn Problem;
    private DataGridViewTextBoxColumn SendingDate;
    private DataGridViewTextBoxColumn MistakeCount;
    private DataGridViewButtonColumn ViewFile;
    private DataGridViewButtonColumn ViewMistakes;
    private DataGridViewButtonColumn DeleteEntry;
  }
}