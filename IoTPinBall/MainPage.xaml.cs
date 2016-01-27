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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTPinBall
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            GamePlay.Visibility = Visibility.Collapsed;
            StackGameover.Visibility = Visibility.Collapsed;
        }

        public int score;
        public string currentPlayer;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            currentPlayer = TextBoxPlayerName.Text;
            PlayerSetup.Visibility = Visibility.Collapsed;
            GamePlay.Visibility = Visibility.Visible;
        }


        private void button_AddPoints_Click(object sender, RoutedEventArgs e)
        {
            score = score + 100;
            TextBox_CURRENTSCORE.Text = score.ToString();

        }

        private void button_GameDone_Click(object sender, RoutedEventArgs e)
        {
            PlayerSetup.Visibility = Visibility.Collapsed;
            GamePlay.Visibility = Visibility.Collapsed;
            StackGameover.Visibility = Visibility.Visible;
        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            currentPlayer = "";
            TextBoxPlayerName.Text = "";

            PlayerSetup.Visibility = Visibility.Visible;
            GamePlay.Visibility = Visibility.Collapsed;
            StackGameover.Visibility = Visibility.Collapsed;
        }
    }
}