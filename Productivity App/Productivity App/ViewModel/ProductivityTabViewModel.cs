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
        private TimeSpan dateTime;
        public TimeSpan DateTime1
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
                        PropertyChanged(this, new PropertyChangedEventArgs("Hours"));
                        PropertyChanged(this, new PropertyChangedEventArgs("Minutes"));
                        PropertyChanged(this, new PropertyChangedEventArgs("Seconds"));

                    }
                }
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EndTime"));
                    }
                }
            }
        }

        public string Hours => DateTime1.Hours.ToString("00");
        public string Minutes => DateTime1.Minutes.ToString("00");
        public string Seconds => DateTime1.Seconds.ToString("00");

        public ICommand ResetTimer { get; private set; }
        public ObservableCollection<BreakModel> Breaks { get; set; } = new ObservableCollection<BreakModel>();

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
                Preferences.Remove("time");
                
                StartTimer();
                GetBreaks();
            });
            StartTimer();
            GetBreaks();
            
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeSpan test = EndTime - DateTime.Now;
                this.DateTime1 = test;
                
                return true;
            });
        }


        private void StartTimer()
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
                EndProgram = time.ToString("HH:mm");
            }
            else
            {
                time = DateTime.Now.AddHours(8);
                EndProgram = time.ToString("HH:mm");
                Preferences.Set("time", time.ToString());
            }
            EndTime = time;
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
            Breaks.Clear();

            AddRange( breaks);
        }


        public void AddRange( List<BreakModel> items)
        {
            foreach (var item in items)
            {
                Breaks.Add(item);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
