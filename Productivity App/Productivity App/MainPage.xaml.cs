using Newtonsoft.Json;
using Plugin.LocalNotification;
using Productivity_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Productivity_App
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Setup();
            Login();
            Wether();
        }

        private List<Event> AllEvents { get; set; }
        private ObservableCollection<BreakModel> AllBrakes { get; set; }


        public async void Login()
        {
            roomTempActivity.IsRunning = true;

            var httpClient = new HttpClient();
            var respons = await httpClient.GetStringAsync("https://weatherapp20210725210906.azurewebsites.net/api/RoomController");
            var login = JsonConvert.DeserializeObject<List<RoomItem>>(respons);
            tempLabel.Text = $"Temp: {login[0].Temp}°C";
            humyLabel.Text = $"Humy: {login[0].Humy}%";

            roomTempActivity.IsRunning = false;
        }


        public async void Wether()
        {
            wetherTempActivity.IsRunning = true;

            var httpClient = new HttpClient();
            var respons = await httpClient.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?zip=107345,ro&appid=44afe348069b1812b24e39c82be13c9e");
            WeatherModel myDeserializedClass = JsonConvert.DeserializeObject<WeatherModel>(respons);

            tempLabel1.Text = $"Temp: {myDeserializedClass.main.temp - 273.15}°C";
            humyLabel1.Text = $"Humy: {myDeserializedClass.main.humidity}%";

            whederType.Source = ImageSource.FromUri(new Uri("https://openweathermap.org/img/wn/" + myDeserializedClass.weather[0].icon + "@4x.png"));


            wetherTempActivity.IsRunning = false;
        }

        private void pickTime_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("time");
            OnAppearing();
        }

        private List<Event> GetEvents()
        {

            DateTime time;
            if (Preferences.ContainsKey("time"))
            {
                var myValue = Preferences.Get("time", "default_value");
                time = DateTime.Parse(myValue);
                if (time < DateTime.Now)
                {
                    time = DateTime.Now.AddHours(8);
                    Preferences.Set("time", time.ToString());
                }
            }
            else
            {
                time = DateTime.Now.AddHours(8);
                Preferences.Set("time", time.ToString());
            }
            return new List<Event>()
            {
                new Event{ EventTitle = "Camping", BgColor = "#EB9999", Date = new DateTime(DateTime.Now.Ticks + new TimeSpan(0, 8, 30, 0).Ticks), DateRemain = time},
            };
        }

        private List<BreakModel> GetBreaks()
        {
            var myValue = Preferences.Get("time", "default_value");
            if (myValue == "default_value")
                return new List<BreakModel>();
            DateTime endtime = DateTime.Parse(myValue);
            DateTime startTime = endtime.AddHours(-8);

            List<BreakModel> breaks = new List<BreakModel>();

            DateTime startBrake = startTime.AddMinutes(50);
            DateTime endBrake = startBrake.AddMinutes(10);

            breaks.Add(new BreakModel { BrakeTimeStart = startBrake, BrakeTimeEnd = endBrake });

            while (DateTime.Compare(endBrake, endtime) < 0)
            {
                startBrake = endBrake.AddMinutes(50);
                endBrake = startBrake.AddMinutes(10);

                breaks.Add(new BreakModel { BrakeTimeStart = startBrake, BrakeTimeEnd = endBrake });
            }

            return breaks;
        }

        private void Setup()
        {
            AllEvents = GetEvents();
            AllBrakes = new ObservableCollection<BreakModel>(GetBreaks());
            breackesList.ItemsSource = AllBrakes;

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                foreach (var evt in AllEvents)
                {
                    var timespan = evt.DateRemain - DateTime.Now;
                    evt.Timespan = timespan;
                }

                houers.Text = null;
                houers.Text = AllEvents[0].Hours;

                minutes.Text = null;
                minutes.Text = AllEvents[0].Minutes;

                seconds.Text = null;
                seconds.Text = AllEvents[0].Seconds;

                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    foreach (var (brk, notification) in from brk in AllBrakes
                                                        where brk.BrakeTimeStart <= DateTime.Now && brk.BrakeTimeEnd >= DateTime.Now && !brk.NotificationSent
                                                        let notification = new NotificationRequest
                                                        {
                                                            BadgeNumber = 1,
                                                            Description = $"Start at {brk.BrakeTimeStart.ToString("HH:mm")}, end at {brk.BrakeTimeEnd.ToString("HH:mm")}",
                                                            Title = "Take A Brake",
                                                            ReturningData = "Dummy Data",
                                                            NotificationId = 1337
                                                        }
                                                        select (brk, notification))
                    {
                        NotificationCenter.Current.Show(notification);
                        brk.NotificationSent = true;
                    }
                }

                dateRemain.Text = $"End Program: {AllEvents[0].DateRemain.ToString("HH:mm")}";

                return true;
            });

        }

        private void refreshTemp_Clicked(object sender, EventArgs e)
        {
            try
            {
                Vibration.Vibrate();

                OnAppearing();

                Vibration.Cancel();
            }
            catch (FeatureNotSupportedException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }

        private async void restartTime_Clicked(object sender, EventArgs e)
        {
            try
            {
                Vibration.Vibrate();

                bool validate = await DisplayAlert("Start/Reset", "Do you want to Start/Reset program ?", "Yes", "No");

                if (validate)
                {
                    Preferences.Remove("time");
                    OnAppearing();
                }

                Vibration.Cancel();
            }
            catch (FeatureNotSupportedException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class Event
    {
        public DateTime Date { get; set; }
        public DateTime DateRemain { get; set; }

        public string EventTitle { get; set; }
        public TimeSpan Timespan { get; set; }
        public string Days => Timespan.Days.ToString("00");
        public string Hours => Timespan.Hours.ToString("00");
        public string Minutes => Timespan.Minutes.ToString("00");
        public string Seconds => Timespan.Seconds.ToString("00");
        public string BgColor { get; set; }
    }
}
