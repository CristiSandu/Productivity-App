using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace ProductivityApp.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    Preferences.Remove("time");
                    Preferences.Set("time", _startTime.ToString());
                    RaisePropertyChanged("StartTime");
                }
            }
        }

        public SettingsViewModel()
        {
            if (Preferences.ContainsKey("time"))
            {
                var myValue = Preferences.Get("time", "default_value");
                try
                {
                    StartTime = TimeSpan.Parse(myValue);
                }catch(FormatException ex)
                {
                    DateTime time;
                    time = DateTime.Parse(myValue);
                    StartTime = new TimeSpan(time.Hour, time.Minute, time.Second);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
