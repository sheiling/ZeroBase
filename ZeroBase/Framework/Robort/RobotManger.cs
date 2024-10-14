using System.Collections.Generic;

namespace ZeroBase
{
    public class RobotManger
    {
        private Dictionary<int, XRobotControler> robotCtrMap = new Dictionary<int, XRobotControler>();
        private Dictionary<int, XRobortDi> robotdiMap = new Dictionary<int, XRobortDi>();
        private Dictionary<int, XRobortDo> robotdoMap = new Dictionary<int, XRobortDo>();
        private static RobotManger instance = new RobotManger();
        RobotManger()
        {

        }
        public static RobotManger Instance
        {
            get { return instance; }
        }

        //通讯控制器
        public RobortInformation BindSocketCtr(int setId, int taskId,int stationId, XCommandRobot command, string name, string comOrIpAddress, int banudRateOrPortNo, RobotIo io, bool failShowDlg = true, int retryTimes = 5)
        {
            RobortInformation robortinf = new RobortInformation();
            if (robotCtrMap.ContainsKey(setId) == false)
            {
                var ctr = new XRobotControler(setId,taskId, command, name, comOrIpAddress, banudRateOrPortNo, io, failShowDlg, retryTimes);
                robotCtrMap.Add(setId, ctr);
                robortinf.RobortId = setId;
                robortinf.TaskId = taskId;
                robortinf.StationId = stationId;
                robortinf.RobortName = name;
                robortinf.Di_Alarm =XDevice.Instance.FindDiById(command.Robort_IO.DiAlarm);
                robortinf.Di_ProgramRunnig = XDevice.Instance.FindDiById(command.Robort_IO.DiProgramRunnig);

            }
            return robortinf;
        }
        public void BindRobortDi(int robortid,int setId,int actid,string name)
        {
            if (robotCtrMap.ContainsKey(robortid) == false)
            {
                return;
            }
            else
            {
                XRobortDi robortdi = new XRobortDi(robotCtrMap[robortid].Command, actid, name, robotCtrMap[robortid].Name);
                robotdiMap.Add(setId, robortdi);
            }
        }
        public void BindRobortDo(int robortid, int setId, int actid, string name)
        {
            if (robotCtrMap.ContainsKey(robortid) == false)
            {
                return;
            }
            else
            {
                XRobortDo robortdo = new XRobortDo(robotCtrMap[robortid].Command, actid, name, robotCtrMap[robortid].Name);
                robotdoMap.Add(setId, robortdo);
            }
        }
        public XRobotControler FindRobotCtrById(int id)
        {
            if (robotCtrMap.ContainsKey(id) == false)
            {
                return null;
            }
            return robotCtrMap[id];
        }
        public XRobortDi FindRobotDiById(int id)
        {
            if (robotdiMap.ContainsKey(id) == false)
            {
                return null;
            }
            return robotdiMap[id];
        }
        public XRobortDo FindRobotDoById(int id)
        {
            if (robotdoMap.ContainsKey(id) == false)
            {
                return null;
            }
            return robotdoMap[id];
        }



    }
}
