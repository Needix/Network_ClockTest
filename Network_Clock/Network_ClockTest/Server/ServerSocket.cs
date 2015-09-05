using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Network_Clock.Clock.Server {
    class ServerSocket : ASocket {
        private readonly ServerAcceptor _acceptor;

        public ServerSocket(ServerAcceptor acceptor, TcpClient tcpClient) : base(tcpClient, Type.Server) {
            _acceptor = acceptor;
        }
        
        /*
         * Network Protocol (Client->Server):
         * REQUEST_TIME //Requests the time
         * REQUEST_CLOSE //Client requests to close the connection
         * 
         * NO_VALID_CMD //Notifies server that cmd was invalid
         */
        protected override bool SearchCommand(string cmd, string[] param) {
            try {
                switch (cmd) {
                    case "REQUEST_TIME":
                        SendLine("TIME "+DateTime.Now);
                        break;
                    case "REQUEST_CLOSE":
                        Close();
                        break;
                    case "NO_VALID_CMD":

                        break;
                    default:
                        Debug(string.Format(GetRemoteAddress()+" send invalid cmd \"{0}\" with parameters: \"{1}\"", cmd, RebuildString(param)));
                        return false;
                }
            } catch (IndexOutOfRangeException) {
                Debug("ERROR: Not enough parameter were sent for cmd \""+cmd+"\"! (parameter: \""+RebuildString(param)+"\")");
                return false;
            }
            return true;
        }

        public override void Close() {
            SendLine("CLOSE"); 
            _acceptor.RemoveSocket(this);
            Closed = true;
        }
    }
}
