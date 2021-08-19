using ProductivityApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProductivityApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }

      

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (timePickerForStart.Time != null && BindingContext != null) {
                TimeSpan time = timePickerForStart.Time;
                (BindingContext as SettingsViewModel).StartTime = time + TimeSpan.FromHours(8);
            }
        }
    }
}