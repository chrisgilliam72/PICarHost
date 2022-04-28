using PICarHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PICarServerLib
{
    public class CameraController
    {
        private UltraborgServo VServo { get; set; }
        private UltraborgServo HServo { get; set; }

        public int VServoNo { get; set; }
        public int HServoNo { get; set; }
        public CameraController()
        {
            VServo = new UltraborgServo(VServoNo, 0);
            HServo = new UltraborgServo(HServoNo, 0);
        }

        private void HServoToLeft()
        {
            HServo.ServoTo90();
        }

        private void HServoToCenter()
        {

            HServo.ServoTo0();
        }

        private void HServoToRight()
        {
            HServo.ServoTo270();
        }

        private void VServoUp()
        {
            HServo.ServoTo90();
        }

        private void VServoCenter()
        {

            HServo.ServoTo0();
        }

        private void VServoDown()
        {
            HServo.ServoTo270();
        }
    }
}
