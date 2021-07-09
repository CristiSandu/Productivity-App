using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Productivity_App
{
    class MainPageViewModel : INotifyPropertyChanged
    {

        Stopwatch stopWatch = new Stopwatch();
        private Timer timer = new Timer();

        public DateTime _selectedTime;
        public DateTime SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                if (value == null)
                {
                    _selectedTime = new DateTime(DateTime.Now.Ticks + new TimeSpan(0, 8, 0, 0).Ticks);
                }
                else
                {
                    _selectedTime = value;
                }
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
    }


}
