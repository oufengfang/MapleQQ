using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using P2PMapleQQ客户端;
namespace Login
{
    public partial class Registered : Form
    {
        public Registered()
        {
            InitializeComponent();

        }
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中

        DbTools Db = new DbTools();
        //注册
        public void Cler()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }
        int QQ = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            QQ = r.Next(100000000, 1000000000);//随机获取账号
            string sex = null;
            string nichen = textBox1.Text;
            string Pwd = textBox2.Text;
            string qrPwd = textBox3.Text;
            string sgin = textBox4.Text.ToString();
            string brithday1 = dateTimePicker1.Value.ToShortTimeString();
            if (radioButton1.Checked)
            {
                sex = "男";
            }
            else
            {
                sex = "女";
            }
            if (nichen == "" || Pwd == "" || qrPwd == "")
            {
                MessageBox.Show("请填写完整的信息");
            }
            else if (Pwd == qrPwd)
            {
                string Address = textBox7.Text.ToString();
                string sql = string.Format(@"insert into UserInfo(qq, pwd, sign, ImagePath, nickname, sex, birthday,address,lstatus)
               values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8})", QQ, Pwd, sgin, null, nichen, sex, brithday1, Address, 0);
                int i = Db.zsg(sql);
                if (i == 1)
                {
                    string sql2 = string.Format(@"INSERT INTO [MapleQQ].[dbo].[GetPwdInfo]
                     ([Question]
                      ,[Answer]
                     ,[gQq])
                     VALUES
                     ('{0}'
                     ,'{1}',{2})", textBox5.Text.ToString(), textBox6.Text.ToString(), QQ);
                    int we = Db.zsg(sql2);
                    if (we == 1)
                    {
                        MessageBox.Show("注册成功，您的号码是：" + QQ);
                        Login.QQ = QQ;
                        Login.Pwd = textBox3.Text;
                        Cler();
                        this.Close();
                    }
                }
            }
        }
        //自定义窗体事件
        #region

        private void Registered_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }

        }
        private void Registered_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;
            }
        }
        private void Registered_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                leftFalg = false;//释放鼠标后标记为false
            }
        }
        #endregion

        private void label11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //窗体最小化事件
        private void label12_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //获取图片
        //using (OpenFileDialog lvse = new OpenFileDialog())
        //{
        //    lvse.Title = "选择图片";
        //    lvse.InitialDirectory = "";
        //    lvse.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
        //    lvse.FilterIndex = 1;

        //    if (lvse.ShowDialog() == DialogResult.OK)
        //    {
        //        pictureBox3.Image = Image.FromFile(lvse.FileName);
        //    }
        //}

        //第二种方法
        // openFileDialog1.ShowDialog();//显示打开文件对话框
        //string ImgPath = openFileDialog1.FileName;//得到文件路径
        //panel2.BackgroundImage = ImgPath;//显示图片
        ////panel2.BackgroundImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/xxx.jpg");
        private void panel2_Click(object sender, EventArgs e)
        {
           using (OpenFileDialog lvse = new OpenFileDialog())
            //{
            //    lvse.Title = "选择图片";
            //    lvse.InitialDirectory = "";
            //    lvse.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            //    lvse.FilterIndex = 1;

                if (lvse.ShowDialog() == DialogResult.OK)
                {
                    string ImgPath = openFileDialog1.FileName;//得到文件路径
                    panel2.BackgroundImage = Image.FromFile(lvse.FileName);
                    string update = string.Format(@"update dbo.UserInfo
                                            set ImagePath='{0}'
                                            where  qq='{1}'", lvse.FileName, QQ);
                    int hs = Db.zsg(update);
                }

            }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        string lujin = "";
        public void textChange(string fileName)
        {
            lujin = fileName;
        }
        private void 拍照ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Member_photo_collection paizao = new Member_photo_collection();
            paizao.Slave2MainDele += textChange;  //总之就是先把form2里的这个事件注册为form1里的内容
            paizao.ShowDialog(this);
            string update = string.Format(@"update dbo.UserInfo
                                            set ImagePath='{0}'
                                            where  qq='{1}'", lujin, QQ);
            int hs = Db.zsg(update);
            if (hs == 1)
            {
                panel2.BackgroundImage = Image.FromFile(lujin);
            }
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            toolTip1.SetToolTip(panel2, "右击选择照片");
        }
        }
    }
