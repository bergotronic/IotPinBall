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
        public DateTime GameStartDateTime;
        public DateTime GameStopDateTime;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            currentPlayer = TextBoxPlayerName.Text;
            GameStartDateTime = DateTime.Now;

            PlayerSetup.Visibility = Visibility.Collapsed;
            GamePlay.Visibility = Visibility.Visible;

        }


        private void button_AddPoints_Click(object sender, RoutedEventArgs e)
        {

            Random random = new Random();
            int randomNumber = random.Next(0, 1000);
            score = score + randomNumber;
            TextBox_CURRENTSCORE.Text = score.ToString();

        }


        private async void Write_Scores()
        {

            //Write Local / Bakcup data to Text
         


            Windows.Storage.StorageFolder storageFolder =    Windows.Storage.ApplicationData.Current.LocalFolder;
            //Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("Scores.txt");
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Scores.txt");

            System.Diagnostics.Debug.WriteLine("Where the F is the data? : " + storageFolder.Path.ToString());


            System.Diagnostics.Debug.WriteLine("Player: " + currentPlayer + " - SCORE:" + score + " - START: " + GameStartDateTime + " - STOP:" + GameStopDateTime);
            await Windows.Storage.FileIO.AppendTextAsync(sampleFile, currentPlayer + "," + score + "," + GameStartDateTime + "," + GameStopDateTime + System.Environment.NewLine);


            //Write Real Data to IoT




        }

        private void button_GameDone_Click(object sender, RoutedEventArgs e)
        {

            GameStopDateTime = DateTime.Now;
            Write_Scores();
       
            PlayerSetup.Visibility = Visibility.Collapsed;
            GamePlay.Visibility = Visibility.Collapsed;
            StackGameover.Visibility = Visibility.Visible;



        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            TextBox_CURRENTSCORE.Text = "";
            currentPlayer = "";
            TextBoxPlayerName.Text = "";

            PlayerSetup.Visibility = Visibility.Visible;
            GamePlay.Visibility = Visibility.Collapsed;
            StackGameover.Visibility = Visibility.Collapsed;
        }
    }
}