using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWASMClient.Pages;

partial class CarClient
{
    [Inject]
    public ILogger<CarClient> Logger { get; set; }
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; }
    const string _baseAddress = @"https://pi4b.local/";

    protected override async Task OnInitializedAsync()
    {
        // Define the hub connection
        var connection = new HubConnectionBuilder()
            .WithUrl("https://pi4b.local/DataStream") // Match the SignalR hub URL
            .Build();

        // Register a handler for receiving messages from the hub
        connection.On<string, string>("Pi Data Message", (clientName, clientMessage) =>
        {

        });

        try
        {
            // Start the connection
            await connection.StartAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
        }
        finally
        {
            // Stop the connection
            await connection.StopAsync();
        }
    }
    void OnCameraUp()
    {
        Logger.LogInformation("Camera Up");
        var client = HttpClientFactory.CreateClient();
        client.GetAsync(_baseAddress + "PanTilt/Up");

    }

    async Task OnCameraDown()
    {
        Logger.LogInformation("Camera Down");
        var client = HttpClientFactory.CreateClient();
        var result = await client.GetAsync(_baseAddress + @"PanTilt/Down");

    }

    void OnCameraRight()
    {
        Logger.LogInformation("Camera Right");
        var client = HttpClientFactory.CreateClient();
        client.GetAsync(_baseAddress + "PanTilt/Right");
    }

    void OnCameraLeft()
    {
        Logger.LogInformation("Camera Left");
        var client = HttpClientFactory.CreateClient();
        client.GetAsync(_baseAddress + "PanTilt/Left");

    }
    void OnCarRight()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Right");
    }


    void OnCarLeft()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Left");
    }

    void OnCarForward()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Forward");
    }
    void OnCarBack()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Back");
    }

   void OnCarSlower()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Slower");
    }

    void OnCarFaster()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Faster");
    }


    void OnCarStop()
    {
        var client = HttpClientFactory.CreateClient();
        Logger.LogInformation("Car Stop");
    }
}
