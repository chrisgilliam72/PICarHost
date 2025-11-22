namespace BatteryLevelReporting;

internal class BatteryLevelReporting : IBatteryLevelReporting
{
    private Ina219? _ina219= null;

    public BatteryLevelReporting()
    {
       
    }

    public void Init(int address)
    {
        _ina219 = new Ina219(address: address);
    }

    public double GetBatteryLevel()
    {
        if (_ina219 is not null)
        {
            double busVoltage = _ina219.GetBusVoltage_V();
            double percent = (busVoltage - 6) / 2.4 * 100;
            if (percent > 100) percent = 100;
            if (percent < 0) percent = 0;

            return percent;
        }
        return -1;
    }
}