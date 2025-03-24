using System;
using System.Collections.Generic;

namespace Adam69Callouts.Stuff
{
    public static class Localization
    {
        public static string CurrentLanguage = "English"; // Default Language

        public static string GetText(string key)
        {
            bool flag = Localization.Translations.ContainsKey(Localization.CurrentLanguage) && Localization.Translations[Localization.CurrentLanguage].ContainsKey(key);
            string result;
            if (flag)
            {
                result = Localization.Translations[Localization.CurrentLanguage][key];
            }
            else
            {
                result = "[MISSING:" + key + "]";
            }
            return result;
        }

        public static void SetLanguage(string language)
        {
            bool flag = Localization.Translations.ContainsKey(language);
            if (flag)
            {
                Localization.CurrentLanguage = language;
            }
        }

        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new Dictionary<string, Dictionary<string, string>>
        {
            { "English", new Dictionary<string, string>
                {
					// Main pack entries
				    {
                        "EndCalloutMessage", 
                        "Press ~y~" + Settings.EndCall.ToString() + "~w~ to end the callout at anytime."
                    },
                    {
                        "AbandonedVehicleCall",
                        ""
                    },

                }
            },
            { "French", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
            { "Italian", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
            { "Spanish", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
            { "Russian", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
            { "Portugese", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
        };
    }
}
