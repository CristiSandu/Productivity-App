using Newtonsoft.Json;
using Productivity_App.Models;
using System;
using System.Collections.Generic;
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
        }

        private List<Event> AllEvents { get; set; }
        
        public async void Login()
        {
            var httpClient = new HttpClient();
            var respons = await httpClient.GetStringAsync("https://weatherapp20210725210906.azurewebsites.net/api/RoomController");
            var login = JsonConvert.DeserializeObject<List<RoomItem>>(respons);
            tempLabel.Text = $"Temp: {login[0].Temp}°C";
            humyLabel.Text = $"Humy: {login[0].Humy}%";

        }


        public async void Wether()
        {
            //
            var httpClient = new HttpClient();
            var respons = await httpClient.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?zip=107345,ro&appid={44afe348069b1812b24e39c82be13c9e}");
            //var login = JsonConvert.DeserializeObject<List<RoomItem>>(respons);
            //tempLabel.Text = $"Temp: {login[0].Temp}°C";
            //humyLabel.Text = $"Humy: {login[0].Humy}%";
        }

        private void pickTime_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("time");
            OnAppearing();
        }

        private List<Event> GetEvents()
        {
            
            DateTime time ;
            if (Preferences.ContainsKey("time"))
            {
                var myValue = Preferences.Get("time", "default_value");
                time = DateTime.Parse(myValue);
                if (time < DateTime.Now)
                {
                    time = DateTime.Now.AddHours(8).AddMinutes(30);
                    Preferences.Set("time", time.ToString());
                }
            }else
            {
                time = DateTime.Now.AddHours(8).AddMinutes(30);
                Preferences.Set("time", time.ToString());
            }
            return new List<Event>()
            {
                new Event{ EventTitle = "Camping", BgColor = "#EB9999", Date = new DateTime(DateTime.Now.Ticks + new TimeSpan(0, 8, 30, 0).Ticks), DateRemain = time},
            };
        }

        private void Setup()
        {
            AllEvents = GetEvents();
            
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

                dateRemain.Text = $"End Program: {AllEvents[0].DateRemain.ToString("HH:mm")}";

                return true;
            });
        }

        private void refreshTemp_Clicked(object sender, EventArgs e)
        {
            OnAppearing();
        }

        private void restartTime_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("time");
            OnAppearing();
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
