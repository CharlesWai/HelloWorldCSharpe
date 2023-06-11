using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Generic.Library.Devices.HKVision
{
    public class CameraHKVision : IHKCamera
    {
        private CCamera _camera = new CCamera();
        private Bitmap? _bitmap = null;
        private System.Drawing.Imaging.PixelFormat _bitmapPixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
        private CImage? _image = null;
        private CFrameSpecInfo? _specInfo = null;
        private object _frameLock = new object();
        private CFrameout _frameInfo = new CFrameout();
        private CDisplayFrameInfo _displayInfo = new CDisplayFrameInfo();
        private CPixelConvertParam _convertParam = new CPixelConvertParam();

        public CCamera Camera { get => _camera; set => _camera = value; }
        public Bitmap Bitmap { get => _bitmap; set => _bitmap = value; }
        public System.Drawing.Imaging.PixelFormat BitmapPixelFormat { get => _bitmapPixelFormat; set => _bitmapPixelFormat = value; }
        public CImage Image { get => _image; set => _image = value; }
        public CFrameSpecInfo FrameSpecInfo { get => _specInfo; set => _specInfo = value; }
        public CFrameout Frameout { get => _frameInfo; set => _frameInfo = value; }
        public CDisplayFrameInfo DisplayFrameInfo { get => _displayInfo; set => _displayInfo = value; }
        public CPixelConvertParam PixelConvertParam { get => _convertParam; set => _convertParam = value; }

        public int GetDevices(out List<MvCamCtrl.NET.CCameraInfo> ltCameras)
        {
            System.GC.Collect();
            List<MvCamCtrl.NET.CCameraInfo> ltCameraList = new List<CCameraInfo>();
            try
            {
                var cams = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE | CSystem.MV_USB_DEVICE | CSystem.MV_VIR_USB_DEVICE, ref ltCameraList);
                ltCameras = ltCameraList;
                return cams;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ltCameras = ltCameraList;
                return -1;
            }
        }

        public List<CGigECameraInfo>? GetGIGEDevices()
        {
            List<CGigECameraInfo>? cameraInfos = null;
            var ret = GetDevices(out List<MvCamCtrl.NET.CCameraInfo> ltCameraList);
            if (ret == 0)
            {
                cameraInfos = new List<CGigECameraInfo>();
                foreach (var cam in ltCameraList)
                {
                    if (cam.nTLayerType == CSystem.MV_GIGE_DEVICE)
                    {
                        CGigECameraInfo gigeInfo = (CGigECameraInfo)cam;
                        cameraInfos.Add(gigeInfo);
                    }
                }
            }
            return cameraInfos;
        }

        public float GetFloatParam(CCamera camera, string paramName)
        {
            CFloatValue? pcFloatValue = new CFloatValue();
            int nRet = camera.GetFloatValue(paramName, ref pcFloatValue);
            if (CErrorDefine.MV_OK == nRet)
            {
                return pcFloatValue.CurValue;
            }
            return float.MaxValue;
        }

        public List<CUSBCameraInfo>? GetUSBDevices()
        {
            List<CUSBCameraInfo>? cameraInfos = null;
            var ret = GetDevices(out List<MvCamCtrl.NET.CCameraInfo> ltCameraList);
            if (ret == 0)
            {
                cameraInfos = new List<CUSBCameraInfo>();
                foreach (var cam in ltCameraList)
                {
                    if (cam.nTLayerType == CSystem.MV_USB_DEVICE)
                    {
                        CUSBCameraInfo gigeInfo = (CUSBCameraInfo)cam;
                        cameraInfos.Add(gigeInfo);
                    }
                }
            }
            return cameraInfos;
        }

        public string GetEnumParam(CCamera camera, string paramName)
        {
            CEnumValue pcEnumValue = new CEnumValue();
            CEnumEntry pcEntryValue = new CEnumEntry();
            int nRet = camera.GetEnumValue("PixelFormat", ref pcEnumValue);
            if (CErrorDefine.MV_OK == nRet)
            {
                pcEntryValue.Value = pcEnumValue.CurValue;
                nRet = camera.GetEnumEntrySymbolic("PixelFormat", ref pcEntryValue);
                if (CErrorDefine.MV_OK == nRet)
                {
                    return pcEntryValue.Symbolic;
                }
            }
            return string.Empty;
        }

        public bool SetFloatParam(CCamera camera, string paramName, float val)
        {
            int nRet = camera.SetFloatValue(paramName, val);
            return nRet == CErrorDefine.MV_OK;
        }

        public bool SetEnumParam(CCamera camera, string paramName, uint val)
        {
            int nRet = camera.SetEnumValue(paramName, val);
            return nRet == CErrorDefine.MV_OK;
        }

        public bool OpenCamera(CCamera camera, CCameraInfo cameraInfo)
        {
            int nRet = camera.CreateHandle(ref cameraInfo);
            if (CErrorDefine.MV_OK != nRet)
            {
                return false;
            }

            nRet = camera.OpenDevice();
            if (CErrorDefine.MV_OK != nRet)
            {
                camera.DestroyHandle();
                return false;
            }

            // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
            if (cameraInfo.nTLayerType == CSystem.MV_GIGE_DEVICE)
            {
                int nPacketSize = camera.GIGE_GetOptimalPacketSize();
                if (nPacketSize > 0)
                {
                    nRet = camera.SetIntValue("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != CErrorDefine.MV_OK)
                    {
                        //ShowErrorMsg("Set Packet Size failed!", nRet);
                        return false;
                    }
                }
                else
                {
                    //ShowErrorMsg("Get Packet Size failed!", nPacketSize);
                    return false;
                }
            }
            return true;
        }

        public bool SetTriggerSource(CCamera camera, MV_CAM_TRIGGER_SOURCE trggerSource)
        {
            int nRet = camera.SetEnumValue("TriggerSource", (uint)trggerSource);
            return nRet == CErrorDefine.MV_OK;
        }

        public bool TriggerOnce(CCamera camera)
        {
            int nRet = camera.SetCommandValue("TriggerSoftware");
            return CErrorDefine.MV_OK == nRet;
        }

        public void ShowLatestImage(CCamera camera, Bitmap bitmap, IntPtr containerHandle, CFrameout pcFrameInfo, CDisplayFrameInfo pcDisplayInfo, CPixelConvertParam pcConvertParam)
        {
            int nRet = camera.GetImageBuffer(ref pcFrameInfo, 1000);
            if (nRet == CErrorDefine.MV_OK)
            {
                lock (_frameLock)
                {
                    _image = pcFrameInfo.Image.Clone() as CImage;
                    _specInfo = pcFrameInfo.FrameSpec;

                    pcConvertParam.InImage = pcFrameInfo.Image;
                    if (System.Drawing.Imaging.PixelFormat.Format8bppIndexed == bitmap.PixelFormat)
                    {
                        pcConvertParam.OutImage.PixelType = MvGvspPixelType.PixelType_Gvsp_Mono8;
                        camera.ConvertPixelType(ref pcConvertParam);
                    }
                    else
                    {
                        pcConvertParam.OutImage.PixelType = MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;
                        camera.ConvertPixelType(ref pcConvertParam);
                    }

                    // ch:保存Bitmap数据 | en:Save Bitmap Data
                    BitmapData m_pcBitmapData = bitmap.LockBits(new Rectangle(0, 0, pcConvertParam.InImage.Width, pcConvertParam.InImage.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    Marshal.Copy(pcConvertParam.OutImage.ImageData, 0, m_pcBitmapData.Scan0, (Int32)pcConvertParam.OutImage.ImageData.Length);
                    bitmap.UnlockBits(m_pcBitmapData);
                }
                pcDisplayInfo.WindowHandle = containerHandle;
                pcDisplayInfo.Image = pcFrameInfo.Image;
                camera.DisplayOneFrame(ref pcDisplayInfo);
                camera.FreeImageBuffer(ref pcFrameInfo);
            }
        }

        public int InitGrab(CCamera camera, Bitmap? bitmap, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            // ch:取图像宽 | en:Get Iamge Width
            CIntValue pcWidth = new CIntValue();
            int nRet = camera.GetIntValue("Width", ref pcWidth);
            if (CErrorDefine.MV_OK != nRet)
            {
                //ShowErrorMsg("Get Width Info Fail!", nRet);
                return nRet;
            }
            // ch:取图像高 | en:Get Iamge Height
            CIntValue pcHeight = new CIntValue();
            nRet = camera.GetIntValue("Height", ref pcHeight);
            if (CErrorDefine.MV_OK != nRet)
            {
                //ShowErrorMsg("Get Height Info Fail!", nRet);
                return nRet;
            }
            // ch:取像素格式 | en:Get Pixel Format
            CEnumValue pcPixelFormat = new CEnumValue();
            nRet = camera.GetEnumValue("PixelFormat", ref pcPixelFormat);
            if (CErrorDefine.MV_OK != nRet)
            {
                //ShowErrorMsg("Get Pixel Format Fail!", nRet);
                return nRet;
            }

            // ch:设置bitmap像素格式
            if ((Int32)MvGvspPixelType.PixelType_Gvsp_Undefined == (Int32)pcPixelFormat.CurValue)
            {
                //ShowErrorMsg("Unknown Pixel Format!", CErrorDefine.MV_E_UNKNOW);
                return CErrorDefine.MV_E_UNKNOW;
            }
            else if (IsMono((MvGvspPixelType)pcPixelFormat.CurValue))
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            }
            else
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            }

            if (null != bitmap)
            {
                bitmap.Dispose();
                bitmap = null;
            }
            bitmap = new Bitmap((Int32)pcWidth.CurValue, (Int32)pcHeight.CurValue, pixelFormat);

            // ch:Mono8格式，设置为标准调色板 | en:Set Standard Palette in Mono8 Format
            if (System.Drawing.Imaging.PixelFormat.Format8bppIndexed == pixelFormat)
            {
                ColorPalette palette = bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                }
                bitmap.Palette = palette;
            }

            return CErrorDefine.MV_OK;
        }
        // ch:像素类型是否为Mono格式 | en:If Pixel Type is Mono 
        private Boolean IsMono(MvGvspPixelType enPixelType)
        {
            switch (enPixelType)
            {
                case MvGvspPixelType.PixelType_Gvsp_Mono1p:
                case MvGvspPixelType.PixelType_Gvsp_Mono2p:
                case MvGvspPixelType.PixelType_Gvsp_Mono4p:
                case MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MvGvspPixelType.PixelType_Gvsp_Mono8_Signed:
                case MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                case MvGvspPixelType.PixelType_Gvsp_Mono14:
                case MvGvspPixelType.PixelType_Gvsp_Mono16:
                    return true;
                default:
                    return false;
            }
        }

        public bool SetTriggerMode(CCamera camera, MV_CAM_TRIGGER_MODE triggerMode)
        {
            int nRet = camera.SetEnumValue("TriggerMode", (uint)triggerMode);
            return nRet == CErrorDefine.MV_OK;
        }
    }
}
