using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;
using System.Data.SqlClient;

namespace 服务端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;//当这个为假时，就可以一个线程访问另一个线程的控件
        }
        //定义回调:解决跨线程访问问题
         private delegate void SetTextValueCallBack(string strValue);
         //定义接收客户端发送消息的回调
         private delegate void ReceiveMsgCallBack(string strReceive);
        //声明回调
        private SetTextValueCallBack setCallBack;
         //声明
        private ReceiveMsgCallBack receiveCallBack;
         //定义回调：给ComboBox控件添加元素
        //private delegate void SetCmbCallBack(string strItem);
        //定义回调：给网格控件
        private delegate void SetCmbCallBack(List<Zhuanfa> mmm);
       //声明
       private SetCmbCallBack setCmbCallBack;
        //定义发送文件的回调
         private delegate void SendFileCallBack(byte[] bf);
        //声明
        private SendFileCallBack sendCallBack;
       
        //定义转发离线消息回调
        private ReceiveMsgCallBack zhangfalixian;
        //声明
       
       //用于通信的Socket
         Socket socketSend;
        //用于监听的SOCKET
        Socket socketWatch;

       //将远程连接的客户端的IP地址和Socket存入集合中
        Dictionary<string,Socket> dicSocket=new Dictionary<string,Socket>();
        List<Zhuanfa> arrlist = new List<Zhuanfa>();
        //讲要转发的离线消息和对象存入键值对集合
        List<COMF> Lixianzhuanfa = new List<COMF>();
       //创建监听连接的线程
        Thread AcceptSocketThread;
        //接收客户端发送消息的线程
         //Thread threadReceive;
        //转发消息的线程
        //Thread zhaunxianchen;
          /// <summary>
         /// 开始监听
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void btn_Start_Click(object sender, EventArgs e)
        {
            try
            {
                button2.Enabled = true;
                UDPjiantin();
                //当点击开始监听的时候 在服务器端创建一个负责监听IP地址和端口号的Socket
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取ip地址
                IPAddress ip = IPAddress.Parse(this.textBox1.Text.Trim());
                //创建端口号
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(this.textBox2.Text.Trim()));
                //绑定IP地址和端口号
                socketWatch.Bind(point);
                //开始监听:设置最大可以同时连接多少个请求
                socketWatch.Listen(60);

                //实例化回调
                setCallBack = new SetTextValueCallBack(SetTextValue);
                receiveCallBack = new ReceiveMsgCallBack(ReceiveMsg);
                setCmbCallBack = new SetCmbCallBack(AddCmbItem);//绑定soket
                sendCallBack = new SendFileCallBack(SendFile);
                //查昵称
                zhangfalixian = new ReceiveMsgCallBack(zhuangfa);
                //创建线程
                AcceptSocketThread = new Thread(new ParameterizedThreadStart(StartListen));
                AcceptSocketThread.IsBackground = true;
                AcceptSocketThread.Start(socketWatch);
                MessageBox.Show("服务器开启成功");
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                button2.Enabled = false;
                button1.Enabled = true;
                IsUdpcRecvStart = false;
            }
            
        }
            /// <summary>
         /// 等待客户端的连接，并且创建与之通信用的Socket
         /// </summary>
         /// <param name="obj"></param>
         Thread threadReceive;
         private void StartListen(object obj)
         {
             Socket socketWatch = obj as Socket;
             while (true)
            {
                try
                {
                    thrRecv = new Thread(ReceiveMessage);
                    thrRecv.Start();
                    //等待客户端的连接，并且创建一个用于通信的Socket
                    socketSend = socketWatch.Accept();
                    //获取远程主机的ip地址和端口号
                    string strIp = socketSend.RemoteEndPoint.ToString();
                    dicSocket.Add(strIp, socketSend);//把连接服务器的主机上的IP和Socket保存起来
                  
                    string strMsg = "远程主机：" + socketSend.RemoteEndPoint + "连接成功";
                    Invoke(zhangfalixian, ".");
                    string[] sf = message.Split(',');
                    message = "";
                    Zhuanfa jj = new Zhuanfa(int.Parse(sf[0]), int.Parse(sf[1]), strIp, socketSend, sf[3].ToString(), sf[2].ToString());
                    arrlist.Add(jj);
                    this.dataGridView1.Invoke(setCmbCallBack, arrlist);
                    //使用回调
                    richTextBox1.Invoke(setCallBack, strMsg);

                    //定义接收客户端消息的线程
                    threadReceive = new Thread(new ParameterizedThreadStart(Receive));
                    threadReceive.IsBackground = true;
                    threadReceive.Start(socketSend);
                }
                catch (Exception)
                {
                    Invoke(zhangfalixian, ".");
                }
             }
         }

        /// <summary>
        /// 服务器端不停的接收客户端发送的消息
        /// </summary>
         /// <param name="obj"></param>
         string strReceiveMsg = "";
         Thread mm;
         bool shaxin = false;
         private void Receive(object obj)
        {
             Socket socketSend = obj as Socket;
             while (true)
            {
                try
                {
                    //客户端连接成功后，服务器接收客户端发送的消息
                    byte[] buffer = new byte[2048*10000];
                    //实际接收到的有效字节数
                    int count = socketSend.Receive(buffer);
                     if (count == 0)//count 表示客户端关闭，要退出循环
                    {
                        break;
                    }
                    else
                    {
                        if (buffer[0]==1)
                        {
                            string str = Encoding.Default.GetString(buffer, 0, count);
                            strReceiveMsg += "\n" + str;
                            richTextBox2.Invoke(receiveCallBack, strReceiveMsg);
                        }
                        bool sk = false;
                        for (int i = 0; i < arrlist.Count; i++)
                        {
                            Zhuanfa zh=(Zhuanfa)arrlist[i];
                            if (socketSend.RemoteEndPoint.ToString() == zh.yonfuIP)
                            {
                                int jqq = zh.JQQ;
                                jsQQ = jqq;
                                for (int j = 0; j < arrlist.Count; j++)
                                {
                                    Zhuanfa js=(Zhuanfa)arrlist[j];
                                    if (jqq == js.FQQ)
                                    {
                                        sk = true;
                                        dicSocket[js.yonfuIP].Send(buffer, SocketFlags.None);
                                    }
                                }
                            }
                        }
                        if (sk==false)////离线转发客户端的线程(不停的转发消息使用定时器)
                        {   
                            if (jsQQ!=0)
                            {
                                COMF duixian = new COMF(jsQQ, buffer);
                                Lixianzhuanfa.Add(duixian);
                                Invoke(zhangfalixian, ".");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    thrRecv.Abort(socketSend.RemoteEndPoint.ToString());
                    dicSocket.Remove(socketSend.RemoteEndPoint.ToString());
                    for (int i = 0; i < arrlist.Count; i++)
                    {
               if (arrlist[i].yonfuIP == socketSend.RemoteEndPoint.ToString()){ arrlist.RemoveAt(i);
               this.richTextBox1.AppendText(socketSend.RemoteEndPoint.ToString() + "下线了，下线时间是：" + DateTime.Now + " \r \n");
                   Invoke(zhangfalixian, ".");
               shaxin = true;
                        }
                    }
                }
             }
        }
         //用来执行转发离线消息的定时器
       
         int  jsQQ = 0;
         //

         public void zhuangfa(string kk) //转发消息的程序
         {
             //通过key，找到 字典集合中对应的 与某个客户端通信的 套接字的 send方法，发送数据给对方
             try
             {
                 if (Lixianzhuanfa.Count>0)
                 {
                     for (int j = 0; j < Lixianzhuanfa.Count; j++)
                     {
                         if (dataGridView1.RowCount>0)
                         {
                             for (int i = 0; i < dataGridView1.RowCount; i++)
                             {
                                 if (dataGridView1.Rows[i].Cells["Column2"].Value.ToString() == Lixianzhuanfa[i].Jqq.ToString())
                                 {
                                     dicSocket[dataGridView1.Rows[i].Cells["Column3"].Value.ToString()].Send(Lixianzhuanfa[i].Neiron, SocketFlags.None);
                                     Lixianzhuanfa.RemoveAt(i);
                                 }
                             }
                         }  
                     }
                 }
        }
        catch (SocketException)
        {
            //COMF lm = new COMF();
       }
       catch (Exception)
       {
          
    }
 }
         /// <summary>
         /// 回调委托需要执行的方法
         /// </summary>
         /// <param name="strValue"></param>
         private void SetTextValue(string strValue)
        {
            this.richTextBox1.AppendText(strValue + " \r \n");
        }
        private void ReceiveMsg(string strMsg)
        {
            this.richTextBox2.AppendText(strMsg + " \r \n");
         }
 
         private void AddCmbItem(List<Zhuanfa> jkjkj)//把用户的IP昵称soket绑定到网格控件
         {
             try
             {
                 Invoke(zhangfalixian, ".");
                 BindingList<Zhuanfa> yonhulist = new BindingList<Zhuanfa>(jkjkj);
                 dataGridView1.DataSource = yonhulist;
             }
             catch (Exception)
             {
                 
             }
         }
 
         /// <summary>
         /// 服务器给客户端发送消息
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void btn_Send_Click(object sender, EventArgs e)
         {
             try
             {
                 if (richTextBox3.Text!="")
                 {
                     //原始的字符串转换成的字节数组 
                     string xiaoxi = "服务器：" + DateTime.Now + "\r\n" + richTextBox3.Text;
                     this.richTextBox2.AppendText(xiaoxi + " \r \n");
                     byte[] data = Encoding.Default.GetBytes(xiaoxi);
                     //在头部加上标记字节 
                     byte[] result = new byte[data.Length + 1];
                     //头部协议字节 1:代表字符串 
                     result[0] = 1;
                     Buffer.BlockCopy(data, 0, result, 1, data.Length);
                     dicSocket[dataGridView1.SelectedRows[0].Cells["Column3"].Value.ToString()].Send(result, 0, result.Length, SocketFlags.None);
                     richTextBox1.Text = "";
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
         }
         /// <summary>
         /// 选择要发送的文件
         /// </summary>
        /// <param name="sender"></param>
         /// <param name="e"></param>
        
         private void btn_Select_Click(object sender, EventArgs e)
         {
             OpenFileDialog dia = new OpenFileDialog();
            //设置初始目录
             dia.InitialDirectory = @"";
             dia.Title = "请选择要发送的文件";
             //过滤文件类型
             dia.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*|图片(*.jpg;*.png;*.gif;*.jpeg;*.bmp)|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
             dia.ShowDialog();
             //将选择的文件的全路径赋值给文本框t
             this.textBox5.Text = dia.FileName;
         }
 
         /// <summary>
         /// 发送文件
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
         private void btn_SendFile_Click(object sender, EventArgs e)
         {
             try
             {
                 //原始的字符串转换成的字节数组 
                 string fileName = System.IO.Path.GetFileName(textBox5.Text);
                 string fileExtension = System.IO.Path.GetExtension(textBox5.Text);

                 List<byte> list = new List<byte>();
                 //获取要发送的文件的路径
                 string strPath = this.textBox5.Text.Trim();
                 using (FileStream sw = new FileStream(strPath, FileMode.Open, FileAccess.Read))
                 {
                     byte[] buffer = new byte[2048 * 10000];
                     int r = sw.Read(buffer, 0, buffer.Length);
                     list.Add(2);
                     list.AddRange(buffer);
                 }
                 byte[] newBuffer = list.ToArray();
                 button4.Invoke(sendCallBack, newBuffer);
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
        }
        private void SendFile(byte[] sendBuffer)
        {
            try
             {
        dicSocket[dataGridView1.SelectedRows[0].Cells["Column3"].Value.ToString()].Send(sendBuffer, SocketFlags.None);
             }
             catch (Exception ex)
             {
                 MessageBox.Show("发送文件出错:"+ex.Message);
            }
         }
        /// <summary>
       /// 停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            /// 
        private void btn_StopListen_Click(object sender, EventArgs e)
        {
            try
            {
                button2.Enabled = false;
                button1.Enabled = true;
                thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
                IsUdpcRecvStart = false;
                udpcRecv.Close();
                socketWatch.Close();
                //终止线程
                AcceptSocketThread.Abort();
            }
            catch (Exception)
            {
              
            }
        }
        /// <summary>

        /// 用于UDP发送的网络服务类

        /// </summary>

        //private UdpClient udpcSend;

        /// <summary>

        /// 用于UDP接收的网络服务类

        /// </summary>

        private UdpClient udpcRecv;
        /// <summary>

        /// 开关：在监听UDP报文阶段为true，否则为false

        /// </summary>

        bool IsUdpcRecvStart = false;

        /// <summary>

        /// 线程：不断监听UDP报文

        /// </summary>

        Thread thrRecv;
        private void UDPjiantin()
        {

            if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            {

                IPEndPoint localIpep = new IPEndPoint(

                IPAddress.Parse("192.168.43.139"), 8848); // 本机IP和监听端口号

                udpcRecv = new UdpClient(localIpep);

                IsUdpcRecvStart = true;

                thrRecv = new Thread(ReceiveMessage);
                thrRecv.Start();
                MessageBox.Show("UDP监听启动");
            }
            else // 正在监听的情况，终止监听
            {

                thrRecv.Abort(); // 必须先关闭这个线程，否则会异常

                udpcRecv.Close();

                IsUdpcRecvStart = false;
                MessageBox.Show("UDP监听关闭");
            }

        }
        /// <summary>

        /// 按钮：接收数据开关

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        /// <summary>

        /// 接收数据

        /// </summary>

        /// <param name="obj"></param>
        string message = "";
        private void ReceiveMessage()
        {
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
                    message = Encoding.Unicode.GetString(
                    bytRecv, 0, bytRecv.Length);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }
        /// <summary>

        /// 关闭程序，强制退出

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        //服务端群发消息或文件
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != ""&&richTextBox3.Text!="")
            {
                MessageBox.Show("文字与文件只能选其一");
            }
            else
            {
                if (textBox5.Text != "")
                {
                    //原始的字符串转换成的字节数组 
                    string fileName = System.IO.Path.GetFileName(textBox5.Text);
                    string fileExtension = System.IO.Path.GetExtension(textBox5.Text);

                    List<byte> list = new List<byte>();
                    //获取要发送的文件的路径
                    string strPath = this.textBox5.Text.Trim();
                    using (FileStream sw = new FileStream(strPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[2048 * 10000];
                        int r = sw.Read(buffer, 0, buffer.Length);
                        list.Add(2);
                        list.AddRange(buffer);
                    }
                    byte[] newBuffer = list.ToArray();
                    foreach (Socket s in dicSocket.Values)
                    {
                        s.Send(newBuffer);
                    }
                    textBox5.Text = "";
                }
                else
                {
                    if (richTextBox3.Text != "")
                    {
                        string xiaoxi = "服务器：" + DateTime.Now + "\r\n" + richTextBox3.Text;
                        this.richTextBox2.AppendText(xiaoxi + " \r \n");
                        //将要发送的字符串 转成 utf8对应的字节数组
                        byte[] arrMsg = Encoding.Default.GetBytes(xiaoxi);
                        //在头部加上标记字节 
                        byte[] result = new byte[arrMsg.Length + 1];
                        //头部协议字节 1:代表字符串 
                        result[0] = 1;
                        Buffer.BlockCopy(arrMsg, 0, result, 1, arrMsg.Length);
                        foreach (Socket s in dicSocket.Values)
                        {
                            s.Send(result);
                        }
                        richTextBox3.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("请输入要发送的信息");
                    }
                }
                } 
        }
        //用于检测用户是否已掉线
        bool jch = false;
        string diaoxianIP = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
            //将要发送的字符串 转成 utf8对应的字节数组
           byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes("aini");
           byte[] result = new byte[arrMsg.Length + 1];
            //头部协议字节 1:代表字符串 
            result[0] = 6;
            Buffer.BlockCopy(result, 0, result, 1, arrMsg.Length);
            foreach (Socket s in dicSocket.Values)
            {
                diaoxianIP = dicSocket.Values.ToString();
                s.Send(result);
            }
            }
            catch (Exception)
            {
                jch = true;
                cha(diaoxianIP);
            }
        }
        public void cha(string lklk) 
        {
            if (jch)
            {
                dicSocket.Remove(lklk);
                for (int i = 0; i < arrlist.Count; i++)
                {
                    if (arrlist[i].yonfuIP== lklk) arrlist.RemoveAt(i);
                }
                this.dataGridView1.Invoke(setCmbCallBack, arrlist);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Invoke(setCmbCallBack, arrlist);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if ( shaxin==true)
            {
                this.button7_Click(sender,e);
                shaxin = false;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox1, "先所有客户端发送窗口震动");
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // 最小化
        }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buffer = new byte[20];
                buffer = Encoding.Default.GetBytes(DateTime.Now+ "\n服务器向你发送了一个窗口震动");
                byte[] result = new byte[buffer.Length + 1];
                //头部协议字节 1:代表字符串 
                result[0] = 3;
                Buffer.BlockCopy(buffer, 0, result, 1, buffer.Length);
                foreach (Socket s in dicSocket.Values)
                {
                    s.Send(result);
                }
            }
            catch (Exception)
            {

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Invoke(zhangfalixian, ".");
        }
    }

}

    

