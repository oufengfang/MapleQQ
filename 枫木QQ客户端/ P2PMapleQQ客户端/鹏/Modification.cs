using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using 聊天项目;


namespace 聊天项目界面
{
    public partial class Modification : Form
    {
        public Modification()
        {
            InitializeComponent();
        }

        GM gm = new GM();
        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Visible==true)
            {
                SqlDataReader reader = gm.reader("select * from dbo.GetPwdInfo where gQq='" + Form1.QQ + "'");
                if (reader.Read())
                {
                    if (textBox4.Text.Trim() == reader["Answer"].ToString())
                    {
                        panel1.Visible = false;
                        label1.Visible = false;
                        textBox1.Visible = false;

                        label2.Visible = true;
                        textBox2.Visible = true;
                        label3.Visible = true;
                        textBox3.Visible = true;
                        this.Text = "修改密码";
                        button2.Text = "关闭";
                    }
                    else
                    {
                        label8.Visible = true;
                    }
                }
                return;
            }
            //输入原密码是否正确
            if (textBox2.Visible==false)
            {
                string ymm = textBox1.Text.Trim();
                if (ymm=="")
                {
                    label4.Visible = true;
                    label4.Text = "请输入原密码";
                    return;
                }
                SqlDataReader reader = gm.reader("select * from dbo.UserInfo where qq='"+Form1.QQ+"'");
                if (reader.Read())
                {
                    if (ymm==reader["pwd"].ToString())
                    {
                        label2.Visible = true;
                        textBox2.Visible = true;
                        label3.Visible = true;
                        textBox3.Visible = true;
                    }
                    else
                    {
                        label4.Text = "密码不正确";
                        label4.Visible = true;
                    }
                }
            }
            else
            {
                if (textBox2.Text.Trim()=="")
                {
                    label5.Visible = true;
                    return;
                }
                if (textBox3.Text.Trim() == "")
                {
                    label6.Text = "请再次输入新密码";
                    label6.Visible = true;
                    return;
                }

                if (textBox2.Text.Trim() != textBox3.Text.Trim())
                {
                    label6.Text = "与新密码不一致";
                    label6.Visible = true;
                    return;
                }
                string xiugai = string.Format(@"update dbo.UserInfo
                                            set pwd='{0}'
                                            where  qq='{1}'", textBox3.Text.Trim(), Form1.QQ);
                int hs = gm.Zsg(xiugai);
                if (hs==1)
                {
                    MessageBox.Show("修改成功","提示");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改失败", "提示");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "关闭")
            {
                this.Close();
            }
            else
            {
                textBox1.Text = "";
                panel1.Visible = false;
                linkLabel1.Visible = true;
                button2.Text = "关闭";
                this.Text = "修改密码";
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label4.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label5.Visible = false;
            label6.Visible = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label6.Visible = false;
        }

        private void Modification_Load(object sender, EventArgs e)
        {
            SqlDataReader reader = gm.reader("select * from dbo.GetPwdInfo where gQq='" + Form1.QQ + "'");
            if (reader.Read())
            {
                label7.Text = reader["Question"].ToString();
            }
            reader.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Visible = true;
            linkLabel1.Visible = false;
            label4.Visible = false;
            textBox4.Text = "";
            label8.Visible = false;
            this.Text = "密保问题";
            button2.Text = "退回";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label8.Visible = false;
        }
    }
}
