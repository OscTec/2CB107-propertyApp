using App1;
using App1.Classes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private String autoTerm = "";
        private String locationSearch = "";
        private HttpClient _client = new HttpClient();

        public HomePage()
        {
            InitializeComponent();
            styleElements();
            defineProperties();

        }

        // get users search term when search button is clicked
       private void searchClicked(String type)
        {
            locationSearch = homeSearch.Text;
            if (locationSearch != null)
            {
                // make sure user input is not empty
                if (locationSearch.Length > 1)
                {
                    
                    int SpecialCharCheck = locationSearch.Count(p => !char.IsLetterOrDigit(p));
                    if (SpecialCharCheck > 0)
                    {
                        inputError("Please enter a valid seach term");
                    }
                    else
                    {

                        // create a search results page
                        App.Current.Properties["searchTerm"] = locationSearch;
                        App.Current.Properties["searchType"] = type;
                        App.Current.Properties["pageNum"] = "1";
                        App.Current.Properties["pageSize"] = "15";

                        App.MasterDetail.IsPresented = false;
                        NavigationPage page = new NavigationPage(new ResultsPage());
                        page.BarBackgroundColor = functions.navbarColour();
                        App.MasterDetail.Detail = page;
                    }
                }
                else
                {
                    // if search term empty tell user
                    inputError("City / postcode search too short");
                }
            }
            else
            {
                inputError("No city or postcode entered");
            }
        }
        //place

        public void buySearch(object sender, EventArgs e)
        {
            searchClicked("sale");
        }

        public void rentSearch(object sender, EventArgs e)
        {
            searchClicked("rent");
        }

        // autocomplete for user input
        // TO DO: create back end json object 
        public void autoComplete(object sender, TextChangedEventArgs e)
        {
            autoTerm = e.NewTextValue;
            Error.Text = "";
            OnSearchResults(autoTerm);
        }

        protected async void OnSearchResults(String search)
        {
            try
            {
                //Getting JSON data from the Web
                var content = await _client.GetStringAsync("http://www.installet.com/json.php?search=" + search);
                //We deserialize the JSON data from this line
                var tr = JsonConvert.DeserializeObject<List<AutoComplete>>(content);
                ObservableCollection<AutoComplete> cities = new ObservableCollection<AutoComplete>(tr);
                citiesList.Children.Clear();
                citiesList.Children.Add(createAutoList(cities, citiesList));
            }
            catch (Exception) { }
        }

        private ListView createAutoList(ObservableCollection<AutoComplete> cities, StackLayout wrapper)
        {
            wrapper.HeightRequest = 30 * (cities.Count + 1);
            ListView autoList = new ListView { ItemTemplate = new DataTemplate(() => new PersonCellCS(homeSearch, cities)), ItemsSource = cities};
            return autoList;
        }

        private void inputError(String error)
        {
            Error.Text = error;
            Error.TextColor = functions.priceTextColour();
            Error.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
        }

        private void minBed(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            int mode = modePicker.SelectedIndex;
            BedPickerLabel.Text = BedPicker.Items[BedPicker.SelectedIndex];
            App.Current.Properties["beds"] = (mode + 1).ToString();
        }

        private void propertyType(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            int mode = modePicker.SelectedIndex;
            switch (mode)
            {
                case 0:
                    App.Current.Properties["property_type"] = "houses";
                    TypePickerLabel.Text = "Houses";
                    break;
                case 1:
                    App.Current.Properties["property_type"] = "flats";
                    TypePickerLabel.Text = "Flats";
                    break;
                case 2:
                    App.Current.Properties["property_type"] = "";
                    TypePickerLabel.Text = "All";
                    break;
            }
        }

        private void minPrice(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            string price = modePicker.Items[modePicker.SelectedIndex];
            MinPickerLabel.Text = MinPricePicker.Items[MinPricePicker.SelectedIndex];
            App.Current.Properties["minPrice"] = Regex.Replace(price, "[^.0-9]", "");
        }

        private void maxPrice(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            string price = modePicker.Items[modePicker.SelectedIndex];
            MaxPickerLabel.Text = MaxPricePicker.Items[MaxPricePicker.SelectedIndex];
            App.Current.Properties["maxPrice"] = Regex.Replace(price, "[^.0-9]", "");
        }


        private void styleElements()
        {
            logo.Source = functions.getLogo();
            backgroundColour.BackgroundColor = functions.backgroundColour();
            buyButton.BackgroundColor = functions.buttonColour();
            buyButton.TextColor = functions.textColour();
            buyButton.CornerRadius = 10;
            rentButton.BackgroundColor = functions.buttonColour();
            rentButton.TextColor = functions.textColour();
            rentButton.CornerRadius = 10;
            MinPricePicker.TextColor = functions.textColour();
            MaxPricePicker.TextColor = functions.textColour();
            BedPicker.TextColor = functions.textColour();
            TypePicker.TextColor = functions.textColour();
            Error.TextColor = functions.textColour();
            homeSearch.TextColor = functions.textColour();
            homeSearch.PlaceholderColor = functions.textColour();
            MinPickerLabel.TextColor = functions.textColour();
            MaxPickerLabel.TextColor = functions.textColour();
            BedPickerLabel.TextColor = functions.textColour();
            TypePickerLabel.TextColor = functions.textColour();
            searchImage.Source = "search" + functions.getLogo() + ".png";
        }

        private void MinPriceTapped(object sender, EventArgs e)
        {
            MinPricePicker.Focus();
        }

        private void MaxPriceTapped(object sender, EventArgs e)
        {
            MaxPricePicker.Focus();
        }

        private void BedTapped(object sender, EventArgs e)
        {
            BedPicker.Focus();
        }

        private void TypeTapped(object sender, EventArgs e)
        {
            TypePicker.Focus();
        }


        private void defineProperties()
        {
            App.Current.Properties["orderBy"] = "";
            App.Current.Properties["ordering"] = "";
            App.Current.Properties["furnished"] = "";
            App.Current.Properties["property_type"] = "";
            App.Current.Properties["beds"] = "1";
            App.Current.Properties["minPrice"] = "";
            App.Current.Properties["maxPrice"] = "";
        }
    }
}


public class PersonCellCS : ViewCell
{
    public PersonCellCS(Entry input,  ObservableCollection<AutoComplete> cities)
    {
        var grid = new Grid();
        Button buttonLabel = new Button
        {
            FontAttributes = FontAttributes.Bold,
            TextColor = functions.textColour(),
            FontSize = 20,
            BackgroundColor = functions.buttonColour(),
            Margin = new Thickness(0, -2, 0, 0),
            HeightRequest = 30
        };

        buttonLabel.Clicked += delegate {
            input.Text = buttonLabel.Text;            
            cities.Clear();
        };

        buttonLabel.SetBinding(Button.TextProperty, "city");
        grid.Children.Add(buttonLabel);
        View = grid;
    }
}
