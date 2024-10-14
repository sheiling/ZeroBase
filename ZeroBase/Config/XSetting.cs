using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;


namespace ZeroBase
{
    public class XSetting : XObject
    {
        protected ConcurrentDictionary<string, string> settingMap = new ConcurrentDictionary<string, string>();
        protected string path = "";
        protected string root = "";
        protected string BackUpPath = "";

        public void SetPathAndRoot(string path, string root)
        {
            this.path = path;
            this.root = root;
        }

        public void SetBackUpPath(string BackUpPath)
        {
            this.BackUpPath = BackUpPath;
        }

        public string Name;
        public string Xml;


        public dynamic LoadSetting()
        {
            try
            {
                if (File.Exists(Xml) == false)
                {
                    XmlSerializer xs1 = new XmlSerializer(this.GetType());
                    Stream stream1 = new FileStream(Xml, FileMode.Create, FileAccess.Write, FileShare.Read);
                    xs1.Serialize(stream1, this);
                    stream1.Close();
                }
                XmlSerializer xs = new XmlSerializer(this.GetType());
                Stream stream = new FileStream(Xml, FileMode.Open, FileAccess.Read, FileShare.Read);
                var ret = xs.Deserialize(stream);
                stream.Close();
                
                return ret;

                // return 0;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("加载文件" + Xml + "失败,点击确定新建文件。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) 
                {
                }
                Trace.WriteLine(ex, this.GetType().ToString());
                //  return -1;
            }
            return new object();
        }

        public int SaveSetting()
        {
            try
            {
                if (BackUpPath != "")
                {
                    SaveFileToBackUp();
                }
                XmlSerializer xs1 = new XmlSerializer(this.GetType());
                Stream stream1 = new FileStream(Xml, FileMode.Create, FileAccess.Write, FileShare.Read);
                xs1.Serialize(stream1, this);
                stream1.Close();
                //MessageBox.Show("  保存成功  ");

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存文件" + Xml + "失败");
                Trace.WriteLine(ex, this.GetType().ToString());
                return -1;
            }
        }

        public bool SaveFileToBackUp()
        {
            string BackUpFileName = "";
            string FileName = "";
            string DirPath = DateTime.Now.ToString("yyyy-MM-dd");
            DirPath = BackUpPath + DirPath + "\\";
            if (Directory.Exists(DirPath) == false)
            {
                Directory.CreateDirectory(DirPath);
            }
            FileName = path.Substring(path.LastIndexOf("\\") + 1);
            FileName = FileName.Remove(FileName.LastIndexOf(".xml")) + " " + DateTime.Now.ToString("HH-mm-ss") + ".xml";
            BackUpFileName = DirPath + FileName;
            File.Copy(path, BackUpFileName, true);
            return true;
        }

    }
}
