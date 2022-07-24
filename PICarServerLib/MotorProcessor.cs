using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;
using System.Device.Pwm.Drivers;
using System.Device.Gpio;

namespace PICarServerLib
{
    public class MotorProcessor
    {


        //const int Left_motor_pwm = 16;
        //const int Right_motor_pwm = 13;

        //const int Left_motor_go = 19;
        //const int Left_motor_back = 12;
        //const int Right_motor_go = 20;
        //const int Right_motor_back = 21;

        const int Left_motor_pwm = 16; //white
        const int Right_motor_pwm = 13;//white

        const int Left_motor_go = 19; //blue
        const int Left_motor_back = 12;//blue
        const int Right_motor_go = 20;//green
        const int Right_motor_back = 21;//green


        private SoftwarePwmChannel pinLeftMotorPWM;
        private SoftwarePwmChannel pinRightMotorPWM;

        private GpioController _gpioController;
        public double SpeedFactor { get; set; }

       readonly ManualResetEvent CommandsWaitingEvent = new ManualResetEvent(false);

        public List<MotorCommand> CommandQueue { get; set; }

        public MotorProcessor()
        {
            pinLeftMotorPWM = new SoftwarePwmChannel(Left_motor_pwm);
            pinRightMotorPWM = new SoftwarePwmChannel(Right_motor_pwm);
            CommandQueue = new List<MotorCommand>();
            SpeedFactor = 0.5;
        }

        public String Init( GpioController gpioController)
        {
            try
            {

                _gpioController = gpioController;
                pinLeftMotorPWM.Start();
                pinRightMotorPWM.Start();
                _gpioController.OpenPin(Left_motor_go, PinMode.Output);
                _gpioController.OpenPin(Left_motor_back, PinMode.Output);
                _gpioController.OpenPin(Right_motor_go, PinMode.Output);
                _gpioController.OpenPin(Right_motor_back, PinMode.Output);

                var actionProcMessages = new Action(ProcessQueueMessages);
                Task.Run(actionProcMessages);
              

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("The motor processor threw an exception:" + ex.Message);
            }
        }

        public void ProcessQuery(String requestedAction, double parameterValue)
        {
           
            var motorCommand = new MotorCommand { Command = requestedAction, Parameter = parameterValue };
            CommandQueue.Add(motorCommand);

            CommandsWaitingEvent.Set();
        }

        public void ProcessQueueMessages()
        {
            Debug.WriteLine("Queue Processor has started");
            while (true)
            {
                CommandsWaitingEvent.WaitOne();
                do
                {
                    var nextCommand = CommandQueue.FirstOrDefault();
                    if (nextCommand != null)
                    {
                        CommandQueue.Remove(nextCommand);
                        ExcecuteCommand(nextCommand.Command, nextCommand.Parameter);
                    }
                }
                while (CommandQueue.Count > 0);
                CommandsWaitingEvent.Reset();
            }

           
        }

        public void ExcecuteCommand(String requestedAction, double parameterValue)
        {
            Console.WriteLine("Executing motor processor action:" + requestedAction);
            LoggingProcessor.AddTrace("Executing motor processor action:" + requestedAction);
            switch (requestedAction)
            {
                case "forward":  GoForward(parameterValue); break;
                case "back":  GoBackwards(parameterValue); break;
                case "right":  TurnRight(parameterValue); ; break;
                case "left":  TurnLeft(parameterValue); break;
                case "speed": UpdateSpeedFactor(parameterValue); break;
                case "stop": Stop(); break;
            }

            Console.WriteLine("Motor processor action completed");
            LoggingProcessor.AddTrace("Motor processor action completed");
        }
        
        public void UpdateSpeedFactor(double speedFactor)
        {
            Console.WriteLine("updating speed factor :"+ speedFactor);
            LoggingProcessor.AddTrace("updating speed factor :" + speedFactor);
            this.SpeedFactor = speedFactor;
            pinLeftMotorPWM.DutyCycle = speedFactor;
            pinRightMotorPWM.DutyCycle = speedFactor;
            Console.WriteLine("speed factor updated");
            LoggingProcessor.AddTrace("speed factor updated");
        }

        public void GoForward(double seconds)
        {
            var mre = new ManualResetEventSlim(false);
            using (mre)
            {
                StartForward();
                if (seconds > 0)
                {
                    Console.WriteLine("forward delay start :" + seconds);
                    LoggingProcessor.AddTrace("forward delay start :" + seconds);
                    mre.Wait(TimeSpan.FromSeconds(seconds));
                    Console.WriteLine("forward delay complete");
                    LoggingProcessor.AddTrace("forward delay complete");
                    Stop();
                }
            }
        }

        public void GoBackwards(double seconds)
        {
            var mre = new ManualResetEventSlim(false);
            using (mre)
            {
                StartBackwards();
                if (seconds > 0)
                {
                    Console.WriteLine("backwards delay start :" + seconds);
                    LoggingProcessor.AddTrace("backwards delay start :" + seconds);
                    mre.Wait(TimeSpan.FromSeconds(seconds));
                    Console.WriteLine("backwards backwards delay complete ");
                    LoggingProcessor.AddTrace("backwards delay complete");
                    Stop();
                }
            }

        }

        public void TurnRight(double seconds)
        {
            var mre = new ManualResetEventSlim(false);
            using (mre)
            {
                StartTurnRight();
                if (seconds > 0)
                {
                    Console.WriteLine("turn right delay start :" + seconds);
                    LoggingProcessor.AddTrace("turn right start :" + seconds);
                    mre.Wait(TimeSpan.FromSeconds(seconds));
                    Console.WriteLine("turn right end ");
                    LoggingProcessor.AddTrace("turn right end ");
                    Stop();
                    UpdateSpeedFactor(SpeedFactor);
                }
            }
        }

        public void TurnLeft(double seconds)
        {
            var mre = new ManualResetEventSlim(false);
            using (mre)
            {
                StartTurnLeft();
                if (seconds > 0)
                {
                    Console.WriteLine("turn left delay start :" + seconds);
                    LoggingProcessor.AddTrace("turn left start :" + seconds);
                    mre.Wait(TimeSpan.FromSeconds(seconds));
                    Console.WriteLine("turn left end :" + seconds);
                    LoggingProcessor.AddTrace("turn left end ");
                    Stop();
                    UpdateSpeedFactor(SpeedFactor);
                }
            }

        }

        public void StartForward()
        {
            Console.WriteLine("Start Forward Start");
            LoggingProcessor.AddTrace("Start Forward Start");
            _gpioController.Write(Left_motor_go, PinValue.High);
            _gpioController.Write(Right_motor_go, PinValue.High);
            _gpioController.Write(Left_motor_back, PinValue.Low);
            _gpioController.Write(Right_motor_back, PinValue.Low);
            Console.WriteLine("Start Forward End");
            LoggingProcessor.AddTrace("Start Forward End");

        }


        public void StartBackwards()
        {
            Console.WriteLine("Start Backward Start");
            LoggingProcessor.AddTrace("Start Backward Start");
            _gpioController.Write(Left_motor_go, PinValue.Low);
            _gpioController.Write(Right_motor_go, PinValue.Low);
            _gpioController.Write(Left_motor_back, PinValue.High);
            _gpioController.Write(Right_motor_back, PinValue.High);
            Console.WriteLine("Start Backward End");
            LoggingProcessor.AddTrace("Start Backward End");

        }

        public void StartTurnRight()
        {
            Console.WriteLine("Start TurnRight Start");
            LoggingProcessor.AddTrace("Start TurnRight Start");
            pinRightMotorPWM.DutyCycle = 0.2;
            _gpioController.Write(Left_motor_go, PinValue.Low);
            _gpioController.Write(Right_motor_go, PinValue.High);
            _gpioController.Write(Left_motor_back, PinValue.Low);
            _gpioController.Write(Right_motor_back, PinValue.Low);
            Console.WriteLine("Start TurnRight End");
            LoggingProcessor.AddTrace("Start TurnRight End");

        }

        public void StartTurnLeft()
        {
            Console.WriteLine("Start TurnLeft Start");
            LoggingProcessor.AddTrace("Start TurnLeft Start");
            pinLeftMotorPWM.DutyCycle = 0.2;
            _gpioController.Write(Left_motor_go, PinValue.High);
            _gpioController.Write(Right_motor_go, PinValue.Low);
            _gpioController.Write(Left_motor_back, PinValue.Low);
            _gpioController.Write(Right_motor_back, PinValue.Low);
            Console.WriteLine("Start TurnLeft End");
            LoggingProcessor.AddTrace("Start TurnLeft End");

        }

        public void Stop()
        {
            Console.WriteLine("Sending stop signal");
            LoggingProcessor.AddTrace("Sending stop signal");
            _gpioController.Write(Left_motor_go, PinValue.Low);
            _gpioController.Write(Right_motor_go, PinValue.Low);
            _gpioController.Write(Left_motor_back, PinValue.Low);
            _gpioController.Write(Right_motor_back, PinValue.Low);
            UpdateSpeedFactor(this.SpeedFactor);
            Console.WriteLine("stopped");
            LoggingProcessor.AddTrace("stop");
        }


    }

}
