namespace TS
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.M3U8Address = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labTSFileDown = new System.Windows.Forms.Label();
            this.M3U8Detail = new System.Windows.Forms.RichTextBox();
            this.CreateDB = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "M3U8LinkAddress";
            // 
            // M3U8Address
            // 
            this.M3U8Address.Location = new System.Drawing.Point(120, 18);
            this.M3U8Address.Name = "M3U8Address";
            this.M3U8Address.Size = new System.Drawing.Size(435, 23);
            this.M3U8Address.TabIndex = 1;
            this.M3U8Address.Text = "plz input the address like http://*****/**.m3u8";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(561, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labTSFileDown
            // 
            this.labTSFileDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labTSFileDown.Location = new System.Drawing.Point(43, 58);
            this.labTSFileDown.Name = "labTSFileDown";
            this.labTSFileDown.Size = new System.Drawing.Size(288, 23);
            this.labTSFileDown.TabIndex = 3;
            this.labTSFileDown.Click += new System.EventHandler(this.labTSFileDown_Click);
            // 
            // M3U8Detail
            // 
            this.M3U8Detail.Location = new System.Drawing.Point(43, 103);
            this.M3U8Detail.Name = "M3U8Detail";
            this.M3U8Detail.Size = new System.Drawing.Size(563, 406);
            this.M3U8Detail.TabIndex = 5;
            this.M3U8Detail.Text = "";
            // 
            // CreateDB
            // 
            this.CreateDB.Location = new System.Drawing.Point(176, 534);
            this.CreateDB.Name = "CreateDB";
            this.CreateDB.Size = new System.Drawing.Size(113, 42);
            this.CreateDB.TabIndex = 6;
            this.CreateDB.Text = "Record";
            this.CreateDB.UseVisualStyleBackColor = true;
            this.CreateDB.Click += new System.EventHandler(this.CreateDB_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(43, 534);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 42);
            this.button2.TabIndex = 7;
            this.button2.Text = "Encrypted TS";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(561, 58);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "MultiDown";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 621);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.CreateDB);
            this.Controls.Add(this.M3U8Detail);
            this.Controls.Add(this.labTSFileDown);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.M3U8Address);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "TSFileDownload";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox M3U8Address;
        private Button button1;
        private Label labTSFileDown;
        private RichTextBox M3U8Detail;
        private Button CreateDB;
        private Button button2;
        private Button button3;
    }
}