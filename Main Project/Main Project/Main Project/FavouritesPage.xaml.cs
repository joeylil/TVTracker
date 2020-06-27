
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Main_Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritesPage : ContentPage
    {
        List<CountrySchedule> FullSchedule { get; set; }
        private ObservableCollection<Show> FavouriteShowsWithScheduledEpisodes { get; set; }
        SQLiteAsyncConnection Connection { get; set; }

        private string ErrorType { get; set; }
        private string currentRegion { get; set; }


        static readonly HttpClient client = new HttpClient();  


        public FavouritesPage()
        {
            InitializeComponent();
            Connection = DependencyService.Get<SQLiteInterface>().GetConnection();
            FullSchedule = GetScheduleAsync().Result;
            AddToFavourites = new Command<Show>(AddShowToFavourites);
        }

        async Task<List<CountrySchedule>> GetScheduleAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://api.tvmaze.com/schedule/full").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CountrySchedule>>(responseBody);
            }
            catch
            {
                var connection = Connectivity.NetworkAccess;
                if (connection != NetworkAccess.Internet)
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

        async Task<Show> GetShowAsync(string showID)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("http://api.tvmaze.com/shows/{0}", showID)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Show>(responseBody);
            }
            catch (HttpRequestException)
            {
                var connection = Connectivity.NetworkAccess;
                if (connection != NetworkAccess.Internet)
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //reset nofavouritedshowslabel
            noFavouritedShowsLabel.Text = "Oops, you haven't favourited any shows yet!";
            noFavouritedShowsLabel.IsVisible = false;
            favouriteShowListView.IsVisible = true;

            //configure visuals
            if (Application.Current.Properties.ContainsKey("theme") && (bool)Application.Current.Properties["theme"] == true)
            {
                this.BackgroundColor = Color.Black;
            }
            else
            {
                this.BackgroundColor = Color.White;
            }

            var connection = DependencyService.Get<SQLiteInterface>().GetConnection();

            FavouriteShowsWithScheduledEpisodes = new ObservableCollection<Show>();

            foreach (var favouritedShow in FavouritedShows.Instance.FavouriteShowsList)
            {
                var showToAdd = GetShowAsync(favouritedShow.ShowID).Result;
                if(showToAdd == null)
                {
                    favouriteShowListView.IsVisible = false;
                    noFavouritedShowsLabel.Text = ErrorType;
                    noFavouritedShowsLabel.IsVisible = true;
                    return;
                }
                FavouriteShowsWithScheduledEpisodes.Add(showToAdd);                
            }
            foreach(var show in FavouriteShowsWithScheduledEpisodes)
            {
                show.IsInFavouriteList = true;
            }


            if (FullSchedule == null)
            {
                //try initialise schedule again
                FullSchedule = GetScheduleAsync().Result;
                if (FullSchedule == null)
                {
                    //show errors if still null and return
                    noFavouritedShowsLabel.Text = ErrorType;
                    noFavouritedShowsLabel.IsVisible = true;
                    favouriteShowListView.IsVisible = false;
                }
            }
            else
            {
                if (Application.Current.Properties.ContainsKey("region"))
                {
                    currentRegion = Application.Current.Properties["region"].ToString();
                }
                else
                {
                    currentRegion = "AU";
                }

                //gather schedule for all favourited shows with a schedule for the current country
                foreach (var scheduledShow in FullSchedule)
                {
                    if (scheduledShow.Embedded.Show.Network?.Country?.Code == currentRegion || scheduledShow.Embedded.Show.WebChannel?.Country?.Code == currentRegion)
                    {
                        foreach (var favouritedShow in FavouritedShows.Instance.FavouriteShowsList)
                        {
                            if (scheduledShow.Embedded.Show.Id.ToString() == favouritedShow.ShowID)
                            {
                                if (scheduledShow.Airstamp > System.DateTime.Now)
                                {
                                    foreach (Show show in FavouriteShowsWithScheduledEpisodes)
                                    {
                                        if (show.Id.ToString() == favouritedShow.ShowID)
                                        {
                                            //make sure episodecollection is not null
                                            if (show.EpisodeCollection == null)
                                            {
                                                show.EpisodeCollection = new EpisodeCollection();
                                            }
                                            //add show to collection
                                            show.EpisodeCollection.Episodes.Add(new EpisodeInfo()
                                            {
                                                Url = scheduledShow.Url,
                                                Name = scheduledShow.Name,
                                                Season = scheduledShow.Season,
                                                Episode = scheduledShow.Episode,
                                                AirStamp = scheduledShow.Airstamp,
                                                Runtime = scheduledShow.Runtime,
                                                Image = scheduledShow.Image,
                                                Summary = scheduledShow.Summary,
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //sort shows by airdate, null airdate is given max value for datetime
                FavouriteShowsWithScheduledEpisodes = new ObservableCollection<Show>(FavouriteShowsWithScheduledEpisodes.ToList().OrderBy(order => order.getAirdateForSorting));
                favouriteShowListView.ItemsSource = FavouriteShowsWithScheduledEpisodes;

                if (FavouriteShowsWithScheduledEpisodes.Count == 0)
                {
                    noFavouritedShowsLabel.IsVisible = true;
                }
                else
                {
                    noFavouritedShowsLabel.IsVisible = false;
                }
            }
        }

        private async void SelectedShowAsync(object sender, SelectionChangedEventArgs e)
        {
            var tappedshow = favouriteShowListView.SelectedItem as Show;
            string showID = tappedshow.Id.ToString();

            await Navigation.PushAsync(new ShowInfoPage());

            MessagingCenter.Send<FavouritesPage, string>(this, ShowInfoPage.showSelected, showID);
        }

        private void FavouriteButtonClicked(object sender, System.EventArgs e)
        {
            ImageButton pressedButton = sender as ImageButton;
            AddShowToFavourites((Show)pressedButton.CommandParameter);
        }

        public ICommand AddToFavourites { get; private set; }

        async void AddShowToFavourites(Show show)
        {
                foreach (Favourites favouriteShow in FavouritedShows.Instance.FavouriteShowsList)
                {
                    if (favouriteShow.ShowID == show.Id.ToString())
                    {
                        await Connection.DeleteAsync(favouriteShow);
                        FavouritedShows.Instance.FavouriteShowsList.Remove(favouriteShow);
                        break;
                    }
                }
            show.IsInFavouriteList = !show.IsInFavouriteList;
            FavouriteShowsWithScheduledEpisodes.Remove(show);
        }       
    }
}