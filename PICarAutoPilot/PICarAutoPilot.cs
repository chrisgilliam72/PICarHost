
using L298NLibrary;
using  Ultraborg;
using Microsoft.Extensions.Logging;
namespace PICarAutoPilotLib;

public class PICarAutoPilot : IPICarAutoPilot
{
    private readonly IUltraborgAPI _ultraBorgAPI;
    private readonly IMotorController _motorController;
    private readonly ILogger<PICarAutoPilot> _logger; 
    // private bool _stop;
    public PICarAutoPilot(IUltraborgAPI ultraBorgAPI, IMotorController motorController,
                        ILoggerFactory loggerFactory)
    {
        _ultraBorgAPI = ultraBorgAPI;
        _motorController = motorController;
        _logger=loggerFactory.CreateLogger<PICarAutoPilot>();   
    }

    public void GoForward(double stopDistance, int frontSensorNo)
    {
        try
        {
            _motorController.UpdateSpeedFactor(0.4);
            var currDistance = _ultraBorgAPI.GetDistance(frontSensorNo);
            Thread.Sleep(10);
            _logger.LogInformation($"start distance {currDistance} ");
            _logger.LogInformation($"distance param: {stopDistance} travel distance={currDistance - stopDistance}");
            _motorController.StartForward();

            while (true)
            {
                _logger.LogInformation("current distance: " + currDistance.ToString());
                currDistance = _ultraBorgAPI.GetDistance(frontSensorNo);
                Thread.Sleep(10);
                if (currDistance < stopDistance + 100)
                {
                    _motorController.Stop();
                    _logger.LogInformation($"final distance {currDistance} ");
                    return;
                }


            }

        }
        catch (Exception ex)
        {
           _logger.LogError(ex.Message);
        }
        finally
        {

            _motorController.Stop();
        }
    }
}