using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Network_Clock.Clock.Client {
    class Client : Socket {
        public void Connect(string host, int port) {
            TcpClient c = new TcpClient(host, port);
            InitSocket(c, Type.Client);
        }

        protected override bool SearchCommand(string message) {
            throw new NotImplementedException();
        }
    }
}
