using System.Text.Json;

namespace WorldeGame
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            this.BackgroundColor = Color.FromArgb(ThemeConstants.GetBackgroundColor());

            Button playButton = CreateButton("Play", 500);
            Button helpButton = CreateButton("Help", 250);
            Button settingsButton = CreateButton("Settings", 250);

            // Set up event handlers
            playButton.Clicked += PlayButtonClicked;
            settingsButton.Clicked += SettingsButtonClicked;
            helpButton.Clicked += helpButtonClicked;

            // Add buttons to respective layouts
            topLevel.Children.Add(playButton);
            middleLevel.Children.Add(helpButton);
            middleLevel.Children.Add(settingsButton);
        }

        private Button CreateButton(string text, int width)
        {
            return new Button
            {
                Text = text,
                BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(1)),
                TextColor = Colors.Black,
                FontSize = 20,
                HeightRequest = 60,
                WidthRequest = width,
                CornerRadius = 10,
                Margin = 5,
                BorderColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
            };
        }

        private async void PlayButtonClicked(object sender, EventArgs e)
        {     
            await Navigation.PushAsync(new GamePage());
        }

        private async void SettingsButtonClicked(object sender, EventArgs e)
        {         
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void helpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HowTo());
        }

    }
}