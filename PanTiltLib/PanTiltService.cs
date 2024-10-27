
using Iot.Device.Board;
using System.Device.I2c;
using Microsoft.Extensions.Logging;

namespace PanTiltHatLib
{
    public class PanTiltService : IPanTiltService
    {
        private const int MINHPOS=20;
        private const int MINVPOS=120;
        private const int MAXHPOS=170;
        private const int MAXVPOS=150;

        private int _hAngle;
        private int _vAngle;
        private int _vIncrement=5;
        private int _hIncrement=5;
        private int _defaultBus;
        private PCA9685 _PCA9685;
        private readonly ILogger<PanTiltService> _logger;

        public PanTiltService(ILogger<PanTiltService> logger)
        {
            _logger=logger;
            var raspberryPibrd = new RaspberryPiBoard();
            _PCA9685= new PCA9685();
            _defaultBus = raspberryPibrd.GetDefaultI2cBusNumber();
            logger.LogInformation("Default Bus no:"+_defaultBus);
            var ic2Bus = I2cBus.Create(_defaultBus);
            var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);
            _hAngle=0;
            _vAngle=0;
        }

        public bool Init(int busAddress, int Frequency)
        {
            
            _PCA9685.Init(_defaultBus, busAddress);
            _PCA9685.SetPWMFreq(Frequency);
            _PCA9685.StartPCA9685();
            Reset();
            return true;
        }

        public void SetIncrements(int hIncrement,int vIncrement)
        {
            _vIncrement=vIncrement;
            _hIncrement=hIncrement;
        }

        public void Stop()
        {
            _PCA9685.ExitPCA9685();
        }

        public void Reset()
        {
            _PCA9685.SetRotationAngle(1,90);
            _PCA9685.SetRotationAngle(0,70);
            _hAngle=90;
            _vAngle=70;
            _logger.LogInformation($"Reset request: H-Angle:{_hAngle}, V-Angle:{_vAngle}");
        }

        public int HPos(double pos)
        {
            _hAngle=Convert.ToInt32(pos);
           _PCA9685.SetRotationAngle(1,_hAngle);          
            _logger.LogInformation("Set HPOS: "+_hAngle);
            return _hAngle;
        }

        public int VPos(double pos)
        {
           _vAngle=Convert.ToInt32(pos);
            _PCA9685.SetRotationAngle(0,_vAngle);
           _logger.LogInformation("Set VPOS: "+_vAngle);
           return _vAngle;
        }
     
        public int HPos(double pos, int min, int max)
        {
           int range= max-min;
           int angle=Convert.ToInt32(range*pos);
           _PCA9685.SetRotationAngle(1,angle+min);
           _hAngle=angle+min;
            _logger.LogInformation("Set HPOS: "+_hAngle);
            return _hAngle;
        }

        public int VPos(double pos, int min, int max)
        {
           int range= max-min;
           int angle=Convert.ToInt32(range*pos);
           _PCA9685.SetRotationAngle(0,angle+min);
           _vAngle=angle+min;
           _logger.LogInformation("Set VPOS: "+_vAngle);
           return _vAngle;
        }
        public int Down()
        {
            if (_vAngle+_vIncrement<=180)
            {
                _logger.LogInformation("Up request");
                _PCA9685.SetRotationAngle(0,_vAngle+_vIncrement);
                _vAngle=_vAngle+_vIncrement;
                _logger.LogInformation($"V-Angle:{_vAngle}");
            }

            return _vAngle;
        }

        public int Up()
        {
            if (_vAngle-_vIncrement>=0)
            {
                _logger.LogInformation("Down request");
                _PCA9685.SetRotationAngle(0,_vAngle-_vIncrement);
                _vAngle=_vAngle-_vIncrement;
                _logger.LogInformation($"V-Angle:{_vAngle}");
            }

            return _vAngle;
        }

        public int Left()
        {
            if (_hAngle+_hIncrement<=180)
            {
                _logger.LogInformation("Left request");
                _PCA9685.SetRotationAngle(1,_hAngle-_hIncrement);
                _hAngle=_hAngle-_hIncrement;
                _logger.LogInformation($"H-Angle:{_hAngle}");
            }

            return _hAngle;

        }

        public int Right()
        {
            if (_hAngle-_hIncrement>=0)
            {
                _logger.LogInformation("Left request");
                _PCA9685.SetRotationAngle(1,_hAngle+_hIncrement);
                _hAngle=_hAngle+_hIncrement;
                _logger.LogInformation($"H-Angle:{_hAngle}");
            }

            return _hAngle;
        }

        public int CurrentHPosition()
        {
            return _hAngle;
        }

        public int CurrentVPosition()
        {
            return _vAngle;
        }
    }
}

