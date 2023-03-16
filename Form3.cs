using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography;
using System.Data.SQLite;






namespace TS
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public bool tsfiledownfromtxt=true ;
        private void Form3_Load(object sender, EventArgs e)
        {

        }
        //use to download string
        //parem webpage address
        public static string HttpGet(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 2000;
                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(),Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.Write("HttpGet 异常，" + ex.Message);
                Console.Write(ex);
                return "";
            }
        }
        //use to download byte
        //parem webpage address
        public static byte[] HttpGetByte(string url)
        {
            try
            {
                byte[] arraryByte = null;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                //request.Timeout = 20000;
                request.Method = "GET";
                using (WebResponse wr = request.GetResponse())
                {
                    int length = (int)wr.ContentLength;
                    using (StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8))
                    {
                        HttpWebResponse response = wr as HttpWebResponse;
                        Stream stream = response.GetResponseStream();
                        //读取到内存
                        MemoryStream stmMemory = new MemoryStream();
                        byte[] buffer1 = new byte[length];
                        int i;
                        //将字节逐个放入到Byte 中
                        while ((i = stream.Read(buffer1, 0, buffer1.Length)) > 0)
                        {
                            stmMemory.Write(buffer1, 0, i);
                        }
                        arraryByte = stmMemory.ToArray();
                        stmMemory.Close();
                    }
                }
                return arraryByte;
            }
            catch (Exception ex)
            {
                Console.Write("HttpGetByte 异常，" + ex.Message);
                Console.Write(ex);
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static byte[] AESDecrypt2(byte[] cipherText, string Key, string IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            byte[] res = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(Key);
                aesAlg.IV = Encoding.ASCII.GetBytes(IV);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] tmp = new byte[cipherText.Length + 32];
                        int len = csDecrypt.Read(tmp, 0, cipherText.Length + 32);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        res = ret;
                    }
                }
            }
            return res;
        }

        //public string basesavepath = AppDomain.CurrentDomain.BaseDirectory;
        //public string m3u8time = System.DateTime.Now.ToString("yyyyMMddhhmmss");
        //public string m3u8time = System.DateTime.Now.Tostring("yyyyMMddhhmmss");
        private void button1_Click(object sender, EventArgs e)
        {
            tsfiledownfromtxt = false;
            //int seefilenum=1;
            // define the path of m3u8 address, each m3u8 file has a sole direct under the app.
            string basesavepath = AppDomain.CurrentDomain.BaseDirectory;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            //set an separate directory for ts files by system time
            string m3u8time = currentTime.ToString("yyyyMMddhhmmss");
            //get the encrypt m3u8 address
            var m3u8url = M3U8Address.Text;
            //test if this m3u8 has existed
            //Form1 fm1 = new Form1();
            if (Form1.M3u8ChkDB(m3u8url, m3u8time)) { MessageBox.Show("m3u8 has been downloaded"); return; }

            //base on the system time to create the file direct
            basesavepath = basesavepath  + m3u8time + @"\";
            //MessageBox.Show(basesavepath);
            //Console.WriteLine(basesavepath);
            if (!Directory.Exists(basesavepath))
                Directory.CreateDirectory(basesavepath);
            //if (Directory.Exists(basesavepath))
            //{//MessageBox.Show(basesavepath, "Sucess"); }
            //else { //MessageBox.Show(basesavepath, "Fail");
            //       return; }

            string m3u8Url = M3U8Address.Text;

            //get m3u8 file name
            var m3u8filename = m3u8url.Substring(m3u8url.LastIndexOf('/') + 1);
            //entire m3u8 filepath;
            var m3u8filepath = basesavepath  + m3u8filename;
            if (!Form1.DownloadFile(m3u8url, m3u8filepath))
            {
                //MessageBox.Show(basesavepath + @"/" + m3u8filename);
                //Log.Error("下载m3u8文件失败：" + m3u8url);
                //failed and quit
                MessageBox.Show("下载m3u8文件失败：" + m3u8url);
                return;
            }
            else
            {
                //MessageBox.Show("下载m3u8文件成功：" + m3u8url);

            }


            // 读取m3u8文件内容            
            string m3u8Content = HttpGet(m3u8Url);

            //按行读取m3u8文件 find the key string
            var list = File.ReadAllLines(m3u8filepath, Encoding.UTF8);
            string keycontent;
            //all of this part codes is trying to get the content of key
            //param: keycontent ;put key content into keycontent
            foreach (var item in list) 
            {
                if (item.ToLower().Contains("key.key")) 
                {
                    int keyline = item.Length;
                    int keyposition= item.ToLower().IndexOf("uri=");
                    keyposition = keyposition + 5;
                    //MessageBox.Show(keyposition.ToString() , keyline.ToString());
                    keyline = keyline - keyposition - 1; 
                    string keyurl = item.Substring(keyposition,keyline);
                    //if key line not start with http
                    if (!item.ToLower().StartsWith("http")) { 
                        keyurl = m3u8url.Substring(0,m3u8url.LastIndexOf('/') + 1)+"key.key";
                    }

                    M3U8Detail.Text = keyurl;
                    M3U8Detail.Refresh();
                    //string key = HttpGet(keyurl);
                    //if (key == "") { MessageBox.Show("Fail to get Key");return; }
                    //M3U8Detail.Text = M3U8Detail.Text+ key;
                    //MessageBox.Show(key);
                    //M3U8Detail.Refresh();
                    //downlaod key file to disk
                    if (!Form1.DownloadFile(keyurl, basesavepath + @"/" + "key.key"))
                    {
                        //MessageBox.Show(basesavepath + @"/" + "key.key");
                        //Log.Error("下载key文件失败：" + m3u8url);
                        //failed and quit
                        MessageBox.Show("下载key文件失败：" + keyurl);
                        return;
                    }
                    else
                    {
                        //MessageBox.Show("下载key文件成功：" + keyurl);

                    }
                    keycontent = File.ReadAllText(basesavepath + @"/" + "key.key");
                    //MessageBox.Show(keycontent);
                    //return;
                }
            }
            //decrypt now
            //Param: tefiList : the ts file path and name
            ArrayList tsfileList = new ArrayList();
            var listts = File.ReadAllLines(m3u8filepath, Encoding.UTF8);
            var m3u8baseurl = m3u8url.Substring(0, m3u8url.LastIndexOf('/') + 1);
            int i = 0;
            foreach (var line in listts)
            {
                //判断.ts结尾的行
                if (line.Length > 0 && line.EndsWith(".ts") )                
                {
                    int tsvilength= line.Length;
                    string tsfilepath;
                    string tsurl;
                    string tsviname;
                    //判断ts路径是否带url路径，如果带的话则用ts本身路径
                    if (line.ToLower().StartsWith("http"))
                    {
                        tsfilepath = basesavepath + line.Substring(line.LastIndexOf('/') + 1);
                        tsurl = line;
                        tsviname = line.Substring(line.LastIndexOf('/') + 1, 8);
                        //MessageBox.Show(tsfilepath);
                        //MessageBox.Show(tsurl);
                        //MessageBox.Show(tsviname);
                        //return;
                        //Console
                    }
                    else
                    {
                        //ts文件保存路径
                    
                        
                        //MessageBox.Show(tsurl);
                        //get the file name as vi if no http as begin
                        tsvilength = line.Length - 3;
                        if (line.ToLower().Contains("/")) { tsviname = line.Substring(line.LastIndexOf('/')+1, 8); }
                        else { tsviname = line.Substring(0, 8); }
                        //ts文件下载路径 and filepath
                        tsurl = m3u8baseurl  + tsviname+".ts";
                        tsfilepath = basesavepath  + tsviname+".ts";
                        
                        //MessageBox.Show(tsurl);
                        //MessageBox.Show(tsfilepath);

                        //MessageBox.Show(tsfilepath);
                        //MessageBox.Show(tsurl);

                    }
                    //key in keycontent, vi in tsviname, the url of ts file in tsurl, we can decrypt now
                    //param: keycontent,tsviname,tsurl
                    try 
                    {
                        string saveFilepath = tsfilepath;
                        //MessageBox.Show(tsfilepath);
                        //return;
                        M3U8Detail.Text = tsfilepath;
                        M3U8Detail.Refresh ();
                        if (!File.Exists(saveFilepath)) 
                        {
                            byte[] encByte = HttpGetByte(tsurl);
                            //M3U8Detail.Text = M3U8Detail.Text + seefilenum.ToString();
                            //M3U8Detail.Refresh();
                            //seefilenum += 1;
                            //MessageBox.Show("we are here1?");
                            if (encByte != null) 
                            {
                               byte[] decByte = null;
                                //MessageBox.Show("we are here2?");
                                //try 
                                //{
                                    keycontent =File.ReadAllText(basesavepath + @"/" + "key.key");
                                //    MessageBox.Show(keycontent);
                                //    MessageBox.Show(tsviname);
                                //vi要16位，我在这里耽误了半天我日
                                    decByte = AESDecrypt2(encByte,keycontent,tsviname+ tsviname);
                                   // MessageBox.Show("we are here3?");
                                //}
                                //catch (Exception e1)
                                //{
                                    //error_arr.Add(tsfilepath);
                                    //Console.WriteLine("解密ts文件异常。" + e1.Message);
                                //}
                                if (decByte != null) 
                                {
                                    //MessageBox.Show("we are here4?");                                    
                                    File.WriteAllBytes(saveFilepath, decByte);
                                    tsfileList.Add(saveFilepath);
                                    //i += 1;
                                    //if (i > 10) { break; }
                                }

                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        //error_arr.Add(tsfilepath);
                        MessageBox .Show ("发生异常。" + ee.Message);
                    }
                }
            }
            //合并文件

            int testfilenum = tsfileList.Count;
            var destFile = basesavepath + @"\" + currentTime.ToString("yyyyMMddhhmmss") + ".mp4";
            //List<string> files = new List<string>((string[])tsfileList.ToArray(typeof(string)));
            //if (Form1.mergefile(destFile, files))
            //{
            //    MessageBox.Show("Well Done");
            //}
            using (var outputStream = File.Create(destFile))
            {
                foreach (string item in tsfileList)
                {
                    M3U8Detail.Text = M3U8Detail.Text + item + "\n";
                    M3U8Detail.Refresh();
                    using (var inputStream = File.OpenRead(item))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                }
                tsfiledownfromtxt = true;
                //MessageBox.Show("Well Done");
                //MessageBox.Show(testfilenum.ToString());
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            string txtfile;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtfile = dialog.FileName;
                var list = File.ReadAllLines(txtfile, Encoding.UTF8);
                //MessageBox.Show(txtfile);
                foreach (string item in list)
                {
                    if (item.Length >0 && item.ToLower().Contains("http")&& item.ToLower().Contains("m3u8")) 
                    {
                        //M3U8Address.Text = item;
                        //M3U8Address.Refresh();
                        //if (tsfiledownfromtxt)
                        //{ button1.PerformClick(); }
                        //break;
                        string basesavepath = AppDomain.CurrentDomain.BaseDirectory;
                        //currentTime = System.DateTime.Now;
                        string m3u8time = System.DateTime.Now.ToString("yyyyMMddhhmmss");
                        //think about the line is not beginning with "https"
                        var m3u8url = item.Substring(item.ToLower().IndexOf("https"));

                        if (Form1.M3u8ChkDB(m3u8url, m3u8time)) { 
                            //MessageBox.Show("m3u8 has been downloaded");
                            break; }

                        basesavepath = basesavepath + m3u8time + @"\";
                        if (!Directory.Exists(basesavepath))
                            Directory.CreateDirectory(basesavepath);

                        var m3u8filename = m3u8url.Substring(m3u8url.LastIndexOf('/') + 1);
                        var m3u8filepath = basesavepath + @"/" + m3u8filename;

                        if (!Form1.DownloadFile(m3u8url, basesavepath + @"/" + m3u8filename))
                        {
                            //MessageBox.Show(basesavepath + @"/" + m3u8filename);
                            //Log.Error("下载m3u8文件失败：" + m3u8url);
                            //failed and quit

                            //return;
                            M3U8Detail.Text = "下载m3u8文件失败：" + m3u8url;
                            M3U8Detail.Refresh();
                            break;
                        }
                        else
                        {
                            M3U8Detail.Text = "下载m3u8文件成功：" + m3u8url;
                            M3U8Detail.Refresh();
                        }

                        string m3u8Content = HttpGet(m3u8url);
                        var tsfilelist = File.ReadAllLines(m3u8filepath, Encoding.UTF8);
                        string keycontent;
                        bool cyberTs = false;
                        foreach(var tsitem in tsfilelist) 
                        {            
                            if (tsitem.ToLower().Contains("key.key"))
                            {
                                cyberTs = true;
                                int keyline = tsitem.Length;
                                int keyposition = tsitem.ToLower().IndexOf("uri=");
                                keyposition = keyposition + 5;
                                //MessageBox.Show(keyposition.ToString() , keyline.ToString());
                                keyline = keyline - keyposition - 1;
                                string keyurl = tsitem.Substring(keyposition, keyline);
                                if (!tsitem.ToLower().StartsWith("http"))
                                {
                                    keyurl = m3u8url.Substring(0, m3u8url.LastIndexOf('/') + 1) + "key.key";
                                }
                                M3U8Detail.Text = keyurl;
                                M3U8Detail.Refresh();
                                //string key = HttpGet(keyurl);
                                //if (key == "") { MessageBox.Show("Fail to get Key");return; }
                                //M3U8Detail.Text = M3U8Detail.Text+ key;
                                //MessageBox.Show(key);
                                //M3U8Detail.Refresh();
                                //downlaod key file to disk
                                if (!Form1.DownloadFile(keyurl, basesavepath + "key.key"))
                                {
                                    //MessageBox.Show(basesavepath + @"/" + "key.key");
                                    //Log.Error("下载key文件失败：" + m3u8url);
                                    //failed and quit
                                    //MessageBox.Show("下载key文件失败：" + keyurl);
                                    M3U8Detail.Text = "下载key文件失败：" + keyurl;
                                    M3U8Detail.Refresh();
                                    break;
                                }
                                else
                                {
                                    M3U8Detail.Text="下载key文件成功：" + keyurl;
                                    M3U8Detail.Refresh();

                                }
                                keycontent = File.ReadAllText(basesavepath + "key.key");
                                //MessageBox.Show(keycontent);
                                //return;
                            }
                        }
                        //decrypt now
                        //Param: tefiList : the ts file path and name
                        ArrayList tsfileList = new ArrayList();
                        var listts = File.ReadAllLines(m3u8filepath, Encoding.UTF8);
                        //do not need to think about the "https" is not the beginer because we do it above;
                        var m3u8baseurl = m3u8url.Substring(0, m3u8url.LastIndexOf('/') + 1);
                        //MessageBox.Show(m3u8baseurl);
                        //return;
                        int i = 0;
                        foreach (var line in listts)
                        {
                            //判断.ts结尾的行
                            if (line.Length > 0 && line.EndsWith(".ts"))
                            {
                                int tsvilength = line.Length;
                                string tsfilepath;
                                string tsurl;
                                string tsviname;
                                //判断ts路径是否带url路径，如果带的话则用ts本身路径
                                if (line.ToLower().StartsWith("http"))
                                {
                                    tsfilepath = basesavepath + line.Substring(line.LastIndexOf('/') + 1);
                                    tsurl = line;
                                    tsviname = line.Substring(line.LastIndexOf('/') + 1, 8);
                                    //MessageBox.Show(tsfilepath);
                                    //MessageBox.Show(tsurl);
                                    //MessageBox.Show(tsviname);
                                    //return;
                                }
                                else
                                {
                                    //ts文件保存路径


                                    //MessageBox.Show(tsurl);
                                    //get the file name as vi if no http as begin
                                    tsvilength = line.Length - 3;
                                    if (line.ToLower().Contains("/")) { tsviname = line.Substring(line.LastIndexOf('/') + 1, 8); }
                                    else { tsviname = line.Substring(0, 8); }
                                    //ts文件下载路径 and filepath
                                    tsurl = m3u8baseurl + tsviname + ".ts";
                                    tsfilepath = basesavepath + tsviname + ".ts";

                                }
                                //key in keycontent, vi in tsviname, the url of ts file in tsurl, we can decrypt now
                                //param: keycontent,tsviname,tsurl
                                try
                                {
                                    string saveFilepath = tsfilepath;
                                    //MessageBox.Show(tsfilepath);
                                    //return;
                                    M3U8Detail.Text = M3U8Detail.Text+ "\n" + tsfilepath;
                                    M3U8Detail.Refresh();
                                    if (!File.Exists(saveFilepath))
                                    {
                                        byte[] encByte = HttpGetByte(tsurl);
                                        //Write the file directly if it is not encrypted
                                        if (encByte != null && cyberTs != true) {
                                            File.WriteAllBytes(saveFilepath, encByte);
                                            tsfileList.Add(saveFilepath);
                                        }
                                        //M3U8Detail.Text = M3U8Detail.Text + seefilenum.ToString();
                                        //M3U8Detail.Refresh();
                                        //seefilenum += 1;
                                        //MessageBox.Show("we are here1?");
                                        //Decrypte the file if it is encrypted
                                        if (encByte != null && cyberTs == true)
                                        {
                                            byte[] decByte = null;
                                            //MessageBox.Show("we are here2?");
                                            //try 
                                            //{
                                            keycontent = File.ReadAllText(basesavepath + @"/" + "key.key");
                                            //    MessageBox.Show(keycontent);
                                            //    MessageBox.Show(tsviname);
                                            //vi要16位，我在这里耽误了半天我日
                                            decByte = AESDecrypt2(encByte, keycontent, tsviname + tsviname);
                                            // MessageBox.Show("we are here3?");
                                            //}
                                            //catch (Exception e1)
                                            //{
                                            //error_arr.Add(tsfilepath);
                                            //Console.WriteLine("解密ts文件异常。" + e1.Message);
                                            //}
                                            if (decByte != null)
                                            {
                                                //MessageBox.Show("we are here4?");                                    
                                                File.WriteAllBytes(saveFilepath, decByte);
                                                tsfileList.Add(saveFilepath);
                                                //i += 1;
                                                //if (i > 10) { break; }
                                            }

                                        }
                                    }
                                }
                                catch (Exception ee)
                                {
                                    //error_arr.Add(tsfilepath);
                                    MessageBox.Show("发生异常。" + ee.Message);
                                }
                            }
                        }
                        //合并文件

                        int testfilenum = tsfileList.Count;
                        var destFile = basesavepath + @"\" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + ".mp4";
                        //List<string> files = new List<string>((string[])tsfileList.ToArray(typeof(string)));
                        //if (Form1.mergefile(destFile, files))
                        //{
                        //    MessageBox.Show("Well Done");
                        //}
                        using (var outputStream = File.Create(destFile))
                        {
                            foreach (string tsfileitem in tsfileList)
                            {
                                M3U8Detail.Text = M3U8Detail.Text + item + "\n";
                                M3U8Detail.Refresh();
                                using (var inputStream = File.OpenRead(tsfileitem))
                                {
                                    inputStream.CopyTo(outputStream);
                                }
                            }
                            tsfiledownfromtxt = true;
                            //MessageBox.Show("Well Done");
                            //MessageBox.Show(testfilenum.ToString());
                            M3U8Detail.Text = "Work Done!"+m3u8url;
                            M3U8Detail.Refresh();
                                
                        }


                    }
                }

            }
            



        }
    }
}
