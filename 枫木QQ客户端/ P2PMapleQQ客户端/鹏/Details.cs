using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 聊天项目;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

namespace 聊天项目界面
{
    public partial class Details : Form
    {
        public Details()
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

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.BackColor = Color.Red;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.BackColor = Color.Transparent;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.BackColor = Color.MediumTurquoise;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = Color.Transparent;
        }
        GM gm = new GM();
        public void HyXx()
        {
            int s = Convert.ToInt32(DateTime.Now.ToLocalTime().ToString("yyyy"));
            SqlDataReader reader = gm.reader("select * from dbo.UserInfo where qq='" + Form1.Xxqq + "'");
            if (reader.Read())
            {
                label16.Text = reader["qq"].ToString();
                label17.Text = reader["nickname"].ToString();
                label9.Text = reader["sex"].ToString();
                label18.Text = (s - Convert.ToInt32(Convert.ToDateTime(reader["birthday"]).ToString("yyyy"))) + "岁";
                label19.Text = Convert.ToDateTime(reader["birthday"]).ToString("yyyy-MM-dd");
                label10.Text = reader["address"].ToString();
                label12.Text = reader["qq"] + "@qq.com";
            }
            reader.Close();
        }
        public void BrXx()
        {
            int s = Convert.ToInt32(DateTime.Now.ToLocalTime().ToString("yyyy"));
            SqlDataReader reader = gm.reader("select * from dbo.UserInfo where qq='" + Form1.QQ + "'");
            if (reader.Read())
            {
                label16.Text = reader["qq"].ToString();
                label22.Text = reader["nickname"]+"的空间";
                label21.Text = reader["sex"].ToString();
                label20.Text = (s - Convert.ToInt32(Convert.ToDateTime(reader["birthday"]).ToString("yyyy"))) + "岁";
                label14.Text = Convert.ToDateTime(reader["birthday"]).ToString("yyyy-MM-dd");
                label10.Text = reader["address"].ToString();
                label12.Text = reader["qq"] + "@qq.com";
            }
            reader.Close();
        }
        private void Details_Load(object sender, EventArgs e)
        {
            if (File.Exists(tupluj))
            {
                Sctp();
            }
            if (Form1.isok)
            {
                HyXx();
            }
            else
            {
                label4.Text = "☆ 我的资料";
                BrXx();
                panel2.Visible = true;
            }
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

        private static string tupluj;
        private static bool isok = true;
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Sctp();
        }
        public void Sctp()
        {
            try
            {
                Fangtupian f = new Fangtupian();
                if (isok == true)
                {
                    tupluj = f.Tplj();
                }
                if (File.Exists(tupluj))
                {
                    isok = false;
                    pictureBox12.Visible = isok;
                    pictureBox11.Image = Image.FromFile(tupluj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (File.Exists(tupluj))
            {
                e.Cancel = false;
            }
        }

        private void 更换头像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isok = true;
            Sctp();
        }
    }
}
