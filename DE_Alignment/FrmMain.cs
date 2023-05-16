using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace DE_Alignment
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap("D:\\code.png");

            BarcodeReader reader = new BarcodeReader
            {
                Options = new DecodingOptions
                {
                    PossibleFormats = new BarcodeFormat[] { BarcodeFormat.DATA_MATRIX }
                }
            };

            // 读取图像文件
            //Bitmap image = new Bitmap("D:\\QRCOde.bmp");
            Bitmap image = new Bitmap("D:\\999b.png");

            Mat clsImage = new Mat("D:\\111.bmp");
            CodeDecode(clsImage);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strRoot = @"D:\Setting\CameraSetting\";
            string strName = "P1";
            string strImage = strRoot + strName + "\\Pattern.bmp";
            string strSerialNum = strRoot + strName + "\\SerialNum.CSN";
            string strSettingPath = strRoot + strName + "\\CameraSetting.ini";
            string strSerialNumber = File.ReadAllText(strSerialNum);

            iDSBox1.InitCamera(iDSControl.CameraChoose.GetDeviceID_From_SerialNumber(strSerialNumber) | (Int32)uEye.Defines.DeviceEnumeration.UseDeviceID);
            iDSBox1.LoadIniSetting(strSettingPath);

            Mat ImgSource =new Mat(strImage);
            Mat ImgGary = new Mat();
            Cv2.CvtColor(ImgSource, ImgGary, ColorConversionCodes.BGR2GRAY);
            
            FrmCameraSetting frmCamereSetting = new FrmCameraSetting();
            frmCamereSetting.ShowDialog();
            iDSControl.MatchResult clsMatchResult = iDSBox1.Match(ImgGary);
        }

        private string CodeDecode(Mat ImgSource)
        {
            string strRtn = "";

            BarcodeReader reader = new BarcodeReader
            {
                Options = new DecodingOptions
                {
                    PossibleFormats = new BarcodeFormat[] { BarcodeFormat.DATA_MATRIX , BarcodeFormat.QR_CODE }
                }
            };

            ///灰階暫存區
            Mat clsGrayImage = new Mat();
            ///灰階轉換
            if (ImgSource.Type() == MatType.CV_8UC1)
                clsGrayImage = ImgSource.Clone();
            else
                Cv2.CvtColor(ImgSource, clsGrayImage, ColorConversionCodes.BGR2GRAY);
            
            ///二值化
            clsGrayImage = clsGrayImage.Threshold(70, 255, ThresholdTypes.Binary);

            int iWidth = clsGrayImage.Width;
            int iHeight = clsGrayImage.Height;
            double angle = 90;
            Point2f center = new Point2f(iWidth / 2f, iHeight / 2f);
            Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Mat rotatedImage = new Mat();
            Result result;
            Cv2.WarpAffine(clsGrayImage, rotatedImage, rotationMatrix, new OpenCvSharp.Size(iWidth, iHeight));
            for (int i = 0; i < 4; i++)
            {
                Cv2.WarpAffine(rotatedImage, rotatedImage, rotationMatrix, new OpenCvSharp.Size(iWidth, iHeight));
                result = reader.Decode(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(rotatedImage));
                if(result != null)
                {
                    strRtn = result.Text;
                    MessageBox.Show("DataMatrix content: " + result.Text);
                    Cv2.ImShow(result.Text, rotatedImage);
                    break;
                }
            }
            return strRtn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmCameraSetting cld = new FrmCameraSetting();
            cld.Show();
        }
    }
}
