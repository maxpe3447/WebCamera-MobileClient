using System;
using System.Collections.Generic;
using System.Text;
using WebCameraClient.Enums;
using static WebCameraClient.Services.UDPClientManager.UDPClientManager;

namespace WebCameraClient.Services.UDPClientManager
{
    public interface IUDPClientManager
    {
        void StartReciving(CaptireHandle handle, InitCameralistHandle cameralistHandle);
        void StopReciving();
        void Connecting(string host, int port);
        void SendStartRecording();
        void SendStopRecording();
        void SendStartShow();
        void SendStopShow();
        void SendSearchPeople();
        void SendDisconnectMe();
        void SendCameraChange(string cameraName);
    }
}
