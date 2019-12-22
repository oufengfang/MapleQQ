using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 服务端
{
    class COMF
    {
        public byte[] Neiron { get; set; }
        public int Jqq { get; set; }
        public COMF(int qq, byte[] shuju) 
        {
            this.Neiron = shuju;
            this.Jqq = qq;
        }
    }
}
