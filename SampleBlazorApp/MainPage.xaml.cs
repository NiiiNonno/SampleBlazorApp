using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using Nonno.Assets.Collections;
using Grid = Microsoft.Maui.Controls.Grid;

namespace SampleBlazorApp;

public partial class MainPage : ContentPage
{
    public static BeadsView BEADS;

	public MainPage()
	{
		InitializeComponent();

        BEADS = beadsView_main;
	}
}

public class MainPageViewModel : INotifyPropertyChanged
{
    ColorTheme Theme { get; set; }
    public ICommand ExitCommand { get; set; }
    public ICommand AddCommand { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public MainPageViewModel()
    {
        ExitCommand = new Command(() => Environment.Exit(0), () => true);
        AddCommand = new Command<string>(key => { switch (key)
            {
            case "色見本":
                {
                    for (int i = 0; i < BAMBOO_COLORS.Length; i++)
                    {
                        MainPage.BEADS.Beads.Add(new Rectangle
                        {
                            Margin = new Thickness(3, 0, 3, 0),
                            WidthRequest = 100,
                            MinimumHeightRequest = 1,
                            Fill = new SolidColorBrush(GetBambooColor(new(i, 0, 0, 0))),
                        });
                    }

                    break;
                }
            default:
                {
                    var grid = new Grid
                    {
                        BackgroundColor = GetBambooColor(TimeSpan.Zero),
                        RowDefinitions =
                    {
                        new RowDefinition(new GridLength(40)),
                        new RowDefinition(GridLength.Star),
                        new RowDefinition(new GridLength(40)),
                    },
                        ColumnDefinitions =
                    {
                        new ColumnDefinition(new GridLength(40)),
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(new GridLength(40)),
                    }
                    };

                    var sWVM = new ScrollWebViewMenu(grid);

                    var bWV = new BlazorWebView
                    {
                        MinimumWidthRequest = 400,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        HostPage = "wwwroot/index.html",
                        RootComponents =
                    {
                        new RootComponent
                        {
                            Selector = "#app",
                            ComponentType = typeof(Pages.Component1),
                            Parameters = new CompactDictionary<string, object>
                            {
                                { IWebViewMenu.PARAMETER_NAME, sWVM }
                            }
                        }
                    },
                    };

                    Grid.SetRow(bWV, 1);
                    Grid.SetColumnSpan(bWV, 3);
                    grid.Add(bWV);

                    MainPage.BEADS.Beads.Add(grid);

                    break;
                }
            } });
    }

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    static readonly Nonno.Assets.Graphics.Color[] BAMBOO_COLORS = new Nonno.Assets.Graphics.Color[]
    {
        new(112, 194, 135),
        new(121, 199, 129),
        new(140, 204, 129),
        new(160, 201, 131),
        new(187, 209, 132),
        new(214, 224, 132),
        new(224, 221, 137),
        new(219, 205, 127),
        new(207, 179, 109),
        new(168, 146, 99),
        new(110, 86, 58),
        new(82, 61, 46),
        new(46, 32, 24),
    };
    Color GetBambooColor(TimeSpan timeSpan)
    {
        double days = timeSpan.TotalHours / 24.0;

        switch (Theme)
        {
        case ColorTheme.Dark:
            {
                var v = (int)days;
                if (v < 0 || v >= BAMBOO_COLORS.Length) return Colors.Black;
                return Cast(BAMBOO_COLORS[v]);
            }
        default:
            {
                var v = (int)days;
                if (v < 0 || v >= BAMBOO_COLORS.Length) return Colors.Black;
                return Cast(BAMBOO_COLORS[v]);
            }
        }

        static Color Cast(Nonno.Assets.Graphics.Color color) => new(color.Red, color.Green, color.Blue);
    }
}

public class ControlMenu
{
    public event EventHandler Deleted;
    public event EventHandler Event;

    public void Delete() => Deleted?.Invoke(this, EventArgs.Empty);
    public void Invoke(object sender, EventArgs e) => Event?.Invoke(this, EventArgs.Empty);
}
