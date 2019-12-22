using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace P2PMapleQQ客户端
{
    public static class Wenjiancaozuo
    {
        public static string lujin() 
        {
            OpenFileDialog dia = new OpenFileDialog();
            //设置初始目录
            dia.InitialDirectory = @"";
            dia.Title = "请选择要发送的文件";
            //过滤文件类型
            dia.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*|图片(*.jpg;*.png;*.gif;*.jpeg;*.bmp)|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
            dia.ShowDialog();
            return dia.FileName;
        }
    }
}
