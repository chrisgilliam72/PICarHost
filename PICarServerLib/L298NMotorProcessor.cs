
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICarServerLib
{
    public  class L298NMotorProcessor
    {
        private L298NMotorController _leftController;
        private L298NMotorController _rightController;

        public double SpeedFactor { get; set; }

        public int IN1
        {
            get
            {
                return _leftController.IN1;
            }
        }

        public int IN2
        {
            get
            {
                return _leftController.IN2;
            }
        }

        public int IN3
        {
            get
            {
                return _rightController.IN1;
            }
        }

        public int IN4
        {
            get
            {
                return _rightController.IN2;
            }
        }
        public int PWMLChannel
        {
            get
            {
                return _leftController.PWMChannel;
            }
        }
        public int PWMRChannel
        {
            get
            {
                return _rightController.PWMChannel;
            }
        }

        public L298NMotorProcessor(GpioController gpioController,int IN1, int IN2, int IN3, int IN4)
        {
             _leftController = new L298NMotorController(gpioController, 0, IN1, IN2);
            _rightController = new L298NMotorController(gpioController, 1, IN3, IN4);
        }

        public void Init()
        {
            _leftController.Init();
            _rightController.Init();
        }

        public void CleanUp()
        {
            _leftController.CleanUp();
            _rightController.CleanUp();
        }

        public void UpdateSpeedFactor(double speedFactor)
        {
            if (speedFactor <= 1)
            {
                Console.WriteLine("updating speed factor :" + speedFactor);
                LoggingProcessor.AddTrace("updating speed factor :" + speedFactor);
                this.SpeedFactor = speedFactor;
                _leftController.UpdateSpeed(speedFactor);
                _rightController.UpdateSpeed(speedFactor);
                Console.WriteLine("speed factor updated");
                LoggingProcessor.AddTrace("speed factor updated");
            }
            else
                Console.WriteLine("speed factor out of range");
                LoggingProcessor.AddTrace("speed factor out of range");  
        }

        public void StartForward()
        {
            Console.WriteLine("Start Forward");
            LoggingProcessor.AddTrace("Start Forward");
            _leftController.StartForward();
            _rightController.StartForward();
        }

        public void StartBackwards()
        {
            Console.WriteLine("Start Backwards");
            LoggingProcessor.AddTrace("Start Backwards");
            _leftController.StartBack();
            _rightController.StartBack();
        }

        public void StartTurnLeft()
        {
            Console.WriteLine("Start Turn Left");
            LoggingProcessor.AddTrace("Start Turn Left");
            _rightController.Stop();
            _leftController.StartForward();

        }

        public void StartTurnRight()
        {
            Console.WriteLine("Start Turn Right");
            LoggingProcessor.AddTrace("Start Turn Right");
            _leftController.Stop();
            _rightController.StartForward();
        }

        public void Stop()
        {
            Console.WriteLine("Stop");
            LoggingProcessor.AddTrace("Stop");
            _leftController.Stop();
            _rightController.Stop();
        }
    }
}
