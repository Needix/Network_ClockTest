using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Network_Clock.Clock.Client;
using Network_Clock.Clock.Server;

namespace Network_Clock {
    static class Program {
        const int port = 5000;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main() {
            Thread s = new Thread(CreateAndStartServer);
            Thread c = new Thread(CreateAndStartClient);

            s.Start();
            c.Start();
        }

        private static void CreateAndStartServer() {
            ServerAcceptor s = new ServerAcceptor();
            s.OpenServer(port);
        }

        private static void CreateAndStartClient() {
            ClientSocket c = new ClientSocket("localhost", port);
            c.SendLine("REQUEST_TIME");
        }
    }
}
