using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.IO;
using App1.Classes;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {

        // This handles the Web data request
        private Label streetName;
        private String noImage = "noimage.png";
        private Label priceLabel;
        private Label town;
        private Label houseType;
        private Label shortDetails;
        private Image agentLogo;
        private Label address;
        private String payTerm = "per week";
        private BoxView bottomBorder;
        private Image mainImage;
        private TapGestureRecognizer tgr;

        public FavoritesPage()
        {
            InitializeComponent();
            fetchFavorites();
            backgroundColour.BackgroundColor = functions.backgroundColour();

        }

        private void fetchFavorites() {         
            List<options> allHouses = functions.dbPull();
            foreach (var property in allHouses){
                renderDisplay(property);
            }

            if(allHouses.Count < 1)
            {
                noResults();
            }
            loader.IsVisible = false;             
            
        }

        private void renderDisplay(options property)
        {
            // create tap to move to info page
            tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) => onStackClick(property);
            if (property.image_url.Length < 1)
            {
                property.image_url = noImage;
            }
            // create a house image for each search result
            mainImage = new Image()
            {
                Source = property.image_url, //a 600x300 picture
                WidthRequest = 600,
                HeightRequest = 300,
                MinimumHeightRequest = 300,
                MinimumWidthRequest = 600,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                Aspect = Aspect.AspectFill
            };

            if (property.listing_status.Equals("sale"))
            {
                payTerm = "";
            }


            //getImage(property.image_url);

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
                Text = "£" + property.price + " " + this.payTerm,
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
                FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"])
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
                //HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
            };

            address = new Label()
            {
                Text = property.displayable_address,
                TextColor = functions.textColour(),
                FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]),
                //HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
            };

            Button removeBtn = new Button()
            {
                Text = "Unfavorite",
                BackgroundColor = functions.buttonColour(),
                TextColor = functions.textColour(),
                Margin = new Thickness(5, 0, 5, 0),
            };

            removeBtn.Clicked += (o, e) => {
                functions.dbDelete(property);
                App.MasterDetail.IsPresented = false;
                NavigationPage page = new NavigationPage(new FavoritesPage());
                page.BarBackgroundColor = functions.navbarColour();
                App.MasterDetail.Detail = page;
            };



            imageStack.Children.Add(priceLabel);
            imageStack.Children.Add(mainImage);
            imageStack.Children.Add(new BoxView() { BackgroundColor = functions.backgroundColour(), Margin = -2 });
            imageStack.GestureRecognizers.Add(tgr);

            // add all elements to the stack layout
            searchTable.Children.Add(imageStack);
            searchTable.Children.Add(createStack(agentLogo, priceLabel, shortDetails, address, tgr, removeBtn));
            searchTable.Children.Add(bottomBorder);
        }

        // Create a stack for all display components to sit in
        private StackLayout createStack(Image agentLogo, Label priceLabel, Label shortDescription, Label address, TapGestureRecognizer tgr, Button remove)
        {
            StackLayout gridStack = new StackLayout()
            {
                        Padding = 20
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(priceLabel, 0, 0);
            grid.Children.Add(shortDescription, 0, 1);
            grid.Children.Add(address, 0, 2);
            grid.Children.Add(agentLogo, 1, 1);
            Grid.SetRowSpan(agentLogo, 2);
            grid.Children.Add(new BoxView(), 0, 3);
            grid.Children.Add(remove, 0, 4);
            Grid.SetColumnSpan(remove, 2);
            gridStack.Children.Add(grid);
            return gridStack;
        }

        private void noResults()
        {
            Label resultLayer = new Label()
            {
                Text = "No favorites found",
                TextColor = functions.textColour(),
                FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]),
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),

            };
            Image brokenLink = new Image()
            {
                Source = "noresult"+ functions.getLogo() +".png",
                WidthRequest = 80,
                HeightRequest = 80

            };

            StackLayout layout = new StackLayout()
            {
                Margin = new Thickness(0, 100, 0, 0)
            };

            layout.Children.Add(brokenLink);
            layout.Children.Add(new BoxView() { BackgroundColor = Color.Transparent });
            layout.Children.Add(resultLayer);
            searchTable.Children.Add(layout);
        }

        async void onStackClick(options house)
        {
            // create a details page and pre-render it
            DetailsPage page = new DetailsPage(house, true);
            await Navigation.PushAsync(page);
        }

    }
}