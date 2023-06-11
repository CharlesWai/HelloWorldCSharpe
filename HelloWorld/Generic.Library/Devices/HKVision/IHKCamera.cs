using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Library.Devices.HKVision
{
    public interface IHKCamera
    {
        int GetDevices(out List<MvCamCtrl.NET.CCameraInfo> ltCameras);
        List<CGigECameraInfo>? GetGIGEDevices();
        List<CUSBCameraInfo>? GetUSBDevices();
        float GetFloatParam(CCamera camera,string paramName);
        string GetEnumParam(CCamera camera, string paramName);

        bool SetFloatParam(CCamera camera, string paramName, float val);
        bool SetEnumParam(CCamera camera, string paramName, uint val);
        bool OpenCamera(CCamera camera, CCameraInfo cameraInfo);
        bool SetTriggerMode(CCamera camera, MV_CAM_TRIGGER_MODE triggerMode);
        bool SetTriggerSource(CCamera camera, MV_CAM_TRIGGER_SOURCE trggerSource);
        bool TriggerOnce(CCamera camera);
        void ShowLatestImage(CCamera camera,Bitmap bitmap,IntPtr windowHandle, CFrameout pcFrameInfo, CDisplayFrameInfo pcDisplayInfo, CPixelConvertParam pcConvertParam);
        int InitGrab(CCamera camera, Bitmap bitmap, System.Drawing.Imaging.PixelFormat pixelFormat);
    }
}
