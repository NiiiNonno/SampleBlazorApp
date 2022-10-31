#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBlazorApp;
public interface IWebViewMenu
{
    public const string PARAMETER_NAME = "WebViewMenu";

    public string Title { set; }
    public string Url { set; }
    public bool IsFixed { get; }
    public event EventHandler? ReloadButtonClicked;
    public event EventHandler? BackButtonClicked;
    public event EventHandler? ForwardButtonClicked;
}

public class ScrollWebViewMenu : IWebViewMenu
{
    readonly Button _titleB;
    readonly Button _reloadB;
    readonly Button _backB;
    readonly Button _forwardB;
    EventHandler? _rBC;
    EventHandler? _bBC;
    EventHandler? _fBC;
    bool _isFixed;

    public string Title { set => _titleB.Text = IsFixed ? "(fixed)" + value : value; }
    public string? Url { get; set; }
    public bool IsFixed
    {
        get => _isFixed;
        set
        {
            if (_isFixed == value) return;

            _isFixed = value;
            if (value)
            {
                _titleB.Text = "(fixed)" + _titleB.Text;
            }
            else
            {
                _titleB.Text = _titleB.Text["(fixed)".Length..];
            }
        }
    }

    public ScrollWebViewMenu(Grid grid)
    {
        var t = new Thickness(12, 0, 12, 0);

        var tB = new Button
        {
            Text = "BlazorWebView",
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            CornerRadius = 0,
            Padding = Thickness.Zero,
        };
        tB.Clicked += (_, _) => _isFixed = !_isFixed;
        Grid.SetRow(tB, 0);
        Grid.SetColumn(tB, 0);
        Grid.SetColumnSpan(tB, 2);
        grid.Add(tB);

        var rB = new Button
        {
            Text = "〇",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Fill,
            CornerRadius = 0,
            Padding = t,
        };
        rB.Clicked += OnReloadButtonClicked;
        Grid.SetRow(rB, 2);
        Grid.SetColumn(rB, 1);
        grid.Add(rB);

        var bB = new Button
        {
            Text = "<",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Fill,
            CornerRadius = 0,
            Padding = t,
        };
        bB.Clicked += OnBackButtonClicked;
        Grid.SetRow(bB, 2);
        Grid.SetColumn(bB, 0);
        grid.Add(bB);

        var fB = new Button
        {
            Text = ">",
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Fill,
            CornerRadius = 0,
            Padding = t,
        };
        fB.Clicked += OnForwardButtonClicked;
        Grid.SetRow(fB, 2);
        Grid.SetColumn(fB, 2);
        grid.Add(fB);

        _titleB = tB;
        _reloadB = rB;
        _backB = bB;
        _forwardB = fB;
    }

    public event EventHandler? ReloadButtonClicked
    {
        add
        {
            if (_rBC is null) _reloadB.IsEnabled = true;
            _rBC += value;
        }
        remove
        {
            _rBC -= value;
            if (_rBC is null) _reloadB.IsEnabled = false;
        }
    }
    public event EventHandler? BackButtonClicked
    {
        add
        {
            if (_bBC is null) _backB.IsEnabled = true;
            _bBC += value;
        }
        remove
        {
            _bBC -= value;
            if (_bBC is null) _backB.IsEnabled = false;
        }
    }
    public event EventHandler? ForwardButtonClicked
    {
        add
        {
            if (_fBC is null) _forwardB.IsEnabled = true;
            _fBC += value;
        }
        remove
        {
            _fBC -= value;
            if (_fBC is null) _forwardB.IsEnabled = false;
        }
    }

    public void OnReloadButtonClicked(object? sender, EventArgs e) => _rBC?.Invoke(sender, e);
    public void OnBackButtonClicked(object? sender, EventArgs e) => _bBC?.Invoke(sender, e);
    public void OnForwardButtonClicked(object? sender, EventArgs e) => _fBC?.Invoke(sender, e);
}
