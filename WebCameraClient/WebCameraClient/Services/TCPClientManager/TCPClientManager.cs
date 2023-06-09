using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCameraClient.Model;
using Xamarin.Forms;

namespace WebCameraClient.Services.TCPClientManager
{
    public class TCPClientManager :ITCPClientManager
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public delegate void CaptureImageHandler(MemoryStream stream, string text);
        event CaptureImageHandler Capturing;
        public void Connected(string host, int port)
        {

            _client = new TcpClient();
            _client.Connect(host, port);
        }

        public void StartGetting(CaptureImageHandler capture)
        {
            Capturing += capture;
            _stream = _client.GetStream();
            Thread receiveThread = new Thread(new ThreadStart(ReceiveImage));
            receiveThread.Start(); //старт потока
            //await Task.Factory.StartNew(()=> ReceiveImage());
        }

        private void ReceiveImage()
        {
            //while (true)
            //{
                try
                {
                byte[] sendport = BitConverter.GetBytes(9090);
                _stream.Write(sendport, 0, sendport.Length);

                    var host = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList().Last();
                var strHost = host.ToString();
                    byte[] data = Encoding.ASCII.GetBytes(host.ToString());
                    _stream.Write(data, 0, data.Length);

                    //var ports = GetOpenPort();
                    //if (ports.Count == 0)
                    //{
                    //    throw new Exception("No open ports");
                    //}

                //await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("start");

                // byte[] reciveLen = new byte[4];
                // _stream.Read(reciveLen, 0, 4);
                // int lenght = BitConverter.ToInt32(reciveLen, 0);


                // //byte[] data = new byte[lenght];
                // byte[] buff = new byte[lenght]; // буфер для получаемых данных
                // List<byte> resultData = new();
                //int bytes = 0;
                // do
                // {
                //     //bytes = _stream.Read(data, 0, data.Length);


                //     bytes = _stream.Read(buff, 0, buff.Length);
                //     resultData.AddRange(buff.Take(bytes));
                //     //builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                // }while (resultData.Count < lenght);//(_stream.DataAvailable);

                //await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(bytes.ToString());
                //Capturing?.Invoke(ImageSource.FromStream(()=>)));
                //using (MemoryStream ms = new MemoryStream(resultData.ToArray()))
                //{
                //    bmp = new Bitmap(ms);
                //}


                //Bitmap bmp = new Bitmap(new MemoryStream(resultData.ToArray()));
                //MemoryStream memory = new MemoryStream();
                //bmp.Save(memory, ImageFormat.Png);
                //memory.Flush();
                //memory.Close();
                //var image = ImageSource.FromStream(() => memory);

                //if (Capturing != null) { 
                //Capturing?.Invoke(null, $"{lenght} {bytes} {resultData.Count}");
                //string message = builder.ToString();
                //Console.WriteLine(message);//вывод сообщения
            }
                catch
                {
                Acr.UserDialogs.UserDialogs.Instance.Alert("error");
                    //Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    //Console.ReadLine();
                    Disconnect();
                }
            //}

        }
        public void Disconnect()
        {
            if (_stream != null)
                _stream.Close();//отключение потока
            if (_client != null)
                _client.Close();//отключение клиента
            //Environment.Exit(0); //завершение процесса
        }

        private List<PortInfoModel> GetOpenPort()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            //IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();
            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            return tcpConnections.Select(p =>
            {
                return new PortInfoModel(
                    i: p.LocalEndPoint.Port);
            }).ToList();
        }
    }
}
