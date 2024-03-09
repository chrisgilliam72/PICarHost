
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iot.Device.Board;
using Microsoft.Extensions.Logging;

namespace L298NLibrary
{
    //testing
    public  class L298NMotorProcessor : IMotorController 
    {
        private L298NMotorController? _leftController;
        private L298NMotorController? _rightController;
        private RaspberryPiBoard? _raspberryPiBoard;
        private GpioController? _gpioOController;
        private readonly ILogger _logger;
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

        public L298NMotorProcessor(ILoggerFactory loggerFactory)
        {
            _logger=loggerFactory.CreateLogger("L298NMotorProcessor");
            _raspberryPiBoard = new RaspberryPiBoard();
        }


        public void Init(int IN1, int IN2, int IN3, int IN4)
        {     
            _gpioOController = _raspberryPiBoard?.CreateGpioController();

            if (_gpioOController!=null)
            {
                _leftController = new L298NMotorController(_gpioOController, 0, IN1, IN2);
                _rightController = new L298NMotorController(_gpioOController, 1, IN3, IN4);
            }
            _leftController?.Init();
            _rightController?.Init();
        }

        public void CleanUp()
        {
            _leftController?.CleanUp();
            _rightController?.CleanUp();
        }

        public double UpdateSpeedFactor(double speedFactor)
        {
            if (speedFactor <= 1 && _leftController!=null && _rightController!=null)
            {
                _logger.LogInformation("updating speed factor :" + speedFactor);
                this.SpeedFactor = speedFactor;
                _leftController.UpdateSpeed(speedFactor);
                _rightController.UpdateSpeed(speedFactor);
                 _logger.LogInformation("speed factor updated");
        
            }
            else
                _logger.LogInformation("speed factor out of range");
                
            return speedFactor;
        }

        public void StartForward()
        {            
            if (_leftController!=null && _rightController!=null)
            {
                 _logger.LogInformation("Start Forward");
                _leftController.Forward();
                _rightController.Forward();
            }
        }

        public void StartBackwards()
        {
            if (_leftController!=null && _rightController!=null)
            {
                 _logger.LogInformation("Start Backwards");
                _leftController.Back();
                _rightController.Back();
            }

        }

        public void StartTurnLeft()
        {    
            if (_leftController!=null && _rightController!=null)
            {
                 _logger.LogInformation("Start Turn Left");
                _rightController.Stop();
                _leftController.Forward();
            }
        }

        public void StartTurnRight()
        {
            if (_leftController!=null && _rightController!=null)
            {
                 _logger.LogInformation("Start Turn Right");
                _leftController.Stop();
                _rightController.Forward();
            }
        }

        public void Stop()
        {
            if (_leftController!=null && _rightController!=null)
            {
                 _logger.LogInformation("Stop");
                _leftController.Stop();
                _rightController.Stop();
            }
        }
    }
}
