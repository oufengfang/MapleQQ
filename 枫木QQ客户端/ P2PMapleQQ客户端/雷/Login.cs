using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading;
using 聊天项目;
namespace Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
         //   this.WindowState = FormWindowState.Minimized;
           // this.ShowInTaskbar = true; 
        }
        public static int QQ;
        public static string Pwd;
        DbTools db = new DbTools();
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        public void RemberPwd()//记住密码
        {
            if (checkBox2.Checked)
            {
                string sql = string.Format(@"SELECT COUNT(*)
                  FROM [MapleQQ].[dbo].[本地密码记录表]
                  where Jqq={0}", int.Parse(comboBox1.Text));
                int jiegu = db.query(sql);
                if (jiegu > 0)
                {
                    sql = string.Format(@"SELECT COUNT(*)
                  FROM [MapleQQ].[dbo].[本地密码记录表]
                  where Jpwd='{0}' and Jqq='{1}'", int.Parse(textBox2.Text), int.Parse(comboBox1.Text));
                    jiegu = db.query(sql);
                    if (int.Parse(comboBox1.Text) > 0)
                    {

                    }
                    else
                    {
                        sql = string.Format(@"UPDATE [MapleQQ].[dbo].[本地密码记录表]
                           SET [Jpwd] ='{0}'
                              ,[Rememb] ='{1}'
                              ,[AutomaticLogin] = '{2}'
                         WHERE Jqq='{3}'", textBox2.Text, "true", "false", comboBox1.Text);
                        db.zsg(sql);
                    }
                }
                else
                {
                    sql = string.Format(@"INSERT INTO [MapleQQ].[dbo].[本地密码记录表]([Jqq],[Jpwd],[AutomaticLogin],Rememb) VALUES
                ('{0}'
                ,'{1}'
                ,'{2}'
                ,'{3}')", comboBox1.Text, textBox2.Text, "false", "true");
                    db.zsg(sql);
                }

            }
        }
        public void ZidonDenlu() 
        {
            string sql = "DELETE FROM [MapleQQ].[dbo].[自动登录表]";
            db.zsg(sql);
            sql = string.Format(@"INSERT INTO [MapleQQ].[dbo].[自动登录表]
                           ([zqq]
                           ,[Jpwd]
                           ,[Rememb]
                           ,[AutomaticLogin])
                     VALUES('{0}','{1}','{2}','{3}')", comboBox1.Text, textBox2.Text, "true", "true");
            db.zsg(sql);
        }
        //获取电脑的IP地址
        public string IP;
        public void GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[1];
            IP = localaddr.ToString();
        }
        //注册
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registered re = new Registered();
            re.ShowDialog();
            comboBox1.Text = QQ.ToString();
            textBox2.Text = Pwd;
        }
        //登入判断
        public string qq;
       
        private void button1_Click(object sender, EventArgs e)
        {
            qq = comboBox1.Text.ToString();
            string pwd = textBox2.Text.ToString();
            if (qq==""||pwd=="")
            {
                MessageBox.Show("请输入账号或者密码");
            }
            #region
            else
            {
                string sql = "select  Count(*)  from dbo.UserInfo where qq='"+qq+"' and pwd='"+pwd+"'";
                int r = db.query(sql);
                if (r == 1)
                {
                    string sql3 = @"select COUNT(*) from UserInfo where qq='" + comboBox1.Text.ToString() + "' and lstatus=1";
                    int q = db.query(sql3);
                    if (q == 1)
                    {
                        MessageBox.Show("你已经登入，请勿重复此操作");
                        return;
                    }
                    string sql2 = @"UPDATE [MapleQQ].[dbo].[UserInfo]
                        SET [lstatus] = 1
                            WHERE qq='" + comboBox1.Text + "'";
                     int h = db.zsg(sql2);
                    if (h == 1)
                    {
                        if (checkBox2.Checked == true)
                        {
                            RemberPwd();//记住密码方法
                        }
                        if (checkBox1.Checked == true)
                        {
                            ZidonDenlu();
                        }
                        this.Hide();
                        聊天项目.Form1 sld = new Form1();
                        Form1.QQ = int.Parse(comboBox1.Text);
                        sld.Show();
                    }
                    else
                    {
                        MessageBox.Show("登入失败");
                    }  
                }
                else
                {
                    MessageBox.Show("没有该用户，请重新输入账号或者密码");
                }
            }
            #endregion

        }
        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //自定义最小化窗体
        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // 最小化
        }
        //自定义窗体
        #region
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                leftFalg = false;//释放鼠标后标记为false
            }
        }
        #endregion

        private void Login_Load(object sender, EventArgs e)
        {
            string sql = "select jqq,jpwd from dbo.本地密码记录表 where AutomaticLogin='true'";
            SqlDataReader reder = db.queny(sql);
            while (reder.Read())
            {
                comboBox1.Items.Add(reder["jqq"]);
                comboBox1.Text = reder["jqq"].ToString();
                textBox2.Text = reder["jpwd"].ToString();
            }
            reder.Close();
            sql = string.Format(@"SELECT *
                      FROM [MapleQQ].[dbo].本地密码记录表
                      where jqq='{0}'", comboBox1.Text.ToString());
            reder = db.queny(sql);
            if (reder.Read())
            {
                textBox2.Text = reder["jpwd"].ToString();
                if (reder["Rememb"].ToString() == "true")
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                if (reder["AutomaticLogin"].ToString() == "true")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
            sql = @"select * from dbo.自动登录表";
            reder = db.queny(sql);
            if (reder.Read())
            {
                comboBox1.Text = reder["zqq"].ToString();
                textBox2.SelectedText = reder["Jpwd"].ToString();
            }
            checkBox2.Checked = true;
            if (reder["Rememb"].ToString() == "true")
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
            if (reder["AutomaticLogin"].ToString() == "true")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            reder.Close();
        }
        //找回密码事件
        private void label3_Click(object sender, EventArgs e)
        {
            ZhaohuiPwd pwd = new ZhaohuiPwd();
            pwd.ShowDialog();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
        }
        private void label4_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(label4, "关闭");
            label4.BackColor = Color.Red;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.Transparent;
        }
        int a=5;
        private void Login_Shown(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true)
                {
                    timer1.Enabled = true;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string sql = string.Format(@"SELECT *
                      FROM dbo.本地密码记录表
                      where jqq={0}", int.Parse(comboBox1.Text));
            SqlDataReader reder = db.queny(sql);
            if (reder.Read())
            {
                textBox2.Text = reder["jpwd"].ToString();
                if (reder["Rememb"].ToString() == "true")
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                if (reder["AutomaticLogin"].ToString() == "true")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label5, "最小化");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a--;
            if (a == 0)
            {
                button1_Click(sender, e);
            }
        }

    }
}
