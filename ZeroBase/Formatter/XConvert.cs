using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS_Define_W32;
using APS168_W32;
using System.Xml;
using System.Data;

namespace ZeroBase
{
    public class XConvert : XObject
    {
        #region PULS<->MM
        public static int MM2PULS(double MM, double lead)
        {
            return Convert.ToInt32(MM * 10000 / lead);
        }
        public static double PULS2MM(int PULS, double lead)
        {
            return PULS * lead / 10000;
        }
        #endregion

        #region Bit



        public static int  convertValue(int sts)
        {
            return sts ^ 0xFFFF;
        }

        public static bool BitEnable(int word, int bits)
        {
            return (word & bits) != 0;
        }



        public static void SetBits(ref int word, int bits)
        {
            word |= bits;
        }
        public static void ClrBits(ref int word, int bits)
        {
            word &= ~bits;
        }
        #endregion

        #region Str
        public static int[] Str2IntG(string str, char c)
        {
            try
            {
                string[] ss = str.Split(c);
                int count = ss.Length;
                int[] ret = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ret[i] = int.Parse(ss[i]);
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static int[] StrG2IntG(string[] str)
        {
            try
            {
                int count = str.Length;
                int[] ret = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ret[i] = int.Parse(str[i]);
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static string IntG2Str(int[] t, string c)
        {
            try
            {
                if (t.Length <= 0)
                {
                    return "";
                }
                int count = t.Length;
                string ret = "";
                for (int i = 0; i < count - 1; i++)
                {
                    ret += t[i].ToString() + c;
                }
                ret += t[count - 1];
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static double[] Str2DoubleG(string str, char c)
        {
            try
            {
                string[] ss = str.Split(c);
                int count = ss.Length;
                double[] ret = new double[count];
                for (int i = 0; i < count; i++)
                {
                    ret[i] = double.Parse(ss[i]);
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static double[] StrG2DoubleG(string[] str)
        {
            try
            {
                int count = str.Length;
                double[] ret = new double[count];
                for (int i = 0; i < count; i++)
                {
                    ret[i] = double.Parse(str[i]);
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static string DoubleG2Str(double[] d, string c)
        {
            try
            {
                if (d.Length <= 0)
                {
                    return "";
                }
                int count = d.Length;
                string ret = "";
                for (int i = 0; i < count - 1; i++)
                {
                    ret += d[i].ToString() + c;
                }
                ret += d[count - 1];
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static string StrG2Str(string[] str, string c)
        {
            try
            {
                if (str.Length <= 0)
                {
                    return "";
                }
                int count = str.Length;
                string ret = "";
                for (int i = 0; i < count - 1; i++)
                {
                    ret += str[i] + c;
                }
                ret += str[count - 1];
                return ret;
            }
            catch
            {
                return null;
            }
        }
        public static string[] Str2StrG(string str, char c)
        {
            try
            {
                string[] ss = str.Split(c);
                int count = ss.Length;
                string[] ret = new string[count];
                for (int i = 0; i < count; i++)
                {
                    ret[i] = ss[i];
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        public static string AdminPower = "";
    }
    
}
