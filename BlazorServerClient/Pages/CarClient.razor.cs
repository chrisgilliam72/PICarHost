using Microsoft.AspNetCore.Components;
using PanTiltHatLib;
using Ultraborg;


namespace BlazorServerClient.Pages;

partial class CarClient
{
    //test commit new commit 
    [Inject]
    public ILogger<CarClient>? Logger { get; set; }

    [Inject]
    public IUltraborgAPI? UltraborgAPI {get;set;}


    private Timer _distanceTimer  =null !;

    private bool _isDistanceSensorRunning = false;
    private string Distance {get;set;} = "";
    private double LastDistance {get;set;}


    protected override void OnInitialized()
    {
        LastDistance = 0.0;
            Distance = "";
    }
 

    private void PollDistance()
    {
       if (!_isDistanceSensorRunning)
        {
            _isDistanceSensorRunning = true;
            _distanceTimer = new Timer(async _ =>
            {
                await InvokeAsync(  () =>
                {
                    var ubDistance =UltraborgAPI?.GetDistance(1);
                    if (ubDistance.HasValue && Math.Abs(LastDistance-ubDistance.Value)>0.5)
                    {
                        Distance=string.Format("{0:F1}",ubDistance);
                    }

                    
                    StateHasChanged(); // Update the UI if needed
                });

            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100)); // Call every 10 seconds
        }
    }

}
