using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.ObjectModel;
using App1.Classes;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultsPage : ContentPage
	{

        private string Url;
        // This handles the Web data request
        private HttpClient _client = new HttpClient();
        private Label streetName;
        private String listingType = "";
        private String noImage = "noimage.png";
        private Label priceLabel;
        private String pageNum, pageSize, searchWord, orderBy, ordering, furnished, propertyType, beds, minPrice, maxPrice;
        private Label town;
        private Label houseType;
        private Label shortDetails;
        private Label address;
        private BoxView bottomBorder;
        private Image mainImage;
        private Image agentLogo;
        private TapGestureRecognizer tgr;
        private int totalShowing = 0;
        private String totalResults = "0";
        private String payTerm = "per week";
        private List<options> favoriteHouses = new List<options>();
        private ObservableCollection<options> properties = new ObservableCollection<options>();

        public ResultsPage ()
		{
			InitializeComponent ();
		    this.listingType = App.Current.Properties["searchType"].ToString();

            if (this.listingType.Equals("sale"))
            {
                FurnishedFrame.IsVisible = false;
                payTerm = "";
            }

            this.pageNum = App.Current.Properties["pageNum"].ToString();
            this.searchWord = App.Current.Properties["searchTerm"].ToString();
            this.orderBy = App.Current.Properties["orderBy"].ToString();
            this.pageSize = App.Current.Properties["pageSize"].ToString();
            this.ordering = App.Current.Properties["ordering"].ToString();
            this.furnished = App.Current.Properties["furnished"].ToString();
            this.propertyType = App.Current.Properties["property_type"].ToString();
            this.beds = App.Current.Properties["beds"].ToString();
            this.minPrice = App.Current.Properties["minPrice"].ToString();
            this.maxPrice = App.Current.Properties["maxPrice"].ToString();

            searchTerm.Text = this.searchWord;
		    buildAPIURL();
		    Title = "Properties to " + getListingType();
            styleElements();
            OnSearchResults(true);
        }

        // click event handler when result row is clicked
        async void onStackClick(options house)
        {
            // create a details page and render it
            await Navigation.PushAsync(new DetailsPage(house));
        }

        protected async void OnSearchResults(Boolean firstTime)
        {
            try
            {
                //Getting JSON data from the Web
                var content = await _client.GetStringAsync(Url);
                //We deserialize the JSON data from this line
                Houses house = JsonConvert.DeserializeObject<Houses>(content);
                totalResults = house.result_count;
                List<options> houses = new List<options>();                
                if (firstTime)
                {
                    favoriteHouses = functions.dbPull();
                }

                foreach (options row in house.listing)
                {
                    Boolean found = false;
                    row._longitude = house.longitude;
                    if (firstTime)
                    {
                        foreach (options favorites in favoriteHouses)
                        {
                            if (favorites.listing_id.Equals(row.listing_id))
                            {
                                houses.Insert(0, row);
                                found = true;
                                row.setFavorite(true);
                                break;
                            }
                        }
                    }                   
                    if (!found) {
                        houses.Add(row);
                    }
                }
                //After deserializing , we store our data in the List called ObservableCollection
                properties = new ObservableCollection<options>(houses);
                //Then finally we attach the List to the ListView. Seems Simple :)
                foreach (options property in properties)
                {                    
                    renderDisplay(property);
                }
                filters.IsVisible = true;
            }
            catch (Exception)
            {
                if(properties.Count < 1) {
                    noResults();
                }
            }
            totalShowing = Convert.ToInt32(this.pageSize) * Convert.ToInt32(this.pageNum);
            if(totalShowing > Convert.ToInt32(totalResults))
            {
                totalShowing = Convert.ToInt32(totalResults);
            }
            resultCount.Text = "Results found (" + totalResults + ") - showing(" + Convert.ToString(totalShowing) + ")";
            if (firstTime)
            {
                if ((totalShowing + Convert.ToInt32(this.pageSize) < Convert.ToInt32(totalResults)))
                {
                    var moreButton = new Button()
                    {
                        Text = "View More",

                    };
                    moreButton.BackgroundColor = functions.backgroundColour();
                    moreButton.TextColor = functions.textColour();
                    moreButton.Clicked += moreResults;
                    showMore.Children.Add(moreButton);
                }
                else
                {
                    if (Convert.ToInt32(totalResults) > 0)
                    {
                        var moreButton = new Button()
                        {
                            Text = "Search again?",

                        };

                        moreButton.BackgroundColor = functions.backgroundColour();
                        moreButton.TextColor = functions.textColour();

                        moreButton.Clicked += delegate
                        {
                            App.MasterDetail.IsPresented = false;
                            NavigationPage page = new NavigationPage(new HomePage());
                            page.BarBackgroundColor = functions.navbarColour();
                            App.MasterDetail.Detail = page;
                        };
                        showMore.Children.Add(moreButton);
                    }
                }
            }
            loader.IsVisible = false;
            moreLoader.IsVisible = false;
        }


        private void renderDisplay(options property)
        {
            // create tap to move to info page
            tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) => onStackClick(property);
            // create a house image for each search result
            mainImage = new Image()
            {
               // Source = property.image_url, //a 600x300 picture
                WidthRequest = 600,
                HeightRequest = 300,
                MinimumHeightRequest = 300,
                MinimumWidthRequest = 600,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                Aspect = Aspect.AspectFill,
            };

            Label favorites = new Label()
            {
                TextColor = functions.priceTextColour(),
                Text = "",
                FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"])
            };

            if (property.getFavorite())
            {
                favorites.Text = "Favorite";
            }

            if (property.image_url.Length > 0)
            {
                mainImage.Source = new UriImageSource
                {
                    Uri = new Uri(property.image_url)
                };
            }
            else
            {
               mainImage.Source = noImage;
            }

            agentLogo = new Image()
            {
                Source = property.agent_logo,
                Margin = new Thickness(0, 0, 5, 0),
            };

            // add a street name for each search
            streetName = new Label()
            {
                Text = property.street_name + " - " + property.post_town,
                FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"])
            };

            // add a price
            priceLabel = new Label()
            {
                Text = "£" + property.price + " " + payTerm,
                TextColor = functions.priceTextColour(),
                FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]),
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
            };

            //add a house type 
            houseType = new Label()
            {
                Text = property.num_bedrooms + " " + property.property_type,
                TextColor = Color.White,
                FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]),
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),
            };

            //add town
            town = new Label()
            {
                Text = property.post_town,
                FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"])
            };

            bottomBorder = new BoxView()
            {
                BackgroundColor = functions.gapColour()
            };
            bottomBorder.HeightRequest = 3;

            StackLayout imageStack = new StackLayout()
            {
                Margin = -6,
                BackgroundColor = Color.White
            };


            shortDetails = new Label()
            {
                Text = property.num_bedrooms + " bedrooms " + property.property_type,
                TextColor = functions.textColour(),
                FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]),
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
            };

            address = new Label()
            {
                Text = property.displayable_address,
                TextColor = functions.textColour(),
                FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]),
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
            };

            imageStack.Children.Add(mainImage);
            imageStack.GestureRecognizers.Add(tgr);

            // add all elements to the stack layout
            searchTable.Children.Add(imageStack);
            searchTable.Children.Add(createStack(agentLogo, priceLabel, shortDetails, address, tgr, favorites));
            searchTable.Children.Add(new BoxView() { BackgroundColor = functions.buttonColour() });
            searchTable.Children.Add(bottomBorder);
        }
         
        private StackLayout createStack(Image agentLogo, Label priceLabel, Label shortDescription, Label address, TapGestureRecognizer tgr, Label favorites)
        {
            StackLayout gridStack = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 20)
            };

            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(priceLabel, 0, 0);
            grid.Children.Add(favorites, 1, 0);
            grid.Children.Add(shortDescription, 0, 1);
            grid.Children.Add(address, 0, 2);
            grid.Children.Add(agentLogo, 1, 1);
            Grid.SetRowSpan(agentLogo, 3);
            gridStack.Children.Add(grid);
            return gridStack;
        }



        private void noResults()
        {
            Label resultLayer = new Label()
            {
                Text = "No Results",
                TextColor = Color.White,
                FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]),
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),

            };
            Image brokenLink = new Image()
            {
                Source = "noresult" + functions.getLogo() + ".png",
                WidthRequest = 80,
                HeightRequest = 80

            };

            StackLayout layout = new StackLayout()
            {
                Margin = new Thickness(20, 100, 20, 0)
            };

            Button searchAgain = new Button()
            {
                Text = "Search again?",
                CornerRadius = 20,
                WidthRequest = 40,
                HeightRequest = 40,
            };


            searchAgain.Clicked += delegate
            {
                App.MasterDetail.IsPresented = false;
                NavigationPage page = new NavigationPage(new HomePage());
                page.BarBackgroundColor = functions.navbarColour();
                App.MasterDetail.Detail = page;
            };

            searchAgain.BackgroundColor = functions.backgroundColour();
            searchAgain.TextColor = functions.textColour();

            layout.Children.Add(brokenLink);
            layout.Children.Add(resultLayer);
            layout.Children.Add(new BoxView() { BackgroundColor = Color.Transparent });
            layout.Children.Add(searchAgain);
            searchTable.Children.Add(layout);
        }

        private String getListingType()
        {
            switch (listingType)
            {
                case "sale":
                    return "buy";
                case "rent":
                    return listingType;
                default:
                    return listingType;
            }
        }

        private void buildAPIURL()
        {
            String appendListing = "";
            if (listingType != null)
            {
                appendListing = "&listing_status=" + listingType;
            }
            Url = "http://api.zoopla.co.uk/api/v1/property_listings.json?api_key=jnwgh2seuv6q7t9u7weur68s&area=" + this.searchWord + appendListing + "&page_number=" + this.pageNum + "&page_size=" + this.pageSize + "order_by=" + this.orderBy + "&ordering=" + this.ordering + "&furnished=" + this.furnished + "&property_type=" +this.propertyType + "&minimum_beds=" + this.beds + "&minimum_price="+ this.minPrice + "&maximum_price=" + this.maxPrice;
        }

        private void moreResults(object sender, EventArgs e)
        {
            moreLoader.IsVisible = true;
            this.pageNum = Convert.ToString(Convert.ToInt32(this.pageNum) + 1);
            buildAPIURL();
            OnSearchResults(false);            
        }

        private void sortByPrice(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            int mode = modePicker.SelectedIndex;
            switch (mode)
            {
                case 0:
                    App.Current.Properties["ordering"] = "descending";
                    PricePickerLabel.Text = "High - Low";
                    break;
                case 1:
                    App.Current.Properties["ordering"] = "ascending";
                    PricePickerLabel.Text = "Low - High";
                    break;
                
            }
        }

        private void sortByOrder(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            int mode = modePicker.SelectedIndex;
            switch (mode)
            {
                case 0:
                    App.Current.Properties["order_by"] = "price";
                    OrderPickerLabel.Text = "Price";
                    break;
                case 1:
                    App.Current.Properties["order_by"] = "age";
                    OrderPickerLabel.Text = "Age";
                    break;

            }
        }

        private void PriceTapped(object sender, EventArgs e)
        {
            SortPricePicker.Focus();
        }

        private void OrderTapped(object sender, EventArgs e)
        {
            SortOrderPicker.Focus();
        }

        private void TypeTapped(object sender, EventArgs e)
        {
            SortTypePicker.Focus();
        }

        private void FurnishedTapped(object sender, EventArgs e)
        {
            SortFurnishedPicker.Focus();
        }


        private void sortByType(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            int mode = modePicker.SelectedIndex;
            switch (mode)
            {
                case 0:
                    App.Current.Properties["property_type"] = "houses";
                    OrderTypeLabel.Text = "Houses";
                    break;
                case 1:
                    App.Current.Properties["property_type"] = "flats";
                    OrderTypeLabel.Text = "Flats";
                    break;
                case 2:
                    App.Current.Properties["property_type"] = "";
                    OrderTypeLabel.Text = "All";
                    break;

            }
        }

        private void sortByFurnished(object sender, EventArgs e)
        {
            Picker modePicker = (Picker)sender;
            int mode = modePicker.SelectedIndex;
            switch (mode)
            {
                case 0:
                    App.Current.Properties["furnished"] = "furnished";
                    OrderFurnishedLabel.Text = "Furnished";
                    break;
                case 1:
                    App.Current.Properties["furnished"] = "unfurnished";
                    OrderFurnishedLabel.Text = "Unfurnished";
                    break;
                case 2:
                    App.Current.Properties["furnished"] = "part-furnished";
                    OrderFurnishedLabel.Text = "Part furnished";
                    break;
                case 3:
                    App.Current.Properties["furnished"] = "";
                    OrderFurnishedLabel.Text = "All";
                    break;

            }
        }

        private void clearFilter()
        {
            App.Current.Properties["furnished"] = "";
            App.Current.Properties["property_type"] = "";
            App.Current.Properties["order_by"] = "";
            App.Current.Properties["ordering"] = "";
            App.MasterDetail.IsPresented = false;
            NavigationPage page = new NavigationPage(new ResultsPage());
            page.BarBackgroundColor = functions.navbarColour();
            App.MasterDetail.Detail = page;
        }

        private void applyFilter()
        {
            App.MasterDetail.IsPresented = false;
            NavigationPage page = new NavigationPage(new ResultsPage());
            page.BarBackgroundColor = functions.navbarColour();
            App.MasterDetail.Detail = page;
        }


        private void styleElements()
        {
            headerHR.BackgroundColor = functions.gapColour();
            headerHR2.BackgroundColor = functions.gapColour();
            searchTerm.TextColor = functions.textColour();
            resultCount.TextColor = functions.textColour();
            backgroundColour.BackgroundColor = functions.backgroundColour();
            loadingSearchText.TextColor = functions.textColour();
            loadingText.TextColor = functions.textColour();
            PricePickerLabel.TextColor = functions.textColour();
            OrderTypeLabel.TextColor = functions.textColour();
            OrderFurnishedLabel.TextColor = functions.textColour();
            filterButton.BackgroundColor = functions.buttonColour();
            filterButton.TextColor = functions.textColour();
            filterButton.CornerRadius = 10;
            clearButton.BackgroundColor = functions.buttonColour();
            clearButton.TextColor = functions.textColour();
            clearButton.CornerRadius = 10;
            resultsLoader.Color = functions.textColour();
            moreLoader.Color = functions.textColour();
            OrderPickerLabel.TextColor = functions.textColour();
        }
    }
}