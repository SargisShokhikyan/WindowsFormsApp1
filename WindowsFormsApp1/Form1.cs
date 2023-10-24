using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

//using GrapeCity.Documents.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public String filelocation = "";
        public Form1()
        {
            InitializeComponent();
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            try { 
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(.*jpg)|*.jpg| PNG files(.*png)|*.png| All Files(*.*)|*.*";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) { 
                    filelocation = dialog.FileName;
                    pictureBox1.ImageLocation = filelocation;

                    checkBox1.Enabled = true;
                    checkBox2.Enabled = true;
                    checkBox3.Enabled = true;
                }
            } catch(Exception) {
                MessageBox.Show("error");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_click(object sender, EventArgs e)
        {
            string var = textBox1.Text;
            string[] arr = var.Split(',');
            pictureBox1.Size = new Size(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));
        }

        private void Effect1_chaked(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                //pictureBox1.Image = ConvertGrayscale();
                pictureBox1.Size = new Size(pictureBox1.Size.Width+100, pictureBox1.Size.Height + 100);
                pictureBox1.Image = BlurEffect(5);
            }

            /*if (checkBox2.Checked == true)
            {
                pictureBox1.Image = BlurEffect(10);
            }*/

            if (checkBox1.Checked == false) {
                pictureBox1.Size = new Size(pictureBox1.Size.Width - 100, pictureBox1.Size.Height - 100);
                pictureBox1.ImageLocation = filelocation;
            }

            /*if (checkBox1.Checked == false && checkBox2.Checked == false) {
                pictureBox1.ImageLocation = filelocation;
            }*/


        }

        public Bitmap ConvertGrayscale()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    int ser = (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B) / 3;
                    bitmap.SetPixel(i, j, Color.FromArgb(ser, ser, ser));
                }
            }
            return bitmap;
        }

        private void Effect2_chaked(object sender, EventArgs e)
        {
            /*if (checkBox1.Checked == true)
            {
                pictureBox1.Image = ConvertGrayscale();
            }
            if (checkBox2.Checked == true)
            {
                pictureBox1.Image = BlurEffect(10);
            } 
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                pictureBox1.ImageLocation = filelocation;
            }*/
            if (checkBox2.Checked == true)
            {
                pictureBox1.Size = new Size(pictureBox1.Size.Width + 100, pictureBox1.Size.Height + 100);
            }
            if (checkBox2.Checked == false)
            {
                pictureBox1.Size = new Size(pictureBox1.Size.Width - 100, pictureBox1.Size.Height - 100);
                //pictureBox1.ImageLocation = filelocation;
            }

        }
        
        private void effect3_chaked(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                pictureBox1.Size = new Size(pictureBox1.Size.Width + 150, pictureBox1.Size.Height + 150);
                pictureBox1.Image = ConvertGrayscale();
                pictureBox1.Image = BlurEffect(10);
            }
            if (checkBox3.Checked == false)
            {
                pictureBox1.Size = new Size(pictureBox1.Size.Width - 150, pictureBox1.Size.Height - 150);
                pictureBox1.ImageLocation = filelocation;
            }
        }

        public Bitmap BlurEffect(int matrixSize)
        {
            Bitmap sourceBitmap = new Bitmap(pictureBox1.Image);
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];


            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;


            int byteOffset = 0;


            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;


            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;


                    neighbourPixels.Clear();


                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {


                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);


                            neighbourPixels.Add(BitConverter.ToInt32(
                                             pixelBuffer, calcOffset));
                        }
                    }


                    neighbourPixels.Sort();

                    middlePixel = BitConverter.GetBytes(
                                       neighbourPixels[filterOffset]);


                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        private void Save_click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "jpg files(.*jpg)|*.jpg| PNG files(.*png)|*.png| All Files(*.*)|*.*";
            if (saveFileDialog.ShowDialog()==DialogResult.OK) {
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
        }

        private void text_enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "10,50 exemple")
            {
                textBox1.Text = string.Empty;
                textBox1.ForeColor = Color.Black;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
