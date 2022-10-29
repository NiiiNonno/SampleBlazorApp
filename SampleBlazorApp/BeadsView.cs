using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls.Shapes;
using Nonno.Assets;
using Nonno.Assets.Collections;
using c = Nonno.Assets.Collections;

namespace SampleBlazorApp;

public class BeadsView : ContentView
{
	const int PAN_THRESHOLD = 30;

	readonly HorizontalStackLayout _content;
	readonly PanGestureRecognizer _panGR;
	readonly IBoundList<IView> _beads;
	bool _isSticked;
	double _x;

	public IBoundCollection<IView> Beads => _beads;
	public ColorTheme Theme { get; set; }
	public event EventHandler Add;

    public BeadsView()
	{
		var content = new HorizontalStackLayout
		{
			Margin = new Thickness(50, 0, 50, 0),
		};

		var panGestureRecognizer = new PanGestureRecognizer
		{
			
		};
		panGestureRecognizer.PanUpdated += PanUpdated;

		var beads = new BoundList<IView>(() => new ArrayList<IView>()) as IBoundList<IView>;
		beads.ItemAdded += BeadAdded;
		beads.ItemRemoved += BeadRemoved;
		beads.ItemReplaced += BeadReplaced;

        var addB = new Button 
		{ 
			BackgroundColor = new Color(171, 66, 17), 
			WidthRequest = 50,
			VerticalOptions = LayoutOptions.Fill,
			Text = "+"
		};
		addB.Clicked += OnAdd;

        _content = content;
		_panGR = panGestureRecognizer;
		_beads = beads;

		BackgroundColor = Colors.White;
		HorizontalOptions = LayoutOptions.Fill;
		VerticalOptions = LayoutOptions.Fill;
		Content = _content;
		GestureRecognizers.Add(panGestureRecognizer);

		Beads.Add(addB);
	}

	private void PanUpdated(object sender, PanUpdatedEventArgs e)
	{
		switch (e.StatusType)
		{
		case GestureStatus.Started:
			_isSticked = true;
			break;
		case GestureStatus.Running:
			if (_isSticked && e.TotalX is < PAN_THRESHOLD and > -PAN_THRESHOLD) break;
			_isSticked = false;
			TranslationX = _x + e.TotalX;
            break;
		case GestureStatus.Completed:
			_x = TranslationX;
			_isSticked = true;
            break;
		case GestureStatus.Canceled:
            break;
		}
	}

	private void OnAdd(object sender, EventArgs e) => Add?.Invoke(sender, e);

	private void BeadAdded(object sender, IBoundList<IView>.ItemAddedEventArgs e)
	{
		if (_content.Children.Count == 0) _content.Children.Add(e.Neo);
		else _content.Children.Insert(1, e.Neo);
	}

	private void BeadRemoved(object sender, IBoundList<IView>.ItemRemovedEventArgs e)
	{
		_content.Children.Remove(e.Old);
	}

	private void BeadReplaced(object sender, IBoundList<IView>.ItemReplacedEventArgs e)
	{
		_content.Children.Remove(e.Old);
		if (_content.Children.Count == 0) _content.Children.Add(e.Neo);
		else _content.Children.Insert(1, e.Neo);
	}
}

public enum ColorTheme
{
	Light,
	Dark,
}