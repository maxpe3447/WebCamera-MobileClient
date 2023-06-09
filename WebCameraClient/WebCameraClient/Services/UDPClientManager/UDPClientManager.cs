using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCameraClient.Enums;
using WebCameraClient.Helpers;
using WebCameraClient.Model;
using WebCameraClient.Services.IPEndPointFinder;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebCameraClient.Services.UDPClientManager
{
    public class UDPClientManager : IUDPClientManager
    {
        public delegate void CaptireHandle(MemoryStream stream, string text);
        public delegate void InitCameralistHandle(List<string> camers);

        event CaptireHandle capturingHandle;
        event InitCameralistHandle InitCamersHandle;

        private bool _isWorkReciving = true;
        private bool _isFirstConnection = true;
        
        private UdpClient _udpClient;
        private UdpClient _sendClient=new();

        private readonly IIPEndPointFinder _iPEndPointFinder;

        private string _host = string.Empty;
        private int _port= 0;

        public UDPClientManager(IIPEndPointFinder iPEndPointFinder)
        {
            _iPEndPointFinder = iPEndPointFinder;
        }
        public void StartReciving(CaptireHandle handle, InitCameralistHandle cameralistHandle)
        {
            _udpClient = new UdpClient(_iPEndPointFinder.GetPort());
            capturingHandle += handle;
            InitCamersHandle += cameralistHandle;

            new Thread(Reciving).Start();
        }
        public void Connecting(string host, int port)
        {
            _host = host;
            _port = port;

            var data = new ActionDataModel()
            {
                ActionType = ActionType.REGISTRATION,
                Port = _iPEndPointFinder.GetPort(),
                Host = _iPEndPointFinder.GetHost()
            };

            var bytes = ActionDataModelConvert.ToBytes(data);


            _sendClient.Send(bytes, bytes.Length, host, port);
        }
        public void SendStartRecording()
        {
            SendBasicAction(ActionType.START_RECORD);
        }
        public void SendStopRecording()
        {
            SendBasicAction(ActionType.STOP_RECORD);
        }
        public void SendStartShow()
        {
            SendBasicAction(ActionType.START_SHOW);
        }
        public void SendStopShow()
        {
            SendBasicAction(ActionType.STOP_SHOW);
        }
        public void SendSearchPeople()
        {
            SendBasicAction(ActionType.SEARCH_PEOPLE);
        }
        public void SendCameraChange(string cameraName)
        {
            var data = new ActionDataModel()
            {
                ActionType = ActionType.CAMERA_CHANGE,
                CameraName = cameraName
            };

            var bytes = ActionDataModelConvert.ToBytes(data);

            _sendClient.Send(bytes, bytes.Length, _host, _port);
        }
        public void StopReciving()
        {
            _isWorkReciving = false;
        }
        public void SendDisconnectMe()
        {
            _isFirstConnection = true;
            var data = new ActionDataModel()
            {
                ActionType = ActionType.DISCONNECT,
                Host = _iPEndPointFinder.GetHost(),
                Port = _iPEndPointFinder.GetPort()
            };

            var bytes = ActionDataModelConvert.ToBytes(data);

            _sendClient.Send(bytes, bytes.Length, _host, _port);
        }
        private async void Reciving()
        {
            try
            {
                while (_isWorkReciving)
                {
                    var res = await _udpClient.ReceiveAsync();

                    if (_isFirstConnection)
                    {
                        _isFirstConnection = false;
                        var model = ActionDataModelConvert.ToModel(res.Buffer);
                        InitCamersHandle?.Invoke(model.Camers);
                        continue;
                    }

                    var mem = new MemoryStream(res.Buffer);

                    capturingHandle?.Invoke(mem, res.Buffer.Length.ToString());
                }

            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message);
            }
        }
        private void SendBasicAction(ActionType actionType)
        {
            var data = new ActionDataModel()
            {
                ActionType = actionType
            };

            var bytes = ActionDataModelConvert.ToBytes(data);

            _sendClient.Send(bytes, bytes.Length, _host, _port);
        }
    }
}
