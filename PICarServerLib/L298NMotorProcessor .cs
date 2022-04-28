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
    public class L298NMotorProcessor : IDisposable
    {
        public int IN1 { get; set; }
        public int IN2 { get; set; }


        private PwmChannel _hrdwPWMChannel;
        //private SoftwarePwmChannel _pinENSpeed;
        private GpioController _gpioController;
        private double _speed = 0.5;

        public L298NMotorProcessor(GpioController gpioController)
        {
          
            _gpioController = gpioController;
            _hrdwPWMChannel=PwmChannel.Create(0, 0);
            //_pinENSpeed = new SoftwarePwmChannel(EN);
        }

        public void Dispose()
        {

            _hrdwPWMChannel.Start();
            _gpioController.ClosePin(IN1);
            _gpioController.ClosePin(IN2);
        }

        public void Init()
        {

            //_pinENSpeed.Start();
            _hrdwPWMChannel.Start();
            _gpioController.OpenPin(IN1, PinMode.Output);
            _gpioController.OpenPin(IN2, PinMode.Output);
        }

        public void Faster()
        {
            if (_speed<1.0)
            {
                _hrdwPWMChannel.DutyCycle = _speed + 0.1;
                _speed = _speed + 0.1;
            }

        }

        public void Slower()
        {
            if (_speed >0)
            {
                _hrdwPWMChannel.DutyCycle = _speed - 0.1;
                _speed = _speed - 0.1;
            }
        }
        public void Stop()
        {
            _gpioController.Write(IN1, PinValue.Low);
            _gpioController.Write(IN2, PinValue.Low);
        }

        public void Forward()
        {
            Console.WriteLine("Speed: "+_speed);
            Console.WriteLine(IN1 + "High "+ IN2 + "Low");
            _gpioController.Write(IN1, PinValue.High);
            _gpioController.Write(IN2, PinValue.Low);
        }


        public void Back()
        {
            Console.WriteLine("Speed: " + _speed);
            Console.WriteLine(IN1 + "Low "+ IN2 + "High");
            _gpioController.Write(IN1, PinValue.Low);
            _gpioController.Write(IN2, PinValue.High);
        }

        public void Status()
        {
           Console.WriteLine("IN1 :"+ _gpioController.Read(IN1));
           Console.WriteLine("IN2 :" + _gpioController.Read(IN2));
        }
    }
}
