// See https://aka.ms/new-console-template for more information
using Iot.Device.Board;
using PICarHost;
using System.Device.I2c;

Console.WriteLine("Hello, World!");

var raspberryPibrd = new RaspberryPiBoard();
var controller = raspberryPibrd.CreateGpioController();

int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
Console.WriteLine("Bus No:" + busNo);
var ic2Bus = I2cBus.Create(busNo);
var busItems = I2cBusExtensions.PerformBusScan(ic2Bus);

var ultraborg = new Ultraborg();
ultraborg.Init(busNo, busItems[0]);

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