using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace Nonno.SampleBlazorApp.WebAssembly.Shared;

public partial class PlesentationTimer
{
    readonly Timer _timer;
    TimeSpan _rest;
    TimeSpan _limit;
    string _buttonLabel;

    [Parameter]
    public TimeSpan Limit
    {
        get => _limit;
        set
        {
            _limit = value;
            _rest = value;
        }
    }

    public PlesentationTimer()
    {
        _buttonLabel = "開始";
        _timer = new Timer();
        _timer.Interval = 1000;
        _timer.Elapsed += (_, _) =>
        {
            _rest -= new TimeSpan(0, 0, 1);

            StateHasChanged();
        };
    }

    public void ButtonClick()
    {
        _timer.Enabled = !_timer.Enabled;
        _buttonLabel = _timer.Enabled ? "停止" : "開始";
    }
}
