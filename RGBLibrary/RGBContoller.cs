
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PICarServerLib
{

    public class RGBContoller
    {
        const int PINB = 17;
        const int PINR = 22;
        const int PING = 27;
    

        private GpioController? gpioCntrller { get; set; }


        public RGBContoller()
        {
           

        }

        public void Init( GpioController gpio)
        {
            try
            {
                gpioCntrller = gpio;
                gpioCntrller.OpenPin(PINR, PinMode.Output);
                gpioCntrller.OpenPin(PING, PinMode.Output);
                gpioCntrller.OpenPin(PINB, PinMode.Output);
            }

            catch (Exception ex)
            {

                throw new Exception("The RGB controller threw an exception:" + ex.Message);
            }
        }


        public void CloseAll()
        {
            gpioCntrller.ClosePin(PINR);
            gpioCntrller.ClosePin(PING);
            gpioCntrller.ClosePin(PINB);
        }


        public void Test()
        {
            var mre = new ManualResetEventSlim(false);
            using (mre)
            {
                LightsOff();
              
                LightsRed(true);
                mre.Wait(TimeSpan.FromMilliseconds(500));
                mre.Reset();
             
                LightsRed(false);
                LightsGreen(true);
                mre.Wait(TimeSpan.FromMilliseconds(500));
                LightsGreen(false) ;
                mre.Reset();

                LightsBlue(true);
                mre.Wait(TimeSpan.FromMilliseconds(500));
                mre.Reset();
              
                LightsOff();
                mre.Wait(TimeSpan.FromMilliseconds(500));
                Console.WriteLine("Testing Servo");
                Console.WriteLine("Testing complete");
            }

        }

        public void LightsRed(bool ledOn)
        {
            Console.WriteLine("Lights Red:"+ ledOn);
            gpioCntrller.Write(PINR, ((ledOn) ? PinValue.High : PinValue.Low));
        }

        public void LightsBlue(bool ledOn)
        {
            Console.WriteLine("Lights Blue:"+ ledOn);
            gpioCntrller.Write(PINB, ((ledOn) ? PinValue.High : PinValue.Low));
        }

        public void LightsGreen(bool ledOn)
        {
            Console.WriteLine("Lights Green:"+ ledOn);
            gpioCntrller.Write(PING, ((ledOn) ? PinValue.High : PinValue.Low));
        }

        public void ToggleAllColors(bool ledOn)
        {
            if (ledOn)
                LightsOn();
            else
                LightsOff();
        }

        public void LightsOn()
        {
            Console.WriteLine("Lights on");
            //LoggingProcessor.AddTrace("Start LightsOn");
            LightsRed(true);
            LightsGreen(true);
            LightsBlue(true);
            //LoggingProcessor.AddTrace("End LightsOn");
            Console.WriteLine("End Lights on");
        }

        public void LightsOff()
        {
            Console.WriteLine("Lights off");
            //LoggingProcessor.AddTrace("Start LightsOff");
            LightsRed(false);
            LightsGreen(false);
            LightsBlue(false);
            //LoggingProcessor.AddTrace("End LightsOff");
            Console.WriteLine("End Lights off");
        }
    }
}
