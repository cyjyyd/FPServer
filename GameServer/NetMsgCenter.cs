using GameServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanguardServer;
using Protocol.Code;

namespace GameServer
{
    /// <summary>
    /// 网络的消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        AccountHandler account = new AccountHandler();
        UserHandler user = new UserHandler();
        MatchHandler match = new MatchHandler();
        ChatHandler chat = new ChatHandler();
        FightHandler fight = new FightHandler();

        public NetMsgCenter()
        {
            match.startFight += fight.StartFight;
        }

        public void OnDisconnect(ClientPeer client)
        {
            //相反顺序
            fight.OnDisconnect(client);
            chat.OnDisconnect(client);
            match.OnDisconnect(client);
            user.OnDisconnect(client);
            account.OnDisconnect(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    account.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.USER:
                    user.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.MATCH:
                    match.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.CHAT:
                    chat.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.FIGHT:
                    fight.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                default:
                    break;
            }          
        }
    }
}
