using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;

namespace App1
{
    class Favorites
    {
        public Favorites()
        {
        }

        public async Task<Boolean> saveFavorite(options houseDetails)
        {

            Boolean saved = false;
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"database.db3");
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<options>();
                db.Insert(houseDetails);
                //bookmark.Icon = "bookmarkFull.png";
                //var house = JsonConvert.SerializeObject(houseDetails);
                //Application.Current.Properties["id"] = house;

                //TO DO:
                // Add some notification that the favorite was saved
                saved = true;
            }
            catch (Exception)
            {
                saved = false;
            }
            return await Task.FromResult(saved);
        }

        public async Task<options> getFavorites()
        {
            options house = null;
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3");
                var db = new SQLiteConnection(dbPath);
                house = db.Get<options>(1);
            }

            catch (Exception){}
            return await Task.FromResult(house);
        }
    }
}
