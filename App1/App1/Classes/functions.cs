using SQLite;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Classes
{
    class functions
    {
        static String localFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        static String fullPath = Path.Combine(localFolder, "mydb.sqlite");

        public static async void dbCreate()
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(fullPath);
            await db.CreateTableAsync<options>();
            await db.CreateTableAsync<options>();
            //db.Close();

        }


        public static async void dbInsert(options houseDetails)
        {
            SQLiteAsyncConnection db = new SQLiteAsyncConnection(fullPath);
            await db.InsertAsync(houseDetails);
            //db.Close();
        }

        public static List<options> dbPull()
        {
            var db = new SQLiteConnection(fullPath);
            List<options> allHouses = db.Query<options>("Select * from [options]");
            db.Close();
            return allHouses;            
        }

        public static void dbDelete(options houseDetails)
        {
            var db = new SQLiteConnection(fullPath);
            String deleteHouse = "DELETE FROM options WHERE listing_id = " + houseDetails.listing_id;
            List<options> allHouses = db.Query<options>(deleteHouse);
            db.Close();
        }

        public static Xamarin.Forms.Color temp()
        {
            return Xamarin.Forms.Color.HotPink;
        }

        public static Xamarin.Forms.Color backgroundColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["backgroundColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#4873A6");
            //#2f4259
            //#5C5389
        }

        public static Xamarin.Forms.Color gapColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["gapColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#2f4259");
            //#40CBC6

        }

        public static Xamarin.Forms.Color navbarColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["navbarColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#4A9ACD");
            //#40CBC6

        }

        public static Xamarin.Forms.Color sidebarHeaderColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["sidebarHeaderColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#4A9ACD");
        }

        public static Xamarin.Forms.Color sidebarBodyColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["sidebarBodyColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#4873A6");
        }

        public static Xamarin.Forms.Color buttonColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["buttonColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#524363");
            //#524363
        }

        public static Xamarin.Forms.Color textColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["textColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#AFDBFB");
        }

        public static Xamarin.Forms.Color priceTextColour()
        {
            return Xamarin.Forms.Color.FromHex(Application.Current.Properties["priceTextColour"] as string);
            //return Xamarin.Forms.Color.FromHex("#534266");
        }

        public static String getLogo()
        {
            return Application.Current.Properties["icons"].ToString();
        }
    }

    class colours
    {
        public static Xamarin.Forms.Color backgroundColour { get; set; }
        public static Xamarin.Forms.Color buttonColour { get; set; }
        public static Xamarin.Forms.Color textColour { get; set; }
        public static Double textSize { get; set; }
    }
}

