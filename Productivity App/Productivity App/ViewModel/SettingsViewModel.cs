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
