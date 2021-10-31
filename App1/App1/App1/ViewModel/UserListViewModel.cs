using App1.Model;
using App1.Service;
using App1.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModel
{
    public class UserListViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        //For navigating pages
        INavigation Navigation => Application.Current.MainPage.Navigation;
        public ICommand MyCollectionSelectedCommand { get; }
        //public ObservableCollection<UserModel> PersonList { get; set; }

        //If running on background
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        //List of users from api
        private ObservableCollection<UserModel> _PersonList;
        public ObservableCollection<UserModel> PersonList
        {
            get { return _PersonList; }
            set
            {
                _PersonList = value;
                OnPropertyChanged("PersonList");
            }
        }

        //selected items
        public UserModel selectedItem;
        public UserModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        //reload function
        Command _ReloadCommand;
        public Command ReloadCommand
        {
            get { return _ReloadCommand; }
            protected set { _ReloadCommand = value; }
        }
        public UserListViewModel()
        {
            PersonList = new ObservableCollection<UserModel>();
            GetUsers();
            MyCollectionSelectedCommand = new Command(GenerateNavigations);
            _ReloadCommand = new Command(Reload);
        }
        
        //for navigation
        private async void GenerateNavigations(object obj)
        {
            var slecteditem = obj as UserModel;
            await Navigation.PushAsync(new UserDetails(slecteditem));
        }

        async void Reload()
        {
            await GetUsers();
        }


        private HttpClient client;
        string url = "https://gist.githubusercontent.com/erni-ph-mobile-team/c5b401c4fad718da9038669250baff06/raw/7e390e8aa3f7da4c35b65b493fcbfea3da55eac9/test.json";
        private async Task GetUsers()
        {
            IsBusy = true;
            var isConnected = CheckConnection();
            if (isConnected == false)
            {
                IsBusy = false;
                await showAlert("turn on your internet connection");
            }
            else
            {
                try
                {
                    /*var assembly = typeof(MainPage).GetTypeInfo().Assembly;
                    Stream stream = assembly.GetManifestResourceStream("https://gist.githubusercontent.com/erni-ph-mobile-team/c5b401c4fad718da9038669250baff06/raw/7e390e8aa3f7da4c35b65b493fcbfea3da55eac9/test.json");
                    using(var reader = new System.IO.StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        List<UserModel> list = JsonConvert.DeserializeObject<List<UserModel>>(json);
                        PersonList = new ObservableCollection<UserModel>(list);
                    }*/
                    client = new HttpClient();
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(url);
                    if(response.IsSuccessStatusCode)
                    {
                        var responseRead = await response.Content.ReadAsStringAsync();

                        var objResponse = JsonConvert.DeserializeObject<List<UserModel>>(responseRead);
                        PersonList = new ObservableCollection<UserModel>(objResponse.GroupBy(x => x.id)
                                      .Select(x => x.First()).ToList());
                    }
                    else
                    {
                        await showAlert("Check your internet connection");
                    }
                }
                catch(Exception ex)
                {
                   await showAlert("Something went wrong! Please try again.");
                    Console.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task showAlert(string word)
        {
            await App.Current.MainPage.DisplayAlert("", word + " (Click Reload to refresh)", "OK");
        }

        private bool CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return false;
            return true;
        }

        private async Task Navigate()
        {
            showAlert("nice");
        }

    }
}
