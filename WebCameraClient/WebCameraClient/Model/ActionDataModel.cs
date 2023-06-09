using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using WebCameraClient.Enums;

namespace WebCameraClient.Model
{
    public class ActionDataModel
    {
        public ActionType ActionType { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public List<string> Camers { get; set; }
        public string CameraName { get; set; }
    }
}
