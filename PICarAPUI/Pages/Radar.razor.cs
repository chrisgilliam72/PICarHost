using Iot.Device.Board;
using PICarServerLib;
using System.Device.Gpio;
using System.Device.I2c;
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

        private RaspberryPiBoard raspberryPibrd;
        private GpioController gpioController;
        private Ultraborg ultraborg;
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



        private Ultraborg CreateUltraBorgInstance()
        {
            int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
            Console.WriteLine("Bus No:" + busNo);
            var ic2Bus = I2cBus.Create(busNo);
            var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);

            Console.WriteLine(("No Bus items: " + busItems.Count));
            var ultraborg = new Ultraborg();
            ultraborg.Init(busNo, busItems[0]);
            return ultraborg;

        }
        protected override void OnInitialized()
        {
            try
            {
                raspberryPibrd = new RaspberryPiBoard();
                gpioController = raspberryPibrd.CreateGpioController();
                ultraborg = CreateUltraBorgInstance();
                motorProcessor = new L298NMotorProcessor(gpioController, 23, 24, 20, 21);
                motorProcessor.Init();

                base.OnInitialized();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

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
