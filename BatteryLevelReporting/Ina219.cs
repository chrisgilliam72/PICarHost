using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryLevelReporting;

public class Ina219
{
    // ====== Registers ======
    private const byte REG_CONFIG = 0x00;
    private const byte REG_SHUNTVOLTAGE = 0x01;
    private const byte REG_BUSVOLTAGE = 0x02;
    private const byte REG_POWER = 0x03;
    private const byte REG_CURRENT = 0x04;
    private const byte REG_CALIBRATION = 0x05;

    private readonly I2cDevice _device;

    private ushort _calibrationValue;
    private double _currentLsb;
    private double _powerLsb;

    public Ina219(int busId = 1, int address = 0x40)
    {
        var settings = new I2cConnectionSettings(busId, address);
        _device = I2cDevice.Create(settings);

        SetCalibration32V2A();
    }

    // ===================== LOW LEVEL ======================
    private ushort ReadRegister(byte reg)
    {
        Span<byte> write = stackalloc byte[1] { reg };
        Span<byte> read = stackalloc byte[2];

        _device.WriteRead(write, read);

        return (ushort)((read[0] << 8) | read[1]);
    }

    private void WriteRegister(byte reg, ushort value)
    {
        Span<byte> data = stackalloc byte[3];
        data[0] = reg;
        data[1] = (byte)(value >> 8);
        data[2] = (byte)(value & 0xFF);

        _device.Write(data);
    }

    // ===================== CALIBRATION ======================
    public void SetCalibration32V2A()
    {
        // Current LSB = 0.1mA per bit
        _currentLsb = 0.1; // mA

        // Calibration value
        _calibrationValue = 4096;

        // Power LSB = 20 × Current LSB = 2mW per bit
        _powerLsb = 0.002;

        WriteRegister(REG_CALIBRATION, _calibrationValue);

        // ===== CONFIG WORD =====
        ushort config =
            (ushort)((1 << 13) |            // Bus range 32V
                     (3 << 11) |            // Gain /8  (320mV)
                     (0x0D << 7) |          // Bus ADC 12-bit 32 samples
                     (0x0D << 3) |          // Shunt ADC 12-bit 32 samples
                     (7));                  // Continuous mode

        WriteRegister(REG_CONFIG, config);
    }

    // ===================== MEASUREMENTS ======================

    public double GetShuntVoltage_mV()
    {
        WriteRegister(REG_CALIBRATION, _calibrationValue);
        short raw = (short)ReadRegister(REG_SHUNTVOLTAGE);
        return raw * 0.01; // 10uV per bit
    }

    public double GetBusVoltage_V()
    {
        WriteRegister(REG_CALIBRATION, _calibrationValue);

        ushort raw = ReadRegister(REG_BUSVOLTAGE);
        raw >>= 3; // lower 3 bits are flags

        return raw * 0.004; // 4mV per bit
    }

    public double GetCurrent_mA()
    {
        short raw = (short)ReadRegister(REG_CURRENT);
        return raw * _currentLsb;
    }

    public double GetPower_W()
    {
        WriteRegister(REG_CALIBRATION, _calibrationValue);

        short raw = (short)ReadRegister(REG_POWER);
        return raw * _powerLsb;
    }
}