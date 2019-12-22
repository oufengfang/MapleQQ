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

namespace Login
{
    public partial class ZhaohuiPwd : Form
    {
        public ZhaohuiPwd()
        {
            InitializeComponent();
        }
        Login l = new Login();
        DbTools db = new DbTools();
        
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text=="")
            {
                MessageBox.Show("请先输入账号");
                return;
            }
            string sql = @" select Question
            from GetPwdInfo
            where gQq='"+textBox1.Text+"'";
            SqlDataReader r = db.queny(sql);
            if (r.Read())
            {
                textBox2.Text = r["Question"].ToString();
            }
            else
            {
                MessageBox.Show("没有找到该账户，要不先注册？？");
                return;
            }
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yz = textBox3.Text;
            if (yz.Trim() == "")
            {
                MessageBox.Show("请输入密保答案！");
            }
            else
            {
                string sq2 = @" select Gpno, Question, Answer, gQq
                from GetPwdInfo
                where gQq='" + textBox1.Text + "' and Answer='" + yz + "'";
                SqlDataReader r = db.queny(sq2);
                if (r.Read())
                {
                    MessageBox.Show("验证成功");
                    panel1.Visible = true;
                }
                else
                {
                    MessageBox.Show("答案错误！");
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string xmm = textBox5.Text;
            string qrmm = textBox4.Text;
            if (xmm.Trim() == "")
            {
                MessageBox.Show("请输入新密码！");
            }
            else if (qrmm.Trim() == "")
            {
                MessageBox.Show("请确认密码！");
            }
            else
            {
                if (xmm == qrmm)
                {
                    string sql5 = @"UPDATE [MapleQQ].[dbo].[UserInfo]
                      SET [pwd] = '"+textBox4.Text+"' WHERE qq='"+textBox1.Text+"'";
                    int r = db.zsg(sql5);
                    if (r == 1)
                    {
                        MessageBox.Show("修改成功！");
                    }
                }
                else
                {
                    MessageBox.Show("密码与确认密码不一致！");
                }

            }
        }
        
        private void label6_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(label6, "最小化");
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.BackColor = Color.Red;
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(label7, "关闭");
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.BackColor = Color.Transparent;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
