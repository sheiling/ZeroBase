using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACS.SPiiPlusNET;
using System.Threading;

namespace ZeroBase
{
    public class XCommandACS:XCommandCard
    {
        public ACS.SPiiPlusNET.Api Ch;
        public bool IsConnected=false ;
        public double TotalAxes;
        private object pWait = 100;
        public XCommandACS()
        {
            CurrentCard = CardStyle.ACS;
            Ch = new ACS.SPiiPlusNET.Api();
        }
        public override int Initial()
        {
            return 0;
        }
        public override int ConnectIp(string ipaddress)
        {
            try
            {
                Ch.OpenCommEthernetTCP(ipaddress, (int)EthernetCommOption.ACSC_SOCKET_STREAM_PORT); //UDP

                this.TotalAxes = (int)GetSysinfo(13);
                this.IsConnected = TotalAxes > 0;
            }
            catch
            {
                this.IsConnected = false;
                return -1;
            }
            return 0;
        }
        public override bool IsACSConnect
        {
            get
            {
                return IsConnected;
            }
            set
            {
                IsConnected = value;
            }
        }
        public override int Disconnect()
        {
            try
            {
                IsConnected = false;
                Ch.CloseComm();
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public double GetSysinfo(int key)
        {
            double value = Ch.SysInfo(key);
            return value;

        }

        public override int Commut(int axisId)//换向
        {
            int result = 0;
            ReadIntAxis("MFLAGS", axisId, out result); 
            bool mflagsBrushLess =(result & (1 << 9)) != 0;
            result = 0;
            ReadIntAxis("MFLAGS", axisId, out result);
            bool mflagsBrushok = (result & (1 << 9)) != 0;

            if (mflagsBrushLess && mflagsBrushok == false)
            {
                try
                {
                    Ch.Commut((Axis)axisId);
                }
                catch
                {
                    return -1;
                }
                
            }

            return 0;

        }
        public override int SetServo(int actCardId, int axisId, bool on)
        {
            try
            {

                if (on)
                {
                    Ch.Enable((Axis)axisId); // Enable motor // Enable motor
                }
                else
                {
                    Ch.Disable((Axis)axisId); // Disable motor
                }
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int SetAxisSpeed(int actCardId, int axisId, int vel)
        {
            try
            {
                Ch.SetVelocity((Axis)axisId, vel);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int GoHome(int actCardId, int axisId)
        {
            int bufferid = -1;
            string label = "";
            bool IsRun = false;

            IsRun = true;
            bufferid = axisId;
            label = "home" + axisId.ToString();
            
            try
            {
                if (IsRun)
                {
                    //ACSChanel.RunBuffer(bufferNo, "", ACSChanel.ACSC_ASYNCHRONOUS, ref pWait);
                    Ch.StopBuffer((ProgramBuffer )bufferid);
                    Thread.Sleep(200);
                    //Ch.RunBuffer(bufferid,label,Ch.ACSC_ASYNCHRONOUS,ref pWait);
                    Ch.RunBuffer((ProgramBuffer)bufferid, null);
                }
            }
            catch
            {
            }
            IsRun = false;
            bufferid = -1;
            label = "";
            return 0;
        }

        public override int RunBuffer(int bufferid, string labelname)
        {
            Ch.StopBuffer((ProgramBuffer)bufferid);
            Thread.Sleep(200);
            //Ch.RunBuffer(bufferid,label,Ch.ACSC_ASYNCHRONOUS,ref pWait);
            Ch.RunBuffer((ProgramBuffer)bufferid, labelname);
            return 0;
        }
        public override int MoveAbs(int actCardId, int axisId, double position, double vel)
        {
            try
            {
                Ch.SetVelocity((Axis)axisId, vel);
                Ch.ToPoint(MotionFlags.ACSC_AMF_WAIT, (Axis)axisId, position);
                Ch.Go((Axis)axisId);
                //Ch.WaitMotionEnd(axisId, 10);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int MoveRel(int actCardId, int axisId, double distance, double vel)
        {
            try
            {
                Ch.SetVelocity((Axis)axisId, vel);
                Ch.ToPoint(MotionFlags.ACSC_AMF_RELATIVE, (Axis)axisId, distance);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int MoveJog(int actCardId, int axisId, double vel)
        {
            try
            {
                Ch.Jog(MotionFlags.ACSC_AMF_VELOCITY, (Axis)axisId, vel);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int Stop(int actCardId, int axisId)
        {
            try
            {
                Ch.Halt((Axis)axisId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int EStop(int actCardId, int axisId)
        {
            try
            {
                Ch.Halt((Axis)axisId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int WaitMotionEnd(int axisId, int timeOutMilliseconds)
        {
            try
            {
                Ch.WaitMotionEnd((Axis)axisId, timeOutMilliseconds);
                //Ch.WaitMotionEnd(axisId, timeOutMilliseconds);
            }
            catch
            {
                return -1;
            }
            return 0;

        }
        public override int GetMotionIo(int actCardId, int axisId, ref int sts)
        {
            int _sts, _rSts;
            try
            {
                //_sts = Ch.GetSafetyInputPort(axisId, Ch.ACSC_SYNCHRONOUS, ref pWait);
                _sts=(int)Ch.ReadVariable("FAULT", ProgramBuffer.ACSC_NONE, axisId, axisId);
                _rSts = 0;
                if (XConvert.BitEnable(_sts, XACS_Define.FAULT_DRIVE))
                {
                    XConvert.SetBits(ref _rSts, XACS_Define.FAULT_DRIVE);
                }
                if (XConvert.BitEnable(_sts, XACS_Define.FAULT_RL))
                {
                    XConvert.SetBits(ref _rSts, XACS_Define.FAULT_RL);
                }
                if (XConvert.BitEnable(_sts, XACS_Define.FAULT_LL))
                {
                    XConvert.SetBits(ref _rSts, XACS_Define.FAULT_LL);
                }
                //if (XConvert.BitEnable(_sts, XACS_Define.FAULT_ES))
                //{
                //    XConvert.SetBits(ref _rSts, XACS_Define.FAULT_ES);
                //}
                sts = _rSts;
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int GetMotionSts(int actCardId, int axisId, ref int sts)
        {
            int _sts, _rSts;
            try
            {
                _sts = (int)Ch.ReadVariable("MST", ProgramBuffer.ACSC_NONE, axisId, axisId);
                _rSts = 0;
                if (XConvert.BitEnable(_sts, XACS_Define.MST_ENABLE))
                {
                    XConvert.SetBits(ref _rSts, XACS_Define.MST_ENABLE);
                }
                if (XConvert.BitEnable(_sts, XACS_Define.MST_INPOS))
                {
                    XConvert.SetBits(ref _rSts, XACS_Define.MST_INPOS);
                }
                if (XConvert.BitEnable(_sts, XACS_Define.MST_MOVE))
                {
                    XConvert.SetBits(ref _rSts, XACS_Define.MST_MOVE);
                }
                sts = _rSts;
            }
            catch
            {
                return -1;
            }
            return 0;

        }

        //public override int GetMotionPos(int actCardId, int axisId, ref int pos)
        //{
        //    return 0;
        //}

        public override int GetMotionPos(int actCardId, int axisId, ref double pos)
        {
            try
            {
                
                //pos = Ch.GetFPosition(axisId, Ch.ACSC_SYNCHRONOUS, ref pWait);
                pos = Ch.GetFPosition((Axis)axisId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int GetCommandPos(int actCardId, int axisId, ref double pos)
        {
            try
            {
                //pos = Ch.GetRPosition(axisId, Ch.ACSC_SYNCHRONOUS, ref pWait);
                pos = Ch.GetRPosition((Axis)axisId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int SetAxisJerk(int actCardId, int axisId, double jerkvalue)
        {
            try
            {
                Ch.SetJerk((Axis)axisId, jerkvalue);
               
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int SetAxisAccAndDec(int actCardId, int axisId, double lead, double acc, double dec)
        {
            try
            {
                Ch.SetAcceleration((Axis)axisId, acc);
                Ch.SetDeceleration((Axis)axisId, dec);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int ReadIntAxis(string variableName, out int result)
        {
            try
            {
                result = (int)Ch.ReadVariable(variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                result =-999;
                return -1;
            }
            return 0;
        }
        public override int ReadIntAxis(string variableName,int axisId, out int result)
        {
            try
            {
                result = (int)Ch.ReadVariable(variableName, ProgramBuffer.ACSC_NONE, axisId, axisId);
            }
            catch
            {
                result = -999;
                return -1;
            }
            return 0;
        }
        public override int ReadIntAxis(string variableName, out int []result)
        {
            try
            {
                result = (int[])Ch.ReadVariable(variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                result =new int[]{ -999};
                return -1;
            }
            return 0;
        }
        public override int ReadBufferIntScalar(string variableName,int bufferid, out int result)
        {
            try
            {
                result = (int)Ch.ReadVariable(variableName, (ProgramBuffer)bufferid);
            }
            catch
            {
                result = -999;
                return -1;
            }
            return 0;
        }
        public override int ReadBufferIntScalar(string variableName, int bufferid, int axisId,out int result)
        {
            try
            {
                result = (int)Ch.ReadVariable(variableName, (ProgramBuffer)bufferid,axisId,axisId);
            }
            catch
            {
                result = -999;
                return -1;
            }
            return 0;
        }
        public override int ReadBufferIntScalar(string variableName, int bufferid, out int []result)
        {
            try
            {
                result = (int[])Ch.ReadVariable(variableName, (ProgramBuffer)bufferid);
            }
            catch
            {
                result =new int[]{ -999};
                return -1;
            }
            return 0;
        }



        public override int ReadDoubleScalar(string variableName, out double result)
        {
            try
            {
                result = (double)Ch.ReadVariable(variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                result = -999;
                return -1;
            }
            return 0;
        }
        public override int ReadDoubleScalar(string variableName, out double []result)
        {
            try
            {
                result = (double[])Ch.ReadVariable(variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                result =new double[]{-999};
                return -1;
            }
            return 0;
        }
        public override int ReadDoubleScalar(string variableName, int axisId, out double result)
        {
            try
            {
                result = (double)Ch.ReadVariable(variableName,ProgramBuffer.ACSC_NONE,axisId,axisId );
            }
            catch
            {
                result = 999;
                return -1;
            }
            return 0;
        }
        public override int ReadBufferDoubleScalar(string variableName, int bufferid, out double result)
        {
            try
            {
                result = (double)Ch.ReadVariable(variableName, (ProgramBuffer)bufferid);
            }
            catch
            {
                result = 999;
                return -1;
            }
            return 0;
        }
        public override int ReadBufferDoubleScalar(string variableName, int bufferid, int axisid,out double result)
        {
            try
            {
                result = (double)Ch.ReadVariable(variableName, (ProgramBuffer)bufferid,axisid,axisid);
            }
            catch
            {
                result = 999;
                return -1;
            }
            return 0;
        }
        public override int ReadBufferDoubleScalar(string variableName, int bufferid, out double []result)
        {
            try
            {
                result = (double[])Ch.ReadVariable(variableName, (ProgramBuffer)bufferid);
            }
            catch
            {
                result = new double []{999};
                return -1;
            }
            return 0;
        }

        public override int WriteIntAxis(string variableName, int value, int axisId)
        {
            try
            {
                Ch.WriteVariable(value, variableName, ProgramBuffer.ACSC_NONE, axisId, axisId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int WriteIntAxis(string variableName, int value)
        {
            try
            {
                Ch.WriteVariable(value, variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int WriteIntAxis(string variableName, int []value)
        {
            try
            {
                Ch.WriteVariable(value, variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int WriteIntBuffer(string variableName, int value, int bufferId)
        {
            try
            {
                Ch.WriteVariable(value, variableName, (ProgramBuffer)bufferId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int WriteIntBuffer(string variableName, int value, int bufferId,int axisid)
        {
            try
            {
                Ch.WriteVariable(value, variableName, (ProgramBuffer)bufferId, axisid, axisid);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int WriteIntBuffer(string variableName, int []value, int bufferId)
        {
            try
            {
                Ch.WriteVariable(value, variableName, (ProgramBuffer)bufferId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int WriteDoubleAxis(string variableName, double value)
        {
            try
            {
                Ch.WriteVariable(value, variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                return -1;
            }
            return 0;
        }


        public override int WriteDoubleAxis(string variableName, double value, int axisId)
        {
            try
            {
                Ch.WriteVariable(value, variableName, ProgramBuffer.ACSC_NONE, axisId, axisId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public override int WriteDoubleAxis(string variableName, double []value)
        {
            try
            {
                Ch.WriteVariable(value, variableName, ProgramBuffer.ACSC_NONE);
            }
            catch
            {
                return -1;
            }
            return 0;
        }


        public override int WriteDoubleBuffer(string variableName, double value, int bufferId)
        {
            try
            {
                Ch.WriteVariable(value, variableName, (ProgramBuffer)bufferId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        //Write double array to buffer 
        public override int WriteDoubleBuffer(string variableName, double[] value, int bufferId)
        {
            try
            {
                Ch.WriteVariable(value, variableName,(ProgramBuffer) bufferId);
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        public override int WriteDoubleBuffer(string variableName, double value, int bufferId,int axisid)
        {
            try
            {
                Ch.WriteVariable(value, variableName, (ProgramBuffer)bufferId,axisid,axisid);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

    }
}
