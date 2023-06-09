using System;
using System.Collections.Generic;
using System.Text;

namespace WebCameraClient.Services.IPEndPointFinder
{
    public interface IIPEndPointFinder
    {
        string GetHost();
        int GetPort();
    }
}
