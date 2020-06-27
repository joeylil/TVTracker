using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Main_Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowInfoPage : ContentPage
    {
        static readonly HttpClient client = new HttpClient();
        readonly SQLiteAsyncConnection Connection = DependencyService.Get<SQLiteInterface>().GetConnection();
        static public string showSelected = "showSelected";
        private ShowInfo _ShowInfo;
        public ShowInfo ShowInfo
        {
            get
            {
                return _ShowInfo;
            }
            set
            {
                _ShowInfo = value;
                OnPropertyChanged();
            }
        }

        public EpisodeCollection episodeCollection { get; set; }
        public SeasonData season { get; set; }
        public SeasonCollection allSeasons { get; set; }

        private string ErrorType { get; set; }




        public ShowInfoPage()
        {
            InitializeComponent();
            Connection.CreateTableAsync<Favourites>();
            AddToFavourites = new Command<ShowInfo>(AddShowToFavourites);

            episodeCollection = new EpisodeCollection();
            season = new SeasonData();
            allSeasons = new SeasonCollection();
            ShowInfo = new ShowInfo();
            

            MessagingCenter.Subscribe<SearchShowsPage, (string, bool)>(this, showSelected, (sender, arg) =>
            {
                LoadPageDetails(arg);
            });
            MessagingCenter.Subscribe<FavouritesPage, string>(this, showSelected, (sender, arg) =>
            {
                LoadPageDetails((arg, true));
            });
        }

        private void LoadPageDetails((string, bool) arg)
        {
            if (Application.Current.Properties.ContainsKey("theme"))
            {
                bool theme = (bool)Application.Current.Properties["theme"];
                if (theme)
                {
                    BackgroundColor = Color.Black;
                }
            }
            long lastSeason = 1;
            ShowInfo = ShowInfoAsync(arg.Item1).Result;
            if(ShowInfo == null)
            {
                showGrid.IsVisible = false;
                ErrorLabel.Text = ErrorType;
                ErrorLabel.IsVisible = true;
                return;
            }
            ShowInfo.IsInFavouriteList = arg.Item2;
            if(arg.Item2 == true)
            {
                favouritesButton.Source = "http://www.clker.com/cliparts/O/Z/4/Q/W/Q/favourite-md.png";
            }
            else
            {
                favouritesButton.Source = "https://img.icons8.com/windows/50/000000/add-to-favorites.png";
            }
            episodeCollection.Episodes = EpisodeInfoAsync(arg.Item1).Result;
            episodeCollection.Reset();
            foreach (EpisodeInfo episode in episodeCollection)
            {
                if (lastSeason == episode.Season)
                {
                    season.Season.Add(episode);
                }
                else
                {
                    allSeasons.Seasons.Add(season);
                    season = new SeasonData();
                    season.Season.Add(episode);
                    lastSeason += 1;
                }
            }
            allSeasons.Seasons.Add(season);
            seasonPicker.ItemsSource?.Clear();
            if (seasonPicker.ItemsSource == null)
            {
                seasonPicker.ItemsSource = allSeasons.seasonDisplay;
            }
            else
            {
                foreach (string season in allSeasons.seasonDisplay)
                {
                    seasonPicker.ItemsSource.Add(season);
                }
            }
            seasonPicker.SelectedIndex = 0;
            allSeasons.ChosenSeason = seasonPicker.SelectedIndex + 1;
            showListView.ItemsSource = allSeasons.DisplayChosenSeason;
            BindingContext = ShowInfo;
        }



        private async Task<ShowInfo> ShowInfoAsync(string showID)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("http://api.tvmaze.com/shows/{0}", showID)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ShowInfo>(responseBody);
            }
            catch (HttpRequestException)
            {
                var connection = Connectivity.NetworkAccess;
                if (connection == null)
                {
                    ErrorType = "No internet connection: Please try again";
                    return null;
                }
                else
                {
                    ErrorType = "Error fetching data. Please contact support";
                    return null;
                }
            }
        }

        private async Task<ObservableCollection<EpisodeInfo>> EpisodeInfoAsync(string showID)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("http://api.tvmaze.com/shows/{0}/episodes", showID)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<EpisodeInfo>>(responseBody);
            }
            catch (HttpRequestException)
            {
                var connection = Connectivity.NetworkAccess;
                if (connection == null)
                {
                    ErrorType = "No internet connection: Please try again";
                    return null;
                }
                else
                {
                    ErrorType = "Error fetching data. Please contact support";
                    return null;
                }
            }
        }

        public ICommand AddToFavourites { get; private set; }

        async void AddShowToFavourites(ShowInfo showInfo)
        {
            if (!showInfo.IsInFavouriteList)
            {

                await Connection.InsertAsync(new Favourites()
                {
                    ShowID = showInfo.ID
                });
                FavouritedShows.Instance.FavouriteShowsList.Add(new Favourites
                {
                    Id = FavouritedShows.Instance.DatabaseIDCount,
                    ShowID = showInfo.ID
                });
                //Add one to DatabaseIDCount to match primary key id in database
                FavouritedShows.Instance.DatabaseIDCount++;
            }
            else
            {
                foreach (Favourites favouriteShow in FavouritedShows.Instance.FavouriteShowsList)
                {
                    if (favouriteShow.ShowID == showInfo.ID)
                    {
                        await Connection.DeleteAsync(favouriteShow);
                        FavouritedShows.Instance.FavouriteShowsList.Remove(favouriteShow);
                        break;
                    }
                }
            }

            showInfo.IsInFavouriteList = !showInfo.IsInFavouriteList;
            if(showInfo.IsInFavouriteList == true)
            {
                favouritesButton.Source = "http://www.clker.com/cliparts/O/Z/4/Q/W/Q/favourite-md.png";
            }
            else
            {
                favouritesButton.Source = "https://img.icons8.com/windows/50/000000/add-to-favorites.png";
            }
            BindingContext = ShowInfo;
        }

        private void seasonPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            allSeasons.ChosenSeason = seasonPicker.SelectedIndex + 1;
            showListView.ItemsSource = allSeasons.DisplayChosenSeason;
        }

        private void FavouriteButtonClicked(object sender, EventArgs e)
        {
            ImageButton pressedButton = sender as ImageButton;
            AddShowToFavourites((ShowInfo)pressedButton.CommandParameter);
        }
    }
}