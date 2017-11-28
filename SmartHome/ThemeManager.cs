using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SmartHome
{
    abstract class ThemeManager
    {
        public static string CurrentTheme { get { return currentTheme; } }
        private static string currentTheme;
        public static Dictionary<string, ResourceDictionary> Themes { get; } = new Dictionary<string, ResourceDictionary>(); 


        public static void Init()
        {
            ResourceDictionary normalResourceDictionary = new ResourceDictionary();
            normalResourceDictionary.Source = new Uri("ms-appx:///NormalTheme.xaml");
            Themes.Add("Normal theme", normalResourceDictionary);
            ResourceDictionary nightResourceDictionary = new ResourceDictionary();
            nightResourceDictionary.Source = new Uri("ms-appx:///NightTheme.xaml");
            Themes.Add("Night theme", nightResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(Themes["Normal theme"]);
            currentTheme = "Normal theme";
        }
        public static void SetTheme(string themeKey)
        {
            if (Application.Current.Resources.MergedDictionaries.Contains(Themes[currentTheme]))
            {
                Application.Current.Resources.MergedDictionaries.Remove(Themes[currentTheme]);
            }
            Application.Current.Resources.MergedDictionaries.Add(Themes[themeKey]);
            currentTheme = themeKey;
        }
    }
}
