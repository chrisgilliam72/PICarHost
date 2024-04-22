using L298NLibrary;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        services.AddSingleton<IMotorController,L298NMotorProcessor>();   
    }).Build();

Console.WriteLine("Initialising...");
var motorCntrller= host.Services.GetService<IMotorController>();
motorCntrller.Init(20,21,1,7);

while (true)
{
    var speedFactor = motorCntrller.SpeedFactor;
    var key = Console.ReadKey();
    switch (key.KeyChar)
    {
        case '1':Console.WriteLine("faster"); motorCntrller.UpdateSpeedFactor(speedFactor - 0.1); break;
        case '2':Console.WriteLine("slower"); motorCntrller.UpdateSpeedFactor(speedFactor+0.1); break;       
        case 'f': Console.WriteLine("Forward"); motorCntrller.StartForward(); ; break;
        case 'b': Console.WriteLine("Backwards"); motorCntrller.StartBackwards(); break;
        case 's': Console.WriteLine("Stop"); motorCntrller.Stop(); break;
        case 'x': motorCntrller.CleanUp(); Environment.Exit(0);break ;
    }
}
