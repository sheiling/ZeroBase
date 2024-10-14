using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public sealed class XStationManager : XObject
    {
        private Dictionary<int, XStation> stations = new Dictionary<int, XStation>();
        private readonly static XStationManager instance = new XStationManager();
        XStationManager()
        {
            BindStation(-1, "Controller");
        }
        public static XStationManager Instance
        {
            get { return instance; }
        }

        public void BindStation(int stationId, string name)
        {
            if (stations.ContainsKey(stationId) == false)
            {
                XStation station = new XStation(stationId, name);
                stations.Add(stationId, station);
            }
        }

        public XStation FindStationById(int stationId)
        {
            if (stations.ContainsKey(stationId) == false)
            {
                return null;
            }
            return stations[stationId];
        }

        public Dictionary<int, XStation> Stations
        {
            get { return stations; }
        }

    }
}
