using GameServer.Cache;
using Newtonsoft.Json;
using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VanguardServer;

namespace GameServer.Logic
{
    /// <summary>
    /// 账号处理的逻辑层
    /// </summary>
    public class AccountHandler : IHandler
    {
        AccountCache accountCache = Caches.Account;

        public void OnDisconnect(ClientPeer client)
        {
            if (accountCache.IsOnline(client))
                accountCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {            
            switch(subCode)
            {
                case AccountCode.REGIST_CREQ:
                    {
                        AccountDto dto = JsonConvert.DeserializeObject<AccountDto>(value.ToString());
                        //Console.WriteLine(dto.Account);
                        //Console.WriteLine(dto.Password);
                        regist(client, dto.Account, dto.Password,dto.avatarid);
                    }
                    break;
                case AccountCode.LOGIN:
                    {
                        AccountDto dto = JsonConvert.DeserializeObject<AccountDto>(value.ToString());
                        //Console.WriteLine(dto.Account);
                        //Console.WriteLine(dto.Password);
                        login(client, dto.Account, dto.Password);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void regist(ClientPeer client, string account, string password,int avatarid)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (accountCache.IsExist(account))
                {
                    //表示账号已经存在
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号已经存在");
                    ConsoleManager.Log(2, OpCode.ACCOUNT, client.ClientSocket.RemoteEndPoint.ToString(), "注册新用户", "账号已存在");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -1);
                    return;
                }

            if (string.IsNullOrEmpty(account) || account.Length < 4 || account.Length>16)
                {
                    //表示账号输入不合法
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号输入不合法");
                    ConsoleManager.Log(2, OpCode.ACCOUNT, client.ClientSocket.RemoteEndPoint.ToString(), "注册新用户", "账号非法输入");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -2);
                    return;
                }

                if (string.IsNullOrEmpty(password) || password.Length < 30|| password.Length > 32)
                {
                    //表示密码输入不合法
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "密码输入不合法");
                    ConsoleManager.Log(2, OpCode.ACCOUNT, client.ClientSocket.RemoteEndPoint.ToString(), "注册新用户", "密码非法输入");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -3);
                    return;
                }

                //可以注册了
                accountCache.Create(account, password,avatarid);
                //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "注册成功");
                ConsoleManager.Log(1, OpCode.ACCOUNT, client.ClientSocket.RemoteEndPoint.ToString(), "注册新用户", "注册成功！用户名:"+account);
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, 0);

                /*此处为上线测试
                accountCache.Online(client, account);
                //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "登录成功");
                Console.WriteLine(DateTime.Now + "<注册管理模块>:" + client.ClientSocket.RemoteEndPoint.ToString() + ":登录成功!用户名:" + account);
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);*/
            });            
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void login(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsExist(account))
                {
                    //表示账号不存在
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号不存在");
                    ConsoleManager.Log(2, OpCode.ACCOUNT, client.ClientSocket.RemoteEndPoint.ToString(), "用户登录", "账号"+account+"未注册！");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -1);
                    return;
                }

                if (accountCache.IsOnline(account))
                {
                    //表示账号在线
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号在线");
                    ConsoleManager.Log(2, OpCode.ACCOUNT, client.ClientSocket.RemoteEndPoint.ToString(), "用户登录", "账号" + account + "已登录！");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -2);
                    return;
                }

                if (!accountCache.IsMatch(account, password))
                {
                    //表示账号密码不匹配
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号密码不匹配");
                    Console.WriteLine(DateTime.Now + "<登录管理模块>:" + client.ClientSocket.RemoteEndPoint.ToString() + ":登录失败！用户:" + account + "输入了错误的密码！");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -3);
                    return;
                }

                //登录成功
                accountCache.Online(client, account);
                //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "登录成功");
                Console.WriteLine(DateTime.Now + "<登录管理模块>:" + client.ClientSocket.RemoteEndPoint.ToString() + ":用户:" + account + "登录成功！");
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);
            });
        }
    }
}
