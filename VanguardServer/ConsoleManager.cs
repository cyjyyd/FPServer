using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VanguardServer
{
    public static class ConsoleManager
    {
        public static void Log(int MsgType,int Module,string Client,string Action,string Message)
        {
            string Mod="";
            switch (Module)
            {
                case OpCode.ACCOUNT:
                    Mod = "账户管理模块";
                    break;
                case OpCode.USER:
                    Mod = "用户功能模块";
                    break;
                case OpCode.MATCH:
                    Mod = "匹配系统";
                    break;
                case OpCode.CHAT:
                    Mod = "聊天系统";
                    break;
                case OpCode.FIGHT:
                    Mod = "战斗系统";
                    break;
                case 127:
                    Mod = "网络连接管理";
                    break;
                default:
                    Mod = "FPServer";
                    break;
            }
            switch (MsgType)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                Console.ForegroundColor= ConsoleColor.White;
                    break;
            }
            if (Client!=null)
            {
                Console.WriteLine(DateTime.Now +":"+ "<" + Mod + ">" + Client + ":" + Action + ":" + Message);
            }
            else
            {
                Console.WriteLine(DateTime.Now +":"+ "<" + Mod + ">" + Action + ":" + Message);
            }
        }
    }
}
