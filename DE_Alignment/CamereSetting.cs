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
    public partial class FrmCameraSetting : Form
    {
        public FrmCameraSetting()
        {
            InitializeComponent();
        }

        private void btnCOnnect_Click(object sender, EventArgs e)
        {
            iDSControl.CameraChoose clsCameraChoose = new iDSControl.CameraChoose();
            if (clsCameraChoose.ShowDialog() == DialogResult.OK)
            {
                if (clsCameraChoose.IsCanUse)
                {
                    IDSBox1.InitCamera(clsCameraChoose.DeviceID | (Int32)uEye.Defines.DeviceEnumeration.UseDeviceID);
                    button1.Enabled = true;
                    ((Button)sender).Tag = clsCameraChoose.SerialNumber;
                    txtSerialNum.Text = clsCameraChoose.SerialNumber;

                }
                else
                    MessageBox.Show("Camera Using");
            }

        }

        private void btnAutoSet_Click(object sender, EventArgs e)
        {

        }

        private void chbGain_CheckedChanged(object sender, EventArgs e)
        {
            IDSBox1.Auto_Gain_Balance(((CheckBox)sender).Checked);
        }

        private void chkWhite_CheckedChanged(object sender, EventArgs e)
        {
            IDSBox1.Auto_White_Balance(((CheckBox)sender).Checked);
        }

        private void chkFocus_CheckedChanged(object sender, EventArgs e)
        {
            IDSBox1.Auto_Focus(((CheckBox)sender).Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog digSelect = new OpenFileDialog();

            digSelect.Filter = "Ini Files (*.ini)|*.ini|All Files (*.*)|*.*";
            if (digSelect.ShowDialog() == DialogResult.OK)
            {
                IDSBox1.LoadIniSetting(digSelect.FileName);
                ((Button)sender).Tag = digSelect.FileName;
                txtPath.Text = digSelect.FileName;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<int> iTs;
            iTs = FineDecode((Mat)btnCompare.Tag);
            
            if(iTs != null)
                if (iTs.Count > 2)
                    CodeDecode((Mat)picPattern.Tag, iTs[iTs.Count/2]);
            //Mat imageSample2 = new Mat();
            //Cv2.CvtColor((Mat)picPattern.Tag, imageSample2, ColorConversionCodes.BGR2GRAY);
            iDSControl.MatchResult clsMatchResult = IDSBox1.Match((Mat)picPattern.Tag);

            lsbMessage.Items.Clear();
            lsbMessage.Items.Add("X:    " + clsMatchResult._clsRectangle.X);
            lsbMessage.Items.Add("Y:    " + clsMatchResult._clsRectangle.Y);
            lsbMessage.Items.Add("Width:    " + clsMatchResult._clsRectangle.Width);
            lsbMessage.Items.Add("HeightX:    " + clsMatchResult._clsRectangle.Height);
            lsbMessage.Items.Add("Score:    " + clsMatchResult._dScore.ToString());
        }

        private void btnLOAD_Click(object sender, EventArgs e)
        {
            btnCompare.Enabled = true;

            try
            {
                Mat clsPattern = IDSBox1.GetMatImage().Clone(new Rect(IDSBox1._clsMouseInfo.iDownX, IDSBox1._clsMouseInfo.iDownY, IDSBox1._clsMouseInfo.iWidth, IDSBox1._clsMouseInfo.iHeight));
                picPattern.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(clsPattern);
                picPattern.Tag = clsPattern;
                Mat clsQRAOI = new Mat(IDSBox1.GetMatImage(), new Rect(IDSBox1._clsMouseInfo.iDownX, IDSBox1._clsMouseInfo.iDownY, IDSBox1._clsMouseInfo.iWidth, IDSBox1._clsMouseInfo.iHeight));
                btnCompare.Tag = clsQRAOI;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Select Pattern");
            }
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
            Bitmap image = new Bitmap("D:\\code.png");

            // 解码 DataMatrix 条码
            Result result = reader.Decode(image);

            // 显示解码结果
            if (result != null)
            {
                MessageBox.Show("DataMatrix content: " + result.Text);
            }
            else
            {
                MessageBox.Show("No DataMatrix found in the image.");
            }
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            string strPath = txtRoot.Text + "\\" + txtName.Text;
            if(Directory.Exists(strPath))
            {
                if(MessageBox.Show("設定已存在是否覆蓋?","已存在",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Directory.Delete(strPath, true);
                }
            }
            Directory.CreateDirectory(strPath);

            picPattern.Image.Save(strPath + "\\Pattern.bmp");
            File.Copy(txtPath.Text, strPath + "\\CameraSetting.ini");
            File.WriteAllText(strPath + "\\SerialNum.CSN", txtSerialNum.Text);
        }

        private void txtRoot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog clsFolderDialog = new FolderBrowserDialog();
            if( clsFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtRoot.Text = clsFolderDialog.SelectedPath;
            }
        }

        private string CodeDecode(Mat ImgSource,int value)
        {
            string strRtn = "";

            BarcodeReader reader = new BarcodeReader
            {
                Options = new DecodingOptions
                {
                    PossibleFormats = new BarcodeFormat[] { BarcodeFormat.DATA_MATRIX, BarcodeFormat.QR_CODE }
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
            clsGrayImage = clsGrayImage.Threshold(value, 255, ThresholdTypes.Binary);

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
                if (result != null)
                {
                    strRtn = result.Text;
                    MessageBox.Show("DataMatrix content: " + result.Text);
                    break;
                }
            }
            Cv2.ImShow("Fin", rotatedImage);
            return strRtn;
        }

        private List<int> FineDecode(Mat ImgSource)
        {
            string strRtn = "";
            List<int> iThreshs = new List<int>();
            BarcodeReader reader = new BarcodeReader
            {
                Options = new DecodingOptions
                {
                    PossibleFormats = new BarcodeFormat[] { BarcodeFormat.DATA_MATRIX, BarcodeFormat.QR_CODE }
                }
            };
            ///灰階暫存區
            Mat clsGrayImage = new Mat();
            Mat clsTmpGrayImage = new Mat();
            ///灰階轉換
            if (ImgSource.Type() == MatType.CV_8UC1)
                clsGrayImage = ImgSource.Clone();
            else
                Cv2.CvtColor(ImgSource, clsGrayImage, ColorConversionCodes.BGR2GRAY);
            clsTmpGrayImage = clsGrayImage.Clone();


            for (int iThresh = 1; iThresh < 255; iThresh++)
            {
                ///二值化
                clsGrayImage = clsTmpGrayImage.Threshold(iThresh, 255, ThresholdTypes.Binary);
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
                    if (result != null)
                    {
                        strRtn = result.Text;
                        //MessageBox.Show("DataMatrix content: " + result.Text);
                        iThreshs.Add(iThresh);
                        break;
                    }
                }
            }

            return iThreshs;
        }
    }
}
