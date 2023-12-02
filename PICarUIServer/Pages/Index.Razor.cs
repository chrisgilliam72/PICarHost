using Iot.Device.Board;
using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Handlers;
using PICarServerLib;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ultraborg.Library.Servo;

namespace PICarUIServer.Pages
{
    public partial class Index 
    {
        public byte[] ImageSrc { get; set; }
        private MMALCamera cam;
        public int ImageSize { get; set; }

        public double Distance { get; set; }
        public double FrontDistance { get; set; }

        private RaspberryPiBoard raspberryPibrd;
        private GpioController gpioController;
        private Ultraborg.Library.Ultraborg ultraborg;
        private L298NMotorProcessor motorProcessor;
        private bool RGBROn { get; set; }

        private bool RGBGOn { get; set; }
        private bool RGBBOn { get; set; }

        public Index()
        {
            RGBROn = false;
            RGBGOn = false;
            RGBBOn = false;
        }

        private Ultraborg.Library.Ultraborg CreateUltraBorgInstance()
        {
            int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
            Console.WriteLine("Bus No:" + busNo);
            var ic2Bus = I2cBus.Create(busNo);
            var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);

            Console.WriteLine(("No Bus items: " + busItems.Count));
            var ultraborg = new Ultraborg.Library.Ultraborg();
            ultraborg.Init(busNo, busItems[0]);
            return ultraborg;

        }

        private void MoveServo(int degrees)
        {


            var ubServo = new UltraborgServo(1, 0);
            ubServo.Init(ultraborg);
            Console.WriteLine("Servo Max: " + ubServo.ServoMax + " Servo Min:" + ubServo.ServoMin);
            Console.WriteLine("Current Pos " + ubServo.GetCurrentPosition());
            switch(degrees)
            {
                case 0: Console.WriteLine("To 0"); ubServo.ServoTo0();break;
                case 90: Console.WriteLine("To 90"); ubServo.ServoTo90(); break;
                case 270: Console.WriteLine("To 270"); ubServo.ServoTo270(); break;

            }

            Console.WriteLine("New Pos " + ubServo.GetCurrentPosition());
        }

        //protected override void OnInitialized()
        //{
        //    try
        //    {
        //        //cam = MMALCamera.Instance;
        //        //MMALCameraConfig.StillResolution = MMALSharp.Common.Utility.Resolution.As03MPixel;
        //        raspberryPibrd = new RaspberryPiBoard();
        //        gpioController = raspberryPibrd.CreateGpioController();
        //        ultraborg = CreateUltraBorgInstance();
        //        motorProcessor = new L298NMotorProcessor(gpioController, 23, 24, 9, 10);
        //        motorProcessor.Init();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception " + ex.Message);
        //    }

        //}
   
        public void SonicServoRight()
        {

            MoveServo(90);

        }

        public void SonicServoForward()
        {
            MoveServo(0);
        }

        public void SonicServoLeft()
        {
            MoveServo(270);
        }


        public void ToggleLightsR()
        {
            Console.WriteLine("Toggle Lights R");
            RGBROn = !RGBROn;
            var rgbCntrl = new RGBContoller();
            rgbCntrl.Init(gpioController);
            rgbCntrl.LightsRed(RGBROn);
            rgbCntrl.CloseAll();
        }

        public void  ToggleLightsG()
        {
            Console.WriteLine("Toggle Lights G");
            RGBGOn = !RGBGOn;
            var rgbCntrl = new RGBContoller();
            rgbCntrl.Init(gpioController);
            rgbCntrl.LightsGreen(RGBGOn);
            rgbCntrl.CloseAll();
        }

        public void ToggleLightsB()
        {
            Console.WriteLine("Toggle Lights B");
            RGBBOn = !RGBBOn;
            var rgbCntrl = new RGBContoller();
            rgbCntrl.Init(gpioController);
            rgbCntrl.LightsBlue(RGBBOn);
            rgbCntrl.CloseAll();
        }

        public void TurnLeft()
        {
            motorProcessor.StartTurnLeft();
        }

        public void TurnRight()
        {
            motorProcessor.StartTurnRight();
        }
        public void MoveForward()
        {
            motorProcessor.StartForward();
        }

        public void MoveBack()
        {
            motorProcessor.StartBackwards();
        }
        public void Stop()
        {
            motorProcessor.Stop();
        }
        public void GetDistance()
        {
            int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
            Console.WriteLine("Bus No:" + busNo);
            var ic2Bus = I2cBus.Create(busNo);
            var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);


            Console.WriteLine(("No Bus items: " + busItems.Count));
            foreach (var busItem in busItems)
            {
                Console.WriteLine(busItem);
            }
    

            var ultraborg = new Ultraborg.Library.Ultraborg();
            ultraborg.Init(busNo, busItems[0]);
            Distance = ultraborg.GetFilteredDistance(1);

            Console.WriteLine(Distance);
        }

        public void TestAutoPilot()
        {
            Console.Write("Start Autopilot");
            var autoPilot = new AutoPilot(ultraborg,motorProcessor);
            autoPilot.GoUntil(100,1);
            Console.Write("End Autopilot");
        }

        public async Task GetVideo()
        {
            //using (var vidCaptureHandler = new MemoryStreamCaptureHandler())
            //{
            //    var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));


            //    //var stream = await _client.GetStreamAsync(urlBlob);

            //    var stream = vidCaptureHandler.CurrentStream;

            //    // Take video for 3 minutes.
            //    await cam.TakeVideo(vidCaptureHandler, cts.Token);
            //}

            //// Only call when you no longer require the camera, i.e. on app shutdown.
            //cam.Cleanup();

        
        }

        public async Task CameraUp()
        {

        }
        public async Task CameraDown()
        {

        }

        public async Task CameraLeft()
        {

        }

        public async Task CameraRight()
        {

        }

        public async Task GetImage()
        {
           
            var captureHandler = new InMemoryCaptureHandler();
            using (captureHandler)
            {
                await cam.TakePicture(captureHandler, MMALEncoding.JPEG, MMALEncoding.I420);

                // Access raw unencoded output.
                ImageSrc = captureHandler.WorkingData.ToArray();
                ImageSize = ImageSrc.Length;
            }
        }
    }
}
