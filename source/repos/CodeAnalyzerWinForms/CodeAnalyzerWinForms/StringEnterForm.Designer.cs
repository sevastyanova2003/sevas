﻿namespace CodeAnalyzerWinForms
{
  partial class StringEnterForm
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
      label1 = new Label();
      textBox1 = new TextBox();
      button1 = new Button();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(300, 158);
      label1.Name = "label1";
      label1.Size = new Size(59, 25);
      label1.TabIndex = 0;
      label1.Text = "";
      // 
      // textBox1
      // 
      textBox1.Location = new Point(320, 186);
      textBox1.Name = "textBox1";
      textBox1.Size = new Size(150, 31);
      textBox1.TabIndex = 1;
      // 
      // button1
      // 
      button1.Location = new Point(336, 223);
      button1.Name = "button1";
      button1.Size = new Size(112, 34);
      button1.TabIndex = 2;
      button1.Text = "OK";
      button1.UseVisualStyleBackColor = true;
      button1.Click += button1_Click;
      // 
      // StringEnterForm
      // 
      AutoScaleDimensions = new SizeF(10F, 25F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(button1);
      Controls.Add(textBox1);
      Controls.Add(label1);
      Name = "StringEnterForm";
      Text = "StringEnterForm";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox textBox1;
    private Button button1;
  }
}