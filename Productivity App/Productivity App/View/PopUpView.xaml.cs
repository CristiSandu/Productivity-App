using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProductivityApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpView
    {
        public PopUpView()
        {
            InitializeComponent();
        }

        private async void setTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                Preferences.Remove("time");
                DateTime time = DateTime.Today + setTime.Time;
                string timeString = time.ToString();
                Preferences.Set("time", timeString);
            }
        }
    }
}