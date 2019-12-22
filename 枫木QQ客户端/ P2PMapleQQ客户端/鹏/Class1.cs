using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 聊天项目界面
{
    public class Class1
    {
        public string QQ { get; set; }
        public string Nicheng { get; set; }
        public string Tuplj { get; set; }
        public string Qm { get; set; }

        public Class1(string qq, string nicheng)
        {
            QQ = qq;
            Nicheng = nicheng;
        }
        public Class1(string qq, string nicheng, string tuplj, string qm)
        {
            QQ = qq;
            Nicheng = nicheng;
            Tuplj = tuplj;
            Qm = qm;
        }
    }
}
