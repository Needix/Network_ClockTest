using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Network_Clock.Clock.Server {
    class Server {
        private List<SClient> sClients = new List<SClient>();
        private Thread listeningThread;
        private int port;

        public Boolean Close { get; set; }

        /// <summary>
        /// Opens the socket and waits for connections
        /// </summary>
        /// <param name="port">The port the server is listening</param>
        public void OpenServer(int port) {
            this.port = port;
            listeningThread = new Thread(RunServer);
            listeningThread.Name = "Server_ConnectionServer";
            listeningThread.Start();
        }

        private void RunServer() {
            TcpListener listener = new TcpListener(port);
            Debug.WriteLine("Server| Listening on port: " + port);
            listener.Start();
            while(!Close) {
                TcpClient client = listener.AcceptTcpClient();
                SClient sClient = new SClient(client);
                sClients.Add(sClient);
            }

            foreach(SClient sClient in sClients) {
                sClient.Close();
            }
        }
    }
}
