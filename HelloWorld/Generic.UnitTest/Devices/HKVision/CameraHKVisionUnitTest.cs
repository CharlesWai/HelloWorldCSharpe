using Generic.Library.Devices.HKVision;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Generic.UnitTest.Devices.HKVision
{
    [TestClass]
    public class CameraHKVisionUnitTest
    {
        [TestMethod]
        public void TestGetDevices()
        {
            CameraHKVision driver = new CameraHKVision();
            var cameras = driver.GetDevices(out List<CCameraInfo> camerasInfos);
            var devices =  driver.GetGIGEDevices();
            if (devices != null && devices.Count > 0)
            {
                CFrameout frameout = driver.Frameout;
                driver.OpenCamera(driver.Camera, devices[0]);
                driver.SetTriggerMode(driver.Camera, MvCamCtrl.NET.CameraParams.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                driver.SetTriggerSource(driver.Camera, MvCamCtrl.NET.CameraParams.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                driver.InitGrab(driver.Camera,driver.Bitmap,driver.BitmapPixelFormat);
                Task.Run(() => { 
                    while (true)
                    {
                        Thread.Sleep(1);
                        int ret = driver.Camera.GetImageBuffer(ref frameout, 1000);
                        if (ret == 0)
                        {

                        }
                        else
                        {

                        }
                    }
                });
                bool succeed = driver.TriggerOnce(driver.Camera);
                if (succeed)
                {
                   
                }
            }
        }
    }
}
