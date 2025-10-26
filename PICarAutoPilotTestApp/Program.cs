
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PICarAutoPilotLib;
using Ultraborg;
using L298NLibrary;
// using Ultraborg;
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostcontext, services) =>
    {
        services.AddPICarAutoPilotLibrary();

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
    }).Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var ultraborgAPI = host.Services.GetRequiredService<IUltraborgAPI>();
var autoPilot = host.Services.GetRequiredService<IPICarAutoPilot>();
var motorController= host.Services.GetRequiredService<IMotorController>();
ultraborgAPI.Setup();
motorController.Init(1,7,20,21);
autoPilot.GoForward(100, 1);
Console.ReadLine();