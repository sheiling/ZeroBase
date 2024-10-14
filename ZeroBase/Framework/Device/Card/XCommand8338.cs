using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS168_W32;
using APS_Define_W32;

namespace ZeroBase
{
    public class XCommandCard8338 : XCommandCard
    {
        private object obj = new object();
        private string CardName;
        public XCommandCard8338()
        {

        }
        public XCommandCard8338(string cardname)
            :this()
            
        {
            this.CardName = cardname; 
        }
        public override int Initial()
        {

            lock (obj)
            {
                int boardId = 0;
                int mode = 1;
                int bus_No = 0;
                //int ret = APS168.APS_initial(ref boardId, mode);
                int ret = APS168.APS_initial(ref boardId, mode);
                if (ret != 0)
                {
                    return ret;
                }
                //ret = APS168.APS_scan_field_bus(3, bus_No);
                ret = 1;
                if (ret >= 0)
                {
                    ret = APS168.APS_start_field_bus(3, bus_No, 0);
                    if (ret >= 0)
                    {
                        return boardId;
                    }
                    else
                    {
                        return ret;
                    }
                }
                else
                {
                    return ret;
                }
            }
        }
        ///


        public override int LoadParam(ushort cardnum,string configFn)
        {
            //return APS168.APS_load_param_from_file(configFn);
            lock (obj)
            {
                return APS168.APS_load_param_from_file(configFn);
            }
        }

        public override int Close()
        {
            lock (obj)
            {
                //return APS168.APS_close();
                return APS168.APS_close();
            }
        }

        public override int Update(int actCardId)
        {
            lock (obj)
            {
                UInt32 diData = 0;
                UInt32 doData = 0;
                if (CardName == null)
                {
                    return 0;
                }
                if (CardName.Contains("输入"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        
                        APS168.APS_get_field_bus_d_port_input(3, 0, actCardId, i, ref diData);

                        //DO_Data[0] = ((int)diData << 8 * i) | DO_Data[0];

                        for (int j = 0; j < 8; j++)
                        {

                            if ((diData & (1 << j)) != 0)
                            {
                                PCIE_8338_DI_Data[i, j] = true;
                            }
                            else
                            {
                                PCIE_8338_DI_Data[i, j] = false;
                            }
                        }
                    }
                }
                else if (CardName.Contains("输出"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        APS168.APS_get_field_bus_d_port_output(3, 0, actCardId, i, ref doData);
                        //DO_Data[0] = ((int)doData << 8 * i) | DO_Data[0];
                        for (int j = 0; j < 8; j++)
                        {
                            if ((doData & (1 << j)) != 0)
                            {
                                PCIE_8338_DO_Data[i,j] = true;
                            }
                            else
                            {
                                PCIE_8338_DO_Data[i,j] = false;
                            }
                        }
                    }
                }
                
                return 0;
            }
            
        }

        public override int SetDo(int actCardId, int channel, int index, int sts)
        {
            //return APS168.APS_write_d_channel_output(actCardId, channel, index, sts);
            lock (obj)
            {                              
                //卡号  总线号    从站号   位
                return APS168.APS_set_field_bus_d_channel_output(3, 0, actCardId, index, sts);//3是boardID  这是第四张卡 所以是3 是实际情况而定
            }

            //return APS168.APS_set_field_bus_d_output(actCardId, channel, index, sts);
             
        }

        public override int GetDo(int actCardId, int channel, int index, ref int sts)
        {
            lock (obj)
            {
                //sts = (DO_Data[0] >> index) & 1;
                int i = index % 8;
                sts = PCIE_8338_DO_Data[channel,i] ? 1 : 0;
                return 0;
            }
        }
       
        public override int GetDi(int actCardId, int channel, int index, ref int sts)
        {
            lock (obj)
            {
                int i = index % 8;
                sts=PCIE_8338_DI_Data[channel,i] ? 1:0;
                //sts = (DI_Data[0] >> index) & 1;
                return 0;
            }
        }

        public override int SetServo(int actCardId, int axisId, bool on)
        {
            lock (obj)
            {
                int i = (on) ? 1 : 0;
                //return APS168.APS_set_servo_on(axisId, i);
                return APS168.APS_set_servo_on(axisId, i);
            }
        }

        public override int GoHome(int actCardId, int axisId)
        {
            //return APS168.APS_home_move(axisId);
            lock (obj)
            {
                return APS168.APS_home_move(axisId);
            }
        }

        public override int MoveAbs(int actCardId, int axisId, int position, int vel)
        {
            //return APS168.APS_absolute_move(axisId, position, vel);
            lock (obj)
            {
                return APS168.APS_absolute_move(axisId, position, vel);
            }
        }

        public override int MoveRel(int actCardId, int axisId, int distance, int vel)
        {
            //return APS168.APS_relative_move(axisId, distance, vel);
            lock (obj)
            {
                return APS168.APS_relative_move(axisId, distance, vel);
            }
        }

        public override int MoveJog(int actCardId, int axisId, int dir)
        {
            lock (obj)
            {
                int ret = 0;
                //IsStart=0为停止1为开始
                //ret = APS168.APS_jog_start(axisId, IsStart);
                ret = APS168.APS_jog_start(axisId, 1);
                return ret;
            }
        }

        public override int APS_SetJogParam(int actCardId, int axisId, int mode, int dir, double lead, double acc, double dec, int vel)
        {
            lock (obj)
            {
                int ret = 0;
                //ret = APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_JG_MODE, mode);
                ret = APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_JG_MODE, mode);
                if (ret != 0)
                    return ret;
                ////dir =0为正方向 1为负方向
                //ret = APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_JG_DIR, dir);
                ret = APS168.APS_set_axis_param(axisId, (int)APS_Define.PRA_JG_DIR, dir);
                if (ret != 0)
                    return ret;
                //ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_ACC, XConvert.MM2PULS(acc, lead));
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_ACC, XConvert.MM2PULS(acc, lead));
                if (ret != 0)
                    return ret;
                //ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_DEC, XConvert.MM2PULS(acc, lead));
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_DEC,
                    XConvert.MM2PULS(acc, lead));
                if (ret != 0)
                    return ret;
                //return APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_VM, XConvert.MM2PULS(vel, lead));
                return ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_JG_VM, XConvert.MM2PULS(vel, lead));
            }
        }


        public override int Stop(int actCardId, int axisId)
        {
            //return APS168.APS_stop_move(axisId);
            lock (obj)
            {
                return APS168.APS_stop_move(axisId);
            }
        }

        public override int EStop(int actCardId, int axisId)
        {
            //return APS168.APS_emg_stop(axisId);
            lock (obj)
            {
                return APS168.APS_emg_stop(axisId);
            }
        }

        public override int GetMotionIo(int actCardId, int axisId, ref int sts)//需要确认
        {
            lock (obj)
            {
                int _sts, _rSts;
                //_sts = APS168.APS_motion_io_status(axisId);
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

        public override int GetMotionSts(int actCardId, int axisId, ref int sts)//需要确认
        {
            lock (obj)
            {
                int _sts, _rSts;
                //_sts = APS168.APS_motion_status(axisId);
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
            //return APS168.APS_get_position(axisId, ref pos);
            lock (obj)
            {
                return APS168.APS_get_position(axisId, ref pos);
            }
        }

        public override int GetCommandPos(int actCardId, int axisId, ref int pos)
        {
            //return APS168.APS_get_command(axisId, ref pos);
            lock (obj)
            {
                return APS168.APS_get_command(axisId, ref pos);
            }
        }


        public override int SetAxisAccAndDec(int actCardId, int axisId, double lead, double acc, double dec)
        {
            lock (obj)
            {
                int ret = 0;
                //ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_ACC, XConvert.MM2PULS(acc, lead));
                ret = APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_ACC, XConvert.MM2PULS(acc, lead));
                if (ret != 0)
                    return ret;
                //return APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_DEC, XConvert.MM2PULS(dec, lead));
                return APS168.APS_set_axis_param_f(axisId, (int)APS_Define.PRA_DEC, XConvert.MM2PULS(dec, lead));
            }
        }

        //public override int APS_SetAxisParam(int actCardId, int axisId, double lead, APS_Define PRA, double value)
        //{
        //    if (PRA == APS_Define.PRA_ACC || PRA == APS_Define.PRA_DEC || PRA == APS_Define.PRA_VE || PRA == APS_Define.PRA_VM || PRA == APS_Define.PRA_VS
        //        || PRA == APS_Define.PRA_HOME_VA || PRA == APS_Define.PRA_HOME_VM || PRA == APS_Define.PRA_HOME_VS
        //        || PRA == APS_Define.PRA_JG_ACC || PRA == APS_Define.PRA_JG_DEC || PRA == APS_Define.PRA_JG_VM
        //        || PRA == APS_Define.PRA_SACC || PRA == APS_Define.PRA_SDEC)
        //    {
        //        return APS168.APS_set_axis_param_f(axisId, (int)PRA, XConvert.MM2PULS(value, lead));
        //    }
        //    else
        //    {
        //        return APS168.APS_set_axis_param_f(axisId, (int)PRA, value);
        //    }
        //}

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

        //public override int APS_get_pt_status(int actCardId, int ptbId, ref PTSTS Status)
        //{
        //    return APS168.APS_get_pt_status(actCardId, ptbId, ref Status);
        //}

    }

    //public enum ArcDir
    //{
    //    Negative = -1,
    //    Positive = 0
    //}

    //public enum CompareTriggerDir
    //{
    //    Negative = 0,
    //    Positive = 1,
    //    Both = 2
    //}
}
