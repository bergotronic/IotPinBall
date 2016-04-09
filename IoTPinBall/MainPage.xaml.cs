using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Windows.System.Threading;
using Microsoft.Azure.Devices.Client;
using Windows.Devices.I2c;
using Windows.Devices.Enumeration;

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
            DispatcherTimerSetup();
            TextBox_CURRENTSCORE.TextAlignment = TextAlignment.Center;
            InitSPI();
        }

        class PinballData
        {
            public String Name { get; set; }
            public String SensorType { get; set; }
            public String GameID { get; set; }
            public String TimeStamp { get; set; }
            public String PlayerName { get; set; }
            public int PlayerScore { get; set; }
            public String PlayerGameStartTime { get; set; }
            public String PlayerGameEndTime { get; set; }
       
        }


        public int score;
        public string currentPlayer;
        public DateTime GameStartDateTime;
        public DateTime GameStopDateTime;
        DispatcherTimer GamePlayBlinker;
        private ThreadPoolTimer ScoreCaptureThreadPoolTimer;

        private const string DeviceConnectionString = "Put your String here :)";

        
        I2cDevice Digit1;
        //I2cDevice Digit2;
        //I2cDevice Digit3;
        //I2cDevice Digit4;
        //I2cDevice Digit5;
        //I2cDevice Digit6;
        //I2cDevice Digit7;
        //I2cDevice Digit8;
        

        public void DispatcherTimerSetup()
        {
            GamePlayBlinker = new DispatcherTimer();
            GamePlayBlinker.Tick += dispatcherTimer_Tick;
            GamePlayBlinker.Interval = new TimeSpan(0, 0, 1);
            GamePlayBlinker.Start();
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            Debug.WriteLine("Blink");

            if (GamePlayHeader.Text == "")
            {
                GamePlayHeader.Text = "GAME IN PROGRESS";
            }
            else
            {
                GamePlayHeader.Text = "";
            }
        }



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


            string FormattedScore = score.ToString();

            ulong ul;

            if (ulong.TryParse(FormattedScore, out ul))
            {
                FormattedScore = string.Format("{0:#,#}", ul);

            }

            TextBox_CURRENTSCORE.Text = FormattedScore.ToString();
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



        public async void InitSPI()
        {

            //    String aqs = I2cDevice.GetDeviceSelector("I2C1");
            //var deviceInfo = await DeviceInformation.FindAllAsync(aqs);
            //sensor = await I2cDevice.FromIdAsync(deviceInfo[0].Id, new I2cConnectionSettings(0x44));

            //byte[] resetCommand = { 0x30, 0xA2 };
            //sensor.Write(resetCommand);

            ScoreCaptureThreadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMilliseconds(1000)); 


        }

        private async void Timer_Tick(ThreadPoolTimer timer)
        {
            try
            {

                Debug.WriteLine("1");
                String aqs = I2cDevice.GetDeviceSelector("I2C1");

                Debug.WriteLine("2");
                byte[] Digit1Data = new byte[8];
                byte[] Digit1Command = new byte[0];

                Debug.WriteLine("3");
                var deviceInfo = await DeviceInformation.FindAllAsync(aqs);

                Debug.WriteLine("4");
                Digit1 = await I2cDevice.FromIdAsync(deviceInfo[0].Id, new I2cConnectionSettings(0x70));

                Debug.WriteLine("5");
                Digit1.WriteRead(Digit1Command,Digit1Data);


                Debug.WriteLine("Trying to do the data thing");

          
                Debug.WriteLine(Digit1Data.ToString());
            }
            catch
            {
                Debug.WriteLine("Fuck you");
            }
    

        }
        private async Task Log_Event(string DataValue, string Name, string Sensor, string DataType, string UnitOfMeasure)
        {
            //Debug
            DateTime localDate = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("Event: " + Name + "-" + Sensor + "-" + DataValue + "-" + localDate.ToString());

            string AzureMode = "Derp";
            string internetConnected = "Derp";

            //to Azure
            if (AzureMode == "Transmit")
                if (internetConnected == "True")
                {
                    {

                        //Init httpClinet:
                        //var httpClient = new HttpClient();

                        System.Diagnostics.Debug.WriteLine("Starting Azure Transmit");


                        PinballData PinballGameInstance = new PinballData();
                        PinballGameInstance.Name = Name;
                        PinballGameInstance.SensorType = Sensor;
                        PinballGameInstance.TimeStamp = DateTime.Now.ToString();
                        PinballGameInstance.PlayerName = currentPlayer;
                        PinballGameInstance.PlayerScore = score;
                        PinballGameInstance.PlayerGameStartTime = GameStartDateTime.ToString();
                        PinballGameInstance.PlayerGameEndTime = GameStopDateTime.ToString();
                        PinballGameInstance.GameID = Guid.NewGuid().ToString();


                        string jsoncontent = JsonConvert.SerializeObject(PinballGameInstance);


                        DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

                        string dataBuffer;
                        dataBuffer = jsoncontent;

                        System.Diagnostics.Debug.WriteLine(jsoncontent);


                        Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
                        await deviceClient.SendEventAsync(eventMessage);


                        System.Diagnostics.Debug.WriteLine("Azure Transmit Done");



                    }
                }


        }









    }
}