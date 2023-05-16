using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Bitmap Gray;
        System.Drawing.Point _sDownPoint, _sMovePoint;
        bool _IsDown = false;
        Mat image;
        Mat cloneMat;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 加载图像
            image = Cv2.ImRead("C:\\Lenna.jpg");
            Mat imageSample = Cv2.ImRead("C:\\Gray2.jpg");
            Mat gray = new Mat();
            Mat imageSample2 = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(imageSample, imageSample2, ColorConversionCodes.BGR2GRAY);

            Mat result = new Mat();
            Cv2.MatchTemplate(gray, imageSample2, result, TemplateMatchModes.CCoeff);

            double minVal, maxVal;
            OpenCvSharp.Point minLoc, maxLoc;
            Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);

            Cv2.Rectangle(image, new Rect(maxLoc.X, maxLoc.Y, imageSample.Width, imageSample.Height), Scalar.Red, 2);

            //Cv2.Rectangle(image, new Rect(0,0,10,10), Scalar.Black);
            // 显示图像
            //Cv2.ImShow("Image", image);

            // 转换为灰度图像
            gray.SaveImage("C:\\Gray.jpg");
            // 显示灰度图像
            //Cv2.ImShow("Gray Image", gray);

            bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
            Gray = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(gray);
            pictureBox1.Width = bitmap.Width;
            pictureBox1.Height = bitmap.Height;
            pictureBox1.Image = bitmap;


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = ((CheckBox)sender).Checked ? bitmap : Gray;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _sDownPoint = new System.Drawing.Point(e.X, e.Y);
            _IsDown = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _sMovePoint = new System.Drawing.Point(e.X, e.Y);
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            int iWidth = _sMovePoint.X -_sDownPoint.X;
            int iHeight = _sMovePoint.Y - _sDownPoint.Y ;

            if (_IsDown)
            {
                cloneMat = image.Clone();
                //Cv2.Rectangle(cloneMat, new Rect(_sDownPoint.X, _sDownPoint.Y, iWidth, iHeight), Scalar.Red, 2);
                bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cloneMat);
                pictureBox1.Image = bitmap;
                e.Graphics.DrawRectangle(new Pen(Color.Red), _sDownPoint.X, _sDownPoint.Y, iWidth, iHeight);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iDSBox1.InitCamera(0);
            trackBar1.Minimum = (int)iDSBox1._iMinFocusDistance;
            trackBar1.Maximum = (int)iDSBox1._iMaxFocusDistance;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iDSBox1.Auto_White_Balance(true);
            iDSBox1.Auto_Gain_Balance(true);
            iDSBox1.Auto_Focus(true);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            iDSBox1.Manual_Focus((uint)trackBar1.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = iDSBox1.Get_Focus().ToString();
            iDSBox1.Save_Image("D:\\99999.bmp");
            iDSBox1.GetMatImage();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mat imageSample = Cv2.ImRead("D:\\CVPP.bmp");
            Mat imageSample2 = new Mat();
            Cv2.CvtColor(imageSample, imageSample2, ColorConversionCodes.BGR2GRAY);
            iDSBox1.Match(imageSample2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            iDSBox1.SetFitMode();
           
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _IsDown = false;
        }
    }
}
