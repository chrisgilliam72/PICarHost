// See https://aka.ms/new-console-template for more information
using Iot.Device.Board;
using PICarServerLib;
using System.Device.I2c;

Console.WriteLine("Ultraborg Distance sensor test app");

var raspberryPibrd = new RaspberryPiBoard();
var controller = raspberryPibrd.CreateGpioController();

int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
Console.WriteLine("Bus No:" + busNo);

var ultraborg = new Ultraborg();
Console.WriteLine("Scanning for Ultraborg address...");
int address = ultraborg.GetUltraBorgAdress();
if (address!=-1)
{
    Console.WriteLine($"UltraBorg found on address {address}");
    ultraborg.Init(busNo, address);

    while (true)
    {
        var keyInfo = Console.ReadKey();
        switch (keyInfo.KeyChar)
        {

            case 'd': var distance = ultraborg.GetFilteredDistance(1); Console.WriteLine($"Filtered Distance: {distance}"); break;
            case 'u': var unfiltDistance = ultraborg.GetDistance(1); ; Console.WriteLine($"Unfiltered Distance: {unfiltDistance}"); break;
            case 'x': Environment.Exit(0); break;

        }
    }
}

Console.WriteLine("No UltraBorg detected");
Console.ReadLine();
