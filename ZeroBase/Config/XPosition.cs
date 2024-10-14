using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public class XPosition : XObject
    {
        protected int[] axisId;
        protected double[] positions;
        protected int count;
        public XPosition(int[] axisId, double[] positions, int count)
        {
            this.axisId = axisId;
            this.positions = positions;
            this.count = count;
        }

        public string Name { get; set; }

        public int[] AxisId
        {
            get { return axisId; }
        }

        public double[] Positions
        {
            get { return positions; }
        }

        public int Count
        {
            get { return count; }
        }
    }
}
