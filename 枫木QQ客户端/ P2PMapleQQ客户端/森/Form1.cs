using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Media;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace 音频播放
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int QQ;
        #region//执行条件，存储路径
        int i = 0;
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        const uint WM_APPCOMMAND = 0x319;
        const uint APPCOMMAND_VOLUME_UP = 0x0a;
        const uint APPCOMMAND_VOLUME_DOWN = 0x09;
        const uint APPCOMMAND_VOLUME_MUTE = 0x08;
        //OpenFileDialog open = new OpenFileDialog();     //实例化一个通用对话框
        Dictionary<string, string> listsongs = new Dictionary<string, string>(); //用来存储音乐文件的全路径
        #endregion
        #region//双击播放
        public void play()
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            timer2.Enabled = true;
            if (i >= 3 || i < 0)
            {
                i = 0;
            }
            this.pictureBox1.Image = imageList1.Images[i];
            i++;
            timer1.Enabled = true;
            if (listBox1.SelectedIndex != -1)
            {
                axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                label2.Text = listBox1.SelectedItem.ToString();
            }

            int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引
            index++;
            if (index == listBox1.Items.Count)
            {
                index = 0;
            }
            listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
            label2.Text = listBox1.SelectedItem.ToString();
            axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
        }
        public void play2()
        {
            timer2.Enabled = true;
            if (i >= 3 || i < 0)
            {
                i = 0;
            }
            this.pictureBox1.Image = imageList1.Images[i];
            i++;
            timer1.Enabled = true;
            if (listBox2.SelectedIndex != -1)
            {
                axWindowsMediaPlayer1.URL = listsongs[listBox2.SelectedItem.ToString()];
                label2.Text = listBox2.SelectedItem.ToString();
            }

        }
        #endregion
        #region//listbox2控件下一首
        public void Slistbox1()
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int index = listBox2.SelectedIndex; //获得当前选中歌曲的索引
                index++;
                if (comboBox1.Text == "单曲循环")
                {
                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                    axWindowsMediaPlayer1.URL = listsongs[listBox2.SelectedItem.ToString()];
                    return;
                }
                else if (comboBox1.Text == "顺序播放")
                {
                    if (index == listBox2.Items.Count)
                    {
                        index = 0;
                    }
                    listBox2.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    //sp.SoundLocation = listsongs[index];
                    label2.Text = listBox2.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                    return;
                }
                if (index == listBox2.Items.Count)
                {
                    index = 0;
                }
                listBox2.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                //sp.SoundLocation = listsongs[index];
                label2.Text = listBox2.SelectedItem.ToString();
                axWindowsMediaPlayer1.URL = listsongs[listBox2.SelectedItem.ToString()];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region//listbox2控件上一首
        public void Slistbox2()
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int index = listBox2.SelectedIndex; //获得当前选中歌曲的索引
                if (index == 0)
                {
                    return;
                }
                index--;
                if (comboBox1.Text == "单曲循环")
                {
                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                    axWindowsMediaPlayer1.URL = listsongs[listBox2.SelectedItem.ToString()];
                    return;
                }
                else if (comboBox1.Text == "顺序播放")
                {
                    if (index == listBox2.Items.Count)
                    {
                        index = 0;
                    }
                    listBox2.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    //sp.SoundLocation = listsongs[index];
                    label2.Text = listBox2.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox2.SelectedItem.ToString()];
                    return;
                }
                if (index == listBox2.Items.Count)
                {
                    index = 0;
                }
                listBox2.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                //sp.SoundLocation = listsongs[index];
                label2.Text = listBox2.SelectedItem.ToString();
                axWindowsMediaPlayer1.URL = listsongs[listBox2.SelectedItem.ToString()];
            }
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo folder = new DirectoryInfo(@"D:\360安全浏览器下载");//本地计算机音乐路径
                foreach (FileInfo file in folder.GetFiles("*.mp3"))
                {
                    listsongs.Add(file.Name, file.FullName);
                    listBox1.Items.Add(file.Name);  //将音乐文件的文件名加载到listBox中
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer2.Enabled = false;
//            string sql = string.Format(@"SELECT [GequName]
//      ,[GequPath]
//  FROM [MapleQQ].[dbo].[LikeGequ]
//  where GQQ={0}", QQ);
//            while (true)
//            {
                
//            }
        }
        #region//定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (i >= 6 || i < 0)
                //{
                //    i = 0;
                //}
                //this.pictureBox1.Image = imageList1.Images[i];
                //i++;
                progressBar1.Maximum = (int)axWindowsMediaPlayer1.currentMedia.duration;
                progressBar1.Minimum = 0;
                progressBar1.Step = (int)(progressBar1.Maximum / 20);
                progressBar1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                textBox1.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (i >= 3 || i < 0)
            {
                i = 0;
            }
            this.pictureBox1.Image = imageList1.Images[i];
            i++;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //播放键
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            timer2.Enabled = true;
            if (listBox1.SelectedIndex >= 0)
            {
                //axWindowsMediaPlayer1.URL=listsongs[listBox1.SelectedItem.ToString()];
                axWindowsMediaPlayer1.Ctlcontrols.play();
                timer1.Enabled = true;
            }
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.setMode("loop", true);//让歌曲循环播放
        }
        private void listBox1_DoubleClick_1(object sender, EventArgs e)//歌曲列表，双击歌名执行代码
        {
            play();

        }
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            play2();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //暂停键
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            timer1.Enabled = false;
            timer2.Enabled = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int a = tabControl1.SelectedIndex;
                if (a == 0)
                {
                    int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引
                    index++;
                    if (comboBox1.Text == "单曲循环")
                    {
                        axWindowsMediaPlayer1.settings.setMode("loop", true);
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        return;
                    }
                    else if (comboBox1.Text == "顺序播放")
                    {
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        label2.Text = listBox1.SelectedItem.ToString();
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        return;
                    }

                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    label2.Text = listBox1.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                }
                else if (tabPage2.Text == "我收藏的歌曲")
                {
                    Slistbox1();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int b = tabControl1.SelectedIndex;
                if (b == 0)
                {

                    int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引
                    if (index == 0)
                    {
                        return;
                    }
                    index--;

                    if (comboBox1.Text == "单曲循环")
                    {
                        axWindowsMediaPlayer1.settings.setMode("loop", true);
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        return;
                    }
                    else if (comboBox1.Text == "顺序播放")
                    {
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        //sp.SoundLocation = listsongs[index];
                        label2.Text = listBox1.SelectedItem.ToString();
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        return;
                    }
                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    //sp.SoundLocation = listsongs[index];
                    label2.Text = listBox1.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                }
                else if (tabPage2.Text == "我收藏的歌曲")
                {
                    Slistbox2();
                }

            }
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //加音量
            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //減音量
            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000);

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (tabControl1.SelectedTab.Text.ToString() == "所有歌曲")
            {
                收藏ToolStripMenuItem.Visible = true;
                移除收藏ToolStripMenuItem.Visible = false;
            }
            if (tabControl1.SelectedTab.Text.ToString() == "我收藏的歌曲")
            {
                收藏ToolStripMenuItem.Visible = false;
                移除收藏ToolStripMenuItem.Visible = true;
            }
        }
        Like like = new Like();
        private void 收藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 移除收藏ToolStripMenuItem.Text = 收藏ToolStripMenuItem.Text;
                string music1 = listBox1.SelectedItem.ToString();
                int index = listBox1.SelectedIndex;
                string sql2 = @"select COUNT(*) from LikeGequ
                    where GQQ='" + QQ + "' and GequName='" + music1 + "'";
                int number2 = like.DB(sql2);
                if (number2 == 1)
                {
                    MessageBox.Show("已收藏，无需重复");
                    return;
                }

                string sql = string.Format(@"INSERT INTO [MapleQQ].[dbo].[LikeGequ]
                                               ([GQQ]
                                               ,[GequName]
                                               ,[GequPath])
                                         VALUES('" + QQ + "','" + music1 + "','" + index + "')");
                int number = like.DIDUpdata(sql);
                if (number == 1)
                {
                    MessageBox.Show("收藏成功");

                    while (listBox1.SelectedItems.Count > 0)
                    {
                        listBox2.Items.Add(listBox1.SelectedItems[0]);
                        //listBox1.Items.Remove(listBox1.SelectedItems[0]);
                        return;
                    }
                    //return;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 移除收藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text.ToString() == "我收藏的歌曲")
            {
                while (listBox1.SelectedItems.Count > 0)
                {
                    listBox2.Items.Remove(listBox2.SelectedItems[0]);
                    string music1 = listBox1.SelectedItem.ToString();
                    int index = listBox1.SelectedIndex;
                    string sql = @"DELETE FROM [MapleQQ].[dbo].[LikeGequ]
                            WHERE GQQ='" + QQ + "' and GequName='" + music1 + "'";
                    int number3 = like.DIDUpdata(sql);
                    if (number3 == 1)
                    {
                        MessageBox.Show("移除成功");
                    }
                    return;
                }

            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

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
    }
}
