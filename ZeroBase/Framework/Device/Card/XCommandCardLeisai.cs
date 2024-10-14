using System;

namespace ZeroBase
{
    public class XCommandCardLeisai : XCommandCard
    {
        private object obj = new object();
        public XCommandCardLeisai()
        {
            CurrentCard = CardStyle.Leisai;
        }

        public override int Initial()
        {
            lock (obj)
            {
                try
                {
                    int num = LTDMC.dmc_board_init();//获取板卡数量
                    if (num < 0 || num > 8)
                    {
                        return -1;
                    }
                    ushort _num = 0;
                    ushort[] cardids = new ushort[8];
                    uint[] cardtypes = new uint[8];
                    short res = LTDMC.dmc_get_CardInfList(ref _num, cardtypes, cardids);
                    if (res != 0)
                    {
                        return -1;
                    }
                    return res;
                }
                catch (Exception e)
                {
                    return -1;

                }
                
            }
        }

        public override int LoadParam(ushort cardnum,string configFn)
        {
            lock (obj)
            {
                try
                {
                    return LTDMC.dmc_download_configfile(cardnum, configFn);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public override int Close()
        {
            lock (obj)
            {
                try
                {
                    return LTDMC.dmc_board_close();
                }
                catch (Exception)
                {

                    return -1;
                }
               
            }
        }

        public override int Update(int actCardId)
        {
            lock (obj)
            {
                try
                {
                   
                    uint diData = 0;
                    uint doData = 0;
                   
                    diData = LTDMC.dmc_read_inport((ushort)actCardId, 0);
                    doData = LTDMC.dmc_read_outport((ushort)actCardId, 0);
                    DI_Data[0] =  XConvert.convertValue((int)diData);
                    DO_Data[0] =  XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 1, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 1, 0);
                    DI_Data[1] = XConvert.convertValue((int)diData);
                    DO_Data[1] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 2, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 2, 0);
                    DI_Data[2] = XConvert.convertValue((int)diData);
                    DO_Data[2] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 3, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 3, 0);
                    DI_Data[3] = XConvert.convertValue((int)diData);
                    DO_Data[3] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 4, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 4, 0);
                    DI_Data[4] = XConvert.convertValue((int)diData);
                    DO_Data[4] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 5, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 5, 0);
                    DI_Data[5] = XConvert.convertValue((int)diData);
                    DO_Data[5] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 6, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 6, 0);
                    DI_Data[6] = XConvert.convertValue((int)diData);
                    DO_Data[6] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 7, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 7, 0);
                    DI_Data[7] = XConvert.convertValue((int)diData);
                    DO_Data[7] = XConvert.convertValue((int)doData);
                    diData = LTDMC.dmc_read_can_inport((ushort)actCardId, 8, 0);
                    doData = LTDMC.dmc_read_can_outport((ushort)actCardId, 8, 0);
                    DI_Data[8] = XConvert.convertValue((int)diData);
                    DO_Data[8] = XConvert.convertValue((int)doData);

                   
                }
                catch (Exception)
                {

                    return -1;
                }
               
                

                return 0;
            }
        }

        public override int SetDo(int actCardId, int channel, int index, int sts)
        {
            lock (obj)
            {
                try
                {
                    sts = sts == 0 ? 1 : 0;
                    int res = 0;
                    if (channel == 0)
                    {
                        res = LTDMC.dmc_write_outbit((ushort)actCardId, (ushort)index, (ushort)sts);
                    }
                    else
                    {
                        res = LTDMC.dmc_write_can_outbit((ushort)actCardId, (ushort)channel, (ushort)index, (ushort)sts);
                    }
                    return res;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public override int GetDo(int actCardId, int channel, int index, ref int sts)
        {
            lock (obj)
            {

                sts = (DO_Data[channel] >> index) & 1;

                return sts;
            }
        }

        public override int GetDi(int actCardId, int channel, int index, ref int sts)
        {
            lock (obj)
            {
                sts = (DI_Data[channel] >> index) & 1;

                return sts;

            }
        }

        public override int SetServo(int actCardId, int axisId, bool on)
        {
            lock (obj)
            {
                try
                {
                    int i = (on) ? 0 : 1;
                    var res = LTDMC.dmc_write_sevon_pin((ushort)actCardId, (ushort)axisId, (ushort)i);
                    return res;
                }
                catch (Exception e)
                {
                    return -1;
                }
                
            }
        }

        public override int GoHome(int actCardId, int axisId)
        {
            lock (obj)
            {
                try
                {
                    var res = LTDMC.dmc_home_move((ushort)actCardId, (ushort)axisId);
                    return res;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public override int SetPosition(int acrCardId, int axisId, double position,double lead)
        {
            lock (obj)
            {
                try
                {
                    var res = LTDMC.dmc_set_position((ushort)acrCardId, (ushort)axisId, XConvert.MM2PULS(position, lead));
                    res = LTDMC.dmc_set_encoder((ushort)acrCardId, (ushort)axisId, XConvert.MM2PULS(position, lead));
                    return res;
                }
                catch (Exception)
                {

                    return -1;
                }
                
            }
        }

        public override int MoveAbs(int actCardId, int axisId, int position, int vel)
        {
            lock (obj)
            {
                try
                {
                    int res = 0;
                    double minvel = 0, maxvel = 0, acc = 0, dec = 0, stopvel = 0;

                    LTDMC.dmc_get_profile((ushort)actCardId, (ushort)axisId, ref minvel, ref maxvel, ref acc, ref dec,
                        ref stopvel);
                    LTDMC.dmc_set_profile((ushort)actCardId, (ushort)axisId, minvel, vel, 0.1, 0.1, stopvel);

                    LTDMC.dmc_set_s_profile((ushort)actCardId, (ushort)axisId, 0, 0.01);//设置S段速度参数

                    LTDMC.dmc_set_dec_stop_time((ushort)actCardId, (ushort)axisId, 0.1); //设置减速停止时间


                    res = LTDMC.dmc_pmove((ushort) actCardId, (ushort) axisId,  position, 1);

                    return res;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public override int MoveRel(int actCardId, int axisId, int distance, int vel)
        {
            lock (obj)
            {
                try
                {
                    double minvel = 0, maxvel=0,acc = 0, dec = 0, stopvel = 0;

                    LTDMC.dmc_get_profile((ushort) actCardId, (ushort) axisId, ref minvel, ref maxvel, ref acc, ref dec,
                        ref stopvel);

                    LTDMC.dmc_set_profile((ushort)actCardId, (ushort)axisId, minvel, vel, 0.1, 0.1, stopvel);

                    LTDMC.dmc_set_s_profile((ushort)actCardId, (ushort)axisId, 0, 0.01);//设置S段速度参数

                    LTDMC.dmc_set_dec_stop_time((ushort)actCardId, (ushort)axisId, 0.1); //设置减速停止时间
                    var res = LTDMC.dmc_pmove((ushort)actCardId, (ushort)axisId, distance, 0);
                    return res;
                }
                catch (Exception)
                {
                    return -1;

                }
               
            }
        }

        public override int Stop(int actCardId, int axisId)
        {
            lock (obj)
            {
                try
                {
                    var res = LTDMC.dmc_stop((ushort)actCardId, (ushort)axisId, 0);
                    return res;
                }
                catch (Exception)
                {
                    return -1;
                }
            } 
        }

        public override int EStop(int actCardId, int axisId)
        {
            lock (obj)
            {
                try
                {
                    var res = LTDMC.dmc_stop((ushort)actCardId, (ushort)axisId, 1);
                    return res;
                }
                catch (Exception)
                {

                    return -1;
                }
                
            }
        }

        public override int GetMotionIo(int actCardId, int axisId, ref int sts)
        {
            lock (obj)
            {
                int _rSts;
                uint res = 1;
                try
                {
                    res = LTDMC.dmc_axis_io_status((ushort)actCardId, (ushort)axisId);
                }
                catch (Exception)
                {
                    return -1;
                }
              
                int _sts = (int) res;
              
                _rSts = 0;
                if (XConvert.BitEnable(_sts, 0x01 << 0))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_ALM);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 1))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_PEL);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 2))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_MEL);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 3))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_EMG);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 4))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_ORG);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 6))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_SPEL);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 7))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_SMEL);
                }
                if (XConvert.BitEnable(_sts, 0x01 << 8))
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MIO_INP);
                }
                sts = _rSts;
                return 0;
            }
        }

        public override int GetMotionSts(int actCardId, int axisId, ref int sts)
        {
            lock (obj)
            {
                int  _rSts=0;
                int res = 0;
                short tt = 0;
                try
                {
                    LTDMC.dmc_get_stop_reason((ushort)actCardId, (ushort)axisId, ref res);
                    tt = LTDMC.dmc_read_sevon_pin((ushort)actCardId, (ushort)axisId);
                }
                catch (Exception)
                {
                    return -1;
                
                }
              

                int _sts = (int)res;
                
                _rSts = 0;
                if (_sts == 0)
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MTS_MDN);
                }
                if (_sts == 1)
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MTS_ALM);
                }
                if (_sts == 15)
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MTS_OTHER);
                }
                if (tt==0)
                {
                    XConvert.SetBits(ref _rSts, Xleisai_Define.MTS_SVON);
                }


                sts = _rSts;
                return 0;
            }

        }

        public override int GetMotionPos(int actCardId, int axisId, ref int pos)
        {
            lock (obj)
            {
                try
                {
                    pos = LTDMC.dmc_get_position((ushort)actCardId, (ushort)axisId);
                    //-----------------雷赛驱动器有些没有接反馈，只能读命令位置-------
                    //pos = LTDMC.dmc_get_encoder((ushort)actCardId, (ushort)axisId);
                    return 0;
                }
                catch (Exception)
                {

                    return -1;
                }
               
               
            }
        }

        public override int GetCommandPos(int actCardId, int axisId, ref int pos)
        {
            lock (obj)
            {
                try
                {
                    pos = LTDMC.dmc_get_position((ushort)actCardId, (ushort)axisId);
                    return 0;
                }
                catch (Exception)
                {
                    return -1;
                    
                }
               
            }
        }
       
        public override int MoveJog(int actCardId, int axisId, int dir)
        {
            lock (obj)
            {
                //LTDMC.dmc_vmove((ushort) actCardId, (ushort) axisId, (ushort)dir);
                return 0;
            }
        }
        public override int SetAxisAccAndDec(int actCardId, int axisId, double lead, double acc, double dec)
        {
            lock (obj)
            {
                try
                {
                    int ret = 0;
                    ret = LTDMC.dmc_set_profile((ushort)actCardId, (ushort)axisId, XConvert.MM2PULS(acc,lead), 400, 0.1,
                        0.1, XConvert.MM2PULS(dec,lead));

                    LTDMC.dmc_set_s_profile((ushort)actCardId, (ushort)axisId, 0, 0.01);//设置S段速度参数

                    LTDMC.dmc_set_dec_stop_time((ushort)actCardId, (ushort)axisId, 0.1); //设置减速停止时间
                    return ret;
                }
                catch (Exception)
                {
                    return -1;
                
                }
               
            }
        }

        //public virtual int LeisaiCheckMoveDone(ushort actCardId, ushort axisId) { return -1; }
        /// <summary>
        /// 1表示停止，0表示运动中
        /// </summary>
        /// <param name="actCardId"></param>
        /// <param name="axisId"></param>
        /// <returns></returns>
        public override int CheckMoveDone(ushort actCardId, ushort axisId)
        {
            try
            {
                int a = LTDMC.dmc_check_done(actCardId, axisId);
                return LTDMC.dmc_check_done(actCardId, axisId);
            }
            catch (Exception)
            {
                return -1;
              
            }
           
        }

        public override int CheckHomeDone(ushort actCardId, ushort axisId)
        {
            try
            {
                ushort state=2;
                LTDMC.dmc_get_home_result(actCardId, axisId, ref state);
                return state;
            }
            catch (Exception)
            {
                return -1;

            }

        }

        public override int SetDAfunction(ushort actCardId,ushort enable)
        {
            try
            {
                return LTDMC.dmc_set_da_enable(actCardId, enable);
            }
            catch (Exception)
            {

                return -1;
            }
        }


        public override int GetADinput(ushort actCardId,ushort channel,ref double Value)
        {
            try
            {
                return LTDMC.dmc_get_ad_input(actCardId, channel, ref Value);
            }
            catch (Exception)
            {

                return -1;
            }
        }



        public override int SetDAoutput(ushort actCardId,ushort channel,double value)
        {
            try
            {
                return LTDMC.dmc_set_da_output(actCardId, channel, value);
            }
            catch (Exception)
            {

                return -1;
            }
        }

        public override int SetConnectStata(ushort CardId, ushort NodeNum, ushort Stata)
        {
            try
            {
                return LTDMC.nmc_set_connect_state(CardId, NodeNum, Stata,0);
            }
            catch (Exception)
            {

                return -1;
            }
        }

        //public override int SetHomeElReturn(ushort CardId)
        //{
        //    try
        //    {
        //        //return LTDMC.dmc_set_home_el_return()
        //    }
        //    catch (Exception)
        //    {

        //        return -1;
        //    }
        //}

        public override int ClearALM(int actCardId, int axisId)
        {
            try
            {
                return LTDMC.dmc_write_erc_pin((ushort)actCardId, (ushort)axisId, 1);
            }
            catch (Exception)
            {

                return -1;
            }
        }

    }

   
}
