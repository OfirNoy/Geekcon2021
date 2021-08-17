namespace GameServer
{
  partial class Form1
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
      this.button1 = new System.Windows.Forms.Button();
      this.labelScoreLeft = new System.Windows.Forms.Label();
      this.labelScoreRight = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.pictureBoxGame = new System.Windows.Forms.PictureBox();
      this.pictureBoxCam = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGame)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).BeginInit();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(697, 24);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(100, 41);
      this.button1.TabIndex = 2;
      this.button1.Text = "Start";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // labelScoreLeft
      // 
      this.labelScoreLeft.AutoSize = true;
      this.labelScoreLeft.BackColor = System.Drawing.Color.Black;
      this.labelScoreLeft.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelScoreLeft.ForeColor = System.Drawing.Color.White;
      this.labelScoreLeft.Location = new System.Drawing.Point(62, 137);
      this.labelScoreLeft.Name = "labelScoreLeft";
      this.labelScoreLeft.Size = new System.Drawing.Size(53, 36);
      this.labelScoreLeft.TabIndex = 3;
      this.labelScoreLeft.Text = "00";
      // 
      // labelScoreRight
      // 
      this.labelScoreRight.AutoSize = true;
      this.labelScoreRight.BackColor = System.Drawing.Color.Black;
      this.labelScoreRight.Font = new System.Drawing.Font("Courier New", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelScoreRight.ForeColor = System.Drawing.Color.White;
      this.labelScoreRight.Location = new System.Drawing.Point(545, 137);
      this.labelScoreRight.Name = "labelScoreRight";
      this.labelScoreRight.Size = new System.Drawing.Size(53, 36);
      this.labelScoreRight.TabIndex = 4;
      this.labelScoreRight.Text = "00";
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::GameServer.Properties.Resources.RoombaPong_logo;
      this.pictureBox1.Location = new System.Drawing.Point(164, 24);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(257, 61);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 5;
      this.pictureBox1.TabStop = false;
      // 
      // pictureBoxGame
      // 
      this.pictureBoxGame.BackColor = System.Drawing.Color.Black;
      this.pictureBoxGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureBoxGame.Location = new System.Drawing.Point(12, 105);
      this.pictureBoxGame.Name = "pictureBoxGame";
      this.pictureBoxGame.Size = new System.Drawing.Size(640, 360);
      this.pictureBoxGame.TabIndex = 1;
      this.pictureBoxGame.TabStop = false;
      // 
      // pictureBoxCam
      // 
      this.pictureBoxCam.Location = new System.Drawing.Point(697, 105);
      this.pictureBoxCam.Name = "pictureBoxCam";
      this.pictureBoxCam.Size = new System.Drawing.Size(640, 360);
      this.pictureBoxCam.TabIndex = 0;
      this.pictureBoxCam.TabStop = false;
      this.pictureBoxCam.Click += new System.EventHandler(this.pictureBoxCam_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(1369, 495);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.labelScoreRight);
      this.Controls.Add(this.labelScoreLeft);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.pictureBoxGame);
      this.Controls.Add(this.pictureBoxCam);
      this.Name = "Form1";
      this.Text = "BONG - RoombaPong";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGame)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxCam;
    private System.Windows.Forms.PictureBox pictureBoxGame;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label labelScoreLeft;
    private System.Windows.Forms.Label labelScoreRight;
    private System.Windows.Forms.PictureBox pictureBox1;
  }
}

