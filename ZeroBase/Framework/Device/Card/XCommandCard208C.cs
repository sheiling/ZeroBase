using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS168_W32;
using APS_Define_W32;

namespace ZeroBase
{
    public class XCommandCard208C : XCommandCard
    {
        private object obj = new object();
        public XCommandCard208C()
        {
            CurrentCard = CardStyle.ADLINK;
        }

        public override int Initial()
        {
            lock (obj)
            {
                int boardId = 0;
                int mode = 1;
                int ret =APS168.APS_initial(ref boardId, mode);
                if (ret != 0)
                {
                    return ret;
                }
                return boardId;
            }
        }

        public override int LoadParam(ushort cardnum,string configFn)
        {
            lock (obj)
            {
                return APS168.APS_load_param_from_file(configFn);
            }
        }

        public override int Close()
        {
            lock (obj)
            {
                return APS168.APS_close();
            }
        }

        public override int Update(int actCardId)
        {
            lock (obj)
            {
                int diData = 0;
                int doData = 0;
                APS168.APS_read_d_input(actCardId, 0, ref diData);
                APS168.APS_read_d_output(actCardId, 0, ref doData);
                DI_Data[0] = diData;
                DO_Data[0] = doData;
                return 0;
            }
        }

        public override int SetDo(int actCardId, int channel, int index, int sts)
        {
            lock (obj)
            {
                return APS168.APS_write_d_channel_output(actCardId, channel, index, sts);
            }
        }

        public override int GetDo(int actCardId, int channel, int index, ref int sts)
        {
            lock (obj)
            {
                //return APS168.APS_read_d_channel_output(actCardId, channel, index, ref sts);
                sts = (DO_Data[channel] >> index) & 1;
                return 0;
            }
        }

        public override int GetDi(int actCardId, int channel, int index, ref int sts)
        {
            lock (obj)
            {
                //int DI_Data = 0;
                //int iRtn = APS168.APS_read_d_input(actCardId, channel, ref DI_Data);
                sts = (DI_Data[channel] >> index) & 1;
                return 0;
            }
        }

        public override int SetServo(int actCardId, int axisId, bool on)
        {
            lock (obj)
            {
                int i = (on) ? 1 : 0;
                return APS168.APS_set_servo_on(axisId, i);
            }
        }

        public override int GoHome(int actCardId, int axisId)
        {
            lock (obj)
            {
                return APS168.APS_home_move(axisId);
            }
        }

        public override int MoveAbs(int actCardId, int axisId, int position, int vel)
        {
            lock (obj)
            {
                return APS168.APS_absolute_move(axisId, position, vel);
            }
        }

        public override int MoveRel(int actCardId, int axisId, int distance, int vel)
        {
            lock (obj)
            {
                return APS168.APS_relative_move(axisId, distance, vel);
            }
        }

        public override int Stop(int actCardId, int axisId)
        {
            lock (obj)
            {
                return APS168.APS_stop_move(axisId);
            }
        }

        public override int EStop(int actCardId, int axisId)
        {
            lock (obj)
            {
                return APS168.APS_emg_stop(axisId);
            }
        }

        public override int GetMotionIo(int actCardId, int axisId, ref int sts)
        {
            lock (obj)
            {
                int _sts, _rSts;
                _sts = APS168.APS_motion_io_status(axisId);
                _rSts = 0;
                if (XConvert.BitEnable(_sts, 0x01 << 0))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MIO_ALM);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 1))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MIO_PEL);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 2))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MIO_MEL);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 3))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MIO_ORG);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 4))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MIO_EMG);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 7))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MIO_SVON);
                }
                sts = _rSts;
                return 0;
            }
        }

        public override int GetMotionSts(int actCardId, int axisId, ref int sts)
        {
            lock (obj)
            {
                int _sts, _rSts;
                _sts = APS168.APS_motion_status(axisId);
                _rSts = 0;
                if (XConvert.BitEnable(_sts, 0x01 << 5))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MTS_MDN);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 6))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MTS_HMV);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 16))
                {
                    XConvert.SetBits(ref _rSts, XAPS_Define.MTS_ASTP);
                }
                sts = _rSts;
                return 0;
            }

        }

        public override int GetMotionPos(int actCardId, int axisId, ref int pos)
        {
            lock (obj)
            {
                return APS168.APS_get_position(axisId, ref pos);
            }
        }

        public override int GetCommandPos(int actCardId, int axisId, ref int pos)
        {
            lock (obj)
            {
                return APS168.APS_get_command(axisId, ref pos);
            }
        }
        public override bool APS_SetLimitParam(int actCardId, int axisId, int pos)
        {
            lock (obj)
            {
                APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_SMEL_EN, 2);
                APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_EL_MODE, 1);
                APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_SMEL_POS, pos);
                return true;
            }
           
        }
        public override int APS_SetJogParam(int actCardId, int axisId, int mode, int dir, double lead, double acc, double dec, int vel)
        {
            lock (obj)
            {
                int ret = 0;
                ret = APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_JG_MODE, mode);
                if (ret != 0)
                    return ret;
                //dir =0为正方向 1为负方向
                ret = APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_JG_DIR, dir);
                if (ret != 0)
                    return ret;
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_ACC, XConvert.MM2PULS(acc, lead));
                if (ret != 0)
                    return ret;
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_DEC, XConvert.MM2PULS(acc, lead));
                if (ret != 0)
                    return ret;
                return APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_VM, XConvert.MM2PULS(vel, lead));
            }
        }
        public override int MoveJog(int actCardId, int axisId, int dir)
        {
            lock (obj)
            {
                int ret = 0;
                //IsStart=0为停止1为开始
                ret = APS168.APS_jog_start(axisId, 1);
                return ret;
            }
        }
        public override int SetAxisAccAndDec(int actCardId, int axisId, double lead, double acc, double dec)
        {
            lock (obj)
            {
                int ret = 0;
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_ACC, XConvert.MM2PULS(acc, lead));
                if (ret != 0)
                    return ret;
                return APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_DEC, XConvert.MM2PULS(dec, lead));
            }
        }
        public override int SetStopDec(int actCardId, int axisId, double lead,  double dec)
        {
            lock (obj)
            {
                int ret = 0;
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_STP_DEC, XConvert.MM2PULS(dec, lead));
                if (ret != 0)
                    return ret;
                return ret;
            }
        }
        public override int APS_SetAxisParam(int actCardId, int axisId, double lead, APS_Define PRA, double value)
        {
            lock (obj)
            {
                if (PRA == APS_Define.PRA_ACC || PRA == APS_Define.PRA_DEC || PRA == APS_Define.PRA_VE || PRA == APS_Define.PRA_VM || PRA == APS_Define.PRA_VS
                    || PRA == APS_Define.PRA_HOME_VA || PRA == APS_Define.PRA_HOME_VM || PRA == APS_Define.PRA_HOME_VS
                    || PRA == APS_Define.PRA_JG_ACC || PRA == APS_Define.PRA_JG_DEC || PRA == APS_Define.PRA_JG_VM
                    || PRA == APS_Define.PRA_SACC || PRA == APS_Define.PRA_SDEC)
                {
                    return APS168.APS_set_axis_param_f(axisId, (int)PRA, XConvert.MM2PULS(value, lead));
                }
                else
                {
                    return APS168.APS_set_axis_param_f(axisId, (int)PRA, value);
                }
            }
        }

        public override int APS_SetBacklashEnable(int actCardId, int axisId, int on)
        {
            lock (obj)
            {
                return APS168.APS_set_backlash_en(axisId, on);
            }
        }


        public override int MoveLineAbs(int[] axisId, double[] pos, double vel)
        {
            lock (obj)
            {
                double TransPara = 0;
                ASYNCALL Wait = new ASYNCALL();
                return APS168.APS_line_v(2, axisId, 0, pos, ref TransPara, vel, ref Wait);
            }
        }

        public override int MoveLineRel(int[] axisId, double[] pos, double vel)
        {
            lock (obj)
            {
                double TransPara = 0;
                ASYNCALL Wait = new ASYNCALL();
                return APS168.APS_line_v(2, axisId, 1, pos, ref TransPara, vel, ref Wait);
            }
        }

        public override int MoveArcAbs(int[] axisId, double[] center, double angle, double vel)
        {
            lock (obj)
            {
                double TransPara = 0;
                ASYNCALL Wait = new ASYNCALL();
                return APS168.APS_arc2_ca_v(axisId, 0, center, angle, ref TransPara, vel, ref Wait);
            }
        }

        public override int MoveArcAbs(int[] axisId, double[] center, double[] end, short dir, double vel)
        {
            lock (obj)
            {
                double TransPara = 0;
                ASYNCALL Wait = new ASYNCALL();
                return APS168.APS_arc2_ce_v(axisId, 0, center, end, dir, ref TransPara, vel, ref Wait);
            }
        }

        public override int MoveArcRel(int[] axisId, double[] center, double angle, double vel)
        {
            lock (obj)
            {
                double TransPara = 0;
                ASYNCALL Wait = new ASYNCALL();
                return APS168.APS_arc2_ca_v(axisId, 1, center, angle, ref TransPara, vel, ref Wait);
            }
        }

        public override int MoveArcRel(int[] axisId, double[] center, double[] end, short dir, double vel)
        {
            lock (obj)
            {
                double TransPara = 0;
                ASYNCALL Wait = new ASYNCALL();
                return APS168.APS_arc2_ce_v(axisId, 1, center, end, dir, ref TransPara, vel, ref Wait);
            }
        }

        public override int APS_pt_start(int actCardId, int ptbId)
        {
            lock (obj)
            {
                return APS168.APS_pt_start(actCardId, ptbId);
            }
        }

        public override int APS_get_pt_status(int actCardId, int ptbId, ref PTSTS Status)
        {
            lock (obj)
            {
                return APS168.APS_get_pt_status(actCardId, ptbId, ref Status);
            }
        }


        /// <summary>
        /// 1表示停止，0表示运动中
        /// </summary>
        /// <param name="actCardId"></param>
        /// <param name="axisId"></param>
        /// <returns></returns>
        public override int CheckMoveDone(ushort actCardId, ushort axisId)
        {
            var sts = APS168.APS_motion_status(axisId);
            var res = sts & 1 << (int) APS_Define.NSTP;
            return res;
        }

        public override int CheckHomeDone(ushort actCardId, ushort axisId)
        {
            var sts = APS168.APS_motion_status(axisId);
            var res = sts & 1 << (int)APS_Define.NSTP;
            return res;

        }

    }

    public enum ArcDir
    {
        Negative = -1,
        Positive = 0
    }

    public enum CompareTriggerDir
    {
        Negative = 0,
        Positive = 1,
        Both = 2
    }
}
