using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 聊天项目界面
{
    public class Fangtupian
    {
        public string Tplj()
        {
            OpenFileDialog dia = new OpenFileDialog();
            try
            {
                //设置初始目录
                dia.InitialDirectory = @"";
                dia.Title = "请选择要发送的文件";
                //过滤文件类型
                dia.Filter = "图片(*.jpg;*.png;*.gif;*.jpeg;*.bmp)|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
                DialogResult rs =dia.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dia.FileName;
        }
    }
}
