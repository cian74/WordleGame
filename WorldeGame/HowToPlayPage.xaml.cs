namespace WorldeGame
{
    public partial class HowToPlayPage : ContentPage
    {

        public HowToPlayPage()
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
                BackgroundColor = Colors.White,
                TextColor = Colors.Black,
                FontSize = 20,
                HeightRequest = 60,
                WidthRequest = 250,
                CornerRadius = 10,
                Margin = 5,
                BorderWidth = 2.5,
                BorderColor = Colors.Black,
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
}
