using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public class XRobot : XCommandCard
    {


       private object obj = new object();
       public XRobot()
        {

        }

       public override int Initial()
       {
           lock (obj)
           {
               int boardId = 0;
                



               return boardId;
           }
       }
 
 
  

        public override int SetServo(int actCardId, int axisId, bool on)
        {
            lock (obj)
            {
                int i = (on) ? 1 : 0;
               // return APS168.APS_set_servo_on(axisId, i);
            } return 0;
        }

        public override int GoHome(int actCardId, int axisId)
        {
            lock (obj)
            {

            } return 0;
        }

        public override int MoveAbs(int actCardId, int axisId, int position, int vel)
        {
            lock (obj)
            {

            } return 0;
        }

        public override int MoveRel(int actCardId, int axisId, int distance, int vel)
        {
            lock (obj)
            {

            } return 0;
        }

        public override int Stop(int actCardId, int axisId)
        {
            lock (obj)
            {

            } return 0;
        }

        public override int EStop(int actCardId, int axisId)
        {
            lock (obj)
            {
                
            }
            return 0;
        }

       
        

        public override int GetMotionPos(int actCardId, int axisId, ref int pos)
        {
            lock (obj)
            {
               // return APS168.APS_get_position(axisId, ref pos);
            } return 0;
        }

        public override int GetCommandPos(int actCardId, int axisId, ref int pos)
        {
            lock (obj)
            {
               // return APS168.APS_get_command(axisId, ref pos);
            } return 0;
        }
       
 
    }
}
