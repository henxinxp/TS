using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Data.SQLite;


namespace TS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // define the path of m3u8 address, each m3u8 file has a sole direct under the app.
            string basesavepath = AppDomain.CurrentDomain.BaseDirectory;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            //set an separate directory for ts files by system time
            string m3u8time= currentTime.ToString("yyyyMMddhhmmss");

            var m3u8url = M3U8Address.Text;
            if (m3u8url == "" || !m3u8url.ToLower().Contains("m3u8"))
            {
                MessageBox.Show("check your m3u8 address plz");
                return;
            }
            //check the record if m3u8 has been downloaded
            if (M3u8ChkDB(m3u8url, m3u8time)) { 
                MessageBox.Show("m3u8 has been downloaded");
                return; 
            }            

            //base on the system time to create the file direct
            basesavepath = basesavepath + "\\" + m3u8time+ "\\";
            //MessageBox.Show(basesavepath);
            //Console.WriteLine(basesavepath);
            if (!Directory.Exists(basesavepath))
                Directory.CreateDirectory(basesavepath);
            if (Directory.Exists(basesavepath))
            { MessageBox.Show(basesavepath, "Sucess"); }
            else { MessageBox.Show(basesavepath, "Fail"); return; }

            //download m3u8 file
            var m3u8filename = m3u8url.Substring(m3u8url.LastIndexOf('/') + 1);
            //entire m3u8 file location;
            var m3u8filepath = basesavepath + @"/" + m3u8filename;
            MessageBox.Show(m3u8filename);
            if (!DownloadFile(m3u8url, basesavepath + @"/" + m3u8filename))
            {
                MessageBox.Show(basesavepath + @"/" + m3u8filename);
                //Log.Error("下载m3u8文件失败：" + m3u8url);
                //failed and quit
                MessageBox.Show("下载m3u8文件失败：" + m3u8url);
                return;
            }
            else
            {
                MessageBox.Show("下载m3u8文件成功：" + m3u8url);
                //write the m3u8 address and time to database
                M3u8ToDB(m3u8url, m3u8time, basesavepath);

            }
            //set url of ts files
            var m3u8baseurl = m3u8url.Substring(0, m3u8url.LastIndexOf('/') + 1);
            //按行读取m3u8文件
            var list = File.ReadAllLines(m3u8filepath, Encoding.UTF8);
            //read m3u8 file to string and display in M3U8Detail box
            string bigM3u8= File.ReadAllText (m3u8filepath);
            M3U8Detail.Text = bigM3u8;
            M3U8Detail.Refresh();
            //judge the m3u8 type if not crytp then begin or not quit
            DialogResult dr;
            dr = MessageBox.Show("m3u8","check the m3u8 content", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No) return;
            //announced dynamic array, need to add System.Collections
            int filetsCount=1;
            int tsall = SubStringCount(bigM3u8, "ts");
            ArrayList tsfileList = new ArrayList();
            foreach (var item in list)
            {

                //判断.ts结尾的行
                if (item.Length > 0 && item.EndsWith(".ts"))
                {
                    //ts文件保存路径
                    var tsfilepath = basesavepath + @"/" + item;
                    //ts文件下载路径
                    var tsurl = m3u8baseurl + @"/" + item;
                    //判断ts路径是否带url路径，如果带的话则用ts本身路径
                    if (item.ToLower().StartsWith("http"))
                    {
                        tsfilepath = basesavepath + @"/" + item.Substring(item.LastIndexOf('/') + 1);
                        tsurl = item;
                    }
                    //下载ts文件
                    int tsfileleft= tsall- filetsCount;
                    labTSFileDown.Text = "正在下载第" + filetsCount + "个文件，还有"+tsfileleft+"个文件";
                    labTSFileDown.Refresh();
                    if (!DownloadFile(tsurl, tsfilepath))
                    {
                        Console.WriteLine("下载ts文件失败：" + tsurl);
                    }
                    else
                    {
                        tsfileList.Add(tsfilepath);
                        filetsCount = filetsCount+1;
                    }

                }
                //System.Threading.Thread.Sleep(100);//暂停100毫秒 防止同一ip下载请求过多被屏蔽

            }
            //merge the ts files to all.mp4
            //Console.WriteLine("Total File Count : " + tsfileList.Count);
            int tsfileCounts = tsfileList.Count;
            //the name of combined mp4 file
            var destFile = basesavepath + @"\"+currentTime.ToString("yyyyMMddhhmmss")+".mp4";
            using (var outputStream = File.Create(destFile)) { 
               //for (int i=0;i<=tsfileList.Count;i++)
               foreach (string item in tsfileList)
                {
                    //string tsfff=tsfileList[i].ToString();
                    //MessageBox.Show(item);
                    labTSFileDown.Text = "File Processed : " + item+ "---"+ tsfileCounts;
                    labTSFileDown.Refresh();
                    using (var inputStream = File.OpenRead(item))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                    
                }
                //update database
                M3u8UpdateDB(m3u8url, m3u8time);
                MessageBox.Show("OK!Perfect!");
                //update database
                //M3u8UpdateDB(m3u8url, m3u8time);
                //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory+m3u8time);

                //return;
                //MessageBox.Show("OK!Perfect!");
            }

        }
        //合并文件
        public static bool mergefile(string destFile, List<string> tsfileList)
        {
            //MessageBox.Show("", tsfileList);
            try 
            { 
                using (var outputStream = File.Create(destFile))
                {
                    foreach (string item in tsfileList) 
                    {
                        using (var inputStream = File.OpenRead(item)) 
                        {
                            inputStream.CopyTo(outputStream);
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
                return false; 
            }
            return false; 
        }


        public static bool DownloadFile(string url, string filepath)
            {
                try
                {

                    FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Delete);
                    // 设置参数
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    Stream responseStream = response.GetResponseStream();
                    //创建本地文件写入流
                    //Stream stream = new FileStream(tempFile, FileMode.Create);
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        //stream.Write(bArr, 0, size);
                        fs.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    //stream.Close();
                    fs.Close();
                    responseStream.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Console .Write("下载文件" + url + "报错:" + ex);
                    return false;
                }

            }

        public static byte[] Downloadbyte(string url) 
        {
            try
            {

                //FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Delete);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    //stream.Write(bArr, 0, size);
                    //fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                //stream.Close();
                //fs.Close();
                responseStream.Close();
                return bArr;
            }
            catch (Exception ex)
            {
                //Log.Error("下载文件" + url + "报错:" + ex);
                //return;
            }

            return null;
        }


        public static int SubStringCount(string bigstr, string thesubstr)
        {
            bigstr = bigstr.ToLower();
            thesubstr = thesubstr.ToLower();
            //compare the length and divide to get how many
            if (bigstr.Contains(thesubstr)) {
                var replaced = bigstr.Replace(thesubstr, "");
                return (bigstr.Length -replaced.Length)/thesubstr.Length ;
            }
            
            return 0;
        }

        private void labTSFileDown_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CreateDB_Click(object sender, EventArgs e)
        {
            Form2 formDb=new Form2();
            formDb.Show();
        }
        private static bool M3u8ToDB(string m3u8url, string m3u8time, string pathfile){
            SQLiteConnection Conn;
            try
            {
                String FilePath = Application.StartupPath + "\\" + "TSDOWN.db";
                Conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
                Conn.Open();
                string sql = "insert into m3u8 (DownTime, m3u8url, Done, path) values ('" +m3u8time+"', '"+ m3u8url+ "', '0', '" + pathfile + "')";
                //MessageBox.Show(sql);

                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                command.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail to write m3u8 database: " + ex.Message);
            }
            return true;
        }
        private static bool M3u8UpdateDB(string m3u8url, string m3u8time)
        {
            SQLiteConnection Conn;
            try
            {
                String FilePath = Application.StartupPath + "\\" + "TSDOWN.db";
                Conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
                Conn.Open();
                //string sql = "insert into m3u8 (DownTime, m3u8url) value(" + m3u8time + ", " + m3u8url + ")";
                string sql = "update m3u8 set Done = '1' where m3u8url='" +m3u8url+ "'";
                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                command.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail to write m3u8 database: " + ex.Message);
            }
            return true;
        }
        public static bool M3u8ChkDB(string m3u8url, string m3u8time)
        {
            SQLiteConnection Conn;
            try
            {
                String FilePath = Application.StartupPath + "\\" + "TSDOWN.db";
                Conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
                Conn.Open();
                //string sql = "insert into m3u8 (DownTime, m3u8url) value(" + m3u8time + ", " + m3u8url + ")";
                string sql = "select * from m3u8 where m3u8url='" + m3u8url + "'";
                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //textBox5.Text = "Name: " + reader["name"] + "\tScore: " + reader["score"] + "\r\n" + textBox5.Text;
                    //string Done = reader["Done"].ToString;
                    string Done=reader["Done"].ToString();
                    string filepath = reader["path"].ToString();
                    //MessageBox.Show(Done);
                    //MessageBox.Show(sql);
                    //return false;
                    //Done =reader.
                    if(Done=="1")
                        MessageBox.Show(filepath);
                        return true;
                }                
                Conn.Close();
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Fail to read database: " + ex.Message);
            }
            //return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 fm = new Form3();
            fm.Show();
        }
    }



}