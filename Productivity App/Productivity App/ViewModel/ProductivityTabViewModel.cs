using Productivity_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProductivityApp.ViewModel
{
    public class ProductivityTabViewModel : INotifyPropertyChanged
    {
        private string dateTime;
        public string DateTime1
        {
            get
            {
                return dateTime;
            }

            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateTime1"));
                    }
                }
            }
        }

        public ICommand ResetTimer { get; private set; }
        public ObservableCollection<BreakModel> Breaks { get; set; }

        private string endProg;
        public string EndProgram
        {
            get { return endProg; }
            set
            {
                if (endProg != value)
                {
                    endProg = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EndProgram"));
                    }
                }
            }
        }

        public ProductivityTabViewModel()
        {
            ResetTimer = new Command(() => {
                GetBreaks();
                Preferences.Remove("time");
            });
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
                    EndProgram = time.ToString("HH:mm");
            }
            else
            {
                time = DateTime.Now.AddHours(8);
                EndProgram = time.ToString("HH:mm");
                Preferences.Set("time", time.ToString());
            }

            GetBreaks();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeSpan test = time - DateTime.Now;
                this.DateTime1 = string.Format("{0:00}:{1:00}:{2:00}", test.Hours, test.Minutes, test.Seconds);
                return true;
            });
        }

        private void GetBreaks()
        {
            var myValue = Preferences.Get("time", "default_value");
            if (myValue == "default_value")
            {
                Breaks = new ObservableCollection<BreakModel>();
                return;
            }

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

            Breaks = new ObservableCollection<BreakModel>(breaks);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
