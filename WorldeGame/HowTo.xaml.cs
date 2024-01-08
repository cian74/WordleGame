namespace WorldeGame;

public partial class HowTo : ContentPage
{
	public HowTo()
	{
		InitializeComponent();
		CreateUI();
	}

    private void CreateUI()
    {
        this.BackgroundColor = Color.FromArgb(ThemeConstants.GetBackgroundColor());
        layout.Children.Add(CreateStyledButton("Exit", OnExitButtonClicked, 1));
    }
    private static Button CreateStyledButton(string text, EventHandler handler, int buttonType)
    {
        var button = new Button
        {
            Text = text,
            BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(buttonType)),
            TextColor = Colors.White,
            FontSize = 20,
            HeightRequest = 60,
            WidthRequest = 250,
            CornerRadius = 10,
            Margin = 5,
            HorizontalOptions = LayoutOptions.Center
        };

        if (handler != null)
        {
            button.Clicked += handler;
        }

        return button;
    }
    private void OnExitButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}