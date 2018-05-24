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
	public partial class FAQPage : ContentPage
	{
		public FAQPage ()
		{
			InitializeComponent ();
            backgroundColour.BackgroundColor = functions.backgroundColour();
            textColour.TextColor = functions.priceTextColour();
            textColour1.TextColor = functions.textColour();
            textColour2.TextColor = functions.priceTextColour();
            textColour3.TextColor = functions.textColour();
            textColour4.TextColor = functions.priceTextColour();
            textColour5.TextColor = functions.textColour();
            textColour6.TextColor = functions.priceTextColour();
            textColour7.TextColor = functions.textColour();
            textHeader.TextColor = functions.textColour();
            textColour.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour1.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour2.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour3.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour4.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour5.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour6.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
            textColour7.FontSize = Convert.ToDouble(Application.Current.Properties["fontSize"]);
        }
	}
}