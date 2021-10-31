using App1.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.ViewModel
{
    public class UserDetailsViewModel : BaseViewModel
    {
        public UserModel userModel { get; set; }
        private string _Image;
        public string Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                OnPropertyChanged("Image");
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        public UserDetailsViewModel(UserModel model)
        {
            this.userModel = model;
            Image = userModel.imageUrl;
            Name = userModel.name;
        }
    }
}
