using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICarServerLib
{
    public class L298NMotorController
    {
        public int IN1 { get; set; }
        public int IN2 { get; set; }


        private PwmChannel _hrdwPWMChannel;

        private GpioController _gpioController;
        private double _speed = 0.5;

        public int PWMChannel { get; init; }
        public double DutyCycle
        {
            get
            {
                return _hrdwPWMChannel.DutyCycle;
            }
        }
        public L298NMotorController(GpioController gpioController, int pwmChannel)
        {
          
            _gpioController = gpioController;
            _hrdwPWMChannel = PwmChannel.Create(0, pwmChannel);
            PWMChannel = pwmChannel;


        }

        public L298NMotorController(GpioController gpioController, int pwmChannel, int in1, int in2)
        {

            _gpioController = gpioController;
            _hrdwPWMChannel = PwmChannel.Create(0, pwmChannel);
            PWMChannel = pwmChannel;
            IN1=in1;
            IN2=in2;
        }

        public void CleanUp()
        {

            _hrdwPWMChannel.Stop();
            _gpioController.ClosePin(IN1);
            _gpioController.ClosePin(IN2);
        }


        public void Init()
        {

            _hrdwPWMChannel.DutyCycle = 0.5;
            _speed = 0.5;
            _hrdwPWMChannel.Start();
            _gpioController.OpenPin(IN1, PinMode.Output);
            _gpioController.OpenPin(IN2, PinMode.Output);
        }

        public void UpdateSpeed(double speedFactor)
        {
            _hrdwPWMChannel.DutyCycle = speedFactor;
            _speed = speedFactor;
        }

        public void Stop()
        {
            _gpioController.Write(IN1, PinValue.Low);
            _gpioController.Write(IN2, PinValue.Low);
        }

        public void StartForward()
        {
            _gpioController.Write(IN1, PinValue.High);
            _gpioController.Write(IN2, PinValue.Low);
        }


        public void StartBack()
        {
            _gpioController.Write(IN1, PinValue.Low);
            _gpioController.Write(IN2, PinValue.High);
        }


    }
}
