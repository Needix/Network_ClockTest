using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Network_Clock.Clock.Server {
    class SClient : Socket {
        public SClient(TcpClient tcpClient) {
            InitSocket(tcpClient, Type.Server);
        }

        protected override bool SearchCommand(string message) {
            throw new NotImplementedException();
        }
    }
}
