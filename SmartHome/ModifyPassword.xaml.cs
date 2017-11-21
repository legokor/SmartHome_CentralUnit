using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartHome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModifyPassword : Page
    {
        public ModifyPassword()
        {
            this.InitializeComponent();
        }

        private async void Modify_Click(object sender, RoutedEventArgs e)
        {
            if(PasswordBox.Password == RePasswordBox.Password  && UserManager.LoginedUser.EncryptedPassword == UserManager.LoginedUser.EncodePassword(OldPassowordBox.Password))
            {
                UserManager.LoginedUser.ModifyPassword(PasswordBox.Password);
                ContentDialogWithOK dialog = new ContentDialogWithOK
                {
                    DataContext = new
                    {
                        MsgText = "Password succesfully changed!"
                    }
                };
                await dialog.ShowAsync();
                this.Frame.Navigate(typeof(MainSettings));
            }
            else
            {
                ContentDialogWithOK dialog = new ContentDialogWithOK
                {
                    DataContext = new
                    {
                        MsgText = "One of the given passwords are bad!"
                    }
                };
                await dialog.ShowAsync();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainSettings));
        }
    }
}
