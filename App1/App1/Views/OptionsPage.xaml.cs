using App1.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OptionsPage : ContentPage
	{
		public OptionsPage ()
		{
			InitializeComponent ();
            backgroundColour.BackgroundColor = functions.backgroundColour();
            fontLabel.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
            colourLabel.FontSize = Convert.ToDouble(Application.Current.Properties["priceTextSize"]);
            addButtons();
            colourLabel.TextColor = functions.textColour();
            colourLabel.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            fontLabel.TextColor = functions.textColour();
            fontLabel.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);

        }

        private void clear(object sender, EventArgs e)
        {
            Application.Current.Properties["colourScheme"] = "default";
        }


        private void addButtons()
        {
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button fontSize10 = new Button()
            {
                Text = "Small",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 0),
                CornerRadius = 10
            };

            fontSize10.Clicked += delegate
            {
                sizeButton_Clicked("10");
            };

            Button fontSize15 = new Button()
            {
                Text = "Medium",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 0),
                CornerRadius = 10
            };

            fontSize15.Clicked += delegate
            {
                sizeButton_Clicked("15");
            };

            Button fontSize20 = new Button()
            {
                Text = "Large",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 0),
                CornerRadius = 10
            };

            fontSize20.Clicked += delegate
            {
                sizeButton_Clicked("20");
            };

            grid.Children.Add(fontSize10, 0, 0);
            grid.Children.Add(fontSize15, 1, 0);
            grid.Children.Add(fontSize20, 2, 0);


            Button blueButton = new Button()
            {
                Text = "Blue",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 0),
                CornerRadius = 10
            };

            blueButton.Clicked += delegate
            {
                Button_Clicked("blue");
            };

            Button lightButton = new Button()
            {
                Text = "Light",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 0),
                CornerRadius = 10
            };

            lightButton.Clicked += delegate
            {
                Button_Clicked("light");
            };

            Button darkButton = new Button()
            {
                Text = "Dark theme",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 0),
                CornerRadius = 10
            };

            darkButton.Clicked += delegate
            {
                Button_Clicked("dark");
            };

            Button defaultButton = new Button()
            {
                Text = "Factory",
                TextColor = functions.textColour(),
                BackgroundColor = functions.buttonColour(),
                Margin = new Thickness(5, 0, 5, 5),
                CornerRadius = 10
            };

            defaultButton.Clicked += delegate
            {
                Button_Clicked("factory");
            };

            fontButtons.Children.Add(grid);
            buttonLayout.Children.Add(blueButton);
            buttonLayout.Children.Add(lightButton);
            buttonLayout.Children.Add(darkButton);
            buttonLayout.Children.Add(defaultButton);
        }


        private void Button_Clicked(string colour)
        {

            Application.Current.Properties["colourScheme"] = colour;
            App.setColours();
            NavigationPage page = new NavigationPage(new OptionsPage());
            page.BarBackgroundColor = functions.navbarColour();
            App.MasterDetail.Detail = page;
        }

        private void sizeButton_Clicked(string size)
        {

            Application.Current.Properties["fontSize"] = size;
            NavigationPage page = new NavigationPage(new OptionsPage());
            page.BarBackgroundColor = functions.navbarColour();
            App.MasterDetail.Detail = page;
        }

    }
}