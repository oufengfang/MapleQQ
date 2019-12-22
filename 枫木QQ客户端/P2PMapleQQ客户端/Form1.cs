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
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using 聊天项目;
using System.Data.SqlClient;

namespace P2PMapleQQ客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;//当这个为假时，就可以一个线程访问另一个线程的控件
        }
        public string WName;
        public int WQQ;
        public string TName;
        public int TQQ;
        #region 窗体界面设置
        #region 窗体移动
        System.Drawing.Point mouseOff;//鼠标移动的坐标
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
        #endregion
        #region 控件事件
        private void label2_MouseEnter(object sender, EventArgs e)//退出按钮之鼠标进入事件
        {
            label2.BackColor = Color.Red;
            label6.Visible = true;
        }

        private void label2_MouseLeave(object sender, EventArgs e)//退出按钮之鼠标移出事件
        {
            label6.Visible = false;
            label2.BackColor = Color.Transparent;
        }

        private void label2_Click(object sender, EventArgs e)//关闭窗体
        {
            this.Close();
        }

        private void label4_MouseEnter(object sender, EventArgs e)//最大化
        {
            label4.BackColor = Color.Gainsboro;
            label4.ForeColor = Color.Blue;
            label7.Visible = true;
        }

        private void label4_MouseLeave(object sender, EventArgs e)//最大化
        {
            label7.Visible = false;
            label4.ForeColor = Color.White;
            label4.BackColor = Color.Transparent;
        }

        private void label4_Click(object sender, EventArgs e)//最大化之点击
        {
            //this.WindowState = FormWindowState.Maximized;  // 最大化
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Blue;
            label3.BackColor = Color.Gainsboro;
            label8.Visible = true;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.White;
            label3.BackColor = Color.Transparent;
            label8.Visible = false;
        }

        private void label3_Click(object sender, EventArgs e)//最小化
        {
            this.WindowState = FormWindowState.Minimized;  // 最小化
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Blue;
            label5.BackColor = Color.Gainsboro;
            label9.Visible = true;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.White;
            label5.BackColor = Color.Transparent;
            label9.Visible = false;
        }

        private void label5_Click(object sender, EventArgs e)//窗口设置
        {

        }
        private void pictureBox11_MouseEnter_1(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(pictureBox11, "发送窗口震动");
            toolTip1.SetToolTip(pictureBox9, "字体设置");
            toolTip1.SetToolTip(pictureBox7, "发送图片");
            toolTip1.SetToolTip(pictureBox6, "发送文件");
            toolTip1.SetToolTip(pictureBox5, "屏幕截图");
            toolTip1.SetToolTip(pictureBox10, "查看聊天记录");
            toolTip1.SetToolTip(pictureBox1, "语音通话");
            toolTip1.SetToolTip(pictureBox2, "视频通话");
        }
        private void pictureBox9_Click(object sender, EventArgs e)//字体设置
        {
            FontDialog f = new FontDialog();
            f.ShowDialog();
            richTextBox1.SelectionFont = f.Font;// 设置选中的字体
            richTextBox2.SelectionFont = f.Font;// 设置选中的字体
            ColorDialog c = new ColorDialog();
            c.ShowDialog();
            richTextBox1.SelectionColor = c.Color;// 设置选中的颜色
            richTextBox2.SelectionColor = c.Color;// 设置选中的颜色
        }
        #endregion
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

        }
        #endregion
        #endregion
        #region 主功能区
        //定义回调
        private delegate void SetTextCallBack(string strValue);
        //声明
    
        //定义接收服务端发送消息的回调
        private delegate void ReceiveMsgCallBack(string strMsg);
     
        //创建连接的Socket
        Socket socketSend;
        //创建接收客户端发送消息的线程
        Thread threadReceive;

        /// 用于UDP发送的网络服务类

        /// </summary>

        private UdpClient udpcSend;

        /// <summary>

        /// 用于UDP接收的网络服务类

        /// </summary>

        private IPEndPoint clientIPEndPoint;
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)//窗体加载
        {
            try
            {
                label1.Text = TName;
                //    //获取主屏
                //Screen s = Screen.PrimaryScreen;
                ////创建一个位图,将其大小设置为何屏幕大小一致,为了获取屏幕的图片
                //Bitmap bit = new Bitmap(s.Bounds.Width,s.Bounds.Height);
                ////利用当前bit获取一个画布,画布已经于Graphics对象关联
                //Graphics g = Graphics.FromImage(bit); 
                ////将屏幕的(0,0)坐标截图内容copy到画布的(0,0)位置,尺寸到校 bit.size;
                //g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bit.Size);
                ////将位图保存到D盘
                //bit.Save("D:\\123.jpg"); 
                ////释放位图资源
                //bit.Dispose(); 
                ////释放画布
                //g.Dispose();

                // 匿名发送
                udpcSend = new UdpClient(0);
                // 启动发送线程
                Thread sendThread = new Thread(SendMessage);
                sendThread.Start(string.Format("{0},{1},{2},{3}", WQQ, TQQ, WName, TName));//填写相关信

                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse("192.168.43.139");
                socketSend.Connect(ip, 8848);

                //开启一个新的线程不停的接收服务器发送消息的线程
                threadReceive = new Thread(new ThreadStart(Receive));
                //设置为后台线程
                threadReceive.IsBackground = true;
                threadReceive.Start();
                //向服务器发送账号信息
                string bendiIP = "192.168.43.139";
                IPAddress ip2 = IPAddress.Parse(bendiIP);
                clientIPEndPoint = new IPEndPoint(ip2, 1234);
            }
            catch (Exception)
            {
                MessageBox.Show("服务器未开启，请稍后再连接");
            }
        }
        /// <summary>
        /// 接口服务器发送的消息
        /// </summary>
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[2048 * 10000];
                    //实际接收到的字节数
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    else
                    {
                        System.Media.SystemSounds.Exclamation.Play();

                        if (buffer[0] == 1)//表示发送的是文字消息
                        {
                            string str = Encoding.Default.GetString(buffer, 1, r - 1);
                            tianjiajilu(str);
                            if (richTextBox1.Text == "")
                            {
                                richTextBox1.Text = TName + " " + DateTime.Now + "\n" + str;
                            }
                            else
                            {
                                richTextBox1.Text = richTextBox1.Text + "\n" +TName + " " + DateTime.Now + "\n"+str;
                            }
                            //判断发送的数据的类型
                        }
                        else if (buffer[0] == 2)
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.InitialDirectory = @"";
                            sfd.Title = "请选择要保存的文件";
                            sfd.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*|图片(*.jpg;*.png;*.gif;*.jpeg;*.bmp)|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
                            sfd.ShowDialog(this);

                            string strPath = sfd.FileName;
                            using (FileStream fsWrite = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                fsWrite.Write(buffer, 1, r - 1);
                            }

                            MessageBox.Show("保存文件成功");
                        }
                        else if (buffer[0] == 3)
                        {
                            string str = Encoding.Default.GetString(buffer, 1, r - 1);
                            if (richTextBox1.Text == "")
                            {
                                richTextBox1.Text = str;
                            }
                            else
                            {
                                richTextBox1.Text += "\n" + str;
                            }
                            this.chuankouzhendon();
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("接收服务端发送的消息出错");
            }
        }
        /// <summary>
        /// 客户端给客户端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox2.Text!="")
                {
                    string strMsg = this.richTextBox2.Text.Trim();
                    richTextBox2.Text = "";
                    byte[] buffer = new byte[2048];
                    buffer = Encoding.Default.GetBytes(strMsg);
                    byte[] result = new byte[buffer.Length + 1];
                    //头部协议字节 1:代表字符串 
                    result[0] = 1;
                    Buffer.BlockCopy(buffer, 0, result, 1, buffer.Length);
                    int receive = socketSend.Send(result);
                    if (richTextBox1.Text == "")
                    {
                        richTextBox1.Text = WName + " " + DateTime.Now + "\n" + strMsg;
                    }
                    else
                    {
                        richTextBox1.Text = richTextBox1.Text + "\n" + WName + " " + DateTime.Now + "\n" + strMsg;
                    }
                    tianjiajilu(strMsg); 
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送消息出错:" + ex.Message);
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //关闭socket
                this.Close();
            }
            catch (Exception)
            {
                
            }
            
        }

        ///使用UDP给服务器发送信息

        /// </summary>

        /// <param name="obj"></param>

        private void SendMessage(object obj)
        {
            string message = (string)obj;
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            IPAddress remoteIp = IPAddress.Parse("192.168.43.139");
            IPEndPoint remoteIPEndPoint = new IPEndPoint(remoteIp, 8848);
            udpcSend.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
            udpcSend.Close();
        }
        private void pictureBox6_Click(object sender, EventArgs e)//文件传输
        {
            try
            {
                string wenjianluj = Wenjiancaozuo.lujin();
                //原始的字符串转换成的字节数组 
                string fileName = System.IO.Path.GetFileName(wenjianluj);
                string fileExtension = System.IO.Path.GetExtension(wenjianluj);

                byte[] buffe = new byte[2048];
                buffe = Encoding.Default.GetBytes("向你发送了一个文件：" + fileName);
                byte[] result = new byte[buffe.Length + 1];
                //头部协议字节 1:代表字符串 
                result[0] = 1;
                Buffer.BlockCopy(buffe, 0, result, 1, buffe.Length);
                socketSend.Send(result);
                Thread.Sleep(1000);

                List<byte> list = new List<byte>();
                //获取要发送的文件的路径
                string strPath = wenjianluj.Trim();
                long lSize = new FileInfo(strPath).Length;
                MessageBox.Show(lSize.ToString());
                using (FileStream sw = new FileStream(strPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[2048 * 10000];
                    int r = sw.Read(buffer, 0, buffer.Length);
                    list.Add(2);
                    list.AddRange(buffer);
                }
                byte[] newBuffer = list.ToArray();
                socketSend.Send(newBuffer);
                if (richTextBox1.Text == "")
                {
                    richTextBox1.Text = WName + DateTime.Now + "\n文件：" + fileName;
                }
                else
                {
                    richTextBox1.Text = richTextBox1.Text + "\n" + WName + DateTime.Now + "\n文件：" + fileName;
                }
                tianjiajilu("文件：" + fileName);
                MessageBox.Show("文件发送成功");
            }
            catch (Exception)
            {

            }
        }
        private void pictureBox5_Click(object sender, EventArgs e)//屏幕截图
        {

        }
        public void chuankouzhendon()//窗口震动
        {
            //下面是核心的代码
            //首先我们来设置一下抖动的幅度
            int rand = 15;//这里我设置5，大小大家自己在定

            int frmx = this.Left;//获取一下窗体左上角X的坐标

            int frmy = this.Top;//获取一下窗体左上角Y的坐标

            //实例化随机数对象
            Random random = new Random();

            for (int i = 0; i < 600; i += 5)
            {
                //我们用循环来控制一下窗体抖动的时间

                //产生2个随机数，控制窗体坐标震动的幅度

                int x = random.Next(rand);
                int y = random.Next(rand);

                //我们用除2取余等于0是来控制抖动
                if (x % 2 == 0)
                {
                    this.Left = this.Left + x;
                }
                else
                {
                    //如果不等于0.则减8像素
                    this.Left = this.Left - x;
                }
                //再来控制一下Y坐标
                if (y % 2 == 0)
                {
                    this.Top = this.Top + y;

                }
                else
                {
                    this.Top = this.Top - y;
                }

                //重新来还原坐标
                this.Left = frmx;
                this.Top = frmy;
                //好了，我们来编译一下，来看看效果如何！
            }
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buffer = new byte[20];
                buffer = Encoding.Default.GetBytes(DateTime.Now + "\n" + WName + "向你发送了一个窗口震动");
                byte[] result = new byte[buffer.Length + 1];
                //头部协议字节 1:代表字符串 
                result[0] = 3;
                Buffer.BlockCopy(buffer, 0, result, 1, buffer.Length);
                int receive = socketSend.Send(result);
                if (richTextBox1.Text == "")
                    richTextBox1.Text = "你向" + label1.Text + "发送了一个窗口震动";
                else richTextBox1.Text = richTextBox1.Text+DateTime.Now + "\n你向" + label1.Text + "发送了一个窗口震动";
                chuankouzhendon();
            }
            catch (Exception)
            {

            }
        }
        #endregion
        string lujin = "";
        public void textChange(string fileName)
        {
            lujin = fileName;
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            try
            {
                List<byte> list = new List<byte>();
                Member_photo_collection paizao = new Member_photo_collection();
                paizao.Slave2MainDele += textChange;  //总之就是先把form2里的这个事件注册为form1里的内容
                paizao.ShowDialog(this);

                string fileName = System.IO.Path.GetFileName(lujin);

                byte[] buffe = new byte[2048];
                buffe = Encoding.Default.GetBytes("向你发送了一张图片：" + fileName);
                byte[] result = new byte[buffe.Length + 1];
                //头部协议字节 1:代表字符串 
                result[0] = 1;
                Buffer.BlockCopy(buffe, 0, result, 1, buffe.Length);
                socketSend.Send(result);
                Thread.Sleep(1000);

                using (FileStream sw = new FileStream(lujin, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[2048 * 1200];
                    int r = sw.Read(buffer, 0, buffer.Length);
                    list.Add(2);
                    list.AddRange(buffer);
                }
                byte[] newBuffer = list.ToArray();
                socketSend.Send(newBuffer);
                if (richTextBox1.Text == "")
                {
                    richTextBox1.Text = WName + DateTime.Now + "\n图片：";
                }
                else
                {
                    richTextBox1.Text = richTextBox1.Text + "\n" + WName + DateTime.Now + "\n";
                }
                tianjiajilu("图片："+fileName);
                MessageBox.Show("图片发送成功");
                lujin = "";
            }
            catch (Exception)
            {
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now scroll it automatically
        }
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
             System.Diagnostics.Process.Start(e.LinkText);  
            //在控件LinkClicked事件中编写如下代码实现内容中的网址单击后可以访问网址
        }
        public void tianjiajilu(string jilu) //往数据库中添加记录
        {
            string sql = string.Format(@"INSERT INTO [MapleQQ].[dbo].[Text]
                           ([tcontext])
                             VALUES('{0}')",jilu);
            GM kk = new GM();
            if (kk.Zsg(sql) > 0) 
            {
                sql = @"SELECT IDENT_CURRENT('Text')";
               SqlDataReader reader=kk.reader(sql);
               if (reader.Read())
               {
                  sql = string.Format(@"INSERT INTO [MapleQQ].[dbo].[ChatInfo]
                                   ([csendqq]
                                   ,[creceiveqq]
                                   ,[cdate]
                                   ,[tno])
                             VALUES ('{0}','{1}','{2}','{3}')", WQQ, TQQ, DateTime.Now.ToString(), int.Parse(reader[0].ToString()));
                  kk.Zsg(sql);
               }
               reader.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //关闭socket
                socketSend.Close();
            }
            catch (Exception)
            {
                
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)//消息记录
        {
            jilu l = new jilu();
            l.WQQ = WQQ;
            l.WName = WName;
            l.TQQ = TQQ;
            l.TName = TName;
            l.Show();
        }
    }
}
       


