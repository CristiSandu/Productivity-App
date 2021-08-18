using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Productivity_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ProductivityApp.View.ProductivityTabView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
