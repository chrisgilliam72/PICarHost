using Iot.Device.Board;
using System;
using PICarHost.Server;
using MMALSharp;
using System.Threading.Tasks;
using MMALSharp.Handlers;
using MMALSharp.Common;
using System.Device.I2c;
using PICarHost;

namespace TestApp
{
    class Program
    {
        private static MMALCamera cam;
        static private async Task TakePicture()
        {


            using (var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/share/", "jpg"))
            {
                await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
            }

            // Only call when you no longer require the camera, i.e. on app shutdown.
   
        }


        static async Task Main(string[] args)
        {
            Console.WriteLine("PiCar Test app v1.1");
            var raspberryPibrd = new RaspberryPiBoard();
            var controller = raspberryPibrd.CreateGpioController();

            int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
            Console.WriteLine("Bus No:" + busNo);
            var ic2Bus = I2cBus.Create(busNo);
            var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);

            var ultraborg = new Ultraborg();
            ultraborg.Init(busNo, busItems[0]);

            var motorProc = new MotorProcessor();
            motorProc.Init(controller);

            var rgbController = new RGBContoller();
            rgbController.Init(ultraborg,controller);
            rgbController.LightsOff();

            //cam = MMALCamera.Instance;
            //MMALCameraConfig.StillResolution = MMALSharp.Common.Utility.Resolution.As8MPixel;
            //MMALCameraConfig.Flips = MMALSharp.Native.MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_VERTICAL;




            //Console.WriteLine(("No Bus items: " + busItems.Count));
            //foreach (var busItem in busItems)
            //{
            //    Console.WriteLine(busItem);
            //}
            //Console.ReadLine();



            //while (true)
            //{
            //    double distance = ultraborg.GetFilteredDistance(1);
            //    Console.WriteLine(distance);
            //}
            while (true)
            {
                var keyInfo = Console.ReadKey();
                switch (keyInfo.KeyChar)
                {
                    case '1': motorProc.GoForward(1); break;
                    case '2': motorProc.TurnLeft(1); break;
                    case '3': motorProc.TurnRight(1); break;
                    case '4': motorProc.GoBackwards(1); break;
                    case 'd': var distance =ultraborg.GetFilteredDistance(1); Console.WriteLine($"Distance: {distance}");break;
                    case 's': motorProc.Stop(); break;
                    case 'p': await TakePicture(); break;
                    case 'r': rgbController.LightsOff(); rgbController.LightsRed(true); break;
                    case 'g': rgbController.LightsOff(); rgbController.LightsGreen(true); break;
                    case 'b': rgbController.LightsOff(); rgbController.LightsBlue(true); break;
                    case 'a': rgbController.LightsRed(true); rgbController.LightsGreen(true); rgbController.LightsBlue(true); break;
                    case 'o': rgbController.LightsOff(); break;
                    case 'x': Environment.Exit(0);break;

                }
            }
        }
    }
}
