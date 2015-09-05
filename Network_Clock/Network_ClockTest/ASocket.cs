using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Network_Clock.Clock.Client;

namespace Network_Clock.Clock {
    public abstract class ASocket {
        public enum Type {
            Server = 1,
            Client = 2
        }
        private Type _type;

        protected TcpClient Remote { get; private set; }
        private Stream _stream;
        private StreamWriter _writer;
        private StreamReader _reader;

        private Thread _listeningThread;
        protected Boolean Closed { get; set; }

        protected ASocket(TcpClient client, Type type) {
            InitSocket(client, type);
        }

        public void InitSocket(TcpClient remote, Type type) {
            this._type = type;
            if(type == Type.Client) Debug("Connected to server: " + GetAddress(remote));
            else Debug("Connected to client: "+GetAddress(remote));

            this.Remote = remote;
            this._stream = this.Remote.GetStream();
            this._writer = new StreamWriter(this._stream);
            this._reader = new StreamReader(this._stream);
            this._listeningThread = new Thread(WaitForMessage);
            this._listeningThread.Name = Enum.GetName(typeof (Type), type)+"_ListeningThread";
            this._listeningThread.Start();
        }


        protected void WaitForMessage() {
            while(!Closed) {
                String message = RecieveLine();
                Debug("Recieved messages: \""+message+"\"");
                String[] split = message.Split(' ');
                bool validCmd = SearchCommand(split[0], Subarray(split, 1));
                if(!validCmd) SendLine("NO_VALID_CMD");
                Thread.Sleep(50);
            }
        }
        protected abstract Boolean SearchCommand(String cmd, String[] param);


        public void SendLine(string cmd, params string[] param) {
            string message = cmd;
            if (param.Length > 0)
                message += " " + RebuildString(param);
            this._writer.WriteLine(message);
            this._writer.Flush();
        }
        private string RecieveLine() {
            return this._reader.ReadLine();
        }
        public abstract void Close();

        //////////////////////////////////////////Util
        protected string RebuildString(String[] split) { return RebuildString(split, 0); }
        protected string RebuildString(String[] split, int begin) {
            String result = "";
            for (int i = 0; i < split.Length-begin; i++) {
                int curIndex = i + begin;
                result += split[curIndex];
                if (curIndex < split.Length) result += " ";
            }
            return result;
        }
 
        protected string[] Subarray(String[] split, int sub) {
            String[] result = new string[split.Length-sub];
            for (int i = 0; i < split.Length-sub; i++) {
                result[i] = split[i + sub];
            }
            return result;
        }

        protected string GetRemoteAddress() { return GetAddress(Remote); }
        protected string GetAddress(TcpClient client) {
            return client.Client.LocalEndPoint.ToString();
        }

        protected void Debug(String text) {
            System.Diagnostics.Debug.WriteLine(Enum.GetName(typeof(Type),this._type)+"| "+text);
        }
    } 
}