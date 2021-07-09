using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        }

        private List<Event> AllEvents { get; set; }
        
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
            eventList.ItemsSource = AllEvents;
            
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                foreach (var evt in AllEvents)
                {
                    var timespan = evt.DateRemain - DateTime.Now;
                    evt.Timespan = timespan;
                }

                eventList.ItemsSource = null;
                eventList.ItemsSource = AllEvents;

                return true;
            });
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
