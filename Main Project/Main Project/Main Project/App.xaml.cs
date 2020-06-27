using Main_Project.Pages;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;

namespace Main_Project
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();



            //initialise the tabbed page as main page function
            var tabbedPage = new Xamarin.Forms.TabbedPage();
            tabbedPage.Children.Add(new SearchShowsPage());
            tabbedPage.Children.Add(new FavouritesPage());
            tabbedPage.Children.Add(new SettingsPage());
            //set tab at bottom of screen, currently there is no support that allows the tab toolbar to be placed at the bottom for windows WPF so it remains at the top
            tabbedPage.On<Android>().SetToolbarPlacement(Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);

            if (Current.Properties.ContainsKey("colourScheme"))
            {
                string colourString = Current.Properties["colourScheme"].ToString();
                Color colour1 = Color.Orange;
                Color colour2 = Color.Orange;
                switch (colourString)
                {
                    case "Blue":
                        colour1 = Color.Blue;
                        colour2 = Color.DarkBlue;
                        break;
                    case "Orange":
                        colour1 = Color.Orange;
                        colour2 = Color.DarkOrange;
                        break;
                    case "Pink":
                        colour1 = Color.Pink;
                        colour2 = Color.DeepPink;
                        break;
                }
                tabbedPage.BarBackgroundColor = colour2;
                tabbedPage.SelectedTabColor = colour1;
            }
            else
            {
                tabbedPage.BarBackgroundColor = Color.DarkOrange;
                tabbedPage.SelectedTabColor = Color.Orange;
            }

            tabbedPage.BarTextColor = Color.Gray;

            //listen for Theme change
            MessagingCenter.Subscribe<SettingsPage, bool>(this, SettingsPage.ThemeChanged, (sender, arg) =>
            {
                if (arg == false)
                {
                    tabbedPage.BackgroundColor = Color.White;
                }
                else if (arg == true)
                {
                    tabbedPage.BackgroundColor = Color.Black;
                }
            });

            //listen for colour scheme change
            MessagingCenter.Subscribe<SettingsPage, (Color, Color)>(this, SettingsPage.ColourSchemeChanged, (sender, arg) =>
            {
                tabbedPage.BarBackgroundColor = arg.Item2;
                tabbedPage.SelectedTabColor = arg.Item1;
            });

            tabbedPage.BackgroundColor = Color.White;
            if (Current.Properties.ContainsKey("theme"))
            {
                if ((bool)Current.Properties["theme"] == false)
                {
                    tabbedPage.BackgroundColor = Color.White;
                }
                else if ((bool)Current.Properties["theme"] == true)
                {
                    tabbedPage.BackgroundColor = Color.Black;
                }
            }

            SetupSQLConnection();
            MainPage = new NavigationPage(tabbedPage);
        }

        private async void SetupSQLConnection()
        {
            //initialise FavouritesList
            var Connection = DependencyService.Get<SQLiteInterface>().GetConnection();
            await Connection.CreateTableAsync<Favourites>();
            FavouritedShows.Instance.FavouriteShowsList = new ObservableCollection<Favourites>(Connection.Table<Favourites>().ToListAsync().Result);
            if(FavouritedShows.Instance.FavouriteShowsList.Count == 0)
            {
                FavouritedShows.Instance.DatabaseIDCount = 1;
            }
            else
            {
                FavouritedShows.Instance.DatabaseIDCount = FavouritedShows.Instance.FavouriteShowsList.Count;
            }           
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
