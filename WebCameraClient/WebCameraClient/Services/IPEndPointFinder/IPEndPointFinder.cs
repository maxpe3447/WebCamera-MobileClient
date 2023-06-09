using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebCameraClient.Services.IPEndPointFinder
{
    public class IPEndPointFinder : IIPEndPointFinder
    {
        private string _host = string.Empty;

        public string GetHost()
        {
            if (_host != string.Empty)
            {
                return _host;
            }

            var host = Dns.GetHostEntry(Dns.GetHostName());
            if (host == null)
            {
                throw new Exception("Hosr is not founded");
            }
            _host = host.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                                    .ToList()
                                    .Where(x => int.Parse(x.ToString().Split('.').Last()) != 1)
                                    .LastOrDefault()
                                    ?.ToString() ?? string.Empty;
            if (_host == string.Empty)
            {
                throw new Exception("NO HOST!");
            }

            return _host;
        }

        public int GetPort() => 9090;
    }
}
