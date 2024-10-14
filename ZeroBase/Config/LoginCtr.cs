using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;



namespace ZeroBase
{

    public enum loginType
    {
        Op,
        Admin,
        Tech,
        IPQC,
        None
    }


    [Serializable]
    public class LoginCtr
    {

        public static loginType loginTp = loginType.None;

        public static string Xml;

        public LoginCtr()
        {
        }

        //public string Xml
        //{
        //    get { return "D:\\Config\\Setting\\Login.xml"; }
        //}

        public string AdminUser { get; set; }
        public string AdminPwd { get; set; }


        public string TecUser { get; set; }
        public string TecPwd { get; set; }

        public string opUser { get; set; }
        public string opPwd { get; set; }

        public string IPQCUser { get; set; }
        public string IPQCPwd { get; set; }

        public string BuildDate { get; set; }
        public string HWVision { get; set; }
        public string SWUpdate { get; set; }



        public bool Save()
        {
            try
            {
                if (!File.Exists(Xml))
                {
                    AdminUser = "ADMIN";
                    AdminPwd = "ADMIN";

                    TecUser = "ITC";
                    TecPwd = "1";

                    opUser = "OP";
                    opPwd = "1";

                    IPQCUser = "IPQC";
                    IPQCPwd = "1";

                    HWVision = "VJ1.0.1";
                    BuildDate = "2017.01.05";
                    SWUpdate = "2018.07.06";

                }
                XmlSerializer xs = new XmlSerializer(typeof(LoginCtr));
                Stream stream = new FileStream(Xml, FileMode.Create, FileAccess.Write, FileShare.Read);
                xs.Serialize(stream, this);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public LoginCtr Load()
        {
            try
            {
                if (File.Exists(Xml) == false)
                {
                    Save();
                }
                XmlSerializer xs = new XmlSerializer(typeof(LoginCtr));
                Stream stream = new FileStream(Xml, FileMode.Open, FileAccess.Read, FileShare.Read);
                var ret = xs.Deserialize(stream) as LoginCtr;
                stream.Close();
                if (ret.IPQCPwd ==null)
                {
                    File.Delete(Xml);
                    Load();
                }
                return ret;
            }
            catch
            {
                MessageBox.Show("加载文件失败：" + Xml);
                return this;
            }
        }
    }
}



