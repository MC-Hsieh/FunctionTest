using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iDSControl
{
    public partial class iDSBox : UserControl
    {
        private uEye.Camera _Camera;
        IntPtr _DisplayHandle = IntPtr.Zero;
        private bool _bLive = false;
        private const int _cnNumberOfSeqBuffers = 3;

        public bool _bSupport_Auto_Gain_Balance = false;
        public bool _bSupport_Auto_White_Balance = false;
        public uint _iMinFocusDistance = 0;
        public uint _iMaxFocusDistance = 1024;
        public uint _iIncFocus = 0;

        public Rectangle _CameraSize;

        public bool _bCross = false;
        public MouseInfo _clsMouseInfo = new MouseInfo();

        MatchResult _TmpMatchResult;
        public iDSBox()
        {
            InitializeComponent();
            _DisplayHandle = picImage.Handle;
            picImage.Tag = _clsMouseInfo;
        }

        public void InitCamera(int iIndex = 0)
        {
            _Camera = new uEye.Camera();
            uEye.Defines.Status statusRet = 0;
            // Open Camera
            statusRet = _Camera.Init(iIndex);
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Camera initializing failed");
                Environment.Exit(-1);
            }

            // Set color mode
            //statusRet = _Camera.PixelFormat.Set(uEye.Defines.ColorMode.BGR8Packed);
            statusRet = _Camera.PixelFormat.Set(uEye.Defines.ColorMode.Mono8);
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Setting color mode failed");
                Environment.Exit(-1);
            }

            // Allocate Memory
            statusRet = AllocImageMems();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Allocate Memory failed");
                Environment.Exit(-1);
            }

            statusRet = InitSequence();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Add to sequence failed");
                Environment.Exit(-1);
            }

            // Start Live Video
            statusRet = _Camera.Acquisition.Capture();
            if (statusRet != uEye.Defines.Status.Success)
            {
                MessageBox.Show("Start Live Video failed");
            }
            else
            {
                _bLive = true;
            }
            _Camera.Size.AOI.Get(out _CameraSize);
            picImage.Width = _CameraSize.Width;
            picImage.Height = _CameraSize.Height;
            // Connect Event
            _Camera.EventFrame += onFrameEvent;
            _Camera.EventAutoBrightnessFinished += onAutoShutterFinished;

            _bSupport_Auto_Gain_Balance = _Camera.AutoFeatures.Software.Gain.Supported;
            _bSupport_Auto_White_Balance = _Camera.AutoFeatures.Software.WhiteBalance.Supported;
            _Camera.Focus.Manual.GetRange(out _iMinFocusDistance, out _iMaxFocusDistance, out _iIncFocus);
        }

        private uEye.Defines.Status AllocImageMems()
        {
            uEye.Defines.Status statusRet = uEye.Defines.Status.SUCCESS;

            for (int i = 0; i < _cnNumberOfSeqBuffers; i++)
            {
                statusRet = _Camera.Memory.Allocate();

                if (statusRet != uEye.Defines.Status.SUCCESS)
                {
                    FreeImageMems();
                }
            }

            return statusRet;
        }

        private uEye.Defines.Status FreeImageMems()
        {
            int[] idList;
            uEye.Defines.Status statusRet = _Camera.Memory.GetList(out idList);

            if (uEye.Defines.Status.SUCCESS == statusRet)
            {
                foreach (int nMemID in idList)
                {
                    do
                    {
                        statusRet = _Camera.Memory.Free(nMemID);

                        if (uEye.Defines.Status.SEQ_BUFFER_IS_LOCKED == statusRet)
                        {
                            Thread.Sleep(1);
                            continue;
                        }

                        break;
                    }
                    while (true);
                }
            }

            return statusRet;
        }

        private uEye.Defines.Status InitSequence()
        {
            int[] idList;
            uEye.Defines.Status statusRet = _Camera.Memory.GetList(out idList);

            if (uEye.Defines.Status.SUCCESS == statusRet)
            {
                statusRet = _Camera.Memory.Sequence.Add(idList);

                if (uEye.Defines.Status.SUCCESS != statusRet)
                {
                    ClearSequence();
                }
            }

            return statusRet;
        }

        private uEye.Defines.Status ClearSequence()
        {
            return _Camera.Memory.Sequence.Clear();
        }

        private void onFrameEvent(object sender, EventArgs e)
        {
            uEye.Camera Camera = sender as uEye.Camera;

            Int32 s32MemID;
            uEye.Defines.Status statusRet = Camera.Memory.GetLast(out s32MemID);

            try
            {
                if ((uEye.Defines.Status.SUCCESS == statusRet) && (0 < s32MemID))
                {
                    if (uEye.Defines.Status.SUCCESS == Camera.Memory.Lock(s32MemID))
                    {
                        Camera.Display.Render(s32MemID, _DisplayHandle, uEye.Defines.DisplayRenderMode.FitToWindow);
                        if (_bCross) DrawCross(_DisplayHandle);
                        DrawRe(_DisplayHandle);
                        Camera.Memory.Unlock(s32MemID);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void onAutoShutterFinished(object sender, EventArgs e)
        {
            MessageBox.Show("AutoShutter finished...");
        }

        public bool Auto_Gain_Balance(bool bEnable)
        {
            bool bSuccess = false;

            if (_Camera != null)
            {
                bSuccess = (_Camera.AutoFeatures.Software.WhiteBalance.SetEnable(bEnable) == uEye.Defines.Status.SUCCESS);
            }

            return bSuccess;
        }
        public bool Auto_White_Balance(bool bEnable)
        {
            bool bSuccess = false;

            if (_Camera != null)
            {
                bSuccess = (_Camera.AutoFeatures.Software.Gain.SetEnable(bEnable) == uEye.Defines.Status.Success);
            }

            return bSuccess;
        }
        public bool Auto_Focus(bool bEnable)
        {
            bool bSuccess = false;

            if (_Camera != null)
            {
                bSuccess = (_Camera.Focus.Auto.SetEnable(bEnable) == uEye.Defines.Status.SUCCESS);
            }

            return bSuccess;
        }

        public bool Manual_Focus(uint iFocus)
        {
            bool bSuccess = false;

            if (_Camera != null)
            {
                bSuccess = (_Camera.Focus.Auto.SetEnable(false) == uEye.Defines.Status.SUCCESS);
                bSuccess = (_Camera.Focus.Manual.Set(iFocus) == uEye.Defines.Status.SUCCESS);
            }

            return bSuccess;
        }

        public bool Save_Image(string strPath)
        {
            bool bSuccess = false;

            if (_Camera != null)
            {
                _Camera.Image.Save(strPath);
            }

            return bSuccess;
        }

        public Mat GetMatImage()
        {
            Mat clsMat = null;
            if (_Camera != null)
            {
                IntPtr iImagePtr;
                _Camera.Memory.GetLast(out iImagePtr);
                clsMat = new Mat(_CameraSize.Height, _CameraSize.Width, MatType.CV_8UC1, iImagePtr);
            }
            return clsMat;
        }

        public MatchResult Match(Mat clsPattern)
        {
            MatchResult clsPatternResult = new MatchResult();
            Mat clsMat = null;
            if (_Camera != null)
            {
                IntPtr iImagePtr;
                _Camera.Memory.GetLast(out iImagePtr);
                clsMat = new Mat(_CameraSize.Height, _CameraSize.Width, MatType.CV_8UC1, iImagePtr);
                clsPatternResult = PatternMatch.Match(clsMat, clsPattern,TemplateMatchModes.CCoeffNormed);
                _TmpMatchResult = clsPatternResult;
                _bCross = true;
                _Camera.Display.Mode.Set(uEye.Defines.DisplayMode.OpenGL);
            }
            return clsPatternResult;
        }

        public void DrawCross(IntPtr iHandle)
        {
            Graphics clsGgraphics = Graphics.FromHwnd(iHandle);
            Pen pen = new Pen(Color.LightGreen, 2);
            
            clsGgraphics.DrawRectangle(pen,_TmpMatchResult._clsRectangle);
        }

        public void DrawRe(IntPtr iHandle)
        {
            Graphics clsGgraphics = Graphics.FromHwnd(iHandle);
            MouseInfo clsMouseInfo = (MouseInfo)(picImage.Tag);
            Pen penRed = new Pen(Color.Red, 2);
            Rectangle clsROI;
            if (clsMouseInfo.IsDown || clsMouseInfo.IsUp)
            {
                clsROI = new Rectangle(clsMouseInfo.iDownX, clsMouseInfo.iDownY, clsMouseInfo.iWidth, clsMouseInfo.iHeight);
                clsGgraphics.DrawRectangle(penRed, clsROI);
            }
        }

        public void SetFitMode()
        {
            picImage.Width = this.Width;
            picImage.Height = this.Height;
        }

        public uint Get_Focus()
        {
            uint iReturn = 0;
            if (_Camera != null)
            {
                int iR, iG, iB, iM;
                _Camera.Focus.Manual.Get(out iReturn);
                _Camera.Gain.Hardware.Scaled.GetRed(out iR);
                _Camera.Gain.Hardware.Scaled.GetGreen(out iG);
                _Camera.Gain.Hardware.Scaled.GetBlue(out iB);
                _Camera.Gain.Hardware.Scaled.GetMaster(out iM);
            }
            return iReturn;
        }

        public bool LoadIniSetting(string strIniPath)
        {
            bool bSuccess = false;

            if (_Camera != null)
            {
                bSuccess = (_Camera.Parameter.Load(strIniPath) == uEye.Defines.Status.Success);
            }

            return bSuccess;
        }


        public void Close()
        {
            if (_Camera != null)
            {
                _Camera.EventFrame -= onFrameEvent;
                _Camera.Acquisition.Stop();
                ClearSequence();
                FreeImageMems();
                // Close the Camera
                _Camera.Exit();
                Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_Camera != null) _Camera.Image.Save("");
        }

        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            MouseInfo clsMouseInfo = (MouseInfo)(((PictureBox)sender).Tag);
            clsMouseInfo.IsDown = true;
            clsMouseInfo.IsUp = false;
            clsMouseInfo.iDownX = e.X;
            clsMouseInfo.iDownY = e.Y;
        }

        private void picImage_MouseUp(object sender, MouseEventArgs e)
        {
            MouseInfo clsMouseInfo = (MouseInfo)(((PictureBox)sender).Tag);
            clsMouseInfo.IsDown = false;
            clsMouseInfo.IsUp = true;
            clsMouseInfo.iUpX = e.X;
            clsMouseInfo.iUpY = e.Y;
        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            MouseInfo clsMouseInfo = (MouseInfo)(((PictureBox)sender).Tag);

            if (clsMouseInfo.IsDown)
            {
                clsMouseInfo.iWidth = e.X - clsMouseInfo.iDownX;
                clsMouseInfo.iHeight = e.Y - clsMouseInfo.iDownY;
            }
            clsMouseInfo.iMoveX = e.X;
            clsMouseInfo.iMoveY = e.Y;
        }
    }

    public class MouseInfo
    {
        public bool IsDown;
        public bool IsUp;
        public int iDownX,iDownY;
        public int iUpX, iUpY;
        public int iMoveX, iMoveY;
        public int iWidth,iHeight;

    }
}
