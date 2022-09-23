using SELKIE.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SELKIE.Models
{
    public class TotalInstallations
    {
        static List<InstallationDetails> installs = new List<InstallationDetails>();

        public static List<InstallationDetails> GetInstalls()
        {
            return installs;
        }

        public static List<string> GetDevicesOnly()
        {
            List<string> foundDevices = new List<string>();

            var _deviceList = installs.Where(x => x.InstallType == InstallType.Device).ToList();
            if (_deviceList.Any())
            {
                foreach (var _device in _deviceList)
                {
                    foundDevices.Add(_device.Taskname);
                }
            }
            return foundDevices;
        }

        public static void Add(InstallationDetails _value)
        {
            installs.Add(_value);
        }

        public static void Update(int index, InstallationDetails _value)
        {
            _value.InstallType = installs[index].InstallType;
            installs[index] = _value;
        }
        public static InstallationDetails GetObj(int inx)
        {
            return installs[inx];
        }
        public static void Init()
        {
            installs = new List<InstallationDetails>();
        }
    }


    public class InstallationDetails
    {

        //Instdata
        public string Taskname { get; set; }
        public string TaskDescription { get; set; }
        public int NoOftechsReq { get; set; }
        public string VesselReq { get; set; }
        public int Numberofdevicespervessel { get; set; }
        public string Base { get; set; }
        public double OperationDuration { get; set; }
        public double Waveheightlimit { get; set; }
        public double Waveperiodlimit { get; set; }
        public double Windspeedlimit { get; set; }
        public double Currentvelocitylimit { get; set; }
        public InstallType InstallType { get; set; }
    }
}
