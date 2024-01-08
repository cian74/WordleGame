
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Platform;
using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ConstrainedExecution;



namespace WorldeGame
{
    public partial class GamePage : ContentPage
    {

        // These constants are used as identifiers for when a letter is incorrect, correct or misplaced
        const int LETTER_INCORRECT = -1;
        const int LETTER_CORRECT = 1;
        const int LETTER_MISPLACED = 2;

        // Game status constants
        const int GAME_WON = 0;
        const int GAME_LOST = 1;
        private int currentRowIndex = 0;
        private int currentColumnIndex = 0;

        // The following variable increases/decreases depending on the amount of characters entered, this will allow me to set a limit of 5 characters entered
        private int indexedCharacters = 0;

        // The amount of attempts the user has made during the game
        private int tries = 0;

        private bool gameEndStatus = false;

        string randomWord; // Randomly chosen word in the game is stored as a string in this variable

        //guesses stored in lettersArray
        private char[] lettersArray = { '-', '-', '-', '-', '-'};

        //random word stored in charArray
        private char[] charArray;

        // Grid positions of all of the character buttons get indexed here
        Dictionary<char, int[]> letterPositons = new Dictionary<char, int[]>();

        private List<Color> randomColors = new List<Color> { };

        //constants used in the creation of the letterbox
        private const int Rows = 6;
        private const int Columns = 5;
        private const int FontSize = 20;
        private const int CellSize = 60;
        private const int CellMargin = 5;

        //constants used for the end of the game
        private const int ButtonHeight = 60;
        private const int ButtonExitWidth = 100;
        private const int ButtonReplayWidth = 150;
        private const int ButtonMargin = 5;
        private const int GridRows = 6;
        private const int GridColumns = 5;

        public GamePage()
        {
            InitializeComponent();

            randomWord = ListOfWords.GetRandomWord().ToUpper(); // Random word gets chosen

            charArray = randomWord.ToCharArray(); // String split up and stored in array

            CreateUI();

            InitializeLetterBox();
            
        }


        // Creates UI
        private void CreateUI()
        {
            this.BackgroundColor = Color.FromArgb(ThemeConstants.GetBackgroundColor());

            //creates keyboard grid with

            letterPositons['A'] = new int[] { 0, 0 };
            letterPositons['B'] = new int[] { 0, 1 };
            letterPositons['C'] = new int[] { 0, 2 };
            letterPositons['D'] = new int[] { 0, 3 };
            letterPositons['E'] = new int[] { 0, 4 };
            letterPositons['F'] = new int[] { 0, 5 };
            letterPositons['G'] = new int[] { 0, 6 };
            letterPositons['H'] = new int[] { 0, 7 };
            letterPositons['I'] = new int[] { 1, 0 };
            letterPositons['J'] = new int[] { 1, 1 };
            letterPositons['K'] = new int[] { 1, 2 };
            letterPositons['L'] = new int[] { 1, 3 };
            letterPositons['M'] = new int[] { 1, 4 };
            letterPositons['N'] = new int[] { 1, 5 };
            letterPositons['O'] = new int[] { 1, 6 };
            letterPositons['P'] = new int[] { 1, 7 };
            letterPositons['Q'] = new int[] { 2, 0 };
            letterPositons['R'] = new int[] { 2, 1 };
            letterPositons['S'] = new int[] { 2, 2 };
            letterPositons['T'] = new int[] { 2, 3 };
            letterPositons['U'] = new int[] { 2, 4 };
            letterPositons['V'] = new int[] { 2, 5 };
            letterPositons['W'] = new int[] { 2, 6 };
            letterPositons['X'] = new int[] { 2, 7 };
            letterPositons['Y'] = new int[] { 3, 0 };

            LoadBtns();
        }

        // The following method simply loops through the grid adding empty spaces that will be filled in by the player with their characters
        private void InitializeLetterBox()
        {
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    Label label = CreateLetterLabel(x == currentRowIndex);

                    lettersGrid.Children.Add(label);
                    Microsoft.Maui.Controls.Grid.SetRow(label, x);
                    Microsoft.Maui.Controls.Grid.SetColumn(label, y);
                }
            }
        }

        private Label CreateLetterLabel(bool isSelected)
        {
            return new Label
            {
                Text = " ",
                FontSize = FontSize,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = isSelected ? Colors.Black : Colors.White,
                BackgroundColor = isSelected ? Colors.White : Color.FromArgb(ThemeConstants.GetIncorrectLetterBackGroundColor()),
                Margin = new Thickness(CellMargin),
                HeightRequest = CellSize,
                WidthRequest = CellSize
            };
        }

        // Reapplys the styling back onto the grid boxes
        private void ReapplyLetterBox()
        {
            for (int y = 0; y < 5; y++) {
                 foreach (var child in lettersGrid.Children)
                    {
                     if (lettersGrid.GetRow(child) == currentRowIndex && lettersGrid.GetColumn(child) == y && child is Label label)
                        {

                        label.BackgroundColor = Colors.White;
                        label.TextColor = Colors.Black;
                       

                        break;
                     }
                }
            }
        }

        // Resets all of the character grid boxes when game resets
        private void ResetLetterBox()
        {
        
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    foreach (var child in lettersGrid.Children)
                    {
                        if (lettersGrid.GetRow(child) == x && lettersGrid.GetColumn(child) == y && child is Label label)
                        {
                            label.Text = "";
                            if (x != currentRowIndex)
                            { 
                                label.BackgroundColor = Colors.SlateGray;
                            }
                            else
                            {
                                label.BackgroundColor = Colors.White;
                            }
                            break;
                        }
                    }
                }
            }
        }
        // This method checks what char was selected and then adds it to the grid
        private void OnCharBtnClicked(object sender, EventArgs e)
        {
            if (!(sender is Button button) || indexedCharacters >= 5) return;

            char selectedLetter = GetSelectedLetter(button);
            AddLetterToGrid(selectedLetter);
            indexedCharacters++;
        }

        private char GetSelectedLetter(Button button)
        {
            return button.Text.Length > 0 ? button.Text[0] : 'A';
        }

        // Whenever the backspace is clicked, it will remove it from the grid
        private void OnBackspaceBtnClicked(object sender, EventArgs e)
        {
            if (indexedCharacters < 0) return;

            RemoveLetterFromGrid();
            UpdateCurrentIndices();
        }

        private void RemoveLetterFromGrid()
        {
            foreach (var child in lettersGrid.Children.OfType<Label>())
            {
                if (lettersGrid.GetRow(child) == currentRowIndex && lettersGrid.GetColumn(child) == currentColumnIndex - 1)
                {
                    child.Text = "";
                    break;
                }
            }
        }

        private void UpdateCurrentIndices()
        {
            if (--currentColumnIndex < 0)
    {
        if (--currentRowIndex < 0)
        {
            ResetIndices();
        }
        else
        {
            currentColumnIndex = lettersGrid.ColumnDefinitions.Count - 1;
        }
    }
        }

        private void ResetIndices()
        {
            currentRowIndex = 0;
            currentColumnIndex = 0;
        }

        // When the player has entered 5 characters, the submit button will check which chars was correct, misplaced, etc
        private void OnSubmitClicked(object sender, EventArgs e)
        {
            if (indexedCharacters < 4) return;

            int correct = 0;
            if (currentRowIndex >= 0 && currentRowIndex < randomWord.Length)
            {
                for (int column = 0; column < charArray.Length; column++)
                {
                    char currentLetter = lettersArray[column];
                    char correctLetter = charArray[column];

                    if (currentLetter == correctLetter)
                    {
                        SetCharGridStatus(currentRowIndex, column, LETTER_CORRECT);
                        correct++;
                    }
                    else if (StringContainsLetter(randomWord, currentLetter))
                    {
                        SetCharGridStatus(currentRowIndex, column, LETTER_MISPLACED);
                    }
                    else
                    {
                        SetCharGridStatus(currentRowIndex, column, LETTER_INCORRECT);
                        DeactivateButtonForIncorrectGuess(currentLetter);
                    }
                }
            }

            HandleHintLogic(correct);
            CheckGameCompletion(correct);

            UpdateGameStatus();
        }

        private int CheckGuess()
        {
            int correct = 0;
            if (currentRowIndex >= 0 && currentRowIndex < randomWord.Length)
            {
                for (int column = 0; column < charArray.Length; column++)
                {
                    char currentLetter = lettersArray[column];
                    char correctLetter = charArray[column];

                    if (currentLetter == correctLetter)
                    {
                        SetCharGridStatus(currentRowIndex, column, LETTER_CORRECT);
                        correct++;
                    }
                    else if (StringContainsLetter(randomWord, currentLetter))
                    {
                        SetCharGridStatus(currentRowIndex, column, LETTER_MISPLACED);
                    }
                    else
                    {
                        SetCharGridStatus(currentRowIndex, column, LETTER_INCORRECT);
                        DeactivateButtonForIncorrectGuess(currentLetter);
                    }
                }
            }
            return correct;
        }

        private void CheckLetterStatus(char currentLetter, char correctLetter, int column, ref int correct)
        {
            if (currentLetter == correctLetter)
            {
                SetCharGridStatus(currentRowIndex, column, LETTER_CORRECT);
                correct++;
            }
            else if (StringContainsLetter(randomWord, currentLetter))
            {
                SetCharGridStatus(currentRowIndex, column, LETTER_MISPLACED);
            }
            else
            {
                SetCharGridStatus(currentRowIndex, column, LETTER_INCORRECT);
                DeactivateButtonForIncorrectGuess(currentLetter);
            }
        }

        private void DeactivateButtonForIncorrectGuess(char incorrectLetter)
        {
            if (letterPositons.TryGetValue(incorrectLetter, out int[] position))
            {
                int rowA = position[0];
                int colA = position[1];
                DeactivateAndHighlightButton(rowA, colA);
            }
        }

        private void HandleHintLogic(int correct)
        {
            if (tries == 2 && ThemeConstants.IsHintsEnabled())
            {
                RevealGridCharacter();
                UpdateMessage("A letter has been unveiled, keep trying!");
            }
        }

        private void CheckGameCompletion(int correct)
        {
            if (correct >= 5)
            {
                EndGame(GAME_WON, "Congrats! You have won!");
                return;
            }

            if (tries == 5)
            {
                EndGame(GAME_LOST, "Correct Word: " + randomWord);
            }
        }

        private void EndGame(int gameState, string message)
        {
            ReapplyLetterBox();
            GameCompleted(gameState);
            UpdateMessage(message);
            // Additional logic if needed
        }

        private void UpdateGameStatus()
        {
            currentRowIndex++;
            tries++;
            currentColumnIndex = 0;
            indexedCharacters = 0;
        }

        // Method updates the header message above the grid
        private void UpdateMessage(String message)
        {
            headingLabel.Text = message;
        }

        // Reveals a random letter of the word and highlights it
        private void RevealGridCharacter()
        {
            Random random = new Random();

            // Get a random index within the length of randomWord
            int randomIndex = random.Next(randomWord.Length);

            // Convert the string to a character array
            char[] wordCharArray = randomWord.ToCharArray();

            // Get the randomly selected letter
            char randomLetter = wordCharArray[randomIndex];

            int[] position = letterPositons[randomLetter];
            int rowA = position[0];
            int colA = position[1];
            foreach (var child in backspaceSubmitGrid.Children)
            {
                if (child is Button button &&
                    Microsoft.Maui.Controls.Grid.GetRow(button) == rowA &&
                    Microsoft.Maui.Controls.Grid.GetColumn(button) == colA)
                {
                    // Deactivate the button and change its background color
                    button.IsEnabled = false;

                    button.BackgroundColor = Color.FromArgb(ThemeConstants.GetColor("yellow"));

                    break;
                }
            }
        }
        
        // Method simply deactivates and highlights the character button
        private void DeactivateAndHighlightButton(int row, int column)
        {
            // Iterate through the children of lettersGrid
            foreach (var child in backspaceSubmitGrid.Children)
            {
                if (child is Button button &&
                    Microsoft.Maui.Controls.Grid.GetRow(button) == row &&
                    Microsoft.Maui.Controls.Grid.GetColumn(button) == column)
                {
                    // Deactivate the button and change its background color
                    button.IsEnabled = false;
                    button.BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(3));
                    break; 
                }
            }
        }

        // Checks whether or not a string contains a character, returns true if so, false if not
        private static bool StringContainsLetter(String text, char expected)
        {
            char[] letters = text.ToCharArray();

            for(int i = 0; i < letters.Length; i++)
            {
                if(expected == letters[i])
                {
                    return true;
                }
            }

            return false;
        }

        // Changes the status of the character in the grid, making its design signal if it was correct, incorrect or misplaced
        private void SetCharGridStatus(int row, int column, int state)
        {
            // Find the label in the specified row and column
            foreach (var child in lettersGrid.Children)
            {
                if (lettersGrid.GetRow(child) == row && lettersGrid.GetColumn(child) == column && child is Label label)
                {

                    switch (state)
                    {
                        case LETTER_INCORRECT:
                            label.BackgroundColor = Color.FromArgb(ThemeConstants.GetIncorrectLetterBackGroundColor());
                            break;
                        case LETTER_CORRECT:
                            label.BackgroundColor = Color.FromArgb(ThemeConstants.GetCorrectLetterBackgroundColor());
                            break;
                        case LETTER_MISPLACED:
                            label.BackgroundColor = Color.FromArgb(ThemeConstants.GetMisplacedLetterBackgroundColor());
                            break;
                    }
                    
                    break;
                }
            }
        }

        // Adds a character to the grid in the correct column/row
        private void AddLetterToGrid(char letter)
        {
            lettersArray[currentColumnIndex] = letter;
            foreach (var child in lettersGrid.Children)
            {
                if (lettersGrid.GetRow(child) == currentRowIndex && lettersGrid.GetColumn(child) == currentColumnIndex && child is Label label)
                {
                    label.Text = "" + letter;
                    break;
                }
            }
            currentColumnIndex++;
        }

        // This method manages all of the resets of variables and game functions
        private void ResetGame()
        {
            backspaceSubmitGrid.Children.Clear();
            currentRowIndex = 0;
            currentColumnIndex = 0;
            indexedCharacters = 0;
            gameEndStatus = false;
          
            tries = 0;

            randomWord = ListOfWords.GetRandomWord().ToUpper();
            for(int i = 0; i < 5; i++) {
                lettersArray[i] = ' ';
            }
            charArray = randomWord.ToCharArray();

            UpdateMessage("");
            ResetLetterBox();
            LoadBtns();
        }

        // When the UI is being created, this method is called so the character buttons can be loaded in
        private void LoadBtns()
        {
            backspaceSubmitGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            backspaceSubmitGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            backspaceSubmitGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            backspaceSubmitGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            backspaceSubmitGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            backspaceSubmitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });


            lettersArray = new char[] { '-', '-', '-', '-', '-' };
            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXY".ToCharArray();

            for (int i = 0; i < alphabet.Length; i++)
            {
                int[] position = letterPositons[alphabet[i]];
                int row = position[0];
                int col = position[1];

                var letterButton = new Button
                {
                    Text = alphabet[i].ToString(),
                    BackgroundColor = Colors.SlateGray,
                    TextColor = Colors.White,
                    FontSize = 20,
                    HeightRequest = 60,
                    WidthRequest = 60,
                    CornerRadius = 10,
                    Margin = new Thickness(5)
                };

                letterButton.Clicked += OnCharBtnClicked;

                backspaceSubmitGrid.Children.Add(letterButton);
                Microsoft.Maui.Controls.Grid.SetRow(letterButton, row);
                Microsoft.Maui.Controls.Grid.SetColumn(letterButton, col);

                var backspaceButton = new Button
                {
                    Text = "Backspace",
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black,
                    BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(1)),
                    CornerRadius = 10,
                    HeightRequest = 60,
                    WidthRequest = 150
                };

                backspaceButton.Clicked += OnBackspaceBtnClicked;

                var submitButton = new Button
                {
                    Text = "Submit",
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.White,
                    BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(3)),
                    CornerRadius = 10,
                    HeightRequest = 60,
                    WidthRequest = 150
                };

                submitButton.Clicked += OnSubmitClicked;

                var exitButton = new Button
                {
                    Text = "Exit",
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.White,
                    BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(3)),
                    CornerRadius = 10,
                    HeightRequest = 60,
                    WidthRequest = 150
                };

                exitButton.Clicked += (sender, e) =>
                {
                    Navigation.PushAsync(new MainPage());
                };


                backspaceSubmitGrid.Children.Add(backspaceButton);
                backspaceSubmitGrid.Children.Add(submitButton);
                backspaceSubmitGrid.Children.Add(exitButton);

                letterButton.IsEnabled = true;
                backspaceButton.IsEnabled = true;
                submitButton.IsEnabled = true;
                exitButton.IsEnabled = true;


                Microsoft.Maui.Controls.Grid.SetRow(backspaceButton, 4);
                Microsoft.Maui.Controls.Grid.SetColumn(backspaceButton, 0);
                Microsoft.Maui.Controls.Grid.SetColumnSpan(backspaceButton, 2);

                Microsoft.Maui.Controls.Grid.SetRow(submitButton, 4);
                Microsoft.Maui.Controls.Grid.SetColumn(submitButton, 2);
                Microsoft.Maui.Controls.Grid.SetColumnSpan(submitButton, 2);

                Microsoft.Maui.Controls.Grid.SetRow(exitButton, 4);
                Microsoft.Maui.Controls.Grid.SetColumn(exitButton, 4);
                Microsoft.Maui.Controls.Grid.SetColumnSpan(exitButton, 2);
            }
        }

        // Once the game finishes, this method will be called into action
        private async void GameCompleted(int gameStatus)
        {
            backspaceSubmitGrid.Children.Clear();
            gameEndStatus = true;

            AddMenuButton();
            AddReplayButton(); 
        }
        private void AddMenuButton()
        {
            var menuButton = CreateButton("Exit", ButtonExitWidth, () => Navigation.PushAsync(new MainPage()));
            backspaceSubmitGrid.Children.Add(menuButton);
            Microsoft.Maui.Controls.Grid.SetRow(menuButton, 0);
            Microsoft.Maui.Controls.Grid.SetColumn(menuButton, 0);
        }

        private void AddReplayButton()
        {
            var replayButton = CreateButton("Play Again", ButtonReplayWidth, ResetGame);
            backspaceSubmitGrid.Children.Add(replayButton);
            Microsoft.Maui.Controls.Grid.SetRow(replayButton, 0);
            Microsoft.Maui.Controls.Grid.SetColumn(replayButton, 1);
        }

        private Button CreateButton(string text, int width, Action action)
        {
            var button = new Button
            {
                Text = text,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.YellowGreen,
                BackgroundColor = Color.FromArgb(ThemeConstants.GetButtonBackgroundColor(0)),
                Margin = new Thickness(ButtonMargin),
                HeightRequest = ButtonHeight,
                WidthRequest = width
            };

            button.Clicked += (sender, e) => action.Invoke();

            return button;
        }

    }

}