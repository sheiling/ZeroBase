using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace ZeroBase
{
    public class XXml : XObject
    {
        public static int NewElement(string path, string urlParent, string nodeName, string innerText)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode parent = doc.SelectSingleNode(urlParent);
                XmlElement child = doc.CreateElement(nodeName);
                child.InnerText = innerText;
                parent.AppendChild(child);
                doc.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int NewElement(string path, string urlParent, string[] nodeName, string[] innerText)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                for (int i = 0; i < nodeName.Length; i++)
                {
                    XmlElement child = dom.CreateElement(nodeName[i]);
                    child.InnerText = innerText[i];
                    parent.AppendChild(child);
                }
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static bool FindChildInParent(string path, string urlParent, string child)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode parent = doc.SelectSingleNode(urlParent);
                XmlNodeList children = parent.ChildNodes;
                foreach (XmlNode node in children)
                {
                    if (node.Name == child)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return false;
            }
        }

        public static int ReadNode(string path, string urlParent, out string[] nodeName)
        {
            nodeName = null;
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                XmlNodeList children = parent.ChildNodes;
                int count = children.Count;
                nodeName = new string[count];
                for (int i = 0; i < count; i++)
                {
                    nodeName[i] = children[i].Name;
                }
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int ReadNodeAndInnerText(string path, string urlParent, out string[] nodeName,out string[] innerText)
        {
            nodeName = null;
            innerText = null;
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                XmlNodeList children = parent.ChildNodes;
                int count = children.Count;
                nodeName = new string[count];
                innerText = new string[count];
                for (int i = 0; i < count; i++)
                {
                    nodeName[i] = children[i].Name;
                    innerText[i] = children[i].InnerText;
                }
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int UpdateInnerText(string path, string urlNode, string innerText)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode node = dom.SelectSingleNode(urlNode);
                node.InnerText = innerText;
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int UpdateNode(string path, string urlParent,string[] nodeName,string[] innerText)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                parent.RemoveAll();
                for (int i = 0; i < nodeName.Length; i++)
                {
                    XmlElement xe = dom.CreateElement(nodeName[i]);
                    xe.InnerText = innerText[i];
                    parent.AppendChild(xe);
                }
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int UpdateNode(string path, string urlParent, List<string> nodeName, List<string> innerText)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                parent.RemoveAll();
                for (int i = 0; i < nodeName.Count; i++)
                {
                    XmlElement xe = dom.CreateElement(nodeName[i]);
                    xe.InnerText = innerText[i];
                    parent.AppendChild(xe);
                }
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int DeleteElement(string path,string urlParent,string nodeName)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                XmlNodeList children = parent.ChildNodes;
                foreach (XmlElement child in children)
                {
                    if (child.Name == nodeName)
                    {
                        parent.RemoveChild(child);
                    }
                }
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int DeleteNodeAndInnerText(string path, string urlParent,string nodeName)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                XmlNodeList children = parent.ChildNodes;
                foreach (XmlElement child in children)
                {
                    if (child.Name == nodeName)
                    {
                        child.RemoveAll();
                        parent.RemoveChild(child);
                    }
                }
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }

        public static int DeleteInnerText(string path, string urlParent, string nodeName)
        {
            try
            {
                XmlDocument dom = new XmlDocument();
                dom.Load(path);
                XmlNode parent = dom.SelectSingleNode(urlParent);
                XmlNodeList children = parent.ChildNodes;
                foreach (XmlElement child in children)
                {
                    if (child.Name == nodeName)
                    {
                        child.RemoveAll();
                    }
                }
                dom.Save(path);
                return 0;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            } 
        }
    }
}
