using Main_Project.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Main_Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel SettingsView { get; set; }
        public static string ColourSchemeChanged = "ColourSchemeChanged";
        public static string ThemeChanged = "ThemeChanged";
        public static string RegionChanged = "RegionChanged";
        public SettingsPage()
        {
            InitializeComponent();
            SettingsView = new SettingsViewModel();
            BindingContext = SettingsView;
            loadAttributes();
        }

        private void loadAttributes()
        {
            if (Application.Current.Properties.ContainsKey("sound") && (bool)Application.Current.Properties["sound"])
            {
                soundSwitch.IsToggled = true;
            }
            if (Application.Current.Properties.ContainsKey("theme") && (bool)Application.Current.Properties["theme"])
            {
                themeSwitch.IsToggled = true;
                themeSwitch.BackgroundColor = Color.White;
                soundSwitch.BackgroundColor = Color.White;
                this.BackgroundColor = Color.Black;
            }            
            if (Application.Current.Properties.ContainsKey("colourScheme"))
            {
                colourSchemePicker.SelectedItem = Application.Current.Properties["colourScheme"];
            }
            else
            {
                colourSchemePicker.SelectedItem = "Orange";
            }
            if (Application.Current.Properties.ContainsKey("region"))
            {
                CalculateColourSchemePickerSelectedItem();
            }
            else
            {
                regionPicker.SelectedItem = "Australia";
            }

            if (Application.Current.Properties.ContainsKey("colourScheme"))
            {
                string colourString = Application.Current.Properties["colourScheme"].ToString();
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

                //set sound colours
                SoundTitle.TextColor = colour1;
                soundSwitch.OnColor = colour2;
                soundSwitch.ThumbColor = colour1;

                //set Theme Colours
                ThemeTitle.TextColor = colour1;
                themeSwitch.OnColor = colour2;
                themeSwitch.ThumbColor = colour1;

                //set Scheme colours
                ColourSchemeTitle.TextColor = colour1;

                //set region colours
                RegionTitle.TextColor = colour1;

                submitButton.BackgroundColor = colour1;
            }
            if (Application.Current.Properties.ContainsKey("region"))
            {
                CalculateColourSchemePickerSelectedItem();
            }
        }

        private void CalculateColourSchemePickerSelectedItem()
        {
            switch (Application.Current.Properties["region"])
            {
                case "AU":
                    regionPicker.SelectedItem = "Australia";
                    break;
                case "GB":
                    regionPicker.SelectedItem = "United Kingdom";
                    break;
                case "US":
                    regionPicker.SelectedItem = "America";
                    break;
                case "RU":
                    regionPicker.SelectedItem = "Russian Federation";
                    break;
                case "CN":
                    regionPicker.SelectedItem = "China";
                    break;
                case "IE":
                    regionPicker.SelectedItem = "Ireland";
                    break;
                case "JP":
                    regionPicker.SelectedItem = "Japan";
                    break;
                case "KR":
                    regionPicker.SelectedItem = "Korea, Republic of";
                    break;
                default:
                    regionPicker.SelectedItem = "Australia";
                    break;
            }
        }

        private void SubmitSettings(object sender, EventArgs e)
        {
            sounds.TapButton();

            Application.Current.Properties["sound"] = soundSwitch.IsToggled;
            Application.Current.Properties["theme"] = themeSwitch.IsToggled;
            Application.Current.Properties["colourScheme"] = colourSchemePicker.SelectedItem;
            Application.Current.Properties["region"] = regionPicker.SelectedItem;
            switch (regionPicker.SelectedItem)
            {
                case "Australia":
                    Application.Current.Properties["region"] = "AU";
                    break;
                case "United Kingdom":
                    Application.Current.Properties["region"] = "GB";
                    break;
                case "America":
                    Application.Current.Properties["region"] = "US";
                    break;
                case "Russian Federation":
                    Application.Current.Properties["region"] = "RU";
                    break;
                case "China":
                    Application.Current.Properties["region"] = "CN";
                    break;
                case "Ireland":
                    Application.Current.Properties["region"] = "IE";
                    break;
                case "Japan":
                    Application.Current.Properties["region"] = "JP";
                    break;
                case "Korea, Republic of":
                    Application.Current.Properties["region"] = "KR";
                    break;
            }
            Application.Current.SavePropertiesAsync();           


            if ((bool)Application.Current.Properties["theme"] == true)
            {
                BackgroundColor = Color.Black;
                soundSwitch.BackgroundColor = Color.Gray;
                themeSwitch.BackgroundColor = Color.Gray;
            }
            else
            {
                BackgroundColor = Color.White;
                soundSwitch.BackgroundColor = Color.White;
                themeSwitch.BackgroundColor = Color.White;
            }
            //let App.xaml.cs know theme may have changed
            MessagingCenter.Send<SettingsPage, bool>(this, ThemeChanged, (bool)Application.Current.Properties["theme"]);

            //update scheme
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

            //***setting TextColor on TableSection in android does not seem to work currently***

            //set sound colours
            SoundTitle.TextColor = LightColour;
            soundSwitch.OnColor = DarkColour;
            soundSwitch.ThumbColor = LightColour;

            //set Theme Colours
            ThemeTitle.TextColor = LightColour;
            themeSwitch.OnColor = DarkColour;
            themeSwitch.ThumbColor = LightColour;

            //set Scheme colours
            ColourSchemeTitle.TextColor = LightColour;

            //set region colours
            RegionTitle.TextColor = LightColour;

            submitButton.BackgroundColor = LightColour;
            //let app.xaml.cs know scheme may have changed
            MessagingCenter.Send<SettingsPage, (Color, Color)>(this, ColourSchemeChanged, (LightColour, DarkColour));

            //let searchShow know region changed
            MessagingCenter.Send<SettingsPage, string>(this, RegionChanged, Application.Current.Properties["region"].ToString());
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            sounds.TapButton();
        }

        private void Switch_OnChanged(object sender, ToggledEventArgs e)
        {
            sounds.SlideSwitch();
        }
    }
}