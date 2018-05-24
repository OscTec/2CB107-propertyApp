using App1.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App1
{
	public partial class App : Application
	{

        public App ()
		{
			InitializeComponent();          
            setColours();
            checkFontSize();
            MainPage = new App1.MainPage();
		}

		protected override void OnStart ()
		{
            functions.dbCreate();
            
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

	    public static MasterDetailPage MasterDetail
	    {
	        get;
	        set;
	    }

        public static void setColours()
        {

            if (Application.Current.Properties.ContainsKey("colourScheme"))
            {
                switch (Application.Current.Properties["colourScheme"].ToString())
                {
                    case "light":
                        lightThemeLayout();
                        break;
                    case "blue":
                        blueLayout();
                        break;
                    case "dark":
                        darkThemeLayout();
                        break;
                    case "factory":
                        defaultLayout();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Application.Current.Properties["colourScheme"] = "default";
                defaultLayout();
            }
        }

        private static void lightThemeLayout()
        {
            Application.Current.Properties["backgroundColour"] = "#FFFFFF";
            Application.Current.Properties["gapColour"] = "#eee";
            Application.Current.Properties["navbarColour"] = "#3385ff";
            Application.Current.Properties["sidebarHeaderColour"] = "#3385ff";
            Application.Current.Properties["sidebarBodyColour"] = "#ffffff";
            Application.Current.Properties["buttonColour"] = "#f2f2f2";
            Application.Current.Properties["textColour"] = "#454545";
            Application.Current.Properties["priceTextColour"] = "#4d94ff";
            Application.Current.Properties["icons"] = "Dark";
        }

        private static void darkThemeLayout()
        {
            Application.Current.Properties["backgroundColour"] = "#1F1F1F";
            Application.Current.Properties["gapColour"] = "#a6a6a6";
            Application.Current.Properties["navbarColour"] = "#131313";
            Application.Current.Properties["sidebarHeaderColour"] = "#f10e80";
            Application.Current.Properties["sidebarBodyColour"] = "#262626";
            Application.Current.Properties["buttonColour"] = "#f4429b";
            Application.Current.Properties["textColour"] = "#ffffff";
            Application.Current.Properties["priceTextColour"] = "#f4429b";
            Application.Current.Properties["icons"] = "";
        }

        private static void blueLayout()
        {
            Application.Current.Properties["backgroundColour"] = "#2f4259";
            Application.Current.Properties["gapColour"] = "#00997a";
            Application.Current.Properties["navbarColour"] = "#1a2532";
            Application.Current.Properties["sidebarHeaderColour"] = "#3e5675";
            Application.Current.Properties["sidebarBodyColour"] = "#2f4259";
            Application.Current.Properties["buttonColour"] = "#1a2532";
            Application.Current.Properties["textColour"] = "#FFFFFF";
            Application.Current.Properties["priceTextColour"] = "#00997a";
            Application.Current.Properties["icons"] = "";
        }

        private static void defaultLayout()
        {
            Application.Current.Properties["backgroundColour"] = "#b74d72";
            Application.Current.Properties["gapColour"] = "#a34364";
            Application.Current.Properties["navbarColour"] = "#361621";
            Application.Current.Properties["sidebarHeaderColour"] = "#a34364";
            Application.Current.Properties["sidebarBodyColour"] = "#bc5c7e";
            Application.Current.Properties["buttonColour"] = "#7f344e";
            Application.Current.Properties["textColour"] = "#FFFFFF";
            Application.Current.Properties["priceTextColour"] = "#5b2538";
            Application.Current.Properties["icons"] = "";
        }

        public static void checkFontSize()
        {

            if (!Application.Current.Properties.ContainsKey("fontSize"))
            {
                Application.Current.Properties["fontSize"] = "12";
                Application.Current.Properties["priceTextSize"] = "20";
            }
        }
    }
}
