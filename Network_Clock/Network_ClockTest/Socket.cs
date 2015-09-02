using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Network_Clock.Clock {
    public abstract class Socket {
        public enum Type {
            Server = 1,
            Client = 2
        }
        private Type type;

        protected TcpClient tcpClient;
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;

        protected Thread listeningThread;
        private Boolean _closed = false;

        public void InitSocket(TcpClient remote, Type type) {
            this.type = type;
            if (type == Type.Client) Debug("Connected to server: " + remote); 
            else Debug("Connected to client: "+remote);

            this.tcpClient = remote;
            this.stream = tcpClient.GetStream();
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            listeningThread = new Thread(WaitForMessage);
            listeningThread.Name = Enum.GetName(typeof (Type), type)+"_ListeningThread";
            listeningThread.Start();
        }


        protected void WaitForMessage() {
            while (!_closed) {
                Debug("Waiting for messages...");
                String message = ReadLine();
                Debug("Recieved messages: "+message);
                SearchCommand(message);
                Thread.Sleep(50);
            }
        }
        protected abstract Boolean SearchCommand(String message);


        protected void WriteLine(string text) {
            writer.WriteLine(text);
        }
        protected string ReadLine() {
            return reader.ReadLine();
        }

        //////////////////////////////////////////Util
        public void Close() {
            _closed = true;
            tcpClient.Close();
        }

        protected void Debug(String text) {
            System.Diagnostics.Debug.WriteLine(Enum.GetName(typeof(Type),type)+"| "+text);
        }
    } 
}