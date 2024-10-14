using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    /// <summary>
    /// 轴信息
    /// </summary>
    public interface IAxis
    {
        string Name { get; }
        bool IsALM { get; }
        bool IsPEL { get; }
        bool IsMEL { get; }
        bool IsORG { get; }
        bool IsEMG { get; }
        bool IsSVON { get; }
        bool HasSVONOFF { get; }
        bool IsMDN { get; }

        bool IsHomeD { get; }
        bool IsHMV { get; }
        bool IsASTP { get; }
        bool IsHomeOk { get; }
        bool HasEStoped { get; set; }
        double POS { get; }
        double CommandPOS { get; }
        int PULS { get; }

        //////////////////////////////////////////////////////
        int ActId { get; }
        int SetId { get; set; }
        int CardId { get; set; }
        int TaskId { get; set; }
        bool IsFeedback
        { get; set; }
        int SetServo(bool on);
        int GoHome();
        int SetPosition(int position);

        int CleanALM();
        int MoveAbs(double position, double vel);
        int MoveRel(double distance, double vel);
        int MoveJog(double vel);
        int Stop();
        int EStop();
        int Update();
        int SetHome(bool b);
        int SetAxisAccAndDec(double acc, double dec);
        int SetJerk(double jerkvalue);
        XAxisDirection AxisDirection { get; set; }
        AxisStyle CurrentAxis { get; }
        int APS_SetAxisJogParam(int mode, int dir, double acc, double dec, int vel);
        int SetStopDec(double dec);
    }
    public enum AxisStyle
    {
        ADLINK_Axis,
        ACS_Axis,
        Leisai_Axis,
    }
}
