using VanguardServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            //指定所关联的应用
            server.SetApplication(new NetMsgCenter());
            server.Start(40960, 10);
            Console.ReadKey();
        }
    }
}
