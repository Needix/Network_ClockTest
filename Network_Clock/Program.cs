using System;
using System.Collections.Generic;
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
            s.Start();

            Thread c = new Thread(CreateAndStartClient);
            c.Start();
        }

        private static void CreateAndStartServer() {
            Server s = new Server();
            s.OpenServer(port);
        }

        private static void CreateAndStartClient() {
            Client c = new Client();
            c.Connect("localhost", port);
        }
    }
}
