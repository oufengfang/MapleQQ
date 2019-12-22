using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using 聊天项目;

namespace P2PMapleQQ客户端
{
    public partial class jilu : Form
    {
        public jilu()
        {
            InitializeComponent();
        }
        public int WQQ;
        public string WName;
        public string TName;
        public int TQQ;
        聊天项目.GM kk = new GM();
        private void jilu_Load(object sender, EventArgs e)//加载记录
        {
            sdf();
        }
        public void sdf() 
        {
            string sql = string.Format(@"SELECT [cdate]
                      ,[tno]
                  FROM [MapleQQ].[dbo].[ChatInfo]
                  where (csendqq={0} and creceiveqq={1})", WQQ, TQQ);
            SqlDataReader reder = kk.reader(sql);
            while (reder.Read())
            {
                int bh = int.Parse(reder["tno"].ToString());
                string shijian = reder["cdate"].ToString();
                string sql2 = string.Format(@"SELECT [tcontext]
                          FROM [MapleQQ].[dbo].[Text]
                          where tno={0}",bh);
                SqlDataReader hh = kk.reader(sql2);
                while (hh.Read())
                {
                    richTextBox1.Text = WName + " " + shijian + "\n" + hh["tcontext"].ToString() +"\n"+ richTextBox1.Text;
                }
            }
             sql = string.Format(@"SELECT [cdate]
                      ,[tno]
                  FROM [MapleQQ].[dbo].[ChatInfo]
                  where csendqq={0} and creceiveqq={1}",TQQ, WQQ);
            reder = kk.reader(sql);
            while (reder.Read())
            {
                int bh = int.Parse(reder["tno"].ToString());
                string shijian = reder["cdate"].ToString();
                string sql2 = string.Format(@"SELECT [tcontext]
                          FROM [MapleQQ].[dbo].[Text]
                          where tno={0}", bh);
                SqlDataReader hh = kk.reader(sql2);
                while (hh.Read())
                {
                    richTextBox1.Text = WName + " " + shijian + "\n" + hh["tcontext"].ToString() + "\n" + richTextBox1.Text;
                }
            }
        }
    }
}
