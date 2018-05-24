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
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();
            backgroundColour.BackgroundColor = functions.backgroundColour();
            header.TextColor = functions.textColour();
            textColour.TextColor = functions.priceTextColour();
            textColour1.TextColor = functions.textColour();
            textColour2.TextColor = functions.priceTextColour();
            textColour3.TextColor = functions.textColour();
            logo.Source = "logo" + functions.getLogo() + ".png";
            textColour.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour1.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour2.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour3.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
        }
	}
}