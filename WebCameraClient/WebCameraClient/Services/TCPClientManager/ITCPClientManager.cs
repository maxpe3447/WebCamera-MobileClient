using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static WebCameraClient.Services.TCPClientManager.TCPClientManager;

namespace WebCameraClient.Services.TCPClientManager
{
    public interface ITCPClientManager
    {
        void Connected(string host, int port);
        void StartGetting(CaptureImageHandler capture);
        void Disconnect();
    }
}
