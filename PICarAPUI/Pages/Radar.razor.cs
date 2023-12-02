using Iot.Device.Board;
using PICarServerLib;
using System.Device.Gpio;
using System.Device.I2c;
using Ultraborg.Library;
namespace PICarAPUI.Pages
{
    public partial class Radar
    {
        public String TopDistance1 { get; set; }
        public String TopDistance2 { get; set; }
        public String TopDistance3 { get; set; }
        public String LDistance1 { get; set; }
        public String LDistance2 { get; set; }
        public String LDistance3 { get; set; }
        public String RDistance1 { get; set; }
        public String RDistance2 { get; set; }
        public String RDistance3 { get; set; }


        private Ultraborg.Library.Ultraborg ultraborg;
        private L298NMotorProcessor motorProcessor;

        public Radar()
        {
            TopDistance1 = "";
            TopDistance2 = "";
            TopDistance3 = "";
            LDistance1 = "";
            LDistance2 = "";
            LDistance3 = "";
            RDistance1 = "";
            RDistance2 = "";
            RDistance3 = "";
        }



        private Ultraborg.Library.Ultraborg CreateUltraBorgInstance()
        {

            var ultraborg = new Ultraborg.Library.Ultraborg();
            int address = ultraborg.GetUltraBorgAdress();
            ultraborg.Init(1, address);
            return ultraborg;

        }
        //protected override void OnInitialized()
        //{
        //    try
        //    {
        //        var raspberryPibrd = new RaspberryPiBoard();
        //        var gpioController = raspberryPibrd.CreateGpioController();
        //        ultraborg = CreateUltraBorgInstance();
        //        motorProcessor = new L298NMotorProcessor(gpioController, 23, 24, 20, 21);
        //        motorProcessor.Init();

        //        base.OnInitialized();
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public void Go()
        {
            Console.Write("Start Autopilot");
            var autoPilot = new AutoPilot(ultraborg, motorProcessor);
            autoPilot.GoUntil(100, 1);
            Console.Write("End Autopilot");
        }

        public void Stop()
        {

        }
    }
}
