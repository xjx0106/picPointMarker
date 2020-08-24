using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace picMark
{
    public partial class Form1 : Form
    {
        public bool isPointOneFinished = false;
        public bool isPointTwoFinished = false;
        public string[] fileList;
        public int x1 = 0;
        public int y1 = 0;
        public int x2 = 0;
        public int y2 = 0;
        public string picName;

        //===============================================================
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            freshBtnAddPoint();
        }

        //鼠标指针点击图片的事件
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(pictureBox1.Image != null) //有图片
            {
                int xCoordinate = e.X * pictureBox1.Image.Width / pictureBox1.Width;
                int yCoordinate = e.Y * pictureBox1.Image.Height / pictureBox1.Height;
                if (!isPointOneFinished)
                {
                    label2.Text = xCoordinate + " " + yCoordinate;
                    x1 = xCoordinate;
                    y1 = yCoordinate;
                    isPointOneFinished = true;
                    label5.Text = xCoordinate + "," + yCoordinate;
                    freshBtnAddPoint();
                }
                else if (isPointOneFinished)
                {
                    label4.Text = xCoordinate + " " + yCoordinate;
                    x2 = xCoordinate;
                    y2 = yCoordinate;
                    isPointOneFinished = false;
                    isPointTwoFinished = true;
                    freshBtnAddPoint();
                    //next.Enabled = true;
                    label5.Text = label5.Text + "," + xCoordinate + "," + yCoordinate + "," + label9.Text;
                    //cutPicture(listBox1.SelectedItem.ToString(),x1,y1,(x2-x1),(y2-y1));
                    if (x2 > x1 && y2 > y1)
                    {
                        cutPicture(listBox1.SelectedItem.ToString(), x1, y1, x2 - x1, y2 - y1);
                    }
                    else
                    {
                        reload_Click(e, e);
                        msg("你点击的两下不是先左上角后右下角");
                    }
                    if (checkBox1.Checked && x2 > x1 && y2 > y1)
                    {
                        addpoint_Click(e, e);
                        cutPicture(listBox1.SelectedItem.ToString(), x1, y1, x2 - x1, y2 - y1);

                    }
                }
            }
        }

        //点击重来按钮
        public void reload_Click(object sender, EventArgs e)
        {
            label2.Text = "-";
            label4.Text = "-";
            label5.Text = "-";
            pictureBox2.Image = null;
            isPointOneFinished = false;
            isPointTwoFinished = false;
            freshBtnAddPoint();
        }

        //点击打开文件夹按钮
        private void openfile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            label6.Text = "路径：" + path.SelectedPath;
            string[] sArray = label6.Text.Split('\\');
            //string[] sArray = Regex.Split(label6.Text, "后", RegexOptions.IgnoreCase);
            label7.Text = sArray[sArray.Length-1].ToString();
            switch(label7.Text) {
                case "下士":
                    label9.Text = "1";
                    break;
                case "一级军士长":
                    label9.Text = "2";
                    break;
                case "二级军士长":
                    label9.Text = "3";
                    break;
                case "三级军士长":
                    label9.Text = "4";
                    break;
                case "四级军士长":
                    label9.Text = "5";
                    break;
                case "学员":
                    label9.Text = "6";
                    break;
                case "列兵":
                    label9.Text = "7";
                    break;
                case "上等兵":
                    label9.Text = "8";
                    break;
                case "上将":
                    label9.Text = "9";
                    break;
                case "中将":
                    label9.Text = "10";
                    break;
                case "少将":
                    label9.Text = "11";
                    break;
                case "上士":
                    label9.Text = "12";
                    break;
                case "中士":
                    label9.Text = "13";
                    break;
                case "上尉":
                    label9.Text = "14";
                    break;
                case "中尉":
                    label9.Text = "15";
                    break;
                case "少尉":
                    label9.Text = "16";
                    break;
                case "中校":
                    label9.Text = "17";
                    break;
                case "大校":
                    label9.Text = "18";
                    break;
                case "上校":
                    label9.Text = "19";
                    break;
                case "少校":
                    label9.Text = "20";
                    break;
            }
            try
            {
                var files = Directory.GetFiles(path.SelectedPath, "*.jpg");
                DirectoryInfo folder = new DirectoryInfo(path.SelectedPath);
                listBox1.Items.Clear();
                listBox3.Items.Clear();
                foreach (FileInfo file in folder.GetFiles("*.jpg"))
                {
                    listBox1.Items.Add(file.FullName);
                    listBox3.Items.Add(file.Name);
                }
            }
            catch
            {
                msg("你没有选中文件夹");
            }
        }

        //点击添加点按钮
        private void addpoint_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(label5.Text); //添加进去

            this.reload_Click(e,e);
        }

        //刷新“添加点”按钮的状态
        public void freshBtnAddPoint()
        {
            if(isPointTwoFinished)
            {
                addpoint.Enabled = true;
                isPointTwoFinished = false;
            }
            else
            {
                addpoint.Enabled = false;
            }
        }

        //裁切图片
        public void cutPicture(String picPath, int x, int y, int width, int height)
        {
            //定义截取矩形
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(x, y, width, height);
            //要截取的区域大小
            //加载图片
            System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(picPath)));
            //判断超出的位置否
            if ((img.Width < x + width) || img.Height < y + height)
            {
                msg("裁剪尺寸超出原有尺寸！");
                img.Dispose();
                return;
            }
            //定义Bitmap对象
            System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);
            //进行裁剪
            System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            //保存成新文件
            //bmpCrop.Save(newPath);
            this.pictureBox2.Image = Image.FromHbitmap(bmpCrop.GetHbitmap());
            //释放对象
            img.Dispose();
            bmpCrop.Dispose();
        }

        //显示消息
        public void msg(string i)
        {
            MessageBox.Show(i);
        }

        //点击删除点按钮
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            }
            catch
            {
                msg("你没有选中要删除的点");
            }
        }

        //点击导出点按钮
        private void exportpoint_Click(object sender, EventArgs e)
        {
            var num = listBox2.Items.Count;
            //msg(num.ToString());
            string points = "";
            for(int i = 0;i < num; i++)
            {
                if (i == 0)
                {
                    points = listBox2.Items[i].ToString();
                } else
                {
                    points = points + " " + listBox2.Items[i];
                }
            }
            textBox1.Text = picName + " " + points;
            Clipboard.SetDataObject(textBox1.Text);
            textBox2.Text = textBox2.Text + "\r\n" + textBox1.Text;
            try
            {
                listBox3.SelectedIndex = listBox3.SelectedIndex + 1;
            }
            catch
            {
                msg("这个文件夹标完了!");
            }
        }

        //选择了文件名列表
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = listBox3.SelectedIndex;
            listBox1.SelectedIndex = selectIndex;
            picName = listBox3.SelectedItem.ToString();
            label13.Text = "文件名：" + picName;
            textBox1.Text = "";

            pictureBox1.Height = pictureBox1.Image.Height * pictureBox1.Width / pictureBox1.Image.Width;
        }

        //选择了文件路径列表
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = listBox1.SelectedIndex;
            listBox3.SelectedIndex = selectIndex;
            pictureBox1.Load(this.listBox1.SelectedItem.ToString());
            listBox2.Items.Clear();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (addpoint.Enabled != false)
            {
                addpoint_Click(e, e);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //textBox1.Visible = false;
            }
            else
            {
                textBox1.Visible = true;
            }
        }

        private void pass_Click(object sender, EventArgs e)
        {
            try
            {
                listBox3.SelectedIndex = listBox3.SelectedIndex + 1;
            }
            catch
            {
                msg("这个文件夹标完了!");
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //label17.Text = e.X + " " + e.Y;
        }
    }
  
}
