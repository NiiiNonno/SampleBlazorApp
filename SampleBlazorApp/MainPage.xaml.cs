using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using Grid = Microsoft.Maui.Controls.Grid;

namespace SampleBlazorApp;

public partial class MainPage : ContentPage
{
    ColorTheme Theme { get; set; }

	public MainPage()
	{
		InitializeComponent();
	}

	private async void BeadsView_Add(object sender, EventArgs e)
	{
        string action = await DisplayActionSheet("追加", "取消", null, "色見本", "文書", "項目");

        switch (action)
        {
        case "色見本":
            {
                for (int i = 0; i < BAMBOO_COLORS.Length; i++)
                {
                    beadsView_main.Beads.Add(new Rectangle
                    {
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
                        new RowDefinition(new GridLength(18)),
                        new RowDefinition(GridLength.Star),
                        new RowDefinition(new GridLength(18)),
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Star),
                    }
                };
                grid.Add(new Label
                {
                    Text = "BlazorWebView",
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = 18,
                }, 0, 0);
                grid.Add(new BlazorWebView
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
                            ComponentType = typeof(Main)
                        }
                    },
                }, 0, 1);

                beadsView_main.Beads.Add(grid);

                break;
            }
        }
    }

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
