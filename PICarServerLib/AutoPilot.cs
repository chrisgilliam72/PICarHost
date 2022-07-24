using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PICarServerLib
{
    public class AutoPilot
    {
        private readonly Ultraborg _ultraBorg;
        private readonly L298NMotorProcessor _l298NMotorController;
        public AutoPilot(Ultraborg ultraBorg, L298NMotorProcessor l298NMotorController)
        {
            _ultraBorg=ultraBorg;
            _l298NMotorController=l298NMotorController;
        }

        public void GoUntil(double distance, int frontSensorNo)
        {
            try
            {
                _l298NMotorController.UpdateSpeedFactor(0.4);
                var currDistance = _ultraBorg.GetFilteredDistance(frontSensorNo);
                Thread.Sleep(10);
                Console.WriteLine($"start distance {currDistance} ");
                Console.WriteLine($"distance param: {distance} travel distance={currDistance-distance}");
                _l298NMotorController.StartForward();

                while (true)
                {
                    Console.WriteLine("current distance: "+ currDistance.ToString());
                    currDistance = _ultraBorg.GetFilteredDistance(frontSensorNo);
                    Thread.Sleep(10);
                    if (currDistance < distance + 100)
                    {
                        _l298NMotorController.Stop();
                        Console.WriteLine($"final distance {currDistance} ");
                    }
                        
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                _l298NMotorController.Stop();
            }

        }
    }
}
