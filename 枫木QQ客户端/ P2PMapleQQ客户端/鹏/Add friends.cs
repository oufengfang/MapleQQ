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
using System.Collections;
using System.IO;

namespace 聊天项目界面
{
    public partial class Add_friends : Form
    {
        public Add_friends()
        {
            InitializeComponent();
        }
        GM gm = new GM();
        public void Xstjhy()
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();
            List<Class1> list = new List<Class1>();
            string z = textBox1.Text.Trim();
            SqlDataReader reader = null;
            if (System.Text.RegularExpressions.Regex.IsMatch(z, @"^[0-9]*$"))
            {
                reader = gm.reader("select * from dbo.UserInfo where qq<>'" + Form1.QQ + "' and (nickname like '%" + z + "%' or qq like '%" + z + "%')");
            }
            else
            {
                reader = gm.reader("select * from dbo.UserInfo where qq<>'" + Form1.QQ + "' and nickname like '%" + z + "%'");
            }
            while (reader.Read())
            {
                Class1 c = new Class1(reader["qq"].ToString(), reader["nickname"].ToString() + "(" + reader["qq"].ToString() + ")", reader["ImagePath"].ToString(), reader["sign"].ToString());
                list.Add(c);
            }
            reader.Close();
            foreach (Class1 item in Form1.Hyqq)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (item.QQ == list[i].QQ)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            int s = 0;
            foreach (Class1 item in list)
            {
                if (File.Exists(item.Tuplj))
                {
                    imageList1.Images.Add(Image.FromFile(item.Tuplj));
                }
                else
                {
                    imageList1.Images.Add(Image.FromFile(@"D:\360安全浏览器下载\Login_副本.jpg"));
                }
                ListViewItem v = new ListViewItem();
                v.Text = item.Nicheng;
                v.Tag = item.QQ;
                v.SubItems.Add(item.Qm);
                v.ImageIndex = s;
                listView1.Items.Add(v);
                s++;
            }
        }
        private void Add_friends_Load(object sender, EventArgs e)
        {
            Xstjhy();
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Xstjhy();
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void 添加为好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
                {
                    Form1.Xxqq = Convert.ToInt32(listView1.SelectedItems[0].Tag);
                    string Tj = string.Format(@"insert into Friends(fqq,Fstatus,qq)
                                    values ('{0}',0,'{1}')", Form1.Xxqq, Form1.QQ);
                    int hs = gm.Zsg(Tj);
                    if (hs == 1)
                    {
                        MessageBox.Show("添加成功", "提示");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void 查看资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.isok = true;
            Details d = new Details();
            Form1.Xxqq = Convert.ToInt32(listView1.SelectedItems[0].Tag);
            d.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; 
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.MediumTurquoise;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.Transparent;
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.BackColor = Color.Red;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.Transparent;
        }
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        private void Add_friends_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        private void Add_friends_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;
            }
        }

        private void Add_friends_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                leftFalg = false;//释放鼠标后标记为false
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(button1, "搜索");
        }
    }
}
