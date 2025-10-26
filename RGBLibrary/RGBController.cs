
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using Iot.Device.Board;
using Microsoft.Extensions.Logging;


namespace RGBLibrary;

public class RGBController : IRGBController
{
    public int PINB { get; private set; } = 17;
    public int PINR { get; private set; } = 22;
    public int PING { get; private set; } = 27;

    private RaspberryPiBoard? _raspberryPiBoard;
    private GpioController? gpioCntrller { get; set; }

    private readonly ILogger<RGBController> _logger;
    public RGBController(ILogger<RGBController> logger)
    {
        _logger = logger;
        _raspberryPiBoard = new RaspberryPiBoard();
    }

    public void SetUp(int pinR, int pinG, int pinB)
    {

        try
        {
            PINR = pinR;
            PING = pinG;
            PINB = pinB;
            gpioCntrller = _raspberryPiBoard?.CreateGpioController();                
            gpioCntrller?.OpenPin(PINR, PinMode.Output);
            gpioCntrller?.OpenPin(PING, PinMode.Output);
            gpioCntrller?.OpenPin(PINB, PinMode.Output);
        }

        catch (Exception ex)
        {

            throw new Exception("The RGB controller threw an exception:" + ex.Message);
        }
    }



    public void CloseAll()
    {
        gpioCntrller?.ClosePin(PINR);
        gpioCntrller?.ClosePin(PING);
        gpioCntrller?.ClosePin(PINB);
    }


    public void Test()
    {
        _logger.LogInformation("Testing started.");
        var mre = new ManualResetEventSlim(false);
        using (mre)
        {
            LightsOff();
            
            LightsRed(true);
            mre.Wait(TimeSpan.FromMilliseconds(500));
            mre.Reset();
            
            LightsRed(false);
            LightsGreen(true);
            mre.Wait(TimeSpan.FromMilliseconds(500));
            LightsGreen(false) ;
            mre.Reset();

            LightsBlue(true);
            mre.Wait(TimeSpan.FromMilliseconds(500));
            mre.Reset();
            
            LightsOff();
            mre.Wait(TimeSpan.FromMilliseconds(500));
            
            _logger.LogInformation("Testing complete");
        }

    }

    public void LightsRed(bool ledOn)
    {
        _logger.LogInformation("Lights Red:"+ ledOn);
        gpioCntrller?.Write(PINR, ((ledOn) ? PinValue.High : PinValue.Low));
    }

    public void LightsBlue(bool ledOn)
    {
        _logger.LogInformation("Lights Blue:"+ ledOn);
        gpioCntrller?.Write(PINB, ((ledOn) ? PinValue.High : PinValue.Low));
    }

    public void LightsGreen(bool ledOn)
    {
        _logger.LogInformation("Lights Green:"+ ledOn);
        gpioCntrller?.Write(PING, ((ledOn) ? PinValue.High : PinValue.Low));
    }

    public void ToggleAllColors(bool ledOn)
    {
        if (ledOn)
            LightsOn();
        else
            LightsOff();
    }

    public void LightsOn()
    {
        _logger.LogDebug("Lights on");
        //LoggingProcessor.AddTrace("Start LightsOn");
        LightsRed(true);
        LightsGreen(true);
        LightsBlue(true);
        //LoggingProcessor.AddTrace("End LightsOn");
        _logger.LogDebug("End Lights on");
    }

    public void LightsOff()
    {
        _logger.LogDebug("Lights off");
        //LoggingProcessor.AddTrace("Start LightsOff");
        LightsRed(false);
        LightsGreen(false);
        LightsBlue(false);
        //LoggingProcessor.AddTrace("End LightsOff");
        _logger.LogDebug("End Lights off");
    }
}

