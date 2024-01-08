using Microsoft.Maui.Controls;

namespace WorldeGame
{
    public partial class LoginPage : ContentPage
    {
        private Entry UsernameEntry { get; set; }
        private Entry PasswordEntry { get; set; }
        private Button LoginButton { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            SetupUI();
            SetupEventHandlers();
        }

        //
        private void SetupUI()
        {
            var headerLabel = CreateHeaderLabel();
            UsernameEntry = CreateEntry("Username", false);
            PasswordEntry = CreateEntry("Password", true);
            LoginButton = CreateLoginButton();

            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { headerLabel, UsernameEntry, PasswordEntry, LoginButton }
            };
        }

        private Label CreateHeaderLabel()
        {
            return new Label
            {
                Text = "Login",
                FontSize = 30,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
        }

        private void SetupEventHandlers()
        {
            LoginButton.Clicked += LoginButtonClicked;
        }

        private Entry CreateEntry(string placeholder, bool isPassword)
        {
            return new Entry
            {
                Placeholder = placeholder,
                IsPassword = isPassword,
                HorizontalOptions = LayoutOptions.Center,
                PlaceholderColor = Colors.LightGray,
                WidthRequest = 240,
                Margin = new Thickness(0, 0, 0, isPassword ? 20 : 10),
                FontSize = 16
            };
        }

        private Button CreateLoginButton()
        {
            return new Button
            {
                Text = "Login",
                TextColor = Colors.YellowGreen,
                BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(1)),
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 120,
                Margin = new Thickness(0, 0, 0, 0)
            };
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;


            if (IsValidCredentials(username, password))
            {
                await NavigateToMainPage();
            }
            else
            {
                DisplayInvalidCredentialsAlert();
            }
        }

        private async Task NavigateToMainPage()
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void DisplayInvalidCredentialsAlert()
        {
            await DisplayAlert("Login Error", "Invalid username or password.", "OK");
        }

        private bool IsValidCredentials(string username, string password)
        {
            return !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);
        }
    }
}
