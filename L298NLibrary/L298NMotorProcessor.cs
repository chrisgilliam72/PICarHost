
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iot.Device.Board;

namespace PICarServerLib
{
    public  class L298NMotorProcessor
    {
        private L298NMotorController? _leftController;
        private L298NMotorController? _rightController;
        private RaspberryPiBoard _raspberryPiBoard;
        private GpioController? _gpioOController;
        public double SpeedFactor { get; set; }

        public int IN1
        {
            get
            {
                return _leftController!=null ? _leftController.IN1 : 0;
            }
        }

        public int IN2
        {
            get
            {
                return _leftController!=null ? _leftController.IN2 :0;
            }
        }

        public int IN3
        {
            get
            {
                return _rightController!=null ?_rightController.IN1 : 0;
            }
        }

        public int IN4
        {
            get
            {
                return  _rightController!=null ? _rightController.IN2 : 0;
            }
        }
        public int PWMLChannel
        {
            get
            {
                return  _leftController!=null ?_leftController.PWMChannel: 0;
            }
        }
        public int PWMRChannel
        {
            get
            {
                return _rightController!=null ? _rightController.PWMChannel : 0;
            }
        }

        public L298NMotorProcessor(int IN1, int IN2, int IN3, int IN4)
        {
            _raspberryPiBoard = new RaspberryPiBoard();
            _gpioOController = _raspberryPiBoard?.CreateGpioController();

            if (_gpioOController!=null)
            {
                _leftController = new L298NMotorController(_gpioOController, 0, IN1, IN2);
                _rightController = new L298NMotorController(_gpioOController, 1, IN3, IN4);
            }

        }


        public void InitControllers()
        {
            _leftController?.Init();
            _rightController?.Init();
        }

        public void CleanUp()
        {
            _leftController?.CleanUp();
            _rightController?.CleanUp();
        }

        public void UpdateSpeedFactor(double speedFactor)
        {
            if (speedFactor <= 1 && _leftController!=null && _rightController!=null)
            {
                Console.WriteLine("updating speed factor :" + speedFactor);
                //LoggingProcessor.AddTrace("updating speed factor :" + speedFactor);
                this.SpeedFactor = speedFactor;
                _leftController.UpdateSpeed(speedFactor);
                _rightController.UpdateSpeed(speedFactor);
                Console.WriteLine("speed factor updated");
                //LoggingProcessor.AddTrace("speed factor updated");
            }
            else
                Console.WriteLine("speed factor out of range");
                //LoggingProcessor.AddTrace("speed factor out of range");  
        }

        public void StartForward()
        {            
            if (_leftController!=null && _rightController!=null)
            {
                Console.WriteLine("Start Forward");
                //LoggingProcessor.AddTrace("Start Forward");
                _leftController.StartForward();
                _rightController.StartForward();
            }
        }

        public void StartBackwards()
        {
            if (_leftController!=null && _rightController!=null)
            {
                Console.WriteLine("Start Backwards");
                //LoggingProcessor.AddTrace("Start Backwards");
                _leftController.StartBack();
                _rightController.StartBack();
            }

        }

        public void StartTurnLeft()
        {    
            if (_leftController!=null && _rightController!=null)
            {
                Console.WriteLine("Start Turn Left");
                //LoggingProcessor.AddTrace("Start Turn Left");
                _rightController.Stop();
                _leftController.StartForward();
            }
        }

        public void StartTurnRight()
        {
            if (_leftController!=null && _rightController!=null)
            {
                Console.WriteLine("Start Turn Right");
                //LoggingProcessor.AddTrace("Start Turn Right");
                _leftController.Stop();
                _rightController.StartForward();
            }
        }

        public void Stop()
        {
            if (_leftController!=null && _rightController!=null)
            {
                Console.WriteLine("Stop");
                //LoggingProcessor.AddTrace("Stop");
                _leftController.Stop();
                _rightController.Stop();
            }
        }
    }
}
