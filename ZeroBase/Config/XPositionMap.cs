using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ZeroBase
{
    public class XPositionMap : XObject
    {
        private Dictionary<string, XPosition> positionMap = new Dictionary<string, XPosition>();
        private Dictionary<string, string> nameMap = new Dictionary<string, string>();
        private string path;
        private string name_path;
        private string root;
        private string node;
        public XPositionMap(string path, string name_path, string root, string node)
        {
            this.path = path;
            this.name_path = name_path;
            this.root = root;
            this.node = node;
        }

        public int LoadPositions()
        {
            try
            {
                positionMap.Clear();
                string[] nodes1, content1;
                XXml.ReadNodeAndInnerText(path, root + "//" + node, out nodes1, out content1);
                if (nodes1.Length > 0)
                {
                    if (content1[0].Contains(","))
                    {
                        int[] axisNo = XConvert.Str2IntG(content1[0], ',');
                        for (int i = 1; i < nodes1.Length; i++)
                        {
                            string posName = nodes1[i];
                            double[] pos = XConvert.Str2DoubleG(content1[i], ',');
                            XPosition xPts = new XPosition(axisNo, pos, axisNo.Length);
                            positionMap.Add(posName, xPts);
                        }
                    }
                    else
                    {
                        int[] axisNo = {int.Parse(content1[0]) };
                        for (int i = 1; i < nodes1.Length; i++)
                        {
                            string posName = nodes1[i];
                            double[] pos = { double.Parse(content1[i]) };
                            XPosition xPts = new XPosition(axisNo, pos, axisNo.Length);
                            positionMap.Add(posName, xPts);
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message, this.GetType().ToString());
                return -1;
            }
        }

        public int LoadPositionNames()
        {
            try
            {
                nameMap.Clear();
                string[] nodes1, content1;
                XXml.ReadNodeAndInnerText(name_path, root + "//" + node, out nodes1, out content1);
                if (nodes1.Length > 0)
                {
                    for (int i = 0; i < nodes1.Length; i++)
                    {
                        nameMap.Add(nodes1[i], content1[i]);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message, this.GetType().ToString());
                return -1;
            }
        }

        public string Node
        {
            get { return node; }
        }

        public XPosition FindPositionById(string id)
        {
            if (!positionMap.ContainsKey(id))
            {
                return null;
            }
            return positionMap[id];
        }

        public Dictionary<string, XPosition> PositionMap
        {
            get { return positionMap; }
        }

        public bool CheckExist(string key, XPosition pos)
        {
            return PositionMap.ContainsKey(key);
        }


        public string FindNameById(string id)
        {
            if (!nameMap.ContainsKey(id))
            {
                return null;
            }
            return nameMap[id];
        }

        public Dictionary<string, string> NameMap
        {
            get { return nameMap; }
        }

        public bool ContainsKey(string key)
        {
            return positionMap.ContainsKey(key);
        }

        public void Add(string key, XPosition position)
        {
            if (positionMap.ContainsKey(key) == false)
            {
                position.Name = key;
                positionMap.Add(key, position);
                nameMap.Add(key, position.Name);

            }
            //if (nameMap.ContainsKey(key) == false)
            //{
            //    nameMap.Add(key, position.Name);
            //}
        }

        public void Remove(string key)
        {
            if (positionMap.ContainsKey(key))
            {
                positionMap.Remove(key);
            }
            if (nameMap.ContainsKey(key))
            {
                nameMap.Remove(key);
            }
        }

    }
    //public class Mapname : Dictionary<string, string>
    //{
    //    public string this[string key]
    //    {
    //        set
    //        {
    //            if (this.ContainsKey(key))
    //            {
    //                this.Remove(key);
    //                this.Add(key, value);
    //            }
    //        }
    //        get
    //        {
    //            if (this.ContainsKey(key))
    //            {
    //                var ss = "";
    //                this.TryGetValue(key, out ss);
    //                return ss;
    //            }
    //            this.Add(key, "");
    //            MessageBox.Show("NO exit");
    //            return "";
    //        }
    //    }






    //}





}
