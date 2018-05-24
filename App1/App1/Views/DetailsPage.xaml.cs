using Plugin.Messaging;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Share;
using Plugin.Share.Abstractions;
using App1.Classes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {


        private options houseDetails;
        private Boolean newPage;
        private Boolean houseFavourited = false;
        private string bigDescription, smallDescription;
        private String payTerm = "per week";
        private HttpClient _client = new HttpClient();
        private ObservableCollection<Stats> allStats = new ObservableCollection<Stats>();

        public DetailsPage(options house, Boolean newPage = false)
        {

            InitializeComponent();
            styleElements();
            fetchStats(house.outcode);
            this.smallDescription = house.short_description;
            this.bigDescription = house.description;

            try
            {
                houseDetails = house;
                this.newPage = newPage;
                List<options> allHouses = functions.dbPull();
                foreach (var property in allHouses)
                {
                    if (property.listing_id.Equals(houseDetails.listing_id))
                    {
                        houseFavourited = true;
                    }
                }

            }
            catch (Exception)
            { }

            if (houseFavourited)
            {
                favAdded();
            }
            else
            {
                AddNavHeader();
            }

            if (house.listing_status.Equals("sale"))
            {
                payTerm = "";
            }

            Title = "House Details";
            imageURL.Source = house.image_url;
            price.Text = "£" + house.price + " " + payTerm;
            shortDescription.Text = house.num_bedrooms + " bedrooms " + house.property_type;
            description.Text = this.smallDescription;
            location.Text = house.displayable_address;
            agentLogo.Source = house.agent_logo;
            agentName.Text = house.agent_name;
            agentNumber.Text = house.agent_phone;
            propertyType.Text = house.property_type;
            
            if (house.property_type.Length < 1)
            {
                propertyType.IsVisible = false;
            }

            if(house.furnished_state != null)
            {
                furnished.Text = house.furnished_state;
                furnished.IsVisible = true;
            }

        }

        private void phoneCallClicked(Object sender, EventArgs e)
        {
            var phoneCallTask = CrossMessaging.Current.PhoneDialer;
            if (phoneCallTask.CanMakePhoneCall)
            {
                phoneCallTask.MakePhoneCall(houseDetails.agent_phone, "Agent");
            }
        }

        private void descBtn(Object sender, EventArgs e)
        {
            if (description.Text.Equals(this.smallDescription))
            {
                descriptionBtn.Text = "View less";
                description.Text = this.bigDescription;
            } else
            {
                descriptionBtn.Text = "Read more";
                description.Text = this.smallDescription;
            }            
        }

        private void shareHouse()
        {
            if (!CrossShare.IsSupported)
                return;

            CrossShare.Current.Share(new ShareMessage
            {
                Title = "Look at this house",
                Text = "I think you will like it!",
                Url = houseDetails.details_url,                
            });
        }

        private void AddNavHeader()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(
                new ToolbarItem(
                    "Favorites", "favorite.png", async () =>
                    {
                        functions.dbCreate();
                        functions.dbInsert(houseDetails);
                        Favorites saveFavorite = new Favorites();
                        await saveFavorite.saveFavorite(houseDetails);
                        favAdded();
                    }
                )
            );
        }

        private void styleElements()
        {
            descriptionBtn.BackgroundColor = functions.backgroundColour();
            descriptionBtn.TextColor = functions.textColour();
            BackgroundColor = functions.backgroundColour();
            share.BackgroundColor = functions.backgroundColour();
            share.TextColor = functions.textColour();
            callAgent.BackgroundColor = functions.backgroundColour();
            callAgent.TextColor = functions.textColour();
            price.TextColor = functions.priceTextColour();
            price.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
            shortDescription.TextColor = functions.textColour();
            shortDescription.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            location.TextColor = functions.textColour();
            location.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour.TextColor = functions.priceTextColour();
            textColour.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
            description.TextColor = functions.textColour();
            description.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour1.TextColor = functions.textColour();
            textColour1.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
            propertyType.TextColor = functions.textColour();
            propertyType.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            agentName.TextColor = functions.textColour();
            agentName.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            agentNumber.TextColor = functions.textColour();
            agentNumber.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            furnished.TextColor = functions.textColour();
            furnished.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            mapsFrame.BackgroundColor = functions.backgroundColour();
            mapsLabel.TextColor = functions.textColour();
            mapsLabel.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
            education.TextColor = functions.textColour();
            education.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            educationFrame.BackgroundColor = functions.backgroundColour();
            council.TextColor = functions.textColour();
            council.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            councilFrame.BackgroundColor = functions.backgroundColour();
            crime.TextColor = functions.textColour();
            crime.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            crimeFrame.BackgroundColor = functions.backgroundColour();
            people.TextColor = functions.textColour();
            people.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            peopleFrame.BackgroundColor = functions.backgroundColour();
            contactFrame.BackgroundColor = functions.navbarColour();
            mapsIcon.Source = "search" + functions.getLogo() + ".png";
            detailsStack.Padding = new Thickness(20, 0, 20, 20);
            dataHeader.Text = "Area details";
            dataHeader.TextColor = functions.priceTextColour();
            dataHeader.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
        }

        private void favAdded()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(
                new ToolbarItem(
                    "f", "favoriteFilled.png", async () =>
                    {
                        try
                        {
                            functions.dbDelete(houseDetails);
                        }
                        catch { }
                        AddNavHeader();
                        if (newPage)
                        {
                            App.MasterDetail.IsPresented = false;
                            NavigationPage page = new NavigationPage(new FavoritesPage());
                            page.BarBackgroundColor = functions.navbarColour();
                            App.MasterDetail.Detail = page;
                        }
                    }
                )
            );
        }

        protected void mapLink()
        {
            if (houseDetails.latitude.Length > 0 && houseDetails._longitude.Length > 0)
            {
                String url = "https://www.google.com/maps/place/" + houseDetails.latitude + "," + houseDetails._longitude;
                mapsLabel.Text = "View on map";

                var mapRecognizer = new TapGestureRecognizer();
                mapRecognizer.Tapped += (s, e) =>
                {
                    Device.OpenUri(new Uri(url));
                };
                mapsFrame.GestureRecognizers.Add(mapRecognizer);
            }
            else
            {
                mapsFrame.IsVisible = false;
            }
        }

        protected async void fetchStats(String area)
        {
            String statsUrl = "https://api.zoopla.co.uk/api/v1/local_info_graphs.js?api_key=jnwgh2seuv6q7t9u7weur68s&area=" + area;
            try
            {
                //Getting JSON data from the Web
                var content = await _client.GetStringAsync(statsUrl);
                //We deserialize the JSON data from this line
                Stats stats = JsonConvert.DeserializeObject<Stats>(content);
                List<Stats> houses = new List<Stats>();
                houses.Add(stats);
                allStats = new ObservableCollection<Stats>(houses);

                foreach (Stats areaStats in allStats)
                {
                    buildLinks(areaStats);
                    mapLink();
                } 
            }
            catch { }
        }

        private void buildLinks(Stats areaStats)
        {

            if (areaStats.education_graph_url.Length > 0)
            {
                education.Text = "Local education";

                var educationRecognizer = new TapGestureRecognizer();
                educationRecognizer.Tapped += (s, e) =>
                {
                    Device.OpenUri(new Uri(areaStats.education_graph_url));
                };
                educationFrame.GestureRecognizers.Add(educationRecognizer);
            }
            else
            {
                educationFrame.IsVisible = false;
            }

            if (areaStats.council_tax_graph_url.Length > 0)
            {
                council.Text = "Local council";
                var councilRecognizer = new TapGestureRecognizer();
                councilRecognizer.Tapped += (s, e) =>
                {
                    Device.OpenUri(new Uri(areaStats.council_tax_graph_url));
                };
                councilFrame.GestureRecognizers.Add(councilRecognizer);
            }
            else
            {
                councilFrame.IsVisible = false;
            }

            if (areaStats.crime_graph_url.Length > 0)
            {
                crime.Text = "Local crime";
                var crimeRecognizer = new TapGestureRecognizer();
                crimeRecognizer.Tapped += (s, e) =>
                {
                    Device.OpenUri(new Uri(areaStats.crime_graph_url));
                };
                crimeFrame.GestureRecognizers.Add(crimeRecognizer);
            }
            else
            {
                crimeFrame.IsVisible = false;
            }

            if (areaStats.crime_graph_url.Length > 0)
            {
                people.Text = "Local people";
                var peopleRecognizer = new TapGestureRecognizer();
                peopleRecognizer.Tapped += (s, e) =>
                {
                    Device.OpenUri(new Uri(areaStats.people_graph_url));
                };
                peopleFrame.GestureRecognizers.Add(peopleRecognizer);
            }
            else
            {
                peopleFrame.IsVisible = false;
            }
        }
    }
}