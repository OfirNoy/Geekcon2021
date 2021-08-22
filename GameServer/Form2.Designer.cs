namespace GameServer
{
  partial class Form2
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.timer2 = new System.Windows.Forms.Timer(this.components);
      this.timer3 = new System.Windows.Forms.Timer(this.components);
      this.PC = new System.Windows.Forms.PictureBox();
      this.Player = new System.Windows.Forms.PictureBox();
      this.Ball = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.PC)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Ball)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.label1.ForeColor = System.Drawing.Color.White;
      this.label1.Location = new System.Drawing.Point(163, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(20, 20);
      this.label1.TabIndex = 3;
      this.label1.Text = "0";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.label2.ForeColor = System.Drawing.Color.White;
      this.label2.Location = new System.Drawing.Point(226, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(17, 20);
      this.label2.TabIndex = 4;
      this.label2.Text = ":";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.label3.ForeColor = System.Drawing.Color.White;
      this.label3.Location = new System.Drawing.Point(287, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(20, 20);
      this.label3.TabIndex = 5;
      this.label3.Text = "0";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(196, 209);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(80, 40);
      this.button1.TabIndex = 6;
      this.button1.Text = "Start Game";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // timer1
      // 
      this.timer1.Interval = 5;
      this.timer1.Tick += new System.EventHandler(this.MovePaddle);
      // 
      // timer2
      // 
      this.timer2.Interval = 25;
      this.timer2.Tick += new System.EventHandler(this.MoveBall);
      // 
      // timer3
      // 
      this.timer3.Interval = 5;
      this.timer3.Tick += new System.EventHandler(this.Computer);
      // 
      // PC
      // 
      this.PC.BackColor = System.Drawing.Color.White;
      this.PC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.PC.Location = new System.Drawing.Point(454, 75);
      this.PC.Name = "PC";
      this.PC.Size = new System.Drawing.Size(30, 90);
      this.PC.TabIndex = 8;
      this.PC.TabStop = false;
      // 
      // Player
      // 
      this.Player.BackColor = System.Drawing.Color.White;
      this.Player.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Player.Location = new System.Drawing.Point(0, 75);
      this.Player.Name = "Player";
      this.Player.Size = new System.Drawing.Size(30, 90);
      this.Player.TabIndex = 7;
      this.Player.TabStop = false;
      // 
      // Ball
      // 
      this.Ball.BackColor = System.Drawing.Color.White;
      this.Ball.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.Ball.Location = new System.Drawing.Point(185, 75);
      this.Ball.Name = "Ball";
      this.Ball.Size = new System.Drawing.Size(80, 80);
      this.Ball.TabIndex = 2;
      this.Ball.TabStop = false;
      this.Ball.Click += new System.EventHandler(this.Ball_Click);
      // 
      // Form2
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(484, 261);
      this.Controls.Add(this.PC);
      this.Controls.Add(this.Player);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.Ball);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Form2";
      this.Text = "Pong";
      this.Load += new System.EventHandler(this.Form2_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Pressed);
      this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Released);
      ((System.ComponentModel.ISupportInitialize)(this.PC)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Ball)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.PictureBox Ball;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Timer timer2;
    private System.Windows.Forms.Timer timer3;
    private System.Windows.Forms.PictureBox Player;
    private System.Windows.Forms.PictureBox PC;
  }
}

