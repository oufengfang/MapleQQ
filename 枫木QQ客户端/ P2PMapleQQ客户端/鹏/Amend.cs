using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace 聊天项目
{
    public partial class Amend : Form
    {
        public Amend()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; 
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.BackColor = Color.MediumTurquoise;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = Color.Transparent;
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.BackColor = Color.Red;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.BackColor = Color.Transparent;
        }
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                leftFalg = false;//释放鼠标后标记为false
            }
        }
        GM gm = new GM();
        private void Amend_Load(object sender, EventArgs e)
        {
            SqlDataReader reader = gm.reader("select * from dbo.UserInfo where qq='" + Form1.QQ + "'");
            if (reader.Read())
            {
                textBox1.Text = reader["nickname"].ToString();
                comboBox1.Text = reader["sex"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(reader["birthday"]);
                textBox4.Text = reader["address"].ToString();
                textBox9.Text = reader["sign"].ToString();
                textBox8.Text = reader["qq"]+"@qq.com";
            }
            reader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nicheng = textBox1.Text.Trim();
            string sex = comboBox1.Text;
            string birthday = dateTimePicker1.Value.ToString();
            string address = textBox4.Text.Trim();
            string sign = textBox9.Text.Trim();
            if (nicheng=="")
            {
                MessageBox.Show("昵称不能为空","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            string update = string.Format(@"update dbo.UserInfo
                        set nickname='{0}',sex='{1}',birthday='{2}',[address]='{3}',[sign]='{4}'
                        where  qq='{5}'", nicheng, sex, birthday, address, sign,Form1.QQ);
            int hs = gm.Zsg(update);
            if (hs==1)
            {
                MessageBox.Show("保存成功","提示");
                this.Close();
            }
            else
            {
                MessageBox.Show("修改失败", "提示");
            }
        }
    }
}
