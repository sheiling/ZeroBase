using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    /// <summary>
    /// 设备操作
    /// </summary>
    public sealed class XDevice : XObject
    {
        private static XDevice instance = new XDevice();
        XDevice()
        {
        }

        public static XDevice Instance
        {
            get { return instance; }
        }

        private Dictionary<int, XCard> cardMap = new Dictionary<int, XCard>();
        private Dictionary<int, IAxis> axisMap = new Dictionary<int, IAxis>();
        private Dictionary<int, XDo> doMap = new Dictionary<int, XDo>();
        private Dictionary<int, XDi> diMap = new Dictionary<int, XDi>();
        private Dictionary<int, XChannelValue> channelValueMap = new Dictionary<int, XChannelValue>();
        private IAxis axis;
        public void BindCard(int setCardId, int actCardId, XCommandCard commandCard, string name,string path=null)
        {
            if (cardMap.ContainsKey(setCardId) == false)
            {
                XCard card = new XCard(actCardId, commandCard, name,path);
                cardMap.Add(setCardId, card);   
            }        
        }
        public XCard FindCardById(int setCardId)
        {
            if (cardMap.ContainsKey(setCardId) == false)
            {
                return null;
            }
            return cardMap[setCardId];
        }
        public Dictionary<int, XCard> CardMap
        {
            get { return this.cardMap; }
        }

        public void BindAxis(int setCardId, int setAxisId, int actAxisId, double lead, string name,XAxisDirection axisDirection)
        {
            if (cardMap.ContainsKey(setCardId) == true)
            {
                if (axisMap.ContainsKey(setAxisId) == false)
                {
                    //if (cardMap[setCardId].CurrentCard == CardStyle.ADLINK)
                    //{
                    switch (cardMap[setCardId].CurrentCard)
                    {
                        case CardStyle.ACS:
                            axis = new XACSAxis(actAxisId, lead, cardMap[setCardId], name);
                            break;
                        case CardStyle.Leisai:
                            axis = new XleisaiAxis(actAxisId, lead, cardMap[setCardId], name);
                            break;
                        default:
                            axis = new XADLINKAxis(actAxisId, lead, cardMap[setCardId], name);
                            break;
                    }

                    axis.CardId = setCardId;
                    axis.SetId = setAxisId;
                    axis.AxisDirection = axisDirection;
                    axisMap.Add(setAxisId, axis);
                }
                //}
            }
        }
        public IAxis FindAxisById(int setAxisId)
        {
            if (axisMap.ContainsKey(setAxisId) == false)
            {
                return null;
            }
            return axisMap[setAxisId];
        }
        public Dictionary<int, IAxis> AxisMap
        {
            get { return this.axisMap; }
        }

        public void BindDo(int setCardId, int setDoId, int channel, int actDoId, string name, string cardname,
            bool isInterference = false, int InterferenceThings = -1, double pos = 0, DOSTSTYPE status = DOSTSTYPE.HIGH)
        {
            if (cardMap.ContainsKey(setCardId) == true)
            {
                if (doMap.ContainsKey(setDoId) == false)
                {
                    XDo _do = new XDo(cardMap[setCardId], channel, actDoId, name, cardname, isInterference, InterferenceThings, pos, status);
                    _do.CardId = setCardId;
                    _do.SetId = setDoId;
                    doMap.Add(setDoId, _do);
                }             
            }
        }
        public XDo FindDoById(int setDoId)
        {
            if (doMap.ContainsKey(setDoId) == false)
            {
                return null;
            }
            return doMap[setDoId];
        }
        public Dictionary<int, XDo> DoMap
        {
            get { return this.doMap; }
        }


        public void BindDi(int setCardId, int setDiId, int channel, int actDiId, string name,string cardname)
        {
            if (cardMap.ContainsKey(setCardId) == true)
            {
                if (diMap.ContainsKey(setDiId) == false)
                {
                    XDi di = new XDi(cardMap[setCardId], channel, actDiId, name, cardname);
                    di.CardId = setCardId;
                    di.SetId = setDiId;
                    diMap.Add(setDiId, di);
                }
            }
        }
        public XDi FindDiById(int setDiId)
        {
            if (diMap.ContainsKey(setDiId) == false)
            {
                return null;
            }
            return diMap[setDiId];
        }
        public Dictionary<int, XDi> DiMap
        {
            get { return this.diMap; }
        }



        public void BindChannelValue(int setCardId, int setId, int channel, string name,bool isCheck=false )
        {
            if (cardMap.ContainsKey(setCardId) == true)
            {
                if (channelValueMap.ContainsKey(setId) == false)
                {
                    XChannelValue channelValue = new XChannelValue(cardMap[setCardId], channel, name, isCheck);
                    channelValue.CardId = setCardId;
                    
                    channelValueMap.Add(setId, channelValue);
                }
            }
        }

        public XChannelValue FindChannelValueById(int setId)
        {
            if (channelValueMap.ContainsKey(setId) == false)
            {
                return null;
            }
            return channelValueMap[setId];
        }

        public Dictionary<int, XChannelValue> ChannelValueMap
        {
            get { return this.channelValueMap; }
        }
    }
}
