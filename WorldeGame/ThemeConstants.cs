﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace WorldeGame
{
    internal class ThemeConstants
    {

        public const int PRIMARY_TYPE = 1,
                         SECONDARY_TYPE = 2,
                         TERTIARY_TYPE = 3;

        public const String LM_BACKGROUND = "#bcbcbc",

                            LM_CORRECT_LETTER_BACKGROUND = "#73c981",
                            LM_INCORRECT_LETTER_BACKGROUND = "#5c5c5c",
                            LM_MISPLACED_LETTER_BACKGROUND = "#ede27e",
                            LM_BUTTON_BACKGROUND_COLOR_PRIMARY = "#ffffff",
                            LM_BUTTON_BACKGROUND_COLOR_SECONDARY = "##d7d7d7",
                            LM_BUTTON_BACKGROUND_COLOR_TERTIARY = "#c7c4bd",

                            LM_RED = "#e8633a",
                            LM_ORANGE = "#e8ba3a",
                            LM_YELLOW = "#e5e83a",
                            LM_GREEN = "#57e83a",
                            LM_BLUE = "#3a97e8";

        public const String DM_BACKGROUND = "#000000",

                            DM_CORRECT_LETTER_BACKGROUND = "#63996c",
                            DM_INCORRECT_LETTER_BACKGROUND = "#404040",
                            DM_MISPLACED_LETTER_BACKGROUND = "#968f50",
                            DM_BUTTON_BACKGROUND_COLOR_PRIMARY = "#7d7b74",
                            DM_BUTTON_BACKGROUND_COLOR_SECONDARY = "#6b6659",
                            DM_BUTTON_BACKGROUND_COLOR_TERTIARY = "#6b6b69",
            
                            DM_RED = "#912707",
                            DM_ORANGE = "#ab7a09",
                            DM_YELLOW = "#ab9b09",
                            DM_GREEN = "#37ab09",
                            DM_BLUE = "#097dab";

        private static readonly Dictionary<string, string> LightModeColors = new Dictionary<string, string>
        {
            {"background", "bcbcbc"},
            {"correct_letter", "bcbcbc"},
            {"incorrect_letter", "bcbcbc"},
            {"misplaced_letter", "bcbcbc"},
            {"button_primary", "bcbcbc"},
            {"button_secondary", "bcbcbc"},
            {"button_tertiary", "bcbcbc"},
            {"lm_red", "bcbcbc"},
            {"lm_orange", "bcbcbc"},
            {"lm_yellow", "bcbcbc"},
            {"lm_green", "bcbcbc"},
            {"lm_blue", "bcbcbc"},
        };

        private static readonly Dictionary<string, string> DarkModeColors = new Dictionary<string, string>
        {
            {"background", "bcbcbc"},
            {"correct_letter", "bcbcbc"},
            {"incorrect_letter", "bcbcbc"},
            {"misplaced_letter", "bcbcbc"},
            {"button_primary", "bcbcbc"},
            {"button_secondary", "bcbcbc"},
            {"button_tertiary", "bcbcbc"},
            {"lm_red", "bcbcbc"},
            {"lm_orange", "bcbcbc"},
            {"lm_yellow", "bcbcbc"},
            {"lm_green", "bcbcbc"},
            {"lm_blue", "bcbcbc"},
        };


        public static string GetBackgroundColor()
        {
            if (IsDarkMode())
            {
                return DM_BACKGROUND;
            }

            return LM_BACKGROUND;
        }

        public static string GetCorrectLetterBackgroundColor()
        {
            if (IsDarkMode())
            {
                return DM_CORRECT_LETTER_BACKGROUND;
            }

            return LM_CORRECT_LETTER_BACKGROUND;
        }

        public static string GetIncorrectLetterBackGroundColor()
        {
            if (IsDarkMode())
            {
                return DM_INCORRECT_LETTER_BACKGROUND;
            }

            return LM_INCORRECT_LETTER_BACKGROUND;
        }

        public static string GetMisplacedLetterBackgroundColor()
        {
            if (IsDarkMode())
            {
                return DM_MISPLACED_LETTER_BACKGROUND;
            }

            return LM_MISPLACED_LETTER_BACKGROUND;
        }

        public static string GetButtonBackgroundColor(int type)
        {

            switch (type)
            {
                case PRIMARY_TYPE:
                    if (IsDarkMode())
                    {
                        return DM_BUTTON_BACKGROUND_COLOR_PRIMARY;
                    }

                    return LM_BUTTON_BACKGROUND_COLOR_PRIMARY;
                case SECONDARY_TYPE:
                    if (IsDarkMode())
                    {
                        return DM_BUTTON_BACKGROUND_COLOR_SECONDARY;
                    }

                    return LM_BUTTON_BACKGROUND_COLOR_SECONDARY;

                case TERTIARY_TYPE:
                    if (IsDarkMode())
                    {
                        return DM_BUTTON_BACKGROUND_COLOR_TERTIARY;
                    }

                    return LM_BUTTON_BACKGROUND_COLOR_TERTIARY;
            }

            if (IsDarkMode())
            {
                return DM_BUTTON_BACKGROUND_COLOR_PRIMARY;
            }

            return LM_BUTTON_BACKGROUND_COLOR_PRIMARY;

        }


        public static bool IsDarkMode()
        {
            return Preferences.Default.Get("dark_mode", false);
        }

        public static int GetFontSize()
        {
            return Preferences.Default.Get("font_size", 20);
        }

        public static bool IsHintsEnabled()
        {
            return Preferences.Default.Get("show_hints", false);

        }

        public static string GetColor(string colorName)
        {
            string color;
            if (IsDarkMode() && DarkModeColors.TryGetValue(colorName, out color))
                return color;

            if (LightModeColors.TryGetValue(colorName, out color))
                return color;

            return "#FFFFFFF"; // Default color
        }

        public static async Task DownloadFileAsync(string url, string destinationPath)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode(); 

                    using (HttpContent content = response.Content)
                    {
                        byte[] data = await content.ReadAsByteArrayAsync();
                        await System.IO.File.WriteAllBytesAsync(destinationPath, data);
                    }
                }
            }
        }
    }

    
}
