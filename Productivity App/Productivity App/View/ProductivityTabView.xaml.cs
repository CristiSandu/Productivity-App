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
    public partial class ProductivityTabView : ContentPage
    {
        public ProductivityTabView()
        {
            InitializeComponent();
            BindingContext = new ProductivityTabViewModel();
        }
    }
}