using System;
using System.Net.Sockets;

namespace Network_Clock.Clock.Client {
    class ClientSocket : ASocket {
        public ClientSocket(TcpClient client) : base(client, Type.Client) { }
        public ClientSocket(string host, int port): base(new TcpClient(host, port), Type.Client) { }

        /*
         * Network Protocol (Server->Client):
         * TIME <time>
         * CLOSE //Closes the connection
         * NO_VALID_CMD //Notifies client that cmd was invalid
         */
        protected override bool SearchCommand(string cmd, string[] param) {
            try {
                switch (cmd) {
                    case "TIME":
                        Debug("Time recieved: " + RebuildString(param));
                        this.Close();
                        break;
                    case "CLOSE":
                        Closed = true;
                        this.Remote.Close();
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

        public override void Close() { SendLine("REQUEST_CLOSE"); }
    }
}
