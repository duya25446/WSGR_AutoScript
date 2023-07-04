using System.Net;
using System.Net.Sockets;

namespace WSGR_AutoScript
{
    class TCPClient
    {
        IPEndPoint _ip;
        public TCPClient(IPEndPoint ip)
        {
            _ip = ip;
        }
        public TCPClient(string ip,int port)
        {
            var iped = new IPEndPoint(IPAddress.Parse(ip),port);
            _ip = iped;
        }
        public TcpClient CreateClient()
        {
            TcpClient client = new TcpClient(_ip);
            return client;
        }
    }
}
