using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;



namespace TS
{
    //public SQLiteConnection Conn;
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void CreateDB_Click(object sender, EventArgs e)
        {
            SQLiteConnection Conn ;
            string FilePath = Application.StartupPath + "\\" + textBox1.Text + ".db";
            if (!File.Exists(FilePath))
            {
                SQLiteConnection.CreateFile(FilePath);
            }
            else { MessageBox.Show("DB:"+textBox1.Text +" has already exit!", "DB");return; }
            try
            {
                Conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
                Conn.Open();
                string sql = "create table " + textBox2.Text + " (name varchar(20), score int)";
                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                Conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("打开数据库：" + FilePath + "的连接失败：" + ex.Message);
            }
            

        }

        private void CreateTB_Click(object sender, EventArgs e)
        {
            SQLiteConnection Conn;
            try
            {
                String FilePath = Application.StartupPath + "\\" + textBox1.Text + ".db";
                Conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
                Conn.Open();
                string sql = "create table " + textBox2.Text + " (DownTime INTEGER, m3u8address varchar, Done INTEGER)";
                SQLiteCommand command = new SQLiteCommand(sql,Conn);
                command.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据表" + textBox2.Text + "失败：" + ex.Message);
            }
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
