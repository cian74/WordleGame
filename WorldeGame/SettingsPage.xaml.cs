namespace WorldeGame
{
    public partial class SettingsPage : ContentPage
    {
        Switch darkModeSwitch;
        Switch hintsToggle;

        public SettingsPage()
        {
            InitializeComponent();
            CreateUI();
            ApplyDefaultSettings();
        }

        private void CreateUI()
        {
            this.BackgroundColor = Color.FromArgb(ThemeConstants.GetBackgroundColor());

            darkModeSwitch = CreateSwitch();
            layout.Children.Add(CreateLabel("Dark Mode"));
            layout.Children.Add(darkModeSwitch);

            hintsToggle = CreateSwitch();
            layout.Children.Add(CreateLabel("Easy Mode"));
            layout.Children.Add(hintsToggle);

            var fontSizeSlider = CreateSlider();
            layout.Children.Add(CreateLabel("Font Size"));
            layout.Children.Add(fontSizeSlider);

            layout.Children.Add(CreateStyledButton("Save", OnSaveButtonClicked, 1));
            layout.Children.Add(CreateStyledButton("Reset to Defaults", OnResetButtonClicked, 1));
            layout.Children.Add(CreateStyledButton("Save and Exit", OnSaveAndExitButtonClicked, 1));
            layout.Children.Add(CreateStyledButton("Exit without saving", ExitWithoutSavingButtonClicked, 1));
        }

        private Slider CreateSlider()
        {
            var slider = new Slider
            {
                Minimum = 10,
                Maximum = 40,
                Value = 20,  // Default font size value
                WidthRequest = 250,
                Margin = 5,
                HorizontalOptions = LayoutOptions.Center
            };
            slider.ValueChanged += OnFontSizeSliderValueChanged;
            return slider;
        }

        private void OnFontSizeSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Update font size for UI elements
            UpdateFontSize(e.NewValue);
        }

        private void UpdateFontSize(double fontSize)
        {
            // Update font size for existing UI elements or any specific UI elements you want
            // For example, updating the font size of a label:
            //ettingsPage.FontSize = fontSize;
        }

        private static Switch CreateSwitch()
        {
            return new Switch {};
        }

        private static Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text
            };
        }

        private static Button CreateStyledButton(string text, EventHandler handler, int buttonType)
        {
            var button = new Button
            {
                Text = text,
                BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(buttonType)),
                TextColor = Colors.Black,
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

        private void ApplyDefaultSettings()
        {
            darkModeSwitch.IsToggled = ThemeConstants.IsDarkMode();
            hintsToggle.IsToggled = ThemeConstants.IsHintsEnabled();
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            ResetToDefaults();
        }

        private void OnSaveAndExitButtonClicked(object sender, EventArgs e)
        {
            SaveAndExit();
        }

        private void ExitWithoutSavingButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void SaveSettings()
        {
            var IsDarkMode = darkModeSwitch.IsToggled;
            var showHints = hintsToggle.IsToggled;

            Preferences.Default.Set("dark_mode", IsDarkMode);
            Preferences.Default.Set("show_hints", showHints);
        }

        private void ResetToDefaults()
        {
            darkModeSwitch.IsToggled = false;
            hintsToggle.IsToggled = false;
            Preferences.Default.Set("dark_mode", false);
            Preferences.Default.Set("font-size", 20);
            Preferences.Default.Set("show_hints", false);
        }

        private void SaveAndExit()
        {
            SaveSettings();
            Navigation.PushAsync(new MainPage());
        }
    }
}
