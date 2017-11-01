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

        public static void SetNightTheme()
        {
            if (currentTheme != "NightTheme")
            {


                ResourceDictionary delResourceDictionary = new ResourceDictionary();
                delResourceDictionary.Source = new Uri("ms-appx:///NormalTheme.xaml");
                if (Application.Current.Resources.MergedDictionaries.Contains(delResourceDictionary))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(delResourceDictionary);
                }
                ResourceDictionary myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("ms-appx:///NightTheme.xaml");
                Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
                currentTheme = "NightTheme";
            }
        }

        public static void SetNormalTheme()
        {
            if (currentTheme != "NormalTheme")
            {
                ResourceDictionary delResourceDictionary = new ResourceDictionary();
                delResourceDictionary.Source = new Uri("ms-appx:///NightTheme.xaml");
                if (Application.Current.Resources.MergedDictionaries.Contains(delResourceDictionary))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(delResourceDictionary);
                }
                ResourceDictionary myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source = new Uri("ms-appx:///NormalTheme.xaml");
                Application.Current.Resources.MergedDictionaries.Add(myResourceDictionary);
                currentTheme = "NormalTheme";
            }

        }



    }
}
