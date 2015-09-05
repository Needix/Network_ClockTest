using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Network_Clock.Clock.Server {
    class ServerAcceptor {
        private readonly List<ServerSocket> _sClients = new List<ServerSocket>();
        private Thread _listeningThread;
        private int _port;

        private volatile Boolean _close;
        public Boolean Close { get { return _close; } set { _close = value; } }

        /// <summary>
        /// Opens the socket and waits for connections
        /// </summary>
        /// <param name="port">The port the server is listening</param>
        public void OpenServer(int port) {
            this._port = port;
            this._listeningThread = new Thread(RunServer);
            this._listeningThread.Name = "Server_ConnectionServer";
            this._listeningThread.Start();
        }

        private void RunServer() {
            TcpListener listener = new TcpListener(IPAddress.Any,this._port);
            Debug.WriteLine("ServerAcceptor| Listening on port: " + this._port);
            listener.Start();
            while(!Close) {
                if (listener.Pending()) {
                    TcpClient client = listener.AcceptTcpClient();
                    ServerSocket serverSocket = new ServerSocket(this, client);
                    this._sClients.Add(serverSocket);
                } else Thread.Sleep(100);
            }

            foreach(ServerSocket sClient in this._sClients) {
                sClient.Close();
            }
        }

        public void RemoveSocket(ServerSocket socket) {
            _sClients.Remove(socket);
            if (_sClients.Count == 0) Close = true;
        }
    }
}
