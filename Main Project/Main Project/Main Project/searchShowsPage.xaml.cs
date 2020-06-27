using Main_Project.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Main_Project
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

public partial class SearchShowsPage : ContentPage
{
        public ListSearchShowResults MatchedShows;
        static readonly HttpClient Client = new HttpClient();
        SQLiteAsyncConnection Connection { get; set; }
        public Color TextColour;

        private string currentRegion { get; set; }
        private string LastRegion { get; set; }
        private string ErrorType { get; set; }




        public SearchShowsPage()
        {
            InitializeComponent();
            MatchedShows = new ListSearchShowResults();
            Connection = DependencyService.Get<SQLiteInterface>().GetConnection();
            BindingContext = MatchedShows;
            AddToFavourites = new Command<SearchShowResult>(AddShowToFavourites);
            Connection.CreateTableAsync<Favourites>();

            if (Application.Current.Properties.ContainsKey("colourScheme"))
            {
                string colourString = Application.Current.Properties["colourScheme"].ToString();
                Color LightColour = Color.Orange;
                Color DarkColour = Color.Orange;
                switch (colourString)
                {
                    case "Blue":
                        LightColour = Color.Blue;
                        DarkColour = Color.DarkBlue;
                        break;
                    case "Orange":
                        LightColour = Color.Orange;
                        DarkColour = Color.DarkOrange;
                        break;
                    case "Pink":
                        LightColour = Color.Pink;
                        DarkColour = Color.DeepPink;
                        break;
                }
                searchButton.BackgroundColor = DarkColour;
                searchBox.BackgroundColor = LightColour;
                searchBoxBoarder.BackgroundColor = LightColour;
            }
            else
            {
                searchButton.BackgroundColor = Color.DarkOrange;
                searchBox.BackgroundColor = Color.Orange;
                searchBoxBoarder.BackgroundColor = Color.Orange;

            }

            MessagingCenter.Subscribe<SettingsPage, string>(this, SettingsPage.RegionChanged, (sender, arg) =>
            {
                //only run if search has been done and region was changed
                if (LastRegion != null)
                {
                    if (LastRegion != arg)
                    {
                        SearchButtonClicked(null, null);
                    }
                }
            });

            MessagingCenter.Subscribe<SettingsPage, (Color, Color)>(this, SettingsPage.ColourSchemeChanged, (sender, arg) =>
            {
                searchButton.BackgroundColor = arg.Item2;
                searchBox.BackgroundColor = arg.Item1;
                searchBoxBoarder.BackgroundColor = arg.Item1;

            });
        }

        public async Task<ObservableCollection<SearchShowResult>> SearchShows(string searchString)
        {
            try
            {
                HttpResponseMessage response = await Client.GetAsync(string.Format("http://api.tvmaze.com/search/shows?q={0}", searchString)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<SearchShowResult>>(responseBody);
            }
            catch (HttpRequestException)
            {
                var connection = Connectivity.NetworkAccess;
                if(connection != NetworkAccess.Internet)
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

        private void SearchButtonClicked(object sender, EventArgs e)
        {
            sounds.TapButton();

            //reset matchedshows
            MatchedShows = new ListSearchShowResults();

            ObservableCollection<SearchShowResult> unfilteredSearchResults = SearchShows(searchBox.Text).Result;

            if(unfilteredSearchResults == null)
            {
                noShowsFoundLabel.Text = ErrorType;
                noShowsFoundLabel.IsVisible = true;            
            }
            else
            {
                noShowsFoundLabel.Text = "No Shows Found";
                if (Application.Current.Properties.ContainsKey("region"))
                {
                    currentRegion = Application.Current.Properties["region"].ToString();
                }
                else
                {
                    currentRegion = "AU";
                }
                LastRegion = currentRegion;

                foreach (var Result in unfilteredSearchResults)
                {
                    if (Result.show.Network?.Country?.Code == currentRegion || Result.show.WebChannel?.Country?.Code == currentRegion)
                    {
                        if (MatchedShows.searchResults is null)
                        {
                            MatchedShows.searchResults = new ObservableCollection<SearchShowResult>();
                            noShowsFoundLabel.IsVisible = false;
                        }
                        MatchedShows.searchResults.Add(Result);
                    }
                }
                if (MatchedShows.searchResults is null)
                {
                    noShowsFoundLabel.IsVisible = true;
                }

                //determine if show is in Favourites list to show correct favourites button   
                if (MatchedShows.searchResults != null)
                {
                    foreach (SearchShowResult show in MatchedShows.searchResults)
                    {
                        foreach (Favourites favouritedShow in FavouritedShows.Instance.FavouriteShowsList)
                        {
                            if (show.show.Id == favouritedShow.ShowID)
                            {
                                show.IsInFavouriteList = true;
                                break;
                            }
                            else
                            {
                                show.IsInFavouriteList = false;
                            }
                        }
                    }
                }

                BindingContext = MatchedShows;
            }            
        }

        public ICommand AddToFavourites { get; private set; }

        async void  AddShowToFavourites(SearchShowResult result)
        {            
            if (!result.IsInFavouriteList)
            {
                
                await Connection.InsertAsync(new Favourites()
                {
                    ShowID = result.show.Id
                });                
                    FavouritedShows.Instance.FavouriteShowsList.Add(new Favourites
                    {
                        Id = FavouritedShows.Instance.DatabaseIDCount,
                        ShowID = result.show.Id
                    });
                //Add one to DatabaseIDCount to match primary key id in database
                FavouritedShows.Instance.DatabaseIDCount++;
            }
            else
            {
                foreach(Favourites favouriteShow in FavouritedShows.Instance.FavouriteShowsList)
                {
                    if(favouriteShow.ShowID == result.show.Id)
                    {
                        await Connection.DeleteAsync(favouriteShow);
                        FavouritedShows.Instance.FavouriteShowsList.Remove(favouriteShow);
                        break;
                    }
                }                                
            }

            result.IsInFavouriteList = !result.IsInFavouriteList;
            BindingContext = MatchedShows;
        }


        private async void SelectedShowAsync(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection.Count != 0)
            {
                var tappedshow = e.CurrentSelection[0] as SearchShowResult;
                string showID = tappedshow.show.Id;

                await Navigation.PushAsync(new ShowInfoPage());

                MessagingCenter.Send<SearchShowsPage, (string, bool)>(this, ShowInfoPage.showSelected, (showID, tappedshow.IsInFavouriteList));
            }           
        }

        private void FavouriteButtonClicked(object sender, EventArgs e)
        {
            ImageButton pressedButton = sender as ImageButton;
            AddShowToFavourites((SearchShowResult)pressedButton.CommandParameter);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();            
            if (Application.Current.Properties.ContainsKey("theme") && (bool)Application.Current.Properties["theme"] == true)
            {
                myCollectionView.BackgroundColor = Color.Black;
                this.BackgroundColor = Color.Black;
            }
            else
            {
                myCollectionView.BackgroundColor = Color.White;
                this.BackgroundColor = Color.White;
            }
            
            myCollectionView.SelectedItem = null;
            if (MatchedShows.searchResults != null)
            {
                foreach (SearchShowResult result in MatchedShows.searchResults)
                {
                    foreach (Favourites favourite in FavouritedShows.Instance.FavouriteShowsList)
                    {
                        if (result.show.Id == favourite.ShowID)
                        {
                            result.IsInFavouriteList = true;
                            break;
                        }
                        else
                        {
                            result.IsInFavouriteList = false;
                        }
                    }
                }
                BindingContext = MatchedShows;
            }
        }
    }
}