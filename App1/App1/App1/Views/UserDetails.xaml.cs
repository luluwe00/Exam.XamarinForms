using App1.Model;
using App1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDetails : ContentPage
    {
        private UserModel model;
        public UserDetails(UserModel model)
        {
            InitializeComponent();
            this.model = model;
            BindingContext = new UserDetailsViewModel(model);
        }
    }
}