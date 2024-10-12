using Iot.Device.Board;
using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Sonic Sensor Stream running");

var raspberryPibrd = new RaspberryPiBoard();
var controller = raspberryPibrd.CreateGpioController();

int busNo = raspberryPibrd.GetDefaultI2cBusNumber();
Console.WriteLine("Bus No:" + busNo);

var ultraborg = new Ultraborg.Library.Ultraborg();
Console.WriteLine("Scanning for Ultraborg address...");
int address = ultraborg.GetUltraBorgAdress();
if (address != -1)
{
    Console.WriteLine($"UltraBorg found on address {address}");
    if (ultraborg.Init(busNo, address))
    {
        Console.WriteLine("Ultraborg Library initialised");
        Console.WriteLine("Connecting to SignalR Hub");
        var connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:8070/DataStream")
                    .Build();

        Console.WriteLine("SignalR Hub State:"+ connection.State.ToString());
        await connection.StartAsync();
        if (connection.State == HubConnectionState.Connected)
        {
            Console.WriteLine("SignalR Hub State:" + connection.State.ToString());
            Console.WriteLine("Connected to Hub");
            while (true)
            {

                var distance = ultraborg.GetFilteredDistance(1);
                Console.WriteLine($"Filtered Distance: {distance}");
                await connection.InvokeCoreAsync("SendMessage", args: new[] { "PICar filtered distance", distance.ToString() });
                Thread.Sleep(500);
                if (Console.KeyAvailable)
                {
                    // Exit loop if a key is pressed
                    Console.ReadKey(true); // Consume the key press
                    Console.WriteLine("Key pressed, exiting...");
                    Environment.Exit(0);
                }
            }
        }
        else
        {
            Console.WriteLine("Connection to SignalR Hub could not be established.");
            Environment.Exit(0);
        }


    }
    Console.WriteLine("Ultraborg Library initialisation failure");
    Console.ReadLine();

}
else
{
    Console.WriteLine("No UltraBorg detected");
}

