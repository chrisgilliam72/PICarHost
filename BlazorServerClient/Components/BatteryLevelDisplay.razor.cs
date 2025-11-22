using BatteryLevelReporting;
using Microsoft.AspNetCore.Components;

namespace BlazorServerClient.Components
{
    public partial class BatteryLevelDisplay
    {
        [Inject]
        required public ILogger<BatteryLevelDisplay> Logger { get; set; }
        [Inject]
        public required IBatteryLevelReporting BatteryLevelReporting { get; set; }
        public double BatteryPercent { get; set; } = 100;
        public int LowThreshold { get; set; } = 20;
        private string FillColor => BatteryPercent <= LowThreshold ? "red" : "green";
        private string FillWidth => $"{BatteryPercent}%";
        private Timer batteryLevelTimer = null!;
        private bool timerRunning = false;


        protected override void OnInitialized()
        {
            BatteryLevelReporting.Init(0x42);

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!timerRunning)
            {
                timerRunning = true;
                batteryLevelTimer = new Timer(async _ =>
                {
                    await InvokeAsync(() =>
                    {
                        if (BatteryLevelReporting is not null)
                        {
                            BatteryPercent = BatteryLevelReporting.GetBatteryLevel();
                            Logger.LogInformation($"Battery level : {BatteryPercent}");
                            StateHasChanged();
                        }

                    });

                }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000)); // Call every 10 seconds
            }
        }
    }
}