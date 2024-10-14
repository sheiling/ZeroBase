using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using System.Xml;

namespace ZeroBase
{
    /// <summary>
    /// 点位信息
    /// </summary>
    public sealed class XPositionManager : XObject
    {
        private string dir_positionXml;
        private string file_positionXml;
        private string root_positionXml;
        private string file_positionNameXml;
        private string dir_BackUp;
        public const string AXISID = "AxisId";
        private const string TABLEHEAD = "Task"; 
        private Dictionary<int, XPositionMap> positionSet = new Dictionary<int, XPositionMap>();
        private readonly static XPositionManager instance = new XPositionManager();
        XPositionManager() 
        {

        }
        public static XPositionManager Instance
        {
            get { return instance; }
        }

        public void SetPositionXmlPathAndRoot(string dir, string file_Position, string file_PositionName, string root, string dir_BackUp)
        {
            this.dir_positionXml = dir;
            this.file_positionXml = file_Position;
            this.file_positionNameXml = file_PositionName;
            this.root_positionXml = root;
            this.dir_BackUp = dir_BackUp;
        }

        public int BindPositionTableByTaskId(int setTaskId)
        {
            if (positionSet.ContainsKey(setTaskId) == true)
            {
                return -1;
            }
            string node = TABLEHEAD + setTaskId.ToString();
            XPositionMap table = new XPositionMap(dir_positionXml + file_positionXml, dir_positionXml + file_positionNameXml, root_positionXml, node);
            positionSet.Add(setTaskId, table);

            if (XXml.FindChildInParent(PositionXml_Path, PositionXml_Root, node) == false)
            {
                XXml.NewElement(XPositionManager.Instance.PositionXml_Path,
                   XPositionManager.Instance.PositionXml_Root,
                   XPositionManager.Instance.PositionSet[setTaskId].Node,
                   "");
                XXml.NewElement(XPositionManager.Instance.PositionXml_Path,
                    XPositionManager.Instance.PositionXml_Root + "//" + XPositionManager.Instance.PositionSet[setTaskId].Node,
                    AXISID,
                    "");
                XXml.UpdateInnerText(XPositionManager.Instance.PositionXml_Path,
                 XPositionManager.Instance.PositionXml_Root + "//" + XPositionManager.Instance.PositionSet[setTaskId].Node + "//" + XPositionManager.AXISID,
                 XConvert.IntG2Str(XTaskManager.Instance.FindTaskById(setTaskId).AxisMap.Keys.ToArray(), ","));
            }
            
            return 0;
        }

        public int UnBindPositionTable(int setId)
        {
            if (positionSet.ContainsKey(setId) == false)
            {
                return 0;
            }
            positionSet.Remove(setId);
            return 0;
        }

        public void LoadPositionSet()
        {
            foreach (KeyValuePair<int, XPositionMap> kvp in positionSet)
            {
                kvp.Value.LoadPositions();
                kvp.Value.LoadPositionNames();
            }
        }

        public XPositionMap FindPositionTableByTaskId(int setTaskId)
        {
            if (positionSet.ContainsKey(setTaskId) == false)
            {
                return null;
            }
            return positionSet[setTaskId];
        }

        public Dictionary<int, XPositionMap> PositionSet
        {
            get { return positionSet; }
        }

        public string PositionXml_Path
        {
            get { return dir_positionXml + file_positionXml; }
        }

        public string PositionNameXml_Path
        {
            get { return dir_positionXml + file_positionNameXml; }
        }

        public string PositionXml
        {
            get { return file_positionXml; }
        }

        public string PositionNameXml
        {
            get { return file_positionNameXml; }
        }


        public string PositionXml_Root
        {
            get { return root_positionXml; }
        }

        public string BackUpPosition_Dir
        {
            get { return dir_BackUp; }
        }

    }
}
