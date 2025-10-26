
namespace RGBLibrary;

public interface IRGBController
{
    public int PINB { get; }
    public int PINR { get; }
    public int PING { get; }
    public void SetUp(int pinR, int pinG, int pinB);
    public void Test();
   
    public void LightsGreen(bool ledOn);
     public void LightsBlue(bool ledOn);
     
     public void LightsRed(bool ledOn);
     public void ToggleAllColors(bool ledOn);

    public void LightsOn();

    public void LightsOff();
}