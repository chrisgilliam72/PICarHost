namespace BatteryLevelReporting;

public interface IBatteryLevelReporting
{
   public void Init(int address);
   public double GetBatteryLevel();
}
