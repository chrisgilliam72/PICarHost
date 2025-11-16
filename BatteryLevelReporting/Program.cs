
using BatteryLevelReporting;
using System;
using System.Device.I2c;

var ina219 = new Ina219(address: 0x42);

while (true)
{
    double busVoltage = ina219.GetBusVoltage_V();
    double shuntVoltage = ina219.GetShuntVoltage_mV() / 1000.0;
    double current_mA = ina219.GetCurrent_mA();
    double power_W = ina219.GetPower_W();

    double percent = (busVoltage - 6) / 2.4 * 100;
    if (percent > 100) percent = 100;
    if (percent < 0) percent = 0;

    Console.WriteLine($"Load Voltage: {busVoltage:F3} V");
    Console.WriteLine($"Current:      {current_mA / 1000:F6} A");
    Console.WriteLine($"Power:        {power_W:F3} W");
    Console.WriteLine($"Percent:      {percent:F1}%\n");

    Thread.Sleep(2000);
}
    