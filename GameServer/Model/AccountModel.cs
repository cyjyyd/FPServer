using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public class AccountModel
    {
        public int Id;
        public string Account;
        public string Password;
        public int avatarid;
        //...创建日期 电话号码

        public AccountModel(int id, string account, string password,int avatarid)
        {
            this.Id = id;
            this.Account = account;
            this.Password = password;
            this.avatarid = avatarid;
        }
    }
}
