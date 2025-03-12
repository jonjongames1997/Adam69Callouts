using System;
using System.Collections.Generic;

namespace Adam69Callouts.Stuff
{
    public static class Localization
    {
        public static string CurrentLanguage = "English"; // Default Language

        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new Dictionary<string, Dictionary<string, string>>
        {
            { "English", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
            { "French", new Dictionary<string, string>
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
            { "Brazilian", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            },
            { "Portuguese", new Dictionary<string, string>
                {
					// Main pack entries
					{"KEY", "Dialogue To Translate"},

                }
            }
        };
    }
}
