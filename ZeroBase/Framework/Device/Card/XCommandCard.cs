using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using APS_Define_W32;
using APS168_W32;


namespace ZeroBase
{
    public abstract class XCommandCard : XObject
    {
        public int[] DI_Data = new int[20];
        public int[] DO_Data = new int[20];
        public bool[,] PCIE_8338_DI_Data = new bool[4,8];
        public bool[,] PCIE_8338_DO_Data = new bool[4,8];
        public CardStyle CurrentCard { get; set; }
        public virtual int ConnectIp(string ipaddress) { return -1; }
        public virtual int Disconnect() { return -1; }
        public virtual int Commut(int axisId) { return -1; }
        public virtual bool IsACSConnect { get; set; }
        public virtual int Initial() { return -1; }
        public virtual int Register(int cardId) { return -1; }
        public virtual int Close() { return -1; }
        public virtual int LoadParam(ushort cardnum, string configFn) { return -1; }

        public virtual int Update(int actCardId) { return -1; }

        public virtual int SetDo(int actCardId, int channel, int index, int sts) { return -1; }
        public virtual int GetDo(int actCardId, int channel, int index, ref int sts) { return -1; }
        public virtual int GetDi(int actCardId, int channel, int index, ref int sts) { return -1; }

        public virtual int ReadChannel(int actCardId, int channel, out double value) { value = 0; return -1; }
        public virtual int WriteChannel(int actCardId, int channel, double value) { return -1; }

        public virtual int SetServo(int actCardId, int axisId, bool on) { return -1; }
        public virtual int GoHome(int actCardId, int axisId) { return -1; }

        public virtual int SetPosition(int acdCardId, int axisId, double position,double lead) { return -1; }

        public virtual int ClearALM(int actCardId, int axisId)
        {
            return -1;
        }

        public virtual int MoveAbs(int actCardId, int axisId, int position, int vel) { return -1; }
        public virtual int MoveRel(int actCardId, int axisId, int distance, int vel) { return -1; }
        public virtual int MoveAbs(int actCardId, int axisId, double position, double vel) { return -1; }
        public virtual int MoveRel(int actCardId, int axisId, double distance, double vel) { return -1; }
        public virtual bool APS_SetLimitParam(int actCardId, int axisId, int pos) { return false; }
        public virtual int APS_SetJogParam(int actCardId, int axisId, int mode, int dir, double lead, double acc, double dec ,int vel) { return -1; }
        public virtual int MoveJog(int actCardId, int axisId, int dir) { return -1; }
        public virtual int MoveJog(int actCardId, int axisId, double vel) { return -1; }
        public virtual int Stop(int actCardId, int axisId) { return -1; }
        public virtual int EStop(int actCardId, int axisId) { return -1; }

        public virtual int GetMotionIo(int actCardId, int axisId, ref int sts) { return -1; }
        public virtual int GetMotionSts(int actCardId, int axisId, ref int sts) { return -1; }
        public virtual int GetMotionPos(int actCardId, int axisId, ref int pos) { return -1; }
        public virtual int GetCommandPos(int actCardId, int axisId, ref int pos) { return -1; }

        public virtual int WaitMotionEnd(int axisId, int timeOutMilliseconds) { return -1; }

        public virtual int ReadIntAxis(string variableName,out int result) { result = 0; return -1; }
        public virtual int ReadIntAxis(string variableName, int axisId, out int result) { result = 0; return -1; }
        public virtual int ReadIntAxis(string variableName, out int []result) { result=new int[]{0}; return -1; }
        public virtual int ReadBufferIntScalar(string variableName,int bufferid, out int result) { result = 0; return -1; }
        public virtual int ReadBufferIntScalar(string variableName, int bufferid,int axisId, out int result) { result = 0; return -1; }
        public virtual int ReadBufferIntScalar(string variableName, int bufferid, out int []result) { result =new int[]{0}; return -1; }

        public virtual int ReadDoubleScalar(string variableName, out double result) { result = 0; return -1; }
        public virtual int ReadDoubleScalar(string variableName, out double []result) { result =new double[]{ 0}; return -1; }
        public virtual int ReadDoubleScalar(string variableName, int axisId, out double result) { result = 0; return -1; }
        public virtual int ReadBufferDoubleScalar(string variableName, int bufferid, out double  result) { result = 0; return -1; }
        public virtual int ReadBufferDoubleScalar(string variableName, int bufferid, int axisId, out double result) { result = 0; return -1; }
        public virtual int ReadBufferDoubleScalar(string variableName, int bufferid, out double[] result) { result = new double[] { 0 }; return -1; }

        
        public virtual int WriteIntAxis(string variableName, int value, int axisId) { return -1; }
        public virtual int WriteIntAxis(string variableName, int value) { return -1; }
        public virtual int WriteIntAxis(string variableName, int[] value) { return -1; }

        public virtual int WriteDoubleAxis(string variableName, double value) { return -1; }
        public virtual int WriteDoubleAxis(string variableName, double value, int axisId) { return -1; }
        public virtual int WriteDoubleAxis(string variableName, double[] value) { return -1; }

        public virtual int WriteIntBuffer(string variableName, int value, int bufferId) { return -1; }
        public virtual int WriteIntBuffer(string variableName, int []value, int bufferId) { return -1; }
        public virtual int WriteIntBuffer(string variableName, int value, int bufferId,int axisId) { return -1; }

        public virtual int WriteDoubleBuffer(string variableName, double value, int bufferId) { return -1; }
        public virtual int WriteDoubleBuffer(string variableName, double[] value, int bufferId) { return -1; }
        public virtual int WriteDoubleBuffer(string variableName, double value, int bufferId,int axisId) { return -1; }

        public virtual int RunBuffer(int bufferid, string labelname) { return -1; }

        public virtual int GetMotionPos(int actCardId, int axisId, ref double pos) { return -1; }
        public virtual int GetCommandPos(int actCardId, int axisId, ref double pos) { return -1; }

        public virtual int SetAxisJerk(int actCardId, int axisId, double jerkvalue) { return -1; }
        public virtual int SetAxisAccAndDec(int actCardId, int axisId, double lead, double acc, double dec) { return -1;}
        public virtual int SetStopDec(int actCardId, int axisId, double lead,  double dec) { return -1; }
        public virtual int SetAxisSpeed(int actCardId, int axisId, int vel) { return -1; }
        public virtual int APS_SetAxisParam(int actCardId, int axisId, double lead, APS_Define PRA, double value) { return -1; }
        public virtual int APS_DIO_SetCOSInterrupt32(int actCardId, byte Port, uint ctl, out uint hEvent, bool ManualReset) { hEvent = 0; return -1; } 
        public virtual int APS_DIO_INT1_EventMessage(int actCardId, int index, uint windowHandle, uint message, MulticastDelegate callbackAddr) { return -1; }
        public virtual int APS_SetBacklashEnable(int actCardId, int axisId, int on) { return -1; }

        public virtual int MoveLineAbs(int[] axisId, double[] pos, double vel) { return -1; }
        public virtual int MoveLineRel(int[] axisId, double[] pos, double vel) { return -1; }
        public virtual int MoveArcAbs(int[] axisId, double[] center, double angle, double vel) { return -1; }
        public virtual int MoveArcAbs(int[] axisId, double[] center, double[] end, short dir, double vel) { return -1; }
        public virtual int MoveArcRel(int[] axisId, double[] center, double angle, double vel) { return -1; }
        public virtual int MoveArcRel(int[] axisId, double[] center, double[] end, short dir, double vel) { return -1; }

        public virtual int APS_pt_start(int actCardId, int ptbId) { return -1; }
        public virtual int APS_get_pt_status(int actCardId, int ptbId, ref PTSTS Status) { return -1; }

        public virtual int CheckMoveDone(ushort actCardId, ushort axisId) { return -1; }

        public virtual int CheckHomeDone(ushort actCardId, ushort axisId) { return -1; }


        public virtual int SetDAfunction(ushort actCardId, ushort enable) { return -1; }


        public virtual int GetADinput(ushort actCardId, ushort channel, ref double Value) { return -1; }

        public virtual int SetDAoutput(ushort actCardId, ushort channel, double value) { return -1; }

        public virtual int SetConnectStata(ushort CardId, ushort NodeNum, ushort Stata) { return -1; }
    }
    public enum CardStyle
    {
        ADLINK,
        ACS,
        Leisai
    }
}
