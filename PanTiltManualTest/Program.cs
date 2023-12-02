// See https://aka.ms/new-console-template for more information
using PanTiltHatLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        services.AddSingleton<IPanTiltService,PanTiltService>();   
    }).Build();

Console.WriteLine("Initialising...");
var panTiltService= host.Services.GetService<IPanTiltService>();
if (panTiltService!=null)
{
    panTiltService.Init(0x40,60);
    panTiltService.Reset();
    panTiltService.VPos(0.75);
    panTiltService.HPos(0.75);
    bool keeprunning=true;
    Console.WriteLine("Waiting for keypress");
    while (keeprunning)
    {
        var keypress= Console.ReadKey();
        switch (keypress.Key)
        {
                case ConsoleKey.UpArrow: panTiltService.Up();break;
                case ConsoleKey.DownArrow: panTiltService.Down();break;
                case ConsoleKey.LeftArrow: panTiltService.Left();break;
                case ConsoleKey.RightArrow: panTiltService.Right();break;
                case ConsoleKey.I: panTiltService.Reset();break;
                case ConsoleKey.X: panTiltService.Stop();keeprunning=false;break;
        }


        Console.WriteLine($"hPOS {panTiltService.CurrentHPosition()} VPost {panTiltService.CurrentVPosition()}");
    }
}
