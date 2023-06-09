using System;
using System.Collections.Generic;
using System.Text;

namespace WebCameraClient.Model
{
    internal class PortInfoModel
    {
        public int PortNumber { get; set; }
        public string Local { get; set; }
        public string Remote { get; set; }
        public string State { get; set; }

        public PortInfoModel(int i/*, string local, string remote, string state*/)
        {
            PortNumber = i;
            //Local = local;
            //Remote = remote;
            //State = state;
        }
    }
}
