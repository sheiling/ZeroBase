using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS_Define_W32;
using APS168_W32;

namespace ZeroBase
{
    /// <summary>
    /// 板卡操作
    /// </summary>
    public class XCard : XObject
    {
        private int actCardId;
        private string name;
        private XCommandCard commandCard;
        private Dictionary<int, IAxis> axes = new Dictionary<int, IAxis>();
        public XCard(int actCardId, XCommandCard commandCard, string name,string path=null)
        {
            this.actCardId = actCardId;
            this.commandCard = commandCard;
            this.name = name;
            ConfigFilePaht = path;
        }
        public string ConfigFilePaht
        {
            get;
            set;
        }
        public CardStyle CurrentCard
        {
            get { return commandCard.CurrentCard; }
        }
        public int Initial()
        {
            int iRtn = commandCard.Initial();
            if (iRtn < 0)
            {
                string append = "[" + name + "]:" + iRtn;
                XAlarmReporter.Instance.NotifyStations(XAlarmLevel.STOP, (int)XSysAlarmId.CARD_INIT_FAIL, append);
            }
            return iRtn;
        }
        public int Register(int cardId)
        {
            int iRtn = commandCard.Register(cardId);
            if (iRtn < 0)
            {
                string append = "[" + name + "]:" + iRtn;
                XAlarmReporter.Instance.NotifyStations(XAlarmLevel.STOP, (int)XSysAlarmId.CARD_INIT_FAIL, append);
            }
            else
            {
                actCardId = iRtn;
            }
            return iRtn;
        }
        public int ConnectIp(string ipaddress)
        {
            return commandCard.ConnectIp(ipaddress);
        }
        public bool IsACSConnect
        {
            get { return commandCard.IsACSConnect; }
        }
        public int LoadParam(ushort cardnum,string configFn)
        {
            int iRtn = commandCard.LoadParam(cardnum,configFn);
          
            if (iRtn != 0)
            {
                string append = "[" + name + "]:" + iRtn;
                XAlarmReporter.Instance.NotifyStations(XAlarmLevel.STOP, (int)XSysAlarmId.CARD_LOAD_PARAM_FAIL, append);
            }
            return iRtn;
        }

        public int ActId
        {
            get { return this.actCardId; }
        }
        public string Name
        {
            get { return this.name; }
        }

        public int Update()
        {
            return commandCard.Update(actCardId);
        }

        public int SetDo(int channel, int index, int sts)
        {
            return commandCard.SetDo(actCardId, channel, index, sts);
        }
        public int GetDo(int channel, int index, ref int sts)
        {
            return commandCard.GetDo(actCardId, channel,index ,ref sts);
        }
        public int GetDi(int channel, int index, ref int sts)
        {
            return commandCard.GetDi(actCardId, channel, index, ref sts);
        }

        public int ReadChannel(int channel, out double value)
        {
            return commandCard.ReadChannel(actCardId, channel, out value);
        }

        public int WriteChannel(int channel, double value)
        {
            return commandCard.WriteChannel(actCardId, channel, value);
        }

        public int SetServo(int axisId, bool on)
        {
            return commandCard.SetServo(actCardId, axisId, on);
        }
        public int GoHome(int axisId)
        {
            return commandCard.GoHome(actCardId, axisId);
        }

        public int SetPosition(int axisId, double position,double lead)
        {
            return commandCard.SetPosition(actCardId, axisId, position,lead);
        }

        public bool SetLimit(int axisId, int position)
        {
            return commandCard.APS_SetLimitParam(actCardId, axisId, position);
        }

        public int ClearALM(int axisId)
        {
            return commandCard.ClearALM(actCardId, axisId);
        }
        public int MoveAbs(int axisId, int position, int vel)
        {
            return commandCard.MoveAbs(actCardId, axisId, position, vel);
        }
        public int MoveRel(int axisId, int distance, int vel)
        {
            return commandCard.MoveRel(actCardId, axisId, distance, vel);
        }
        public int MoveAbs(int axisId, double position, double vel)
        {
            return commandCard.MoveAbs(actCardId, axisId, position, vel);
        }
        public int MoveRel(int axisId, double distance, double vel)
        {
            return commandCard.MoveRel(actCardId, axisId, distance, vel);
        }
       
        public int MoveJog(int axisId, int dir)
        {
            return commandCard.MoveJog(actCardId,axisId ,dir);
        }
        public int MoveJog(int axisId, double  vel)
        {
            return commandCard.MoveJog(actCardId, axisId, vel);
        }
        public int Stop(int axisId)
        {
            return commandCard.Stop(actCardId, axisId);
        }
        public int EStop(int axisId)
        {
            return commandCard.EStop(actCardId, axisId);
        }

        public int GetMotionIo(int axisId, ref int sts)
        {
            return commandCard.GetMotionIo(actCardId, axisId, ref sts);
        }
        public int GetMotionSts(int axisId, ref int sts)
        {
            return commandCard.GetMotionSts(actCardId, axisId, ref sts);
        }
        public int GetMotionPos(int axisId, ref int pos)
        {
            return commandCard.GetMotionPos(actCardId, axisId, ref pos);
        }
        public int GetCommandPos(int axisId, ref int pos)
        {
            return commandCard.GetCommandPos(actCardId, axisId, ref pos);
        }



        #region ACS
        public int ReadIntVariable(string variableName, out int result)
        {
            return commandCard.ReadIntAxis(variableName, out result);
        }
        public int ReadIntVariable(string variableName, out int []result)
        {
            return commandCard.ReadIntAxis(variableName, out result);
        }
        public int ReadIntVariable(string variableName, int axisid, out int result)//读全局变量数组里的某一个
        {
            return commandCard.ReadIntAxis(variableName, axisid, out result);
        }
        public int ReadBufferIntVariable(string variableName, int burrerid,out int result)
        {
            return commandCard.ReadBufferIntScalar(variableName, burrerid,out result);
        }
        public int ReadBufferIntVariable(string variableName, int burrerid, out int []result)
        {
            return commandCard.ReadBufferIntScalar(variableName, burrerid, out result);
        }
        public int ReadBufferIntVariable(string variableName, int burrerid, int axisid,out int result)
        {
            return commandCard.ReadBufferIntScalar(variableName, burrerid, axisid,out result);
        }

        public int ReadDoubleVariable(string variableName, out double result)
        {
            return commandCard.ReadDoubleScalar(variableName, out result);
        }
        public int ReadDoubleVariable(string variableName, out double[] result)
        {
            return commandCard.ReadDoubleScalar(variableName, out result);
        }
        public int ReadDoubleVariable(string variableName, int axisid, out double result)//读全局变量数组里的某一个
        {
            return commandCard.ReadDoubleScalar(variableName, axisid, out result);
        }
        public int ReadBufferDoubleVariable(string variableName, int burrerid, int axisid, out double result)
        {
            return commandCard.ReadBufferDoubleScalar(variableName, burrerid, axisid, out result);
        }
        public int ReadBufferDoubleVariable(string variableName, int burrerid,out double result)
        {
            return commandCard.ReadBufferDoubleScalar(variableName, burrerid,out result);
        }
        public int ReadBufferDoubleVariable(string variableName, int burrerid, out double []result)
        {
            return commandCard.ReadBufferDoubleScalar(variableName, burrerid, out result);
        }

        public  int WriteIntVariable(string variableName, int value, int axisId)
        {
            return commandCard.WriteIntAxis(variableName, value, axisId);
        }
        public int WriteIntVariable(string variableName, int value)
        {
            return commandCard.WriteIntAxis(variableName, value);
        }
        public int WriteIntVariable(string variableName, int []value)
        {
            return commandCard.WriteIntAxis(variableName, value);
        }
        public int WriteIntBuffer(string variableName, int value, int bufferId)
        {
            return commandCard.WriteIntBuffer(variableName,value,bufferId);
        }
        public int WriteIntBuffer(string variableName, int value, int bufferId, int axisid)
        {
            return commandCard.WriteIntBuffer(variableName, value, bufferId,axisid);
        }
        public int WriteIntBuffer(string variableName, int []value, int bufferId)
        {
            return commandCard.WriteIntBuffer(variableName, value, bufferId);
        }
        public int WriteDoubleVariable(string variableName, double value, int axisId)
        {
            return commandCard.WriteDoubleAxis(variableName,value,axisId);
        }
        public int WriteDoubleVariable(string variableName, double value)
        {
            return commandCard.WriteDoubleAxis(variableName, value);
        }
        public int WriteDoubleVariable(string variableName, double []value)
        {
            return commandCard.WriteDoubleAxis(variableName, value);
        }
        public int WriteDoubleBuffer(string variableName, double value, int bufferId)
        {
            return commandCard.WriteDoubleBuffer(variableName, value, bufferId);
        }
        public int WriteDoubleBuffer(string variableName, double value, int bufferId,int axisid)
        {
            return commandCard.WriteDoubleBuffer(variableName, value, bufferId,axisid);
        }
        public int WriteDoubleBuffer(string variableName, double []value, int bufferId)
        {
            return commandCard.WriteDoubleBuffer(variableName, value, bufferId);
        }

        public int RunBuffer(int bufferid, string labelname)
        {

            return commandCard.RunBuffer(bufferid, labelname);
        }
        #endregion




        public int GetMotionPos(int axisId, ref double pos)
        {
            return commandCard.GetMotionPos(actCardId, axisId, ref pos);
        }
        public int GetCommandPos(int axisId, ref double  pos)
        {
            return commandCard.GetCommandPos(actCardId, axisId, ref pos);
        }
        public int SetAxisAccAndDec(int axisId, double lead, double acc, double dec)
        {
            return commandCard.SetAxisAccAndDec(actCardId, axisId, lead, acc, dec);
        }
        public int SetAxisJerk(int axisId, double jerkvalue)
        {
            return commandCard.SetAxisJerk(actCardId,axisId,jerkvalue);
        }
        public int SetStopDec(int axisId, double lead,  double dec)
        {
            return commandCard.SetStopDec(actCardId, axisId, lead, dec);
        }
        public int APS_SetJogParam( int axisId, int mode, int dir, double lead, double acc, double dec, int vel)
        {
            return commandCard.APS_SetJogParam(actCardId, axisId,mode,dir, lead, acc, dec,vel);
        }
        public int APS_SetAxisParam(int axisId, double lead, APS_Define PRA, double value)
        {
            return commandCard.APS_SetAxisParam(actCardId, axisId, lead, PRA, value);
        }
        public int APS_DIO_SetCOSInterrupt32(byte Port, uint ctl, out uint hEvent, bool ManualReset)
        {
            return commandCard.APS_DIO_SetCOSInterrupt32(actCardId, Port, ctl, out hEvent, ManualReset);
        }
        public int APS_DIO_INT1_EventMessage(int index, uint windowHandle, uint message, MulticastDelegate callbackAddr)
        {
            return commandCard.APS_DIO_INT1_EventMessage(actCardId, index, windowHandle, message, callbackAddr);
        }
        public int APS_SetBacklashEnable(int axisId, int on)
        {
            return commandCard.APS_SetBacklashEnable(actCardId, axisId, on);
        }

        public int MoveLineAbs(int[] axisId, double[] pos, double vel)
        {
            return commandCard.MoveLineAbs(axisId, pos, vel);
        }
        public int MoveLineRel(int[] axisId, double[] pos, double vel)
        {
            return commandCard.MoveLineRel(axisId, pos, vel);
        }
        public int MoveArcAbs(int[] axisId, double[] center, double angle, double vel)
        {
            return commandCard.MoveArcAbs(axisId, center, angle, vel);
        }
        public int MoveArcAbs(int[] axisId, double[] center, double[] pos, ArcDir dir, double vel)
        {
            return commandCard.MoveArcAbs(axisId, center, pos, (short)dir, vel);
        }
        public int MoveArcRel(int[] axisId, double[] center, double angle, double vel)
        {
            return commandCard.MoveArcRel(axisId, center, angle, vel);
        }
        public int MoveArcRel(int[] axisId, double[] center, double[] pos, ArcDir dir, double vel)
        {
            return commandCard.MoveArcRel(axisId, center, pos, (short)dir, vel);
        }


        public int APS_pt_start(int ptbId)
        {
            return commandCard.APS_pt_start(actCardId, ptbId);
        }
        public int APS_get_pt_status(int ptbId, ref PTSTS Status)
        {
            return commandCard.APS_get_pt_status(actCardId, ptbId, ref Status);
        }

        #region  雷赛

        public int CheckMoveDone(ushort axisId)
        {
            return commandCard.CheckMoveDone((ushort) actCardId, axisId);
        }
        public int CheckHomeDone(ushort axisId)
        {
            return commandCard.CheckHomeDone((ushort)actCardId, axisId);
        }

        public int SetDAfunction(ushort enable)
        {
            return commandCard.SetDAfunction((ushort)actCardId, enable);
        }


        public int GetADinput(ushort channel, ref double Value)
        {
            return commandCard.GetADinput((ushort)actCardId, channel, ref Value);
        }



        public int SetDAoutput(ushort channel, double value)
        {
            return commandCard.SetDAoutput((ushort)actCardId, channel, value);
        }

        public int SetConnectState(int CardId, int NodeNum, int Stata)
        {
            return commandCard.SetConnectStata((ushort)CardId, (ushort)NodeNum, (ushort)Stata);
        }
       

        #endregion
    }

    
}
