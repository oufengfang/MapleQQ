using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 聊天项目界面;

namespace 聊天项目
{
    public partial class BackColor : Form
    {
        public BackColor()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

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

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Size = new Size(67, 85);
            panel1.Location = new Point(42, 23);
            panel1.BringToFront();
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panel1.Size = new Size(58, 78);
            panel1.Location = new Point(46, 27);
        }

        private void panel6_MouseEnter(object sender, EventArgs e)
        {
            panel6.Size = new Size(67, 85);
            panel6.Location = new Point(101, 23);
            panel6.BringToFront();
        }

        private void panel6_MouseLeave(object sender, EventArgs e)
        {
            panel6.Size = new Size(58, 78);
            panel6.Location = new Point(105, 27);
        }

        private void panel7_MouseEnter(object sender, EventArgs e)
        {
            panel7.Size = new Size(67, 85);
            panel7.Location = new Point(160, 23);
            panel7.BringToFront();
        }

        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            panel7.Size = new Size(58, 78);
            panel7.Location = new Point(164, 27);
        }

        private void panel8_MouseEnter(object sender, EventArgs e)
        {
            panel8.Size = new Size(67, 85);
            panel8.Location = new Point(221, 23);
            panel8.BringToFront();
        }

        private void panel8_MouseLeave(object sender, EventArgs e)
        {
            panel8.Size = new Size(58, 78);
            panel8.Location = new Point(225, 27);
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            panel2.Size = new Size(67, 85);
            panel2.Location = new Point(42, 107);
            panel2.BringToFront();
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            panel2.Size = new Size(58, 78);
            panel2.Location = new Point(46, 111);
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            panel3.Size = new Size(67, 85);
            panel3.Location = new Point(101, 107);
            panel3.BringToFront();
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            panel3.Size = new Size(58, 78);
            panel3.Location = new Point(105, 111);
        }

        private void panel5_MouseEnter(object sender, EventArgs e)
        {
            panel5.Size = new Size(67, 85);
            panel5.Location = new Point(160, 107);
            panel5.BringToFront();
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.Size = new Size(58, 78);
            panel5.Location = new Point(164, 111);
        }

        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            panel4.Size = new Size(67, 85);
            panel4.Location = new Point(221, 107);
            panel4.BringToFront();
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.Size = new Size(58, 78);
            panel4.Location = new Point(225, 111);
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.DarkGray;
            this.Close();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.Olive;
            this.Close();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.Lavender;
            this.Close();
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.DarkViolet;
            this.Close();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.Teal;
            this.Close();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.Firebrick;
            this.Close();
        }
        private void panel4_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.Orange;
            this.Close();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            Form1.cl = Color.Chartreuse;
            this.Close();
        }
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        private void BackColor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        private void BackColor_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;
            }
        }

        private void BackColor_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                leftFalg = false;//释放鼠标后标记为false
            }
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Red;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.DarkSlateGray;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Fangtupian t = new Fangtupian();
            Form1.TpLj = t.Tplj();
            this.Close();
        }
    }
}
