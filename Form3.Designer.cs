namespace TS
{
    partial class Form3
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
            this.M3U8Address = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.M3U8Detail = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // M3U8Address
            // 
            this.M3U8Address.Location = new System.Drawing.Point(31, 30);
            this.M3U8Address.Name = "M3U8Address";
            this.M3U8Address.Size = new System.Drawing.Size(372, 23);
            this.M3U8Address.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(424, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "SingleFile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // M3U8Detail
            // 
            this.M3U8Detail.Location = new System.Drawing.Point(33, 69);
            this.M3U8Detail.Name = "M3U8Detail";
            this.M3U8Detail.Size = new System.Drawing.Size(370, 290);
            this.M3U8Detail.TabIndex = 2;
            this.M3U8Detail.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(421, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "TxTFileDownload";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(484, 154);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.M3U8Detail);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.M3U8Address);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox M3U8Address;
        private Button button1;
        private RichTextBox M3U8Detail;
        private Button button2;
        private Button button3;
    }
}