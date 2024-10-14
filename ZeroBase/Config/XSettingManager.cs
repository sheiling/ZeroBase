using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public sealed class XSettingManager
    {
        private Dictionary<int, dynamic> settingMap = new Dictionary<int, dynamic>();

        private static XSettingManager instance = new XSettingManager();

        XSettingManager()
        {
        }
        public static XSettingManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setId">User ctr Id</param>
        /// <param name="setting"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public void BindSetting(int setId, XSetting setting, string name, string path)
        {
            if (settingMap.ContainsKey(setId) == false)
            {
                setting.Name = name;
                setting.Xml = path;
                settingMap.Add(setId, setting);
            }

        }

        public XSetting FindSettingById(int setId)
        {
            if (settingMap.ContainsKey(setId) == false)
            {
                return null;
            }
            return settingMap[setId];
        }

        public Dictionary<int, dynamic> SettingMap
        {
            get { return this.settingMap; }
        }

        public void LoadSettings()
        {
            var test = new List<int>(settingMap.Keys);
            for (int i = 0; i < settingMap.Count; i++)
            {
                var index = test[i];
                settingMap[index] = settingMap[index].LoadSetting();
            }
        }
    }
}
