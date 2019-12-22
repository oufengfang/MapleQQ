using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace 服务端
{
    public class Zhuanfa
    {
        public int FQQ { get; set; }//发送者QQ号
        public int JQQ { get; set; }//接收者QQ号
        public string FQQName { get; set; }//发送者QQ昵称
        public string JQQName { get; set; }//接收者昵称
        public string yonfuIP { get; set; }//连接服务器的客户端的IP地址
        public Socket Send { get; set; }
        public Zhuanfa(int fqq,int jqq,string ip,Socket send,string jqqName,string fqqName) 
        {
            this.FQQ = fqq;
            this.JQQ = jqq;
            this.yonfuIP = ip;
            this.Send = send;
            this.FQQName = fqqName;
            this.JQQName = jqqName;
        }
    }
}
