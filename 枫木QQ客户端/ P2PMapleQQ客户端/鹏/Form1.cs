using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.Net;//IPAdress,IPEndPoint(ip和端口)类
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Data.SqlClient;
using 聊天项目界面;
using System.Runtime.InteropServices;
using P2PMapleQQ客户端;
using Login;
using 音频播放;
using P2PMapleQQ客户端;
namespace 聊天项目
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            
        }
        public static Color cl = Color.Lavender;//用来设置背景颜色
        GM gm = new GM();
        public static int QQ;//登录的QQ，本人QQ
        public static List<Class1> Hyqq = new List<Class1>();
        public static int Xxqq;//选中的好友QQ
        public static bool isok;
        public static string TpLj;//图片路径

        //声明API  
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_VER_POSITIVE = 0x0004;
        const int AW_VER_NEGATIVE = 0x0008;
        const int AW_CENTER = 0x0010;
        const int AW_HIDE = 0x10000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_SLIDE = 0x40000;
        const int AW_BLEND = 0x80000;

        #region 窗体颜色
        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.Red;
        }
        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
        }
        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.MediumTurquoise;
        }
        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.Transparent;
        }
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.BackColor = Color.DodgerBlue;
        }
        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.Transparent;
        }
        #endregion

        #region 窗体移动
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
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

        public int WQQ;
        //弹出更换主题窗体
        private void label3_Click(object sender, EventArgs e)
        {
            try
            {
                BackColor bc = new BackColor();
                bc.ShowDialog();
                BackColor = cl;
                listView1.BackColor = cl;
                if (File.Exists(TpLj))
                {
                    BackgroundImage = Image.FromFile(TpLj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        //更换头像功能
        private void 更换头像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Fangtupian Tp = new Fangtupian();
                string lujing = Tp.Tplj();
                if (File.Exists(lujing))//File.Exists判断文件是否存在
                {
                    string update = string.Format(@"update dbo.UserInfo
                                            set ImagePath='{0}'
                                            where  qq='{1}'", lujing, QQ);
                    int hs = gm.Zsg(update);
                    if (hs == 1)
                    {
                        pictureBox2.Image = Image.FromFile(lujing);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //修改本人资料
        private void 修改个人资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Amend a = new Amend();
            a.ShowDialog();
            txqm();
        }
        //登录成功狗显示头像名称个性签名在主窗体
        public void txqm()
        {
            try
            {
                SqlDataReader reader = gm.reader("select * from dbo.UserInfo where qq='" + QQ + "'");
                if (reader.Read())
                {
                    label4.Text = reader["nickname"].ToString();
                    WQQ = int.Parse(reader["qq"].ToString());
                    label5.Text = reader["sign"].ToString();
                    if (reader["ImagePath"].ToString() != "")
                    {
                        if (File.Exists(reader["ImagePath"].ToString()))
                        {
                            pictureBox2.Image = Image.FromFile(reader["ImagePath"].ToString());
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //联系人
        public void xshy()
        {
            try
            {
                Hyqq.Clear();
                listView1.Items.Clear();
                imageList1.Images.Clear();
                string sx = textBox1.Text.Trim();
                SqlDataReader reader = null;
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, @"^[0-9]*$"))
                {
                    reader = gm.reader(@"select * from dbo.Friends f,dbo.UserInfo u
                                where f.fqq=u.qq and f.qq=" + QQ + " and (f.fqq like '%" + sx + "%' or u.nickname like '%" + sx + "%')");
                }
                else
                {
                    reader = gm.reader(@"select * from dbo.Friends f,dbo.UserInfo u
                                where f.fqq=u.qq and f.qq='" + QQ + "' and u.nickname like '%" + sx + "%'");
                }
                int i = 0;
                while (reader.Read())
                {
                    if (File.Exists(reader["ImagePath"].ToString()))
                    {
                        imageList1.Images.Add(Image.FromFile(reader["ImagePath"].ToString()));
                    }
                    else
                    {
                        imageList1.Images.Add(Image.FromFile(@"D:\360安全浏览器下载\Login_副本.jpg"));
                    }
                    ListViewItem v = new ListViewItem();
                    v.Text = reader["nickname"].ToString() + "(" + reader["fqq"].ToString() + ")";
                    v.Tag = reader["fqq"];
                    v.SubItems.Add(reader["sign"].ToString());
                    v.ImageIndex = i;
                    listView1.Items.Add(v);
                    i++;

                    Class1 c = new Class1(reader["fqq"].ToString(), reader["nickname"].ToString());
                    Hyqq.Add(c);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txqm();
            xshy();
            AnimateWindow(this.Handle, 600, AW_HOR_POSITIVE | AW_ACTIVATE);//从下到上且不占其它程序焦点
        }
        //弹出查看好友信息窗体
        private void 查看信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
                {

                    isok = true;
                    Details d = new Details();
                    Xxqq = Convert.ToInt32(listView1.SelectedItems[0].Tag.ToString());
                    d.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请选择好友", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //点击头像显示本人信息
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            isok = false;
            Details d = new Details();
            d.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (label7.Visible == false)
            {
                label7.Visible = true;
            }
            else
            {
                label7.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Add_friends a = new Add_friends();
            a.ShowDialog();
            xshy();
        }

        private void 删除好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
                {
                    int hyqq = Convert.ToInt32(listView1.SelectedItems[0].Tag.ToString());
                    DialogResult r = MessageBox.Show("您确定要删除该好友", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (DialogResult.Yes == r)
                    {
                        int hs = gm.Zsg("delete Friends where qq='" + QQ + "' and fqq='" + hyqq + "'");
                        if (hs == 1)
                        {
                            MessageBox.Show("删除成功", "提示");
                            xshy();
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示");
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请选择好友", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label7.Visible = false;
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.BackColor = Color.LightGray;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.BackColor = Color.White;
        }
        private void label7_Click(object sender, EventArgs e)
        {
            Modification m = new Modification();
            m.ShowDialog();
            label7.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            xshy();
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(button1, "搜索");
            toolTip1.SetToolTip(pictureBox4, "主菜单");
            toolTip1.SetToolTip(pictureBox1, "添加好友");
            toolTip1.SetToolTip(pictureBox3, "音乐");
            toolTip1.SetToolTip(pictureBox2, "头像");
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            P2PMapleQQ客户端.Form1 ll = new P2PMapleQQ客户端.Form1();
            ll.WQQ = WQQ;
            ll.WName = label4.Text;
            ll.TQQ = int.Parse(listView1.SelectedItems[0].Tag.ToString());
            ll.TName = listView1.SelectedItems[0].Text.ToString();
            ll.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sql2 = @"UPDATE [MapleQQ].[dbo].[UserInfo]
                        SET [lstatus] = 0
                            WHERE qq='" + QQ+ "'";
            DbTools db = new DbTools();
            int h = db.zsg(sql2);
            Application.Exit();
        }
        #region 窗体特效
        [DllImport("User32.dll")]
        public static extern bool PtInRect(ref Rectangle Rects, Point lpPoint);
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                System.Drawing.Point cursorPoint = new Point(Cursor.Position.X, Cursor.Position.Y);//获取鼠标在屏幕的坐标点
                Rectangle Rects = new Rectangle(this.Left, this.Top, this.Left + this.Width, this.Top + this.Height);//存储当前窗体在屏幕的所在区域
                bool prInRect = PtInRect(ref Rects, cursorPoint);
                if (prInRect)
                {//当鼠标在当前窗体内
                    if (this.Top < 0)//窗体的Top属性小于0
                        this.Top = 0;
                    else if (this.Left < 0)//窗体的Left属性小于0
                        this.Left = 0;
                    else if (this.Right > Screen.PrimaryScreen.WorkingArea.Width)//窗体的Right属性大于屏幕宽度
                        this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                }
                else
                {
                    if (this.Top < 5)               //当窗体的上边框与屏幕的顶端的距离小于5时
                        this.Top = 5 - this.Height; //将窗体隐藏到屏幕的顶端
                    else if (this.Left < 5)         //当窗体的左边框与屏幕的左端的距离小于5时
                        this.Left = 5 - this.Width; //将窗体隐藏到屏幕的左端
                    else if (this.Right > Screen.PrimaryScreen.WorkingArea.Width - 5)//当窗体的右边框与屏幕的右端的距离小于5时
                        this.Left = Screen.PrimaryScreen.WorkingArea.Width - 5;//将窗体隐藏到屏幕的右端
           }
         
        }
        #endregion
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            音频播放.Form1 kk = new 音频播放.Form1();
            kk.QQ = QQ;
            kk.Show();
        }
        string lujin = "";
        public void textChange(string fileName)
        {
            lujin = fileName;
        }
        private void 自拍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Member_photo_collection paizao = new Member_photo_collection();
            paizao.Slave2MainDele += textChange;  //总之就是先把form2里的这个事件注册为form1里的内容
            paizao.ShowDialog(this);
            string update = string.Format(@"update dbo.UserInfo
                                            set ImagePath='{0}'
                                            where  qq='{1}'", lujin, QQ);
            int hs = gm.Zsg(update);
            if (hs == 1)
            {
                pictureBox2.Image = Image.FromFile(lujin);
            }
        }
    }
}
