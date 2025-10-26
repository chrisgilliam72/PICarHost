// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RGBLibrary;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        services.AddSingleton<IRGBController,RGBController>();   
    }).Build();


var rgbController= host.Services.GetRequiredService<IRGBController>();
rgbController.SetUp(14, 15, 18);
bool keeprunning=true;
Console.WriteLine("Waiting for keypress");
while (keeprunning)
{
    var keypress= Console.ReadKey();
    switch (keypress.Key)
    {
        case ConsoleKey.R: rgbController.LightsRed(true); break;
        case ConsoleKey.G: rgbController.LightsGreen(true); break;
        case ConsoleKey.B: rgbController.LightsBlue(true); break;
        case ConsoleKey.O: rgbController.LightsOff(); break;
        case ConsoleKey.A: rgbController.LightsOn(); break;        
        case ConsoleKey.X: keeprunning = false; break;
   
    }


  
}