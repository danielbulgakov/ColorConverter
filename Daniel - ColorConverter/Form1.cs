using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Daniel___ColorConverter
{
    public partial class Form1 : Form
    {
        ColorConvert cc = new ColorConvert();
        Image Buff;
        (int,int) delta;

        int tB1 = 0, tB2 = 0, tB3 = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string imagePath;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "image files|*.jpg;*.png";
            dialog.Title = "Open an Image File.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = dialog.FileName;
                cc.rgbImage = new Bitmap(imagePath);
                this.pictureBox1.Image = cc.rgbImage;
                Buff = this.pictureBox1.Image;
                
            }
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "image files|*.jpg;*.png";
            dialog.Title = "Save an Image File.";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName != "" && this.pictureBox1.Image != null)
                {
                    System.IO.FileStream fs = (System.IO.FileStream)dialog.OpenFile();

                    switch (dialog.FilterIndex)
                    {
                        case 1:
                            this.pictureBox1.Image.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            this.pictureBox1.Image.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }

                    fs.Close();
                }
            }
            else
                MessageBox.Show("Can`t find an image to save", "Can`t save image", MessageBoxButtons.OK, MessageBoxIcon.Warning);


        }

        // Update stored values of trackBars
        private void tB_update()
        {
            tB1 = this.trackBar1.Value;
            tB2 = this.trackBar2.Value;
            tB3 = this.trackBar3.Value;
        }

        // Cancel latest step
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Buff;
            if (delta.Item2 == 1) trackBar1.Value = delta.Item1;
            else if (delta.Item2 == 2) trackBar2.Value = delta.Item1;
            else if (delta.Item2 == 3) trackBar3.Value = delta.Item1;
        }

        // Color Handler
        private void button1_Click(object sender, EventArgs e) 
        {
            Buff = this.pictureBox1 != null ? this.pictureBox1.Image : null;
            this.Cursor = Cursors.WaitCursor;
            int width = cc.rgbImage.Width;
            int height = cc.rgbImage.Height;
            cc.ConvertRGBimagetoHSV();
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    int tmpH = cc.hsvImage[i, j].h + (trackBar1.Value - tB1);
                    if (tmpH < 0)
                        cc.hsvImage[i, j].h = (ushort)(tmpH + 360);
                    else if (tmpH >= 360)
                        cc.hsvImage[i, j].h = (ushort)(tmpH - 360);
                    else
                        cc.hsvImage[i, j].h = (ushort)tmpH;
                }
            delta = (tB1, 1);
            tB_update();
            cc.ConvertHSVimagetoRGB();
            pictureBox1.Image = cc.rgbImage;
            this.Cursor = Cursors.Default;
        }

        // Saturation handler
        private void button2_Click(object sender, EventArgs e) 
        {
            Buff = this.pictureBox1 != null ? this.pictureBox1.Image : null;
            this.Cursor = Cursors.WaitCursor;
            cc.ConvertRGBimagetoHSV();
            int width = cc.rgbImage.Width;
            int height = cc.rgbImage.Height;

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    int tmpS = cc.hsvImage[i, j].s + (trackBar2.Value - tB2);
                    if (tmpS < 0) cc.hsvImage[i, j].s = 0;
                    else if (tmpS > 100) cc.hsvImage[i, j].s = 100;
                    else cc.hsvImage[i, j].s = (byte)tmpS;




                }
            delta = (tB2, 2);
            tB_update();
            cc.ConvertHSVimagetoRGB();
            pictureBox1.Image = cc.rgbImage;
            this.Cursor = Cursors.Default;

        }

        // Brightness handler
        private void button3_Click(object sender, EventArgs e) 
        {
            Buff = this.pictureBox1 != null ? this.pictureBox1.Image : null;
            this.Cursor = Cursors.WaitCursor;
            cc.ConvertRGBimagetoHSV();
            int width = cc.rgbImage.Width;
            int height = cc.rgbImage.Height;

            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                {
                    int tmpV = cc.hsvImage[i, j].v + (trackBar3.Value - tB3);
                    if (tmpV < 0) cc.hsvImage[i, j].v = 0;
                    else if (tmpV > 100) cc.hsvImage[i, j].v = 100;
                    else cc.hsvImage[i, j].v = (byte)tmpV;




                }
            delta = (tB3, 3);
            tB_update();
            cc.ConvertHSVimagetoRGB();
            pictureBox1.Image = cc.rgbImage;
            this.Cursor = Cursors.Default;

        }


    }
}
