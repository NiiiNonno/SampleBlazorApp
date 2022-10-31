using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SampleBlazorApp.Pages;
partial class Component1
{
    DateTime _date;
    IWebViewMenu _wVM;

    [Parameter]
    public IWebViewMenu WebViewMenu
    {
        get => _wVM;
        set
        {
            value.ReloadButtonClicked += (_, _) =>
            {
                _date = DateTime.Now;
                StateHasChanged();
            };
            _wVM = value;
        }
    }

    public Component1()
    {
    }
}
