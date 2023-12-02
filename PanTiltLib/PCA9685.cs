using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanTiltHatLib
{

    internal class PCA9685
    {
        const int SUBADR1 = 0x02;
        const int SUBADR2 = 0x03;
        const int SUBADR3 = 0x04;
        const int MODE1 = 0x00;
        const int MODE2 = 0x01;
        const int PRESCALE = 0xFE;
        const int LED0_ON_L = 0x06;
        const int LED0_ON_H = 0x07;
        const int LED0_OFF_L = 0x08;
        const int LED0_OFF_H = 0x09;
        const int ALLLED_ON_L = 0xFA;
        const int ALLLED_ON_H = 0xFB;
        const int ALLLED_OFF_L = 0xFC;
        const int ALLLED_OFF_H = 0xFD;

        private I2cDevice? ubI2C;
        private bool _showDebug;
        public PCA9685()
        {
            _showDebug = false;
        }
        //testing
        public void Init(int busNo, int busAddress, bool showDebug = false)
        {
            _showDebug = showDebug;
            var settings = new I2cConnectionSettings(busNo, busAddress);
            ubI2C = I2cDevice.Create(settings);
            Console.WriteLine("Resetting PCA9685");
            Write(MODE1,0);
        }
        public void Write(byte reg, byte val)
        {

            byte[] sendBytes = new byte[] { reg, val };
            ubI2C?.Write(sendBytes);
            if (_showDebug)
                Console.WriteLine($"I2C: Write {val} to register {reg}");
        }

        public byte Read(byte reg)
        {
            ubI2C?.WriteByte(reg);
            byte[] recvBytes = new byte[1];
            ubI2C?.Read(recvBytes);
            if (_showDebug)
                Console.WriteLine($"I2C: returned {recvBytes[0]&0xff} from reg {reg}");
            return recvBytes[0];
        }

        public void SetPWMFreq(int freq)
        {
            var prescaleval = 25000000.0;
            prescaleval /= 4096.0;
            prescaleval /= (float)freq;
            prescaleval -= 1.0; ;
            if (_showDebug)
            {
                Console.WriteLine($"Setting PWM frequency to {freq} Hz");
                Console.WriteLine($"Estimated pre-scale: {prescaleval}");
            }

            var prescale = Math.Floor(prescaleval + 0.5);
            if (_showDebug)
                Console.WriteLine($"Final pre-scale: {prescale}");

            byte oldmode = Read(MODE1);
            byte newmode =(byte) ((oldmode & 0x7F) |  0x10);
            Write(MODE1, newmode);
            Write(PRESCALE, (byte)(Math.Floor(prescale)));
            Write(MODE1, oldmode);
            Thread.Sleep(5);
            Write(MODE1, (byte)(oldmode | 0x80));
            Write(MODE2, 0x04);
        }

        public void SetPWM(int channel, int on, int off)
        {
            Write((byte)(LED0_ON_L + 4 * channel), (byte)(on & 0xFF));
            Write((byte)(LED0_ON_H + 4 * channel), (byte)(on >> 8));
            Write((byte)(LED0_OFF_L + 4 * channel), (byte)(off & 0xFF));
            Write((byte)(LED0_OFF_H + 4 * channel), (byte)(off >> 8));
            if (_showDebug)
                Console.WriteLine($"channel {channel} LED_ON {on} LED_OFF {off}");
        }

        public void SetServoPulse(int channel, int pulse)
        {
            pulse = pulse * 4096 / 20000;
            SetPWM(channel, 0, pulse);
        }

        public void SetRotationAngle(int channel, int angle)
        {
            if (angle>=0 && angle<=180)
            {
                var temp = angle * (2000 / 180) + 501;
                SetServoPulse(channel, temp);
            }
        }
        public void StartPCA9685()
        {
            Write(MODE2, 0x04);

        }


        public void ExitPCA9685()
        {
            Write(MODE2, 0x00);
        }


    }
} 
